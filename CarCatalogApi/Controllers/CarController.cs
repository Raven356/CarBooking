using CarBookingBLL.Interfaces;
using CarCatalogApi.EventPublishers;
using CarCatalogApi.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMqLibrary.Events;
using System.Text.Json;

namespace CarCatalogApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? type, string? fromPrice, string? toPrice, string? model, int? userId)
        {
            var cars = await carService.GetAllAsymc(type, fromPrice, toPrice, model);

            if (userId.HasValue)
            {
                var rpcPublisher = HttpContext.RequestServices.GetRequiredService<CarEventsPubisher>();

                var request = new GetUserOrdersEvent { UserId = userId.Value };

                var response = await rpcPublisher.PublishWithReply(request, "get_user_orders");

                var orders = JsonSerializer.Deserialize<IEnumerable<UserOrderResponse>>(response);

                if (orders != null)
                {
                    // Групуємо замовлення за CarId і підраховуємо кількість
                    var carOrderCounts = orders
                        .GroupBy(o => o.CarId)
                        .ToDictionary(g => g.Key, g => g.Count());

                    // Сортуємо машини: спочатку ті, що найчастіше замовлялися, далі - за іншими критеріями
                    cars = cars
                        .OrderByDescending(c => carOrderCounts.GetValueOrDefault(c.Id, 0)) // Замовлення
                        .ThenBy(c => c.CarType.Type == type)                                    // Тип
                        .ThenBy(c => c.Model.Model == model)                                    // Модель
                        .ThenBy(c => c.RentPrice >= double.Parse(fromPrice ?? "0") &&
                                    c.RentPrice <= double.Parse(toPrice ?? double.MaxValue.ToString())) // Ціна
                        .ToList();
                }
            }

            return Json(cars);
        }

        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await carService.GetAllTypesAsync();

            return Json(types);
        }

        [HttpGet("GetAllModels")]
        public async Task<IActionResult> GetAllModels()
        {
            var models = await carService.GetAllModelsAsync();

            return Json(models);
        }
    }
}
