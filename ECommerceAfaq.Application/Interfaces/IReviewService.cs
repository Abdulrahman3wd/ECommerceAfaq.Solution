using ECommerceAfaq.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ProductRatingSummaryDto> GetProductReviewAsync(int productId);
        Task<ReviewResponseDto> AddAsync(string userId, string userFullName, ReviewCreateDto reviewCreateDto);
        Task<bool> DeleteAsync(string userId, int reviewId); 
    }
}
