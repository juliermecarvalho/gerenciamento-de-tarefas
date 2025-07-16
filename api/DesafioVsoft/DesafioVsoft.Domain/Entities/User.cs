namespace DesafioVsoft.Domain.Entities;

/// <summary>
/// Representa um usuário do sistema
/// </summary>
public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = "123";
    public string Email { get; set; } = string.Empty;

    // Lista de tarefas atribuídas ao usuário
    public List<TaskItem> Tasks { get; set; } = new();
}
