using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Cart
{
    public class CartResponseDto
    {
        public int Id { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal); 
    }
}
