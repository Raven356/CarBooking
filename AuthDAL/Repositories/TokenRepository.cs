using AuthDAL.Context;
using AuthDAL.Interfaces;
using AuthDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDAL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IServiceProvider serviceProvider;

        public TokenRepository(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TokenDTO?> GetRefreshTokenAsync(int userId)
        {
            using var authContext = CreateContext();

            return await authContext.Tokens.FirstOrDefaultAsync(token => token.UserId == userId
                && token.ExpiresAt > DateTime.UtcNow
                && token.Type == TypeEnum.RefreshToken);
        }

        public async Task<TokenDTO> SaveTokenAsync(TokenDTO token)
        {
            using var authContext = CreateContext();

            authContext.Attach(token.User);
            authContext.Tokens.Add(token);
            await authContext.SaveChangesAsync();
            return token;
        }

        private AuthContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            return authContext;
        }
    }
}
