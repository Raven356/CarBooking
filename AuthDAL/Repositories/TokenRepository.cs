using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDAL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthContext authContext;

        public TokenRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
        }

        public async Task<TokenDTO?> GetRefreshTokenAsync(int userId)
        {
            return await authContext.Tokens.FirstOrDefaultAsync(token => token.UserId == userId
                && token.ExpiresAt > DateTime.UtcNow
                && token.Type == TypeEnum.RefreshToken);
        }

        public async Task<TokenDTO> SaveTokenAsync(TokenDTO token)
        {
            authContext.Attach(token.User);
            authContext.Tokens.Add(token);
            await authContext.SaveChangesAsync();
            return token;
        }
    }
}
