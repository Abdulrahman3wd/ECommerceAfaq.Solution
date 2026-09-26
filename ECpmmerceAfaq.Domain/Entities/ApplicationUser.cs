using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;   
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName {  get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        }
    }
