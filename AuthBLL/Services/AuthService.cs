﻿using AuthBLL.Interfaces;
using AuthBLL.Mappers;
using AuthBLL.Models;
using AuthDAL.Interfaces;

namespace AuthBLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;

        public AuthService(IAuthRepository authRepository) 
        {
            this.authRepository = authRepository;
        }

        public bool Login(User user)
        {
            return authRepository.Login(AuthMapper.Map(user));
        }

        public void Logout()
        {
            authRepository.Logout();
        }

        public void Register(User user)
        {
            authRepository.Register(AuthMapper.Map(user));
        }
    }
}
