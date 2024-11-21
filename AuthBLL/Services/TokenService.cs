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

            if (await tokenRepository.GetRefreshTokenAsync(dbUser.Id) == null)
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

                var refreshToken = await tokenRepository.GetRefreshTokenAsync(userId);

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
            var claimsIdentity = new ClaimsIdentity(
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim("Type", type.ToString())
            ]);

            // Use a fixed, secure key stored in configuration or environment variables
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sooZ5a6Zj2mAOEXQaNmrKojwTwKYxfLH"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create the token
            var jwtToken = tokenHandler.CreateJwtSecurityToken(
                subject: claimsIdentity,
                notBefore: issuedAt,
                expires: expires,
                signingCredentials: signingCredentials,
                issuer: "car-booking-auth-service",
                audience: "car-booking-mobile-app");

            var tokenString = tokenHandler.WriteToken(jwtToken);

            // Return the token model
            var token = new TokenModel()
            {
                Token = tokenString,
                ExpiresAt = expires,
                Type = type,
                User = user
            };

            return token;
        }
    }
}
