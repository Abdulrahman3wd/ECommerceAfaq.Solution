using ECommerceAfaq.Application.DTOs.Common;
using ECommerceAfaq.Application.DTOs.Product;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public ProductServices(IUnitOfWork unitOfWork , IFileService fileService)
        {
           _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
        {
            var categoryExist = await _unitOfWork.Categories.ExistesAsync(c => c.Id == dto.CategoryId);

            if (!categoryExist)
                throw new ArgumentException("Category does not exist");

            var product = new Product
            {
                
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.stock,
                CategoryId = dto.CategoryId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow

            }; 

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.Id);

            if (created is null)
                return null;

            return MapToResponseDto(created);



        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product is null) return false;

            _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return products.Select(MapToResponseDto).ToList();
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);

            return product is null ? null : MapToResponseDto(product);

        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {

            var  product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product is null) return false;

            var categoryExist = await _unitOfWork.Categories.ExistesAsync(c => c.Id == dto.CategoryId);

            if (!categoryExist)
                throw new ArgumentException("Category does not exist");


            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.Stock = dto.stock; 
            product.Price = dto.Price;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;


            _unitOfWork.Products.UpdateAsync(product);

            await _unitOfWork.SaveChangesAsync();
            return true;


        }

        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty, 
                CreatedAt = product.CreatedAt

            };
        }

        public async Task<bool> UploadImageAsync(int id, IFormFile file)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if(product is null)
                return false;

            _fileService.DeleteFile(product.ImageUrl);
            var newImageUrl = await _fileService.SaveFileAsync(file, "products"); 
            product.ImageUrl = newImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.UpdateAsync(product); 
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<PagedResult<ProductResponseDto>> GetFilterdAsync(ProductQueryParameters parameters)
        {
            var(products ,totalCount) = await _unitOfWork.Products.GetFilteredAsync(parameters);

            return new PagedResult<ProductResponseDto>
            {
                Items = products.Select(MapToResponseDto).ToList(),
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount

            };
        }
    }
}
