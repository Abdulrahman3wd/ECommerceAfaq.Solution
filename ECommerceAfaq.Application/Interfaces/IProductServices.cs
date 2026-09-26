using ECommerceAfaq.Application.DTOs.Category;
using ECommerceAfaq.Application.DTOs.Common;
using ECommerceAfaq.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IProductServices
    {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto> GetByIdAsync(int id);
        Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UploadImageAsync(int id, IFormFile file);

        Task<PagedResult<ProductResponseDto>> GetFilterdAsync(ProductQueryParameters parameters);

    }
}
