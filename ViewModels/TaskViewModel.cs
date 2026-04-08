using TaskManagement.Enums;

namespace TaskManagement.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }

        // Runtime-computed, not stored in DB
        public Enums.TaskStatus Status { get; set; }

        // Validation errors collected manually
        public Dictionary<string, string> ValidationErrors { get; set; } = new();

        public bool IsValid => ValidationErrors.Count == 0;

        public void Validate()
        {
            ValidationErrors.Clear();

            if (string.IsNullOrWhiteSpace(Title))
            {
                ValidationErrors["Title"] = "Title cannot be empty.";
            }

            if (Deadline.HasValue && Deadline.Value.Date < DateTime.Today)
            {
                ValidationErrors["Deadline"] = "Deadline cannot be a past date.";
            }
        }
    }
}
