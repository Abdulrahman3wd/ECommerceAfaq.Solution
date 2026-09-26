using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string UserId { get; set; } = null!; 
        public string Title { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Area { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string? Building { get; set; } = null!;

        public string? Apartment { get; set; } = null!;
        public bool IsDefault { get; set; }


    }
}
