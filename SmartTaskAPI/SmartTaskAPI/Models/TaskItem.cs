using SmartTaskAPI.Models.Enums;
namespace SmartTaskAPI.Models;

public class TaskItem
    {
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public Status Status { get; set; } // Offen, In Bearbeitung, Erledigt / Open, InProgress, Done
        public Priority Priority { get; set; }  // Niedrig, Mittel, Hoch
    }