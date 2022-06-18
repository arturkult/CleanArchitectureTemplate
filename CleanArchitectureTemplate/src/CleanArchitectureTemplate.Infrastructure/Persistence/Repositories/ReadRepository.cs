using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class ReadRepository<T>: IReadRepository<T, Guid> where T : BaseAggregate
{
    private readonly ApplicationDbContext _context;

    public ReadRepository(IApplicationDbContext context)
    {
        _context = (ApplicationDbContext)context;
    }
    public IQueryable<T> Queryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public ValueTask<T?> GetById(Guid id)
    {
        return _context.FindAsync<T>(id);
    }
}