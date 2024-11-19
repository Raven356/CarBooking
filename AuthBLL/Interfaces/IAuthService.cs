using AuthBLL.Models;

namespace AuthBLL.Interfaces
{
    public interface IAuthService
    {
        public void Register(User user);

        public bool Login(User user);

        public void Logout();
    }
}
