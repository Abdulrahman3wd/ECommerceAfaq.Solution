using ECommerceAfaq.Application.DTOs.Category;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Infrastructure.Presistenece;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IFileService _fileService;

        public CategoryService(IUnitOfWork unitOfWork
            ,IFileService fileService,
            IGenericRpository<Category> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };


            await  _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();




            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                ImageUrl = category.ImageUrl
            };




        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return false;

            _unitOfWork.Categories.DeleteAsync(category);

            await _unitOfWork.SaveChangesAsync();


            return true;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {

            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories.Select(c=> new CategoryResponseDto
                {
                   Id= c.Id,
                   Name = c.Name,
                   Description= c.Description,
                   ImageUrl = c.ImageUrl,
                   IsActive= c.IsActive,
                   CreatedAt=c.CreatedAt

                }).ToList();

        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {

            var category = await _unitOfWork.Categories.GetByIdAsync(id);


            if (category is null)
                return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt

            };


        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return false;


            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.UpdateAsync(category);

            await   _unitOfWork.SaveChangesAsync();

            return true;
            
                
            
        }

        public async Task<bool> UploadImageAsync(int id, IFormFile file)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return false;

            // Delete old image  first (if any)  to  avoid orphaned files and save the new image
            _fileService.DeleteFile(category.ImageUrl);

            var newImageUrl  = await _fileService.SaveFileAsync(file , "categories");

            category.ImageUrl = newImageUrl;

            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
