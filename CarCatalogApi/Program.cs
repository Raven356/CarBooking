using CarBookingDAL.Context;
using CarCatalogApi.EventConsumers;
using CarCatalogApi.EventPublishers;
using RabbitMqLibrary.TimedRoutine;

namespace CarCatalogApi
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

            builder.Services.AddSingleton<TimedEventHistoryPublisher>()
                .AddSingleton<CarEventsPubisher>();

            RegisterEventConsumers(builder.Services);

            CarBookingBLL.Setup.SetupBLLServices(builder.Services, builder.Configuration["ConnectionString"]);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CarCatalogContext>();

                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Seed data
                    CarBookingBLL.Setup.SeedDatabase(context);
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

        private static void RegisterEventConsumers(IServiceCollection services)
        {
            services.AddHostedService<OrderStartedEventConsumer>()
                .AddHostedService<CarRpcConsumer>()
                .AddHostedService<EndOrderEventConsumer>();
        }
    }
}
