﻿using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthContext authContext;
        private readonly PasswordHasher passwordHasher;

        public AuthRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            passwordHasher = new PasswordHasher();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            return await authContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<UserDTO?> GetUserByLoginAsync(string login)
        {
            return await authContext.Users.FirstOrDefaultAsync(user => user.Login == login);
        }

        public bool Login(UserDTO userDTO)
        {
            var registeredUser = authContext.Users.FirstOrDefault(user => user.Login == userDTO.Login && user.IsActive);
            if (registeredUser != null) 
            {
                return passwordHasher.VerifyHashedPassword(registeredUser.PasswordHash, userDTO.PasswordHash) == PasswordVerificationResult.Success;
            }

            return false;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void Register(UserDTO userDTO)
        {
            userDTO.PasswordHash = passwordHasher.HashPassword(userDTO.PasswordHash);
            authContext.Users.Add(userDTO);
            authContext.SaveChanges();
        }
    }
}
