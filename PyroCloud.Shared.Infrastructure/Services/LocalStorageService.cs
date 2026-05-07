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
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), _settings.LocalPath);
            var targetDirectory = Path.Combine(rootPath, folder);

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var physicalPath = Path.Combine(targetDirectory, fileName);

            using (var file = new FileStream(physicalPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            var relativeUrl = Path.Combine("/cdn", folder, fileName).Replace("\\", "/");

            return relativeUrl;
        }

        public Task DeleteFileAsync(string fileUrl)
        {
            var relativePath = fileUrl.Replace("/cdn/", "").Replace("/", Path.DirectorySeparatorChar.ToString());
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), _settings.LocalPath, relativePath);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            return Task.CompletedTask;
        }
    }
}
