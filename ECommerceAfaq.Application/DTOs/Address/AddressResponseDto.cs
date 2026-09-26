using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Address
{
    public class AddressResponseDto
    {

        public int Id { get; set; }
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
