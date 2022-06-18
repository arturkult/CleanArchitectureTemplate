using CleanArchitectureTemplate.Domain.Common;

namespace CleanArchitectureTemplate.Domain.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseAggregate> entitiesWithEvents,
        CancellationToken cancellationToken = default);
}