using ECommerceAfaq.Application.DTOs.Review;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ICurrentUserService _currentUserService;

        public ReviewController(IReviewService reviewService , ICurrentUserService currentUserService)
        {
            _reviewService = reviewService;
            _currentUserService = currentUserService;
        }

        [HttpGet("products/{productId:int}/reviewes")]
        public async Task<ActionResult<ProductRatingSummaryDto>> GetProductReviews(int productId)
        {
            var reviews = await _reviewService.GetProductReviewAsync(productId);
            return Ok(reviews);
        }

        [HttpPost("reviews")]
        [Authorize]
        public async Task<ActionResult<ReviewResponseDto>> Add(ReviewCreateDto dto)
        {
            try
            {
                var review = await _reviewService.AddAsync(_currentUserService.UserId!, _currentUserService.FullName!, dto);
                return Ok(review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("reviews/{id:int}")]
        [Authorize]

        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reviewService.DeleteAsync(_currentUserService.UserId!, id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
