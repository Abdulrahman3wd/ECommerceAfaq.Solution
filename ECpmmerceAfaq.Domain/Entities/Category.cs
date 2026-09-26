using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true; 

        // Naviagtion Property
        public ICollection<Product>  Products{ get; set; } = new List<Product>();

}
}
