using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Order
{
    public class OrderItemResponseDto
    {
        public int productId { get; set; }
        public string productName { get; set; }= string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
