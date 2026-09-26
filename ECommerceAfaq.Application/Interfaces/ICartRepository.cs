using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface ICartRepository : IGenericRpository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
    }
}
