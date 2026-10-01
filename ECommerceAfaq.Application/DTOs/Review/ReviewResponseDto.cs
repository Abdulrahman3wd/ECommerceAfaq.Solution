using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; } = null!;
        public int Rating { get; set; } // 1 - 5
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
