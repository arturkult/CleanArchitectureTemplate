using System.Collections.ObjectModel;
using CleanArchitectureTemplate.Domain.Interfaces;

namespace CleanArchitectureTemplate.Domain.Common;

public abstract class BaseAggregate : BaseAuditableEntity<Guid>, IUpdatable<BaseAggregate>
{
    public ICollection<BaseEvent> DomainEvents { get; private set; }
    
    public virtual void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }

    public abstract void Update(BaseAggregate newObject);
}