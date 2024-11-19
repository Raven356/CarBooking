using AuthBLL.Models;
using AuthDAL.Models;

namespace AuthBLL.Mappers
{
    public static class AuthMapper
    {
        public static UserDTO Map(User user)
        {
            return new UserDTO
            {
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Id = user.Id,
                IsActive = user.IsActive,
                Login = user.Login,
                Name = user.Name,
                PasswordHash = user.Password,
                Surname = user.Surname,
                Phone = user.Phone,
                Role = (AuthDAL.Models.RolesEnum)user.Role
            };
        }

        public static User Map(UserDTO user)
        {
            return new User
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.PasswordHash,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                IsActive = user.IsActive,
                Name = user.Name,
                Phone = user.Phone,
                Role = (Models.RolesEnum)user.Role,
                Surname = user.Surname
            };
        }

        public static TokenDTO Map(TokenModel token)
        {
            return new TokenDTO
            {
                Id = token.Id,
                Token = token.Token,
                ExpiresAt = token.ExpiresAt,
                Type = (AuthDAL.Models.TypeEnum)token.Type,
                User = Map(token.User),
                UserId = token.User.Id
            };
        }
    }
}
