using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AuthContext authContext;
        private readonly PasswordHasher passwordHasher;

        public UserRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            passwordHasher = new PasswordHasher();
        }

        public async Task EditUserAsync(UserDTO user)
        {
            var existingUser = await authContext.Users.FirstAsync(u => u.Id == user.Id);

            if (!string.IsNullOrEmpty(user.Surname))
            {
                existingUser.Surname = user.Surname;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                existingUser.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.Phone)) 
            { 
                existingUser.Phone = user.Phone; 
            }

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = passwordHasher.HashPassword(user.PasswordHash);
            }

            if (user.DateOfBirth != DateOnly.FromDateTime(DateTime.Now))
            {
                existingUser.DateOfBirth = user.DateOfBirth;
            }

            if (!string.IsNullOrEmpty(user.Name))
            {
                existingUser.Name = user.Name;
            }

            authContext.Update(existingUser);

            await authContext.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await authContext.Users.FirstAsync(user => user.Id == userId);

            return user;
        }
    }
}
