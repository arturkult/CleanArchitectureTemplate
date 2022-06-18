using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ISaveChangesInterceptor _saveChangesInterceptor;

    public ApplicationDbContext(IMediator mediator, ISaveChangesInterceptor saveChangesInterceptor,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _saveChangesInterceptor = saveChangesInterceptor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}