using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task is required.")]
        [StringLength(200, ErrorMessage = "Task cannot exceed 200 characters.")]
        [Display(Name = "Task")]
        public string Task { get; set; } = string.Empty;

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; } = false;

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Priority")]
        public Priority Priority { get; set; } = Priority.Medium;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
