using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace DesafioVsoft.Api.Controllers.v1;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return Ok(tasks);
    }

    // GET: api/tasks/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItem task)
    {
        // Comentário: Validação básica pode ser adicionada aqui
        await _taskRepository.AddOrUpdateAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    // PUT: api/tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem updatedTask)
    {
        if (id != updatedTask.Id)
            return BadRequest("ID mismatch.");

        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask is null)
            return NotFound();

        await _taskRepository.AddOrUpdateAsync(updatedTask);
        return NoContent();
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _taskRepository.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        await _taskRepository.DeleteAsync(id);
        return NoContent();
    }
}
