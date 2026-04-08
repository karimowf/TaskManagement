using TaskManagement.Enums;
using TaskManagement.Models;
using TaskManagement.Repositories.Interfaces;
using TaskManagement.Services.Interfaces;
using TaskManagement.ViewModels;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskListViewModel> GetAllAsync(string? sortBy, string? sortOrder)
        {
            var tasks = await _repository.GetAllAsync();

            var viewModels = tasks.Select(MapToViewModel).ToList();

            viewModels = ApplySorting(viewModels, sortBy, sortOrder);

            return new TaskListViewModel
            {
                Tasks = viewModels,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
        }

        public async Task<TaskViewModel?> GetByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            return task is null ? null : MapToViewModel(task);
        }

        public async Task CreateAsync(TaskViewModel viewModel)
        {
            var task = MapToModel(viewModel);
            await _repository.AddAsync(task);
        }

        public async Task UpdateAsync(TaskViewModel viewModel)
        {
            var task = MapToModel(viewModel);
            await _repository.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

     
        private static Enums.TaskStatus ComputeStatus(TaskItem task)
        {
            if (task.IsCompleted)
                return Enums.TaskStatus.Done;

            if (task.Deadline.HasValue)
            {
                var now = DateTime.UtcNow;

                if (task.Deadline.Value < now)
                    return Enums.TaskStatus.Overdue;

                if (task.Deadline.Value <= now.AddHours(24))
                    return Enums.TaskStatus.Urgent;
            }

            return Enums.TaskStatus.Active;
        }

       
        private static List<TaskViewModel> ApplySorting(
            List<TaskViewModel> tasks,
            string? sortBy,
            string? sortOrder)
        {
            bool descending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            return sortBy?.ToLower() switch
            {
                "priority" => descending
                    ? tasks.OrderByDescending(t => (int)t.Priority).ToList()
                    : tasks.OrderBy(t => (int)t.Priority).ToList(),

                "deadline" => descending
                    ? tasks.OrderByDescending(t => t.Deadline ?? DateTime.MaxValue).ToList()
                    : tasks.OrderBy(t => t.Deadline ?? DateTime.MaxValue).ToList(),

                _ => tasks
            };
        }

      
        private static TaskViewModel MapToViewModel(TaskItem task)
        {
            return new TaskViewModel
            {
                Id          = task.Id,
                Title       = task.Title,
                Description = task.Description,
                Priority    = task.Priority,
                Deadline    = task.Deadline,
                IsCompleted = task.IsCompleted,
                Status      = ComputeStatus(task)
            };
        }

        private static TaskItem MapToModel(TaskViewModel vm)
        {
            return new TaskItem
            {
                Id          = vm.Id,
                Title       = vm.Title.Trim(),
                Description = vm.Description?.Trim() ?? string.Empty,
                Priority    = vm.Priority,
                Deadline    = vm.Deadline,
                IsCompleted = vm.IsCompleted
            };
        }
    }
}
