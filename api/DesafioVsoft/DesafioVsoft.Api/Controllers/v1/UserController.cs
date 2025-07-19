using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Mappers;
using DesafioVsoft.Api.Services;
using DesafioVsoft.Domain.Commons;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DesafioVsoft.Api.Controllers;

[Authorize]
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    [HttpGet]
    public async Task<ActionResult<List<UserOutputDto>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
        var result = users.Select(UserMapper.ToDto).ToList();
        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<Pagination<UserOutputDto>>> GetPaged([FromQuery(Name = "page-number")] int pageNumber)
    {
        var paginationUsers = await _userRepository.GetPaginationAsync(page: pageNumber, orderBy: o => o.OrderBy(x => x.Name));

        return Ok(UserMapper.ToDto(paginationUsers));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<UserOutputDto>> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        return Ok(UserMapper.ToDto(user));
    }


    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserInputDto dto)
    {
        var user = UserMapper.ToEntity(dto);
        await _userRepository.AddOrUpdateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, null);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UserInputDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        UserMapper.UpdateEntity(user, dto);
        await _userRepository.AddOrUpdateAsync(user);
        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        await _userRepository.DeleteAsync(id);
        return NoContent();
    }


    /// <summary>
    /// Cria múltiplos usuários aleatórios com base em uma máscara
    /// </summary>    
    [HttpPost("createRandom")]
    public async Task<ActionResult> CreateRandomUsers([FromBody] UserBatchInputDto dto)
    {
        if (dto.Amount <= 0 || string.IsNullOrWhiteSpace(dto.UserNameMask))
            return BadRequest("Amount deve ser maior que zero e a máscara deve ser válida.");

        var createdUsers = new List<User>();

        var rng = new Random();

        for (int i = 0; i < dto.Amount; i++)
        {
            var randomPart = GenerateRandomString(8, rng);
            var userName = dto.UserNameMask.Replace("{{random}}", randomPart);

            var user = new User
            {
                Name = userName,
                Email = $"{userName}@example.com".Replace(" ", "")
            };

            createdUsers.Add(user);
        }

        foreach (var user in createdUsers)
            await _userRepository.AddOrUpdateAsync(user);

        return Ok($"{dto.Amount} usuários criados com sucesso.");
    }

    /// <summary>
    /// Gera uma string aleatória de letras e números
    /// </summary>
    private static string GenerateRandomString(int length, Random rng)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
            sb.Append(chars[rng.Next(chars.Length)]);
        return sb.ToString();
    }

}
