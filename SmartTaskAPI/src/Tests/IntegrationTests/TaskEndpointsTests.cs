using System.Net.Http.Json;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class TaskEndpointsTests : IClassFixture<TestingWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TaskEndpointsTests(TestingWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostTask_ShouldReturnCreatedTask()
    {
        // Arrange
        var newTask = new
        {
            title = "Integration Test",
            description = "Testing endpoint",
            dueDate = DateTime.UtcNow.AddDays(3),
            status = 0,
            priority = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", newTask);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var created = await response.Content.ReadFromJsonAsync<TaskItem>();
        created!.Title.Should().Be("Integration Test");
    }
}
