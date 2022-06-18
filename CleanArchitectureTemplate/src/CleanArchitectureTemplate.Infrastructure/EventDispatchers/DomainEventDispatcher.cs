using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Domain.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Infrastructure.EventDispatchers;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAndClearEvents(IEnumerable<BaseAggregate> entitiesWithEvents,
        CancellationToken cancellationToken = default)
    {
        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents) await _mediator.Publish(domainEvent, cancellationToken);
    }
}