using Application.Services;
using Domain.Entities;
using Application.Interfaces;
using Moq;

namespace UnitTests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnCreatedTask()
        {
            // Arrange
            var task = new TaskItem { TaskId = Guid.NewGuid(), Title = "New Task" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).ReturnsAsync(task);

            // Act
            var result = await _service.CreateTaskAsync(task);

            // Assert
            Assert.Equal("New Task", result.Title);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem { TaskId = taskId, Title = "Test Task" };
            _mockRepo.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            // Act
            var result = await _service.GetTaskByIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result!.TaskId);
        }

        [Fact]
        public async Task GetTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { TaskId = Guid.NewGuid(), Title = "Task1" },
                new TaskItem { TaskId = Guid.NewGuid(), Title = "Task2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _service.GetAllTasksAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, ((List<TaskItem>)result).Count);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnUpdatedTask_WhenExists()
        {
            // Arrange
            var task = new TaskItem { TaskId = Guid.NewGuid(), Title = "Old" };
            var updated = new TaskItem { TaskId = task.TaskId, Title = "Updated" };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).ReturnsAsync(updated);

            // Act
            var result = await _service.UpdateTaskAsync(task);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result!.Title);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(taskId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteTaskAsync(taskId);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(taskId)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteTaskAsync(taskId);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteAsync(taskId), Times.Once);
        }
    }
}
