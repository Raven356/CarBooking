using AuthApi.Models;

namespace AuthApi.Mappers
{
    public static class AuthMapper
    {
        public static AuthBLL.Models.User Map(UserModel model)
        {
            return new AuthBLL.Models.User
            {
                Email = model.Email,
                Login = model.Login,
                Name = model.Name,
                Password = model.Password,
                Role = (AuthBLL.Models.RolesEnum)model.Role,
                Surname = model.Surname,
                DateOfBirth = model.DateOfBirth,
                Id = model.Id,
                IsActive = model.IsActive,
                Phone = model.Phone
            };
        }

        public static AuthBLL.Models.User Map(UserLoginModel user)
        {
            return new AuthBLL.Models.User
            {
                Login = user.Login,
                Password = user.Password,
            };
        }
    }
}
