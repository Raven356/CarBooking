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

        public static AuthBLL.Models.User Map(UserRegisterModel model)
        {
            return new AuthBLL.Models.User
            {
                Login = model.Login,
                Password = model.Password,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                IsActive = true,
                Name = model.Name,
                Phone = model.Phone,
                Role = AuthBLL.Models.RolesEnum.Customer,
                Surname = model.Surname
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
