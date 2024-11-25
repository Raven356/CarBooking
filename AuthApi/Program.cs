using AuthDAL.Context;
using Ocelot.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthApi.Middleware;
using Ocelot.Middleware;

namespace AuthApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication()
                .AddJwtBearer("AuthKey", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false, // Allow expired tokens (handled by middleware)
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "car-booking-auth-service",
                        ValidAudience = "car-booking-mobile-app",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sooZ5a6Zj2mAOEXQaNmrKojwTwKYxfLH"))
                    };
                });

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile("ocelot.Development.json", optional: false, reloadOnChange: true);

            builder.Services.AddOcelot(builder.Configuration);

            AuthBLL.Setup.SetupBLLServices(builder.Services, builder.Configuration["ConnectionString"]);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AuthContext>();

                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Seed data
                    AuthBLL.Setup.SeedDatabase(context);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
                }
            }

            app.UseMiddleware<TokenValidationMiddleware>();

            app.MapWhen(
                ctx => !ctx.Request.Path.StartsWithSegments("/swagger") && !ctx.Request.Path.StartsWithSegments("/api/v1/Auth"), 
                app => app.UseOcelot().Wait()
            );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
