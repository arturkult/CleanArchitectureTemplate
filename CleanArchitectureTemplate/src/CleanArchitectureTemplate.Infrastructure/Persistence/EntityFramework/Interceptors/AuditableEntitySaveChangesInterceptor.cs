using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService<Guid> _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public AuditableEntitySaveChangesInterceptor(
        ICurrentUserService<Guid> currentUserService,
        IDateTime dateTime,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        await DispatchDomainEvents(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUserService.UserId;
                entry.Entity.Created = _dateTime.Now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                entry.HasChangedOwnedEntities())
            {
                entry.Entity.UpdatedBy = _currentUserService.UserId;
                entry.Entity.Updated = _dateTime.Now;
            }
        }
    }

    private Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken = default)
    {
        if (context is null) return Task.CompletedTask;

        var entities = context.ChangeTracker
            .Entries<BaseAggregate>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);
        return _domainEventDispatcher.DispatchAndClearEvents(entities, cancellationToken);
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}