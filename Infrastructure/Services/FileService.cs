using System;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FileService:IFileService
{
    
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public async Task<string> SaveFileAsync(string folder, IFormFile file)
    {
        try
    {
        var webRootPath = _environment.WebRootPath;
        var path = Path.Combine(webRootPath, folder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    
        var fullPath = Path.Combine(path, file.FileName);
        
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return file.FileName;
    }
    catch (Exception e)
    {
            Console.WriteLine(e);
        throw;
    }
        
    }

    public void DeleteFileAsync(string folder, string fileName)
    {
        try
        {
            var webRootPath = _environment.WebRootPath;
        var fullPath = Path.Combine(webRootPath, folder, fileName);
        
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
