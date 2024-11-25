using AuthBLL.Interfaces;
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

        public async Task<bool> Login(User user)
        {
            return await authRepository.Login(AuthMapper.Map(user));
        }

        public void Logout()
        {
            authRepository.Logout();
        }

        public async Task Register(User user)
        {
            await authRepository.Register(AuthMapper.Map(user));
        }
    }
}
