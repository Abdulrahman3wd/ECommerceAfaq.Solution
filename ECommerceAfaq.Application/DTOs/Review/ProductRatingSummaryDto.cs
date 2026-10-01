using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Review
{
    public class ProductRatingSummaryDto
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        public List<ReviewResponseDto> Reviews { get; set; } = new ();
    }
}
