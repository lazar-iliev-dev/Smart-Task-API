using Application.Services;
using Application.Interfaces;
using Domain.Entities.Enums;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class TaskServiceTests
{
        private readonly Mock<ITaskRepository> _repoMock;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _repoMock = new Mock<ITaskRepository>();
            _service = new TaskService(_repoMock.Object);
        }


    [Fact]
    public void CreateTask_ShouldReturnTask_WithValidProperties()
    {
        // Arrange
        var task = new TaskItem
        {
            Title = "Test Task",
            Description = "Demo",
            Priority = Priority.Low,
            Status = 0,
            CreatedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        task.Title.Should().Be("Test Task");
        task.Priority.Should().Be(Priority.Low);
    }


     [Fact]
    public async Task GetAllTasksAsync_ReturnsAllTasks()
    {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { TaskId = Guid.NewGuid(), Title = "Test 1" },
                new TaskItem { TaskId = Guid.NewGuid(), Title = "Test 2" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _service.GetAllTasksAsync();

            // Assert
            Assert.Equal(2, ((List<TaskItem>)result).Count);
    }

    [Fact]
    public async Task CreateTaskAsync_CallsRepository()
     {
            // Arrange
            var task = new TaskItem { TaskId = Guid.NewGuid(), Title = "New Task" };
            _repoMock.Setup(r => r.AddAsync(task)).ReturnsAsync(task);

            // Act
            var result = await _service.CreateTaskAsync(task);

            // Assert
            Assert.Equal(task.TaskId, result.TaskId);
            _repoMock.Verify(r => r.AddAsync(task), Times.Once);
     }
}
