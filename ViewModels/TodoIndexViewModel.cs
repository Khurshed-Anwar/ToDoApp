using ToDoApp.Models;

namespace ToDoApp.ViewModels;

public class TodoIndexViewModel
{
    public List<TodoItem> Items { get; set; } = new();
    public TodoStatusFilter Status { get; set; } = TodoStatusFilter.All;
    public TodoSortBy Sort { get; set; } = TodoSortBy.DueDate;
    public string? Search { get; set; }
    public string? Message { get; set; }
    public int MessageTimeoutSeconds { get; set; } = 3;
}
