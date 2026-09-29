using ECommerceAfaq.Application.DTOs.Order;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderResponseDto> CheckOutAsync(string userId, CheckoutDto dto)
        {

            // Step 1 - Validate the cart

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);

            if (cart is null || cart.CartItems.Count == 0)
                throw new ArgumentException("Cart is empty!");

            // Step 2 - Validate the address (and ownership)

            var address = await _unitOfWork.Repository<Address>().GetByIdAsync(dto.AddressId);
            if (address is null || address.UserId != userId)
                throw new ArgumentException("Invalid Address");

            // Step 3 - Validate Every Product still Exist , is active  , and has enough

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);

                if (product is null || !product.IsActive)
                    throw new ArgumentException($"product '{cartItem.Product.Name}' is no longer available");

                if (product.Stock < cartItem.Quantity)
                    throw new ArgumentException($"Insufficient stock for '{product.Name}'");
            }


            // Step 4 - Begin Transaction - everything from here must success together

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Step 5 - Calc Total on the server 

                var order = new Order
                {
                    UserId = userId,
                    ShippingFullname = address.FullName,
                    ShippingPhone = address.PhoneNumber,
                    ShippingCity = address.City,
                    ShippingArea = address.Area,
                    ShippingStreet = address.Street,
                    ShippingBuilding = address.Building,
                    ShippingAppartment = address.Apartment,
                    PaymentMethod = dto.PaymentMethod,
                    PaymentStatus = PaymentStatus.Pending,
                    OrderStatus = OrderStatus.Pending
                };

                decimal totalAmount = 0; 

                foreach(var cartItem in cart.CartItems)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);

                    var orderItem = new OrderItem
                    {
                        ProductId = product!.Id,
                        ProductName = product.Name, // snapshot
                        UnitPrice = product.Price, // snapshot
                        Quantity = cartItem.Quantity
                    };

                    order.OrderItems.Add(orderItem);
                    totalAmount +=  orderItem.Total;

                    // Step 6 - Reduce Stock 

                    product.Stock -= cartItem.Quantity;
                    _unitOfWork.Products.UpdateAsync(product);


                }

                order.TotalAmount = totalAmount;

                await _unitOfWork.Repository<Order>().AddAsync(order);

                // Step 7 - Clear Cart

                cart.CartItems.Clear();

                // Step 8 - Save Everything and commit 

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return MapToResponseDto(order);

            }

            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }



        }

        public async Task<List<OrderResponseDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Repository<Order>().FindAsync(o => o.UserId == userId); 
            return orders.OrderByDescending(o => o.CreatedAt).Select(MapToResponseDto).ToList();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(string userId, int orderId, bool isAdmin)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId); 
            if (order is null)
                return null;

            // Admins can view any order , customers only thier one

            if (!isAdmin && order.UserId != userId)
                return null;

            return MapToResponseDto(order);
        }

        public async Task<bool> CancelOrderAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId); 

            if (order is null || order.UserId != userId)
                return false;

            if (order.OrderStatus is not (OrderStatus.Pending or OrderStatus.Confirmed))
                throw new ArgumentException("Order can no longer be cancelled at this stage");


            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Restore stock for every item 

                foreach (var  item in order.OrderItems)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product is not null)
                    {
                        product.Stock += item.Quantity;
                        _unitOfWork.Products.UpdateAsync(product); 
                    }

                    
                }
                order.OrderStatus = OrderStatus.Canceled;
                order.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return orders.OrderByDescending(o=>o.CreatedAt).Select(MapToResponseDto).ToList();
        }




        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order is null)
                return false;

            order.OrderStatus = newStatus;
            order.UpdatedAt = DateTime.UtcNow; 

            if(newStatus == OrderStatus.Delevered && order.PaymentMethod == PaymentMethod.CashOnDelivery)
            {
                order.PaymentStatus = PaymentStatus.Paid; 

            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    productId = oi.ProductId,
                    productName = oi.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    Total = oi.Total,
                }).ToList(),
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                ShippingFullname = order.ShippingFullname,
                ShippingCity = order.ShippingCity,
                ShippingArea = order.ShippingArea,
                ShippingAppartment = order.ShippingAppartment,
                ShippingBuilding = order.ShippingBuilding,
                ShippingPhone = order.ShippingPhone,
                ShippingStreet = order.ShippingStreet,
                CreateAt = order.CreatedAt,
            };
        }
    }
}
