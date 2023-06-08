using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface IFileService
{
    Task<string> CreateFile (string folder,IFormFile file);
    bool DeleteFile (string folder,string filename);
}
