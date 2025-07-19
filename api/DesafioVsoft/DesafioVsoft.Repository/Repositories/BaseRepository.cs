using DesafioVsoft.Domain.Commons;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace DesafioVsoft.Repository.Repositories;

/// <summary>
/// Implementação base genérica para repositórios
/// </summary>
public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly int _totalDeRegistrosPorPagina = 10;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }


    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task AddOrUpdateAsync(T entity)
    {

        var existing = await _dbSet.FindAsync(entity.Id);
        if (existing == null)
        {
            _dbSet.Add(entity); // Entidade não existe no banco, adiciona
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(entity); // Atualiza os valores
        }


        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        params Func<IQueryable<T>, IQueryable<T>>[]? includes)
    {
        var query = _dbSet.AsQueryable();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = orderBy is not null ? orderBy(query) : query.OrderBy(q => q.Id);

        query = includes?.Aggregate(query, (current, include) => include(current));

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<Pagination<T>> GetPaginationAsync(
    int page,
    Expression<Func<T, bool>>? filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    params Expression<Func<T, object>>[] includes)
    {

        var query = _dbSet.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = orderBy is not null ? orderBy(query) : query.OrderBy(q => q.Id);

        query = includes
            .Aggregate(
                query,
                (current, include) => current.Include(include.AsPath())
            );

        var listItens = await query
            .Skip(_totalDeRegistrosPorPagina * (page - 1))
            .Take(_totalDeRegistrosPorPagina).ToListAsync();

        var toReturn = new Pagination<T>
        {
            TotalRecords = query.Count(),
            PageSize = _totalDeRegistrosPorPagina,
            PageNumber = page,
            Items = listItens
        };

        return toReturn;
    }
}
