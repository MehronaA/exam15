using System;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(string folder, IFormFile file);
    void DeleteFileAsync(string folder, string fileName);
}
