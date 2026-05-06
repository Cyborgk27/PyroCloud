using Microsoft.Extensions.Options;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;

namespace PyroCloud.Shared.Infrastructure.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly StorageSettings _settings;

        public LocalStorageService(IOptions<InfrastructureSettings> options)
        {
            _settings = options.Value.Storage;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder)
        {
            var path = Path.Combine(_settings.LocalPath, folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, fileName);
            using var file = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(file);

            return filePath;
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            return Task.CompletedTask;
        }
    }
}
