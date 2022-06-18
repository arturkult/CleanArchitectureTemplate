using CleanArchitectureTemplate.Domain.Common;

namespace CleanArchitectureTemplate.Application.Interfaces;

public interface IReadRepository<T, in TId> where T : BaseAggregate where TId : struct
{
    IQueryable<T> Queryable();
    ValueTask<T?> GetById(TId id);
}
