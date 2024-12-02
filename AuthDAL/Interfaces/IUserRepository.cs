using AuthDAL.Models;

namespace AuthDAL.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserById(int userId);

        Task EditUserAsync(UserDTO user);
    }
}
