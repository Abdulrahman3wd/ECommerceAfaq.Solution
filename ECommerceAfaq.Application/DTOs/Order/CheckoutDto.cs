using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Order
{
    public class CheckoutDto
    {

        [Required]
        public int AddressId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }




    }
}
