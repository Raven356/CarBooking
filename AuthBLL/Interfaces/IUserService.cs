using AuthBLL.Models;

namespace AuthBLL.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);

        Task EditUserAsync(User user);
    }
}
