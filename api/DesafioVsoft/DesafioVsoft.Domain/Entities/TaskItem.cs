namespace DesafioVsoft.Domain.Entities;

/// <summary>
/// Representa uma tarefa atribuída a um usuário
/// </summary>
public class TaskItem : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? UserId { get; set; }

    public User? User { get; set; }
}
