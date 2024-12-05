using Microsoft.AspNetCore.Mvc;
using ReviewApi.Models;
using ReviewBLL.Interfaces;

namespace ReviewApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await reviewService.GetAllAsync();

            return Json(reviews);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int reviewId)
        {
            var review = await reviewService.GetByIdAsync(reviewId);

            return Json(review);
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            var reviews = await reviewService.GetByUserIdAsync(userId);

            return Json(new ReviewsCollectionResponse { Reviews = reviews });
        }

        [HttpGet("GetByOrderId")]
        public async Task<IActionResult> GetByOrderId([FromQuery] int orderId)
        {
            var order = await reviewService.GetByOrderIdAsync(orderId);

            return Json(order);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateReviewModel createReviewModel)
        {
            var error = ValidateReviewMode(createReviewModel);
            if (error != null)
            {
                return BadRequest(error);    
            }

            await reviewService.CreateReviewAsync(Mappers.ReviewMapper.Map(createReviewModel));

            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateReviewModel updateReviewModel)
        {
            var error = ValidateReviewMode(updateReviewModel);
            if (error != null)
            {
                return BadRequest(error);
            }

            await reviewService.UpdateReviewAsync(Mappers.ReviewMapper.Map(updateReviewModel));

            return Ok();
        }

        private string? ValidateReviewMode(ReviewBaseModel reviewBaseModel)
        {
            if (reviewBaseModel == null)
            {
                return "Input model was empty!";
            }

            if (reviewBaseModel.Rating < 0 || reviewBaseModel.Rating > 5)
            {
                return "Rating should be in range from 0 to 5";
            }

            return null;
        }
    }
}
