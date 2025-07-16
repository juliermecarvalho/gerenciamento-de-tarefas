namespace DesafioVsoft.Domain.Entities;

/// <summary>
/// Representa uma tarefa atribuída a um usuário
/// </summary>
public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public int UserId { get; set; }

    // Relação com o usuário dono da tarefa
    public User? User { get; set; }
}
