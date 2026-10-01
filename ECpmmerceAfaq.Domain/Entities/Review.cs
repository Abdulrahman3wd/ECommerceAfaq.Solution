using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string UserId { get; set; } = null!; 

        public string UserFullName { get; set; } = null!;

        public int ProductId { get; set; } 

        public int Rating { get; set; } // 1 - 5

        public string? Comment { get; set; }


        public Product Product { get; set; } = null!;

    }
}
