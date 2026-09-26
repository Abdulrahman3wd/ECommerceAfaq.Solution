using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Product
{
    public class ProductCreateDto
    {

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string?  Description {  get; set; }


        [Range(0.01 ,  double.MaxValue , ErrorMessage = "Price must be  greater than zero")]
        public decimal Price { get; set; }

        [Range( 0, double.MaxValue, ErrorMessage = "stock canonot be negative")]
        public int stock {  get; set; }

        [Required]
        public int CategoryId { get; set; }
        

    }
}
