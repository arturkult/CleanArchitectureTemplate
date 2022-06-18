using CleanArchitectureTemplate.Domain.Common;

namespace CleanArchitectureTemplate.Domain.Interfaces;

public interface IUpdatable<in T> where T: BaseAggregate
{
    void Update(T newObject);
}