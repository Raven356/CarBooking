using TimedEventsApi.EventConsumers;

namespace TimedEventsApi
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

            builder.Services.AddHostedService<AddTimedEventHistoryConsumer>()
                .AddHostedService<MakeTimedEventNotHandledConsumer>()
                .AddHostedService<GetTimedEventRpcConsumer>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
