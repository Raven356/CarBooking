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
        private readonly PasswordHasher passwordHasher;
        private readonly IServiceProvider serviceProvider;

        public UserRepository(IServiceProvider serviceProvider)
        {
            passwordHasher = new PasswordHasher();
            this.serviceProvider = serviceProvider;
        }

        public async Task EditUserAsync(UserDTO user)
        {
            using var authContext = CreateContext();

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
            using var authContext = CreateContext();

            var user = await authContext.Users.FirstAsync(user => user.Id == userId);

            return user;
        }

        private AuthContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            return authContext;
        }
    }
}
