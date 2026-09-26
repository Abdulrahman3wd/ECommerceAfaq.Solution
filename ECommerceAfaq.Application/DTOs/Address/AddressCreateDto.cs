using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Address
{
    public class AddressCreateDto
    {
        [Required, StringLength(50)]
        public string Title { get; set; } = null!;

        [Required, StringLength(150)]
        public string FullName { get; set; } = null!;

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, StringLength(100)]
        public string City { get; set; } = null!;

        [Required, StringLength(100)]
        public string Area { get; set; } = null!;

        [Required, StringLength(200)]

        public string Street { get; set; } = null!;
        public string? Building { get; set; } = null!;

        public string? Apartment { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
