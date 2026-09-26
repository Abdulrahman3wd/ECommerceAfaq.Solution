using ECommerceAfaq.Application.DTOs.Product;
using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IProductRepository : IGenericRpository<Product>
    {
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task<List<Product>> GetAllWithCategoryAsync();

        Task<(List<Product> Products, int TotalCount)> GetFilteredAsync(ProductQueryParameters parameters);
    }
}
