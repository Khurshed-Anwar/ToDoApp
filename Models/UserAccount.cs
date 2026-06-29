using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models;

public enum UserRole
{
    Admin,
    User
}

public enum UserStatus
{
    Active,
    Inactive
}

public enum DenyAccessOption
{
    Yes,
    No
}

public class UserAccount
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public UserStatus Status { get; set; } = UserStatus.Active;

    public DenyAccessOption DenyAccess { get; set; } = DenyAccessOption.Yes;

    public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    public DateTime? VerifiedDateTime { get; set; }
}
