using AuthBLL.Models;

namespace AuthBLL.Interfaces
{
    public interface ITokenService
    {
        public Task<TokenModel> CreateAccessToken(User user);

        public Task<ValidateTokenResult> ValidateAccessToken(string? accessToken);

        int GetUserIdFromToken(string accessToken);
    }
}
