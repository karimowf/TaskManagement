using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services.Interfaces;
using TaskManagement.ViewModels;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? sortBy, string? sortOrder)
        {
            var model = await _taskService.GetAllAsync(sortBy, sortOrder);
            return View(model);
        }

        // GET: /Task/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task is null)
                return NotFound();

            return View(task);
        }

        // GET: /Task/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TaskViewModel());
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel viewModel)
        {
            viewModel.Validate();

            if (!viewModel.IsValid)
                return View(viewModel);

            await _taskService.CreateAsync(viewModel);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Task/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task is null)
                return NotFound();

            return View(task);
        }

        // POST: /Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            viewModel.Validate();

            if (!viewModel.IsValid)
                return View(viewModel);

            await _taskService.UpdateAsync(viewModel);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
