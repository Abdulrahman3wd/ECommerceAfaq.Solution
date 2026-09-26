using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Product
{
    public class ProductResponseDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int stock { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public  string? ImageUrl {  get; set; } 

        public DateTime CreatedAt { get; set; }

    }
}
