

using DesafioVsoft.Api.Controllers.v1;
using DesafioVsoft.Api.Dtos;
using DesafioVsoft.Api.Mapping;
using DesafioVsoft.Api.Services;
using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.RabbitMq;
using DesafioVsoft.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DesafioVsoft.Test.Controllers;

public class TaskControllerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRabbitMqProducer> _rabbitMqProducerMock = new();
    private readonly Mock<IUsuarioLogged> _usuarioLoggedMock = new();
    private readonly TaskController _controller;

    public TaskControllerTests()
    {
        _controller = new TaskController(_taskRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllShouldReturnListOfTasks()
    {
        // Arrange
        Expression<Func<TaskItem, bool>> filter = It.IsAny<Expression<Func<TaskItem, bool>>>();
        var tasks = new List<TaskItem> { new() { Id = Guid.NewGuid(), Title = "Teste" } };

        _taskRepositoryMock.Setup(r => r.GetAllAsync(
          null,
          null,
          true,
          new Func<IQueryable<TaskItem>, IQueryable<TaskItem>>[]
             {
                It.IsAny<Func<IQueryable<TaskItem>, IQueryable<TaskItem>>>()
             }
        )).ReturnsAsync(tasks);


        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<List<TaskOutputDto>>(okResult.Value);
        Assert.Single(returned);
    }

    [Fact]
    public async Task GetByIdShouldReturnTaskWhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var task = new TaskItem { Id = id, Title = "Teste" };
        _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<TaskOutputDto>(okResult.Value);
        Assert.Equal(id, dto.Id);
    }

    [Fact]
    public async Task GetByIdShouldReturnNotFoundWhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateShouldAddTaskAndReturnCreatedAt()
    {
        // Arrange
        var dto = new TaskInputDto { Title = "Nova tarefa", Description = "Desc" };
        _usuarioLoggedMock.Setup(x => x.Id).Returns(Guid.NewGuid());

        // Act
        var result = await _controller.Create(dto, _usuarioLoggedMock.Object, _rabbitMqProducerMock.Object);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        _taskRepositoryMock.Verify(r => r.AddOrUpdateAsync(It.IsAny<TaskItem>()), Times.Once);
        _rabbitMqProducerMock.Verify(r => r.PublishUserChangedAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateShouldModifyTaskWhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new TaskItem { Id = id, Title = "Original" };
        _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);

        var dto = new TaskInputDto { Title = "Atualizado", Description = "Atualizado" };

        // Act
        var result = await _controller.Update(id, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Atualizado", existingTask.Title);
    }

    [Fact]
    public async Task DeleteShouldRemoveTaskWhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new TaskItem { Id = id };
        _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _taskRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task AssignTaskToUserShouldAssignUserWhenValid()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var task = new TaskItem { Id = taskId };
        var user = new User { Id = userId, Name = "Fulano" };

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var dto = new TaskAssignDto { TaskId = taskId, UserId = userId };

        // Act
        var result = await _controller.AssignTaskToUser(dto, _rabbitMqProducerMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _taskRepositoryMock.Verify(r => r.AddOrUpdateAsync(task), Times.Once);
        _rabbitMqProducerMock.Verify(r => r.PublishUserChangedAsync(It.Is<string>(msg => msg.Contains(user.Name))), Times.Once);
    }
}
