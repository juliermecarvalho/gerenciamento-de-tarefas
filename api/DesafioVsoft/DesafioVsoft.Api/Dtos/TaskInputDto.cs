namespace DesafioVsoft.Api.Dtos;

public class TaskInputDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public Guid? UserId { get; set; }
}

public class TaskUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public Guid? UserId { get; set; }
}


public class TaskAssignDto
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class TaskOutputDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public Guid? UserId { get; set; }
    public string? Name { get; set; }
}
