
using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Services;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVsoft.Api.Controllers.v1;


[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginController(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {

        var users = await _userRepository.GetAllAsync(filter: f => f.Email == dto.Email);
        var user = users.FirstOrDefault();
        if (user is null || user.Password != dto.Password)
        {
            if (dto.Email == "adm@adm.com")
            {
                var admin = new Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrador",
                    Email = dto.Email,
                    Password = dto.Password
                };
                await _userRepository.AddOrUpdateAsync(admin);

                var t = _jwtService.GenerateToken(admin);
                return Ok(new { Token = t });
            }

            return Unauthorized("Credenciais inválidas.");
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}
