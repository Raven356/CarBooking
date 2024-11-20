using AuthDAL.Models;
using Microsoft.AspNet.Identity;

namespace AuthDAL.Context
{
    public class AuthContextSeeder
    {
        public static void SeedData(AuthContext context)
        {
            //context.Database.Migrate();

            // Check if data already exists

            if (!context.Users.Any()) 
            {
                context.Users.Add(new UserDTO { IsActive = true,
                    Login = "admin",
                    Email = "admin@gmail.com",
                    Name = "Admin",
                    PasswordHash = new PasswordHasher().HashPassword("admin"),
                    Surname = "Admin",
                    DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                    Phone = "+380000",
                    Role = RolesEnum.Admin});

                context.SaveChanges();
            }
        }
    }
}
