using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Domain.Entities;

namespace DesafioVsoft.Api.Mapping;

public static class TaskMapper
{
    public static TaskItem ToEntity(TaskInputDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description
    };

    public static void UpdateEntity(TaskItem task, TaskInputDto dto)
    {
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.UserId = dto.UserId;
    }

    public static TaskOutputDto ToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted,
        UserId = task.UserId,
        Name = task.User?.Name
    };
}
