using CarBookingBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApi.Controllers
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
        public IActionResult GetAll()
        {
            var cars = carService.GetAll().ToList();

            return Json(cars);
        }
    }
}
