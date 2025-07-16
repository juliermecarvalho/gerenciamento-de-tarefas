using DesafioVsoft.Repository.Data;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace DesafioVsoft.Repository.Repositories;


/// <summary>
/// Repositório concreto para usuários
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> AnyAsync(Guid id)
    {
        return await _dbSet.AnyAsync(u => u.Id == id);
    }
}