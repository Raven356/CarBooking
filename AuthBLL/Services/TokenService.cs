using AuthBLL.Interfaces;
using AuthBLL.Mappers;
using AuthBLL.Models;
using AuthDAL.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TokenService> logger;

        public TokenService(ITokenRepository tokenRepository, IAuthRepository authRepository, ILogger<TokenService> logger)
        {
            this.tokenRepository = tokenRepository;
            this.authRepository = authRepository;
            this.logger = logger;
        }

        public async Task<TokenModel> CreateAccessToken(User user)
        {
            logger.LogInformation("Starting creating new access token");

            var dbUser = await authRepository.GetUserByLoginAsync(user.Login);
            var token = CreateTokenAsync(DateTime.UtcNow.AddMinutes(30), TypeEnum.AccessToken, AuthMapper.Map(dbUser));

            if (await tokenRepository.GetRefreshTokenAsync(dbUser.Id) == null)
            {
                logger.LogInformation("Refresh token was null, creating new one!");

                var refreshToken = CreateTokenAsync(DateTime.UtcNow.AddHours(12), TypeEnum.RefreshToken, AuthMapper.Map(dbUser));
                await tokenRepository.SaveTokenAsync(AuthMapper.Map(refreshToken));
            }

            logger.LogInformation("Access token created successfully");

            return token;
        }

        public int GetUserIdFromToken(string accessToken)
        {
            logger.LogInformation("Reading access token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(accessToken);

            int userId = int.Parse(securityToken.Claims.First(claim => claim.Type == "UserId").Value);

            logger.LogInformation($"Access token read successfully, userId: {userId}");

            return userId;
        }

        public async Task<ValidateTokenResult> ValidateAccessToken(string? accessToken)
        {
            logger.LogInformation("Validating access token");
            if (accessToken == null)
            {
                logger.LogInformation("Access token was empty");
                return CreateValidateTokenResult(false, null);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(accessToken);

            if (securityToken != null) 
            { 
                if (securityToken.ValidTo > DateTime.UtcNow 
                    && securityToken.Claims.First(claim => claim.Type == "Type").Value == TypeEnum.AccessToken.ToString())
                {
                    logger.LogInformation("Token correct!");
                    return CreateValidateTokenResult(true, token: null);
                }

                logger.LogInformation("Trying to refresh access token");

                int userId = int.Parse(securityToken.Claims.First(claim => claim.Type == "UserId").Value);

                var refreshToken = await tokenRepository.GetRefreshTokenAsync(userId);

                if (refreshToken != null) 
                {
                    logger.LogInformation("Refreshing access token");
                    var user = await authRepository.GetUserByIdAsync(userId);
                    return CreateValidateTokenResult(true, CreateTokenAsync(DateTime.UtcNow.AddMinutes(30), TypeEnum.AccessToken, AuthMapper.Map(user)));
                }
            }

            logger.LogInformation("Validation failed!");

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
            logger.LogInformation("Creating new access token");
            DateTime issuedAt = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = new ClaimsIdentity(
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim("Type", type.ToString())
            ]);

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

            logger.LogInformation("Token created successfully!");

            return token;
        }
    }
}
