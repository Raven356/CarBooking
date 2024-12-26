using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PasswordHasher passwordHasher;
        private readonly IServiceProvider serviceProvider;

        public AuthRepository(IServiceProvider serviceProvider)
        {
            passwordHasher = new PasswordHasher();
            this.serviceProvider = serviceProvider;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            using var authContext = CreateContext();
            return await authContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<UserDTO?> GetUserByLoginAsync(string login)
        {
            using var authContext = CreateContext();
            return await authContext.Users.FirstOrDefaultAsync(user => user.Login == login);
        }

        public async Task<bool> Login(UserDTO userDTO)
        {
            using var authContext = CreateContext();
            var registeredUser = await authContext.Users.FirstOrDefaultAsync(user => user.Login == userDTO.Login && user.IsActive);
            if (registeredUser != null) 
            {
                return passwordHasher.VerifyHashedPassword(registeredUser.PasswordHash, userDTO.PasswordHash) == PasswordVerificationResult.Success;
            }

            return false;
        }

        public async Task Logout(int userId)
        {
            using var authContext = CreateContext();
            var existingToken = await authContext.Tokens.FirstOrDefaultAsync(token => token.UserId == userId);

            if (existingToken != null)
            {
                authContext.Tokens.Remove(existingToken);
            }
        }

        public async Task Register(UserDTO userDTO)
        {
            using var authContext = CreateContext();
            userDTO.PasswordHash = passwordHasher.HashPassword(userDTO.PasswordHash);
            authContext.Users.Add(userDTO);
            await authContext.SaveChangesAsync();
        }

        private AuthContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            return authContext;
        }
    }
}
