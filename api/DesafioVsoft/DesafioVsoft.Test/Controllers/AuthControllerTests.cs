using DesafioVsoft.Api.Controllers.v1;
using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Linq.Expressions;

namespace DesafioVsoft.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly LoginController _controller;

    public AuthControllerTests()
    {
        _controller = new LoginController(_userRepositoryMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task LoginShouldReturnToken_WhenUserExistsAndPasswordMatches()
    {
        // Arrange
        var dto = new LoginDto { Email = "user@test.com", Password = "123" };
        var user = new User { Id = Guid.NewGuid(), Email = dto.Email, Password = dto.Password };

        _userRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null,
                true,
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>[]>()))
            .ReturnsAsync(new List<User> { user });

        _jwtServiceMock.Setup(s => s.GenerateToken(user)).Returns("fake-jwt-token");

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("Token", okResult.Value.ToString()!);
        Assert.Contains("fake-jwt-token", okResult.Value.ToString()!);
    }

    [Fact]
    public async Task LoginShouldReturnUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginDto { Email = "user@notfound.com", Password = "wrong" };
        _userRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null,
                true,
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>[]>()))
            .ReturnsAsync(new List<User>()); // nenhum usuário

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciais inválidas.", unauthorized.Value);
    }

    [Fact]
    public async Task LoginShouldCreateAdminUser_WhenEmailIsAdminAndUserDoesNotExist()
    {
        // Arrange
        var dto = new LoginDto { Email = "adm@adm.com", Password = "admin123" };
        _userRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null,
                true,
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>[]>()))
            .ReturnsAsync(new List<User>()); // nenhum usuário

        _jwtServiceMock
            .Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns("admin-jwt-token");

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("Token", okResult.Value.ToString()!);
        Assert.Contains("admin-jwt-token", okResult.Value.ToString()!);
        _userRepositoryMock.Verify(r => r.AddOrUpdateAsync(It.Is<User>(u => u.Email == dto.Email)), Times.Once);
    }

    [Fact]
    public async Task LoginShouldReturnUnauthorized_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var dto = new LoginDto { Email = "user@test.com", Password = "wrong" };
        var user = new User { Id = Guid.NewGuid(), Email = dto.Email, Password = "correct" };

        _userRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                null,
                true,
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>[]>()))
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciais inválidas.", unauthorized.Value);
    }
}
