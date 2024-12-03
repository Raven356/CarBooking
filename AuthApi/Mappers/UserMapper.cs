using AuthApi.Models;
using AuthBLL.Models;

namespace AuthApi.Mappers
{
    public static class UserMapper
    {
        public static UserDetailsResponseModel Map(User user)
        {
            return new UserDetailsResponseModel
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Surname = user.Surname
            };
        }

        public static User Map(EditUserRequest request)
        {
            return new User
            {
                Id = request.Id,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(request.DateOfBirth)),
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                Phone = request.Phone,
                Surname = request.Surname
            };
        }
    }
}
