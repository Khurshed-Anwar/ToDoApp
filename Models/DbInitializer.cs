using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models;

public static class DbInitializer
{
    public static void SeedDefaultAdmin(TodoDbContext context)
    {
        context.Database.Migrate();

        var admin = context.UserAccounts.FirstOrDefault(x => x.UserId == "admin");
        if (admin is null)
        {
            context.UserAccounts.Add(new UserAccount
            {
                FirstName = "admin",
                LastName = "1",
                UserId = "admin",
                Password = PasswordHasher.ToMd5("admin"),
                Role = UserRole.Admin,
                Status = UserStatus.Active,
                DenyAccess = DenyAccessOption.No,
                CreatedDateTime = DateTime.Now,
                VerifiedDateTime = DateTime.Now
            });

            context.SaveChanges();
            return;
        }

        admin.FirstName = "admin";
        admin.LastName = "1";
        admin.Role = UserRole.Admin;
        admin.Status = UserStatus.Active;
        admin.DenyAccess = DenyAccessOption.No;
        admin.Password = PasswordHasher.ToMd5("admin");
        admin.VerifiedDateTime ??= DateTime.Now;
        context.SaveChanges();
    }
}
