using AuthBLL.Helpers;
using AuthBLL.Interfaces;
using AuthBLL.Mappers;
using AuthBLL.Models;
using AuthDAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthBLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IAuthRepository authRepository;

        public TokenService(ITokenRepository tokenRepository, IAuthRepository authRepository)
        {
            this.tokenRepository = tokenRepository;
            this.authRepository = authRepository;
        }

        public async Task<TokenModel> CreateAccessToken(User user)
        {
            var dbUser = await authRepository.GetUserByLoginAsync(user.Login);
            var token = CreateTokenAsync(DateTime.UtcNow.AddMinutes(30), TypeEnum.AccessToken, AuthMapper.Map(dbUser));
            token.Token = EncryptionHelper.Encrypt(token.Token);

            if (await tokenRepository.GetRefreshToken(dbUser.Id) == null)
            {
                var refreshToken = CreateTokenAsync(DateTime.UtcNow.AddHours(12), TypeEnum.RefreshToken, AuthMapper.Map(dbUser));
                await tokenRepository.SaveTokenAsync(AuthMapper.Map(refreshToken));
            }

            return token;
        }

        public async Task<ValidateTokenResult> ValidateAccessToken(string? accessToken)
        {
            if (accessToken == null)
            {
                return CreateValidateTokenResult(false, null);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(EncryptionHelper.Decrypt(accessToken));

            if (securityToken != null) 
            { 
                if (securityToken.ValidTo > DateTime.UtcNow 
                    && securityToken.Claims.First(claim => claim.Type == "Type").Value == TypeEnum.AccessToken.ToString())
                {
                    return CreateValidateTokenResult(true, token: null);
                }

                int userId = int.Parse(securityToken.Claims.First(claim => claim.Type == "UserId").Value);

                var refreshToken = tokenRepository.GetRefreshToken(userId);

                if (refreshToken != null) 
                {
                    var user = await authRepository.GetUserByIdAsync(userId);
                    return CreateValidateTokenResult(true, CreateTokenAsync(DateTime.UtcNow.AddMinutes(30), TypeEnum.AccessToken, AuthMapper.Map(user)));
                }
            }

            return CreateValidateTokenResult(false, token: null);
        }

        private static ValidateTokenResult CreateValidateTokenResult(bool success, TokenModel? token)
        {
            return new ValidateTokenResult
            {
                Success = success,
                Token = token
            };
        }

        private TokenModel CreateTokenAsync(DateTime expires, TypeEnum type, User user)
        {
            DateTime issuedAt = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claimsIdentity = new();

            claimsIdentity.AddClaim(new Claim("UserId", user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim("Type", type.ToString()));

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(GenerateRandomString(128)));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwtToken = tokenHandler.CreateJwtSecurityToken(
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(jwtToken);

            var token = new TokenModel()
            {
                Token = tokenString,
                ExpiresAt = expires,
                Type = type,
                User = user
            };

            return token;
        }

        private static string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
