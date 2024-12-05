using AuthBLL.Interfaces;

namespace AuthApi.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/auth") || context.Request.Path.StartsWithSegments("/swagger")
                || context.Request.Path.StartsWithSegments("/api/v1/user")
                || context.Request.Path.StartsWithSegments("/car/GetAll")
                || context.Request.Path.StartsWithSegments("/car/GetAllModels")
                || context.Request.Path.StartsWithSegments("/car/GetAllTypes"))
            {
                await _next(context);
                return;
            }

            var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (accessToken != null)
            {
                var validationResult = await _tokenService.ValidateAccessToken(accessToken);

                if (!validationResult.Success)
                {
                    // Token is invalid or expired, and no refresh is possible
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(new
                    {
                        message = "Invalid or expired token. Please log in again."
                    }.ToString());
                    return;
                }

                if (validationResult.Token != null)
                {
                    // Optionally include a refreshed token in the response headers for app-side storage
                    context.Response.Headers.Add("New-Access-Token", validationResult.Token.Token);
                }
            }
            else
            {
                // No token provided
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new
                {
                    message = "Token missing. Please log in."
                }.ToString());
                return;
            }

            await _next(context);
        }
    }

}
