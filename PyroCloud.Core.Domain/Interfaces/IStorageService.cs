namespace PyroCloud.Core.Domain.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder);
        Task DeleteFileAsync(string filePath);
    }
}
