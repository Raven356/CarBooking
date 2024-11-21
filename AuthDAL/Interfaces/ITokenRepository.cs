using AuthDAL.Models;

namespace AuthDAL.Interfaces
{
    public interface ITokenRepository
    {
        public Task<TokenDTO> SaveTokenAsync(TokenDTO token);

        public Task<TokenDTO?> GetRefreshTokenAsync(int userId);
    }
}
