using ECommerceAfaq.Application.DTOs.WishList;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<WishListItemResponseDto> AddAsync(string userId, int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product is null)
                throw new ArgumentException("product does not exist");

            var alearyExist = await _unitOfWork.Repository<WishListItem>()
                .ExistesAsync(w => w.UserId == userId && w.ProductId == productId);

            if (alearyExist)
                throw new ArgumentException("product is already in your wishlist");

            var item = new WishListItem
            {
                UserId = userId,
                ProductId = productId
            };

            await _unitOfWork.Repository<WishListItem>() .AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return new WishListItemResponseDto
            {
                Id = item.Id,
                ProductId = productId,
                ProductName = product.Name,
                ProductImageUrl = product.ImageUrl!,
                ProductPrice = product.Price,
                IsInStock = product.Stock > 0 && product.IsActive,
                AddedAt = product.CreatedAt
            };
        }

        public async Task<List<WishListItemResponseDto>> GetWishListAsync(string userId)
        {
            var items = await _unitOfWork.Repository<WishListItem>().FindAsync(w => w.UserId == userId);

            var result = new List<WishListItemResponseDto>();

            // N + 1
            foreach (var item in items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product is null)
                    continue;

                result.Add(new WishListItemResponseDto
                {
                    Id = item.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImageUrl = product.ImageUrl!,
                    ProductPrice = product.Price,
                    IsInStock = product.Stock > 0 && product.IsActive, 
                    AddedAt = item.CreatedAt,

                });

            }
            return result;
        }

        public async Task<bool> IsInWishListAsync(string userId, int productId)
        {
           return await _unitOfWork.Repository<WishListItem>()
                    .ExistesAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task<bool> RemoveAsync(string userId, int productId)
        {

            var items = await _unitOfWork.Repository<WishListItem>()
                .FindAsync(w => w.UserId == userId && w.ProductId == productId); 
            var item = items.FirstOrDefault();

            if (item is null)
                return false;

            _unitOfWork.Repository<WishListItem>().DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return true;


        }
    }
}
