using DesafioVsoft.Domain.Commons;
using DesafioVsoft.Domain.Entities;
using System.Linq.Expressions;

namespace DesafioVsoft.Domain.Repositories;

/// <summary>
/// Interface base genérica para repositórios
/// </summary>
public interface IBaseRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        params Func<IQueryable<T>, IQueryable<T>>[]? includes);

    Task<Pagination<T>> GetPaginationAsync(
    int page,
    Expression<Func<T, bool>>? filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    params Expression<Func<T, object>>[] includes);

    Task<T?> GetByIdAsync(Guid id);

    Task AddOrUpdateAsync(T entity);

    Task DeleteAsync(Guid id);
}