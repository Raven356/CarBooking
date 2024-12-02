using AuthBLL.Interfaces;
using AuthBLL.Mappers;
using AuthBLL.Models;
using AuthDAL.Interfaces;

namespace AuthBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task EditUserAsync(User user)
        {
            await userRepository.EditUserAsync(AuthMapper.Map(user));
        }

        public async Task<User> GetUserById(int userId)
        {
            return AuthMapper.Map(await userRepository.GetUserById(userId));
        }
    }
}
