using ECommerceAfaq.Application.DTOs.Order;
using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IOrderService
    {

        Task<OrderResponseDto> CheckOutAsync(string userId, CheckoutDto dto); 

        Task<List<OrderResponseDto>> GetUserOrdersAsync(string userId);

        Task<OrderResponseDto?> GetByIdAsync(string userId ,int orderId , bool isAdmin );

        Task<bool> CancelOrderAsync(string userId , int orderId);

        Task<List<OrderResponseDto>> GetAllOrdersAsync(); // Admin

        Task<bool> UpdateStatusAsync( int orderId , OrderStatus newStatus);
    }
}
