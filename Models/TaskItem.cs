using TaskManagement.Enums;

namespace TaskManagement.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }
}
