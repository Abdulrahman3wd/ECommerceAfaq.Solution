using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.WishList
{
    public class WishListItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string ProductImageUrl { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }
        public bool IsInStock { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
