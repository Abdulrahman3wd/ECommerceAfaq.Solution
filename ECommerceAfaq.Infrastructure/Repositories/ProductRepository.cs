using ECommerceAfaq.Application.DTOs.Product;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Infrastructure.Presistenece;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
           _context = context;
        }
        public async Task<List<Product>> GetAllWithCategoryAsync()
        {
            return await  _context.Products
                .AsNoTracking()
                .Include(p=>p.Category).ToListAsync();
        }
        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(List<Product> Products, int TotalCount)> GetFilteredAsync(ProductQueryParameters parameters)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(p => p.Name.Contains(parameters.Search));
            }

            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);               
            }

            query = (parameters.SortBy?.ToLower(), parameters.SortOrder.ToLower()) switch
            {
                ("price", "desc") => query.OrderByDescending(p => p.Price),
                ("price", "_") => query.OrderBy(p => p.Price),
                ("createdat", "desc") => query.OrderByDescending(p => p.CreatedAt),
                ("createdat", "_") => query.OrderBy(p => p.CreatedAt),
                ("_", "desc") => query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            var totalcount = await query.CountAsync();

            // Pagination 
            var products = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (products , totalcount);

        }
    }
}
