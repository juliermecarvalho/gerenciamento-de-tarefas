using System.Linq.Expressions;

namespace DesafioVsoft.Domain.Repositories;

/// <summary>
/// Interface base genérica para repositórios
/// </summary>
public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        params Func<IQueryable<T>, IQueryable<T>>[]? includes);

    Task<T?> GetByIdAsync(Guid id);

    Task AddOrUpdateAsync(T entity);

    Task DeleteAsync(Guid id);
}