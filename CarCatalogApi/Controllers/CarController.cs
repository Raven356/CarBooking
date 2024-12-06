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
        private readonly ILogger<CarController> logger;

        public CarController(ICarService carService, ILogger<CarController> logger)
        {
            this.carService = carService;
            this.logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? type, string? fromPrice, string? toPrice, string? model, int? userId)
        {
            try
            {
                logger.LogInformation("Started getting cars");
                var cars = await carService.GetAllAsync(type, fromPrice, toPrice, model);

                if (userId.HasValue)
                {
                    var rpcPublisher = HttpContext.RequestServices.GetRequiredService<CarEventsPubisher>();

                    var request = new GetUserOrdersEvent { UserId = userId.Value };

                    var response = await rpcPublisher.PublishWithReply(request, "get_user_orders");

                    var orders = JsonSerializer.Deserialize<IEnumerable<UserOrderResponse>>(response);

                    if (orders != null)
                    {
                        logger.LogInformation("Sorting cars");
                        var carOrderCounts = orders
                            .GroupBy(o => o.CarId)
                            .ToDictionary(g => g.Key, g => g.Count());

                        cars = cars
                            .OrderByDescending(c => carOrderCounts.GetValueOrDefault(c.Id, 0))
                            .ThenBy(c => c.CarType.Type == type)
                            .ThenBy(c => c.Model.Model == model)
                            .ThenBy(c => c.RentPrice >= double.Parse(fromPrice ?? "0") &&
                                        c.RentPrice <= double.Parse(toPrice ?? double.MaxValue.ToString()))
                            .ToList();
                    }
                }

                return Json(cars);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error hapenned when getting cars, error: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                logger.LogInformation("Getting car types");
                var types = await carService.GetAllTypesAsync();

                return Json(types);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened during getting car types, error: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllModels")]
        public async Task<IActionResult> GetAllModels()
        {
            try
            {
                logger.LogInformation("Getting car models");
                var models = await carService.GetAllModelsAsync();

                return Json(models);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened during getting car models, error: {ex.Message}");
                return BadRequest(ex);
            }
        }
    }
}
