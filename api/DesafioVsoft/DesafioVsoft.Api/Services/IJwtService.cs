using DesafioVsoft.Domain.Entities;

namespace DesafioVsoft.Api.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}