using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "User Id")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? Message { get; set; }
}
