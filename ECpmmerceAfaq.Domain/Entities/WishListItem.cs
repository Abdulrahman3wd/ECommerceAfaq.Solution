using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class WishListItem : BaseEntity
    {
        public string UserId { get; set; } = string.Empty; 
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!; 
    }
}
