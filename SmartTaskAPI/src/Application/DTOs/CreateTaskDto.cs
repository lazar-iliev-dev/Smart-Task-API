using Domain.Entities.Enums;

namespace Application.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
}