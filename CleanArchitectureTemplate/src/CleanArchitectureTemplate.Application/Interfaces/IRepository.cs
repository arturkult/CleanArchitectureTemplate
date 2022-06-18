using CleanArchitectureTemplate.Domain.Common;

namespace CleanArchitectureTemplate.Application.Interfaces;

public interface IRepository<T, in TId> where T : BaseAggregate where TId: struct
{
    T Add(T entity);
    Task<T> Update(T entity);
    Task Delete(TId id);
}