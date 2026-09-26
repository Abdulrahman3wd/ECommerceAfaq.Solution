using ECommerceAfaq.Application.DTOs.Cart;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
         _unitOfWork = unitOfWork;
        }
        public async Task<CartResponseDto> AddItemAsync(string userId, AddCartItemDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);

            if (product is null)
                throw new ArgumentException("Product does not exist.");

            if (!product.IsActive)
                throw new ArgumentException("product is not available.");

            if (product.Stock < dto.Quantity)
                throw new ArgumentException("Requested quantity exceeds available stock");

            var cart = await GetOrCreateCartAsync(userId);

            var existingItems = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (existingItems is not null )
            {
                // product already in cart - update cart instead of creating a dubicate row in database

                var newQuantity = existingItems.Quantity + dto.Quantity;
                if (product.Stock < newQuantity)
                    throw new ArgumentException("Requested quantity exceeds available stock.");
                existingItems.Quantity = newQuantity; // update 
                _unitOfWork.Carts.UpdateAsync(cart);              
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CartId = cart.Id
                }); 
            }

            await _unitOfWork.SaveChangesAsync();

            var updatedCart = await _unitOfWork.Carts.GetByUserIdAsync(userId);

            if (updatedCart is null)
                return null; 

              return MapToResponseDto(updatedCart);

        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart is null)
                return false;
            cart.CartItems.Clear();
            await _unitOfWork.SaveChangesAsync();
            return true;


        }

        public async Task<CartResponseDto> GetCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return MapToResponseDto(cart);
                
        }

        public async Task<bool> RemoveItemAsync(string userId, int productId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart is null)
                return false;

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item is null)
                return false;

            cart.CartItems.Remove(item);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateItemsAsync(string userId, int productId, UpdateCartItemDto dto)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart is null)
                return false;

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item is null)
                return false; 

            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if(product is null || product.Stock < dto.Quantity)
                throw new ArgumentException("Requested quantity exceeds available stock.");

            item.Quantity = dto.Quantity;

            await _unitOfWork.SaveChangesAsync();
            return true; 




        }

        private async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);

            if(cart is not null)
                return cart;

            cart = new Cart { UserId = userId };

            await _unitOfWork.Carts.AddAsync(cart);

            await _unitOfWork.SaveChangesAsync();

            return cart;

           
            
        }

        private static CartResponseDto MapToResponseDto(Cart cart)
        {

            return new CartResponseDto
            {
                Id = cart.Id,
                Items = cart.CartItems.Select(ci => new CartItemResponseDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name, 
                    ProductImageUrl = ci.Product.ImageUrl,
                    UnitPrice = ci.Product.Price, 
                    Quantity = ci.Quantity

                }).ToList(),
            };

        }
    }
}
