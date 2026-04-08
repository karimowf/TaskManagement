namespace TaskManagement.ViewModels
{
    public class TaskListViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; } = Enumerable.Empty<TaskViewModel>();
        public string? SortBy { get; set; }       // "priority" | "deadline"
        public string? SortOrder { get; set; }    // "asc" | "desc"
    }
}
