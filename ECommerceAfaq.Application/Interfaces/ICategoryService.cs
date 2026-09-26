using ECommerceAfaq.Application.DTOs.Category;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto> GetByIdAsync(int id);
        Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> UpdateAsync( int id, CategoryUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UploadImageAsync(int id, IFormFile file);

    }
}
