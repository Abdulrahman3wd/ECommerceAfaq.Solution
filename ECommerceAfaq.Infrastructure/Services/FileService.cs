using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class FileService : IFileService
    {


        private readonly string[] _allowedExtensions =  { ".jpg", ".jpeg", ".png", ".gif" , ".webp" };

        private readonly long MaxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB

        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            ValidateFile(file);

            var UploadsFolder = Path.Combine(_environment.WebRootPath, "images", folderName); 

            if(!Directory.Exists (UploadsFolder))
                Directory.CreateDirectory(UploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; 

            var filePath = Path.Combine(UploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Dispose the stream after copying the file
            }

            return $"/images/{folderName}/{uniqueFileName}";

        }
        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var fullpath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(filePath))
                File.Delete(fullpath);

        }

        private void ValidateFile(IFormFile file)
        {
            if(file is null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            if(file.Length > MaxFileSizeInBytes)
                throw new ArgumentException($"File size exceeds the maximum limit of {MaxFileSizeInBytes / (1024 * 1024)} MB."); // 2Mb

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException($"File extension '{extension}' is not allowed. Allowed extensions are: {string.Join(", ", _allowedExtensions)}.");
        }


    }
}
