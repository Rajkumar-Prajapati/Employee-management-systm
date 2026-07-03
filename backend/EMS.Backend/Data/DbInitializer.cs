using EMS.Backend.Models;

namespace EMS.Backend.Data
{
    public static class DbInitializer
    {
        // Creates the database (if needed) and seeds a default admin user.
        // Default login -> username: admin | password: Admin@123
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    Email = "admin@ems.com",
                    Role = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
                };
                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
