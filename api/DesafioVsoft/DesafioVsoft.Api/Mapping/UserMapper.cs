using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Domain.Entities;

namespace DesafioVsoft.Api.Mappers;

/// <summary>
/// Conversores manuais entre User e seus DTOs
/// </summary>
public static class UserMapper
{
    public static UserOutputDto ToDto(User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

    public static User ToEntity(UserInputDto dto) =>
        new()
        {
            Name = dto.Name,
            Email = dto.Email
        };

    public static void UpdateEntity(User entity, UserInputDto dto)
    {
        entity.Name = dto.Name;
        entity.Email = dto.Email;
    }
}
