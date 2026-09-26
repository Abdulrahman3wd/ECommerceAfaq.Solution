using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(ApplicationUser user, IList<string> roles); 
    }
}
