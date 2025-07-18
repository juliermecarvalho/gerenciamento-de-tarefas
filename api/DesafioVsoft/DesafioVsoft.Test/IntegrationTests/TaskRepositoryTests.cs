using Bogus;
using Bogus.DataSets;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Repository.Data;
using DesafioVsoft.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DesafioVsoft.IntegrationTests;

public class TaskRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public TaskRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TaskRepoTestDb_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddOrUpdateAsync_ShouldAddTask_WhenNew()
    {
        // Arrange
        var faker = new Faker("pt_BR");
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(3),
            Description = faker.Lorem.Paragraph(),
        };

        await using var context = new AppDbContext(_dbContextOptions);
        var repository = new TaskRepository(context);

        // Act
        await repository.AddOrUpdateAsync(task);

        // Assert
        var stored = await repository.GetByIdAsync(task.Id);
        Assert.NotNull(stored);
        Assert.Equal(task.Title, stored!.Title);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllInsertedTasks()
    {
        // Arrange
        var tasks = new Faker<TaskItem>("pt_BR")
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Title, f => f.Lorem.Sentence())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .Generate(5);

        await using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();
        }

        await using var readContext = new AppDbContext(_dbContextOptions);
        var repository = new TaskRepository(readContext);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTask_WhenExists()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Para deletar",
            Description = "Description",

        };

        await using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
        }

        await using var deleteContext = new AppDbContext(_dbContextOptions);
        var repository = new TaskRepository(deleteContext);

        // Act
        await repository.DeleteAsync(task.Id);

        // Assert
        var deleted = await repository.GetByIdAsync(task.Id);
        Assert.Null(deleted);
    }
}
