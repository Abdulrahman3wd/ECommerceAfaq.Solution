using ECommerceAfaq.Application.DTOs.Cart;
using ECommerceAfaq.Application.DTOs.Common;
using ECommerceAfaq.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(string userId);
        Task<CartResponseDto> AddItemAsync(string userId , AddCartItemDto dto);
        Task<bool> UpdateItemsAsync(string userId,int productId, UpdateCartItemDto dto);
        Task<bool> RemoveItemAsync(string userId , int productId);
        Task<bool> ClearCartAsync(string userId);

    }
}
