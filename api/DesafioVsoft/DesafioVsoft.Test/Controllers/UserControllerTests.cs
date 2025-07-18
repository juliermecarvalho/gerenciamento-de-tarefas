using DesafioVsoft.Api.Controllers;
using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Mappers;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DesafioVsoft.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _controller = new UserController(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllShouldReturnListOfUsers()
    {
        // Arrange
        var users = new List<User> { new() { Id = Guid.NewGuid(), Name = "Usuário Teste" } };
        _userRepositoryMock.Setup(r => r.GetAllAsync(
          null,
          null,
          true,
          It.IsAny<Func<IQueryable<User>, IQueryable<User>>[]>()
          )).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<List<UserOutputDto>>(okResult.Value);
        Assert.Single(returned);
    }

    [Fact]
    public async Task GetByIdShouldReturnUser_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User { Id = id, Name = "Fulano" };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<UserOutputDto>(okResult.Value);
        Assert.Equal(id, dto.Id);
    }

    [Fact]
    public async Task GetByIdShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateShouldReturnCreatedAt_WhenSuccess()
    {
        // Arrange
        var dto = new UserInputDto { Name = "Novo Usuário", Email = "email@teste.com" };

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        _userRepositoryMock.Verify(r => r.AddOrUpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateShouldReturnNoContent_WhenUserExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UserInputDto { Name = "Atualizado", Email = "email@atualizado.com" };
        var user = new User { Id = id, Name = "Original" };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        // Act
        var result = await _controller.Update(id, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _userRepositoryMock.Verify(r => r.AddOrUpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task UpdateShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UserInputDto { Name = "Teste", Email = "teste@email.com" };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Update(id, dto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteShouldReturnNoContent_WhenUserExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User { Id = id };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _userRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateRandomUsersShouldReturnBadRequest_WhenInvalidInput()
    {
        // Arrange
        var dto = new UserBatchInputDto { Amount = 0, UserNameMask = "" };

        // Act
        var result = await _controller.CreateRandomUsers(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Amount deve ser maior que zero e a máscara deve ser válida.", badRequest.Value);
    }

    [Fact]
    public async Task CreateRandomUsersShouldCreateUsers_WhenValidInput()
    {
        // Arrange
        var dto = new UserBatchInputDto { Amount = 2, UserNameMask = "user_{{random}}" };

        // Act
        var result = await _controller.CreateRandomUsers(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _userRepositoryMock.Verify(r => r.AddOrUpdateAsync(It.IsAny<User>()), Times.Exactly(dto.Amount));
    }
}
