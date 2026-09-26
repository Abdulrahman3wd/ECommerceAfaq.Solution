using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
