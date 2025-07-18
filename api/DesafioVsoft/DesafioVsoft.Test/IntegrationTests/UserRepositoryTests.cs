using Bogus;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Repository.Data;
using DesafioVsoft.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DesafioVsoft.IntegrationTests;

public class UserRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public UserRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"UserRepoDb_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddOrUpdateAsync_ShouldAddUser_WhenNew()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Fulano",
            Email = "fulano@email.com",
            Password = "123"
        };

        await using var context = new AppDbContext(_dbContextOptions);
        var repo = new UserRepository(context);

        // Act
        await repo.AddOrUpdateAsync(user);

        // Assert
        var saved = await repo.GetByIdAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal(user.Name, saved!.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new Faker<User>("pt_BR")
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, "123")
            .Generate(3);

        await using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        await using var readContext = new AppDbContext(_dbContextOptions);
        var repo = new UserRepository(readContext);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task AnyAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Existente",
            Email = "existe@email.com",
            Password = "123"
        };

        await using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        await using var checkContext = new AppDbContext(_dbContextOptions);
        var repo = new UserRepository(checkContext);

        // Act
        var exists = await repo.AnyAsync(userId);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task AnyAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var repo = new UserRepository(new AppDbContext(_dbContextOptions));

        // Act
        var exists = await repo.AnyAsync(Guid.NewGuid());

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser_WhenExists()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Para Deletar",
            Email = "delete@email.com",
            Password = "123"
        };

        await using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        await using var deleteContext = new AppDbContext(_dbContextOptions);
        var repo = new UserRepository(deleteContext);

        // Act
        await repo.DeleteAsync(user.Id);

        // Assert
        var deleted = await repo.GetByIdAsync(user.Id);
        Assert.Null(deleted);
    }
}
