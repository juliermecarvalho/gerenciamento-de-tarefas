using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Mapping;
using DesafioVsoft.Domain.RabbitMq;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DesafioVsoft.Api.Controllers.v1;

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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskOutputDto>> GetById(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        return Ok(TaskMapper.ToDto(task));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TaskInputDto dto)
    {
        var task = TaskMapper.ToEntity(dto);
        await _taskRepository.AddOrUpdateAsync(task);
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

        var exist = await _userRepository.AnyAsync(dto.UserId);
        if (!exist)
            return NotFound("Usuário não encontrado");

        task.UserId = dto.UserId;
        await _taskRepository.AddOrUpdateAsync(task);


        await rabbitMqProducer.PublishUserChangedAsync(dto.UserId);

        return Ok("Tarefa atribuída com sucesso.");
    }
}

