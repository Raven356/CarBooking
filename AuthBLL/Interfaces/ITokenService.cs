using AuthBLL.Models;

namespace AuthBLL.Interfaces
{
    public interface ITokenService
    {
        Task<TokenModel> CreateAccessToken(User user);

        Task<ValidateTokenResult> ValidateAccessToken(string? accessToken);

        int GetUserIdFromToken(string accessToken);
    }
}
