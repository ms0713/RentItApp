using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Abstractions;

namespace RentIt.Infrastructure.Repositories;
internal abstract class Repository<T>
    where T : Entity
{
    private readonly ApplicationDbContext m_DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        m_DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await m_DbContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        m_DbContext.Add(entity);
    }
}
