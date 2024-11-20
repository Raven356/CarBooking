
using ReviewDAL.Context;

namespace ReviewApi
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

            ReviewBLL.Setup.SetupBLLServices(builder.Services, builder.Configuration["ConnectionString"]);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ReviewContext>();

                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Seed data
                    ReviewBLL.Setup.SeedDatabase(context);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
