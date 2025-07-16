using DesafioVsoft.Repository.Data;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;


namespace DesafioVsoft.Repository.Repositories;


/// <summary>
/// Repositório concreto para usuários
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

}