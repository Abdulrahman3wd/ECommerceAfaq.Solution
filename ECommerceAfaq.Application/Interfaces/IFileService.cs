using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }
}
