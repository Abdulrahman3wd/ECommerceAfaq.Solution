using System.ComponentModel.DataAnnotations;

namespace ECommerceAfaq.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price {  get; set; }

        public int Stock {  get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }

        // Foriegn Key 
        public int CategoryId { get; set; } 

        //  Navigation  Property
        public Category Category { get; set; } = null!;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();


    }
}