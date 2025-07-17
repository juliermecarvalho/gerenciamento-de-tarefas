using DesafioVsoft.Domain.Entities;
using System.Security.Claims;

namespace DesafioVsoft.Api.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}

public interface IUsuarioLogged
{
    Guid Id { get; }
    string Name { get; }
    string Email { get; }
}



public class UsuarioLogged : IUsuarioLogged
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioLogged(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id => Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty;

    public string Name => User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public string Email => User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
}
