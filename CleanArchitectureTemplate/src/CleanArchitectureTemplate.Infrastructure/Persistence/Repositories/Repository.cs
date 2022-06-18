using CleanArchitectureTemplate.Application.Exceptions;
using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T, Guid> where T : BaseAggregate
{
    private readonly ApplicationDbContext _context;

    public Repository(IApplicationDbContext context)
    {
        _context = (ApplicationDbContext)context;
    }
    
    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        var fromDb = await _context.FindAsync<T>(entity.Id);
        if (fromDb is null)
        {
            throw new NotFoundException(entity.Id.ToString());
        }
        
        fromDb.Update(entity);
        return entity;
    }

    public async Task Delete(Guid id)
    {
        var fromDb = await _context.FindAsync<T>(id);
        if (fromDb is null)
        {
            throw new NotFoundException(id.ToString());
        }

        _context.Set<T>().Remove(fromDb);
    }
}