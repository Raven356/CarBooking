using AuthDAL.Models;

namespace AuthDAL.Interfaces
{
    public interface IAuthRepository
    {
        public Task Register(UserDTO userDTO);

        public Task<bool> Login(UserDTO userDTO);

        public void Logout();

        public Task<UserDTO?> GetUserByIdAsync(int id);

        public Task<UserDTO?> GetUserByLoginAsync(string login);
    }
}
