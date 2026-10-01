using ECommerceAfaq.Application.DTOs.Review;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductRatingSummaryDto> GetProductReviewAsync(int productId)
        {
            var reviews = await _unitOfWork.Repository<Review>().FindAsync(r => r.ProductId == productId);
            var reviewList = reviews.OrderByDescending(r => r.CreatedAt).ToList();

            return new ProductRatingSummaryDto
            {
                AverageRating = reviewList.Count == 0 ? 0 : Math.Round(reviewList.Average(r => r.Rating), 1),
                TotalReviews = reviewList.Count,
                Reviews =  reviewList.Select(MapToResponseDto).ToList()

            };


        }

        public async Task<ReviewResponseDto> AddAsync(string userId, string userFullName, ReviewCreateDto reviewCreateDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(reviewCreateDto.ProductId);

            if (product is null)
                throw new ArgumentException("product does nor exist");

            var alreadyReviewed = await _unitOfWork.Repository<Review>()
                .ExistesAsync(r => r.UserId == userId && r.ProductId == reviewCreateDto.ProductId);

            if (alreadyReviewed)
                throw new ArgumentException("you have already reviewed this product");

            // Check purchase using OrderItems to ensure we detect purchased products even when Order.OrderItems navigation is not populated
            var hasPurchased = await _unitOfWork.Repository<OrderItem>()
                .ExistesAsync(oi => oi.ProductId == reviewCreateDto.ProductId && oi.Order.UserId == userId && oi.Order.OrderStatus != OrderStatus.Canceled);

            if (!hasPurchased)
                throw new ArgumentException("you can only review products you have purchased");

            var review = new Review
            {
                UserId = userId,
                UserFullName = userFullName,
                ProductId = reviewCreateDto.ProductId,
                Rating = reviewCreateDto.Rating,
                Comment = reviewCreateDto.Comment
            };

            await _unitOfWork.Repository<Review>().AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(review);

        }

        public async Task<bool> DeleteAsync(string userId, int reviewId)
        {
            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(reviewId);

            if (review is null || review.UserId == userId)
                return false;

            _unitOfWork.Repository<Review>().DeleteAsync(review);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        private static ReviewResponseDto MapToResponseDto(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                UserFullName = review.UserFullName,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

    }
}
