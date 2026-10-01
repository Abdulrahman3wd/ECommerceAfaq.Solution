using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Review
{
    public class ReviewCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1,5)]
        public int Rating { get; set; } // 1 - 5

        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}
