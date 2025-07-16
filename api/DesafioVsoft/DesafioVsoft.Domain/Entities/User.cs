namespace DesafioVsoft.Domain.Entities;

/// <summary>
/// Representa um usuário do sistema
/// </summary>
public class User : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = "123"; // Senha padrão para novos usuários
    public string Email { get; set; } = string.Empty;

    // Lista de tarefas atribuídas ao usuário
    public List<TaskItem> Tasks { get; set; } = new();
}
