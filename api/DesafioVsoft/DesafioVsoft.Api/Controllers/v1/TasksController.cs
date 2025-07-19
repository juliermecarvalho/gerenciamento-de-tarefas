using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Mappers;
using DesafioVsoft.Api.Mapping;
using DesafioVsoft.Api.Services;
using DesafioVsoft.Domain.Commons;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.RabbitMq;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DesafioVsoft.Api.Controllers.v1;

[Authorize]
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public TaskController(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskOutputDto>>> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync(includes: t => t.Include(x => x.User));
        var result = tasks.Select(TaskMapper.ToDto).ToList();
        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<Pagination<TaskOutputDto>>> GetPaged([FromQuery(Name = "page-number")] int pageNumber)
    {
        var paginationUsers = await _taskRepository.GetPaginationAsync(page: pageNumber,
            orderBy: o => o.OrderBy(x => x.Title),
            includes: t => t.Include(x => x.User));

        return Ok(TaskMapper.ToDto(paginationUsers));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskOutputDto>> GetById(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        return Ok(TaskMapper.ToDto(task));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TaskInputDto dto, [FromServices] IUsuarioLogged usuarioLogged, [FromServices] IRabbitMqProducer rabbitMqProducer)
    {
        var task = TaskMapper.ToEntity(dto);
        task.UserId = usuarioLogged.Id;
        await _taskRepository.AddOrUpdateAsync(task);
        await rabbitMqProducer.PublishUserChangedAsync("Tarefa crianda com sucesso!!!");
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] TaskInputDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        TaskMapper.UpdateEntity(task, dto);
        await _taskRepository.AddOrUpdateAsync(task);


        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        await _taskRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("assign")]
    public async Task<ActionResult> AssignTaskToUser([FromBody] TaskAssignDto dto, [FromServices] IRabbitMqProducer rabbitMqProducer)
    {
        var task = await _taskRepository.GetByIdAsync(dto.TaskId);
        if (task is null)
            return NotFound("Tarefa não encontrada");

        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            return NotFound("Usuário não encontrado");

        task.UserId = dto.UserId;
        await _taskRepository.AddOrUpdateAsync(task);


        await rabbitMqProducer.PublishUserChangedAsync($"Tarefa atribuída a {user.Name} com sucesso!!!");

        return Ok("Tarefa atribuída com sucesso.");
    }

    /// <summary>
    /// Cria múltiplas tasks aleatórias para simular um cenário de teste.
    /// </summary>    
    [HttpPost("createRandom")]
    public async Task<ActionResult> CreateRandomTasks([FromServices] IUsuarioLogged usuarioLogged)
    {
        var createdUsers = new List<TaskItem>();
        var tasks = await _taskRepository.GetAllAsync();
        for (int i = 0; i < 1000; i++)
        {
            var count = (tasks.Count() + i + 1);
            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = $"Tarefa {count}",
                Description = $"Descrição da tarefa {count}",
                IsCompleted = false,
                UserId = usuarioLogged.Id // Simulando um usuário aleatório
            };


            createdUsers.Add(taskItem);
        }

        foreach (var taskItem in createdUsers)
            await _taskRepository.AddOrUpdateAsync(taskItem);

        return Ok($"Tasks criados com sucesso.");
    }
}

