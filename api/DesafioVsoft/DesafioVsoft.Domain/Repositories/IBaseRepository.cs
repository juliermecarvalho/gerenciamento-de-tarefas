namespace DesafioVsoft.Domain.Repositories;

/// <summary>
/// Interface base genérica para repositórios
/// </summary>
public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task AddOrUpdateAsync(T entity);

    Task DeleteAsync(int id);
}