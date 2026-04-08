using TaskManagement.ViewModels;

namespace TaskManagement.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskListViewModel> GetAllAsync(string? sortBy, string? sortOrder);
        Task<TaskViewModel?> GetByIdAsync(int id);
        Task CreateAsync(TaskViewModel viewModel);
        Task UpdateAsync(TaskViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
