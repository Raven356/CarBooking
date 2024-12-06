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
        private readonly ILogger<ReviewController> logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            this.reviewService = reviewService;
            this.logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                logger.LogInformation("Started getting reviews");
                var reviews = await reviewService.GetAllAsync();

                logger.LogInformation("Reviews were successfully retrieved");
                return Json(reviews);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while getting reviews: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int reviewId)
        {
            try
            {
                logger.LogInformation($"Getting review by id: {reviewId}");
                var review = await reviewService.GetByIdAsync(reviewId);

                return Json(review);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened while getting review by id: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            try
            {
                logger.LogInformation($"Getting reviews by userId: {userId}");
                var reviews = await reviewService.GetByUserIdAsync(userId);

                return Json(new ReviewsCollectionResponse { Reviews = reviews });
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while getting reviews by userId: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetByOrderId")]
        public async Task<IActionResult> GetByOrderId([FromQuery] int orderId)
        {
            try
            {
                logger.LogInformation($"Getting review by orderId: {orderId}");
                var order = await reviewService.GetByOrderIdAsync(orderId);

                return Json(order);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while getting review by orderId: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateReviewModel createReviewModel)
        {
            try
            {
                logger.LogInformation("Creating review");
                var error = ValidateReviewMode(createReviewModel);
                if (error != null)
                {
                    logger.LogInformation("Create review request was not in a correct format");
                    return BadRequest(error);
                }

                await reviewService.CreateReviewAsync(Mappers.ReviewMapper.Map(createReviewModel));

                logger.LogInformation("Review created");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened while creating review: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateReviewModel updateReviewModel)
        {
            try
            {
                logger.LogInformation($"Updating review, reviewId: {updateReviewModel.Id}");
                var error = ValidateReviewMode(updateReviewModel);
                if (error != null)
                {
                    logger.LogInformation("Update review request was not in a correct format");
                    return BadRequest(error);
                }

                await reviewService.UpdateReviewAsync(Mappers.ReviewMapper.Map(updateReviewModel));

                logger.LogInformation("Review updated successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened while updating review: {ex.Message}");
                return BadRequest(ex);
            }
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
