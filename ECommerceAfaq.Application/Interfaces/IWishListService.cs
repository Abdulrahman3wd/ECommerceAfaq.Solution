using ECommerceAfaq.Application.DTOs.WishList;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IWishListService
    {
        Task<List<WishListItemResponseDto>> GetWishListAsync(string userId);
        Task<WishListItemResponseDto> AddAsync(string userId , int productId);
        Task<bool> RemoveAsync(string userId, int productId);
        Task<bool> IsInWishListAsync(string userId, int productId);
    }
}
