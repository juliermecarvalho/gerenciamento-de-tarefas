namespace DesafioVsoft.Api.Dtos;



/// <summary>
/// DTO para criação/atualização de usuários
/// </summary>
public class UserInputDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// DTO para retorno de dados de usuários
/// </summary>
public class UserOutputDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}



/// <summary>
/// DTO para criação em lote de usuários aleatórios
/// </summary>
public class UserBatchInputDto
{
    public int Amount { get; set; }
    public string UserNameMask { get; set; } = string.Empty;
}
