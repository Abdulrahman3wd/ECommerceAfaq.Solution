using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Infrastructure.Presistenece;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly AppDbContext _dbContext;

        public CartRepository(AppDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Carts
                .Include(c=>c.CartItems)
                .ThenInclude( ci=> ci.Product)
                .FirstOrDefaultAsync(c=>c.UserId == userId);
        }
    }
}
