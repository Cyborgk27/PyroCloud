using Microsoft.AspNetCore.Http;

namespace PyroCloud.Core.Application.Interfaces
{
    public interface IFileAppService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task<bool> RemoveFileAsync(string fileUrl);
    }
}
