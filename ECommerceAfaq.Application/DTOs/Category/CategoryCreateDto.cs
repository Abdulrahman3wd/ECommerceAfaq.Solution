using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Category
{
    public class CategoryCreateDto
    {

        [Required]
        [MaxLength(100 , ErrorMessage ="Name must be less than  100 characters")]
        [MinLength(2, ErrorMessage = "Name must be greater than than  2 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Description { get; set; }
    }
}
