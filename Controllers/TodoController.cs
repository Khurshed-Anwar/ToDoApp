using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers;

public class TodoController : Controller
{
    private readonly TodoDbContext _context;
    private readonly int _messageTimeoutSeconds;

    public TodoController(TodoDbContext context, IConfiguration configuration)
    {
        _context = context;
        _messageTimeoutSeconds = configuration.GetValue<int>("AppSettings:MessageTimeoutSeconds", 3);
    }

    public IActionResult Index(TodoStatusFilter status = TodoStatusFilter.All, string? search = null, TodoSortBy sort = TodoSortBy.DueDate, string? message = null)
    {
        IQueryable<TodoItem> query = _context.TodoItems;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Task.Contains(search));
        }

        query = status switch
        {
            TodoStatusFilter.Active => query.Where(t => !t.IsCompleted),
            TodoStatusFilter.Completed => query.Where(t => t.IsCompleted),
            _ => query
        };

        query = sort switch
        {
            TodoSortBy.Priority => query.OrderByDescending(t => t.Priority).ThenBy(t => t.DueDate),
            TodoSortBy.Created => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate)
        };

        var model = new TodoIndexViewModel
        {
            Items = query.ToList(),
            Status = status,
            Sort = sort,
            Search = search,
            Message = message,
            MessageTimeoutSeconds = _messageTimeoutSeconds
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new TodoItem { DueDate = DateTime.Today });
    }

    [HttpPost]
    public IActionResult Create(TodoItem todoItem)
    {
        if (!ModelState.IsValid)
        {
            return View(todoItem);
        }

        todoItem.CreatedAt = DateTime.Now;
        _context.TodoItems.Add(todoItem);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index), new { message = "Task created successfully." });
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoItem = _context.TodoItems.Find(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return View(todoItem);
    }

    [HttpPost]
    public IActionResult Edit(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(todoItem);
        }

        _context.TodoItems.Update(todoItem);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index), new { message = "Task updated successfully." });
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoItem = _context.TodoItems.FirstOrDefault(t => t.Id == id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return View(todoItem);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var todoItem = _context.TodoItems.Find(id);
        if (todoItem != null)
        {
            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index), new { message = "Task deleted successfully." });
    }

    [HttpPost]
    public IActionResult ToggleComplete(int id)
    {
        var todoItem = _context.TodoItems.Find(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.IsCompleted = !todoItem.IsCompleted;
        _context.SaveChanges();

        var message = todoItem.IsCompleted ? "Task marked as completed." : "Task marked as active.";
        return RedirectToAction(nameof(Index), new { message });
    }
}
