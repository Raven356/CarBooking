using AuthBLL.Models;

namespace AuthBLL.Interfaces
{
    public interface IAuthService
    {
        public Task Register(User user);

        public Task<bool> Login(User user);

        public Task Logout(int userId);
    }
}
