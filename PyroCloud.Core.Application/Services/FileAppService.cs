using Microsoft.AspNetCore.Http;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.Core.Application.Services
{
    public class FileAppService : IFileAppService
    {
        private readonly IStorageService _storage;
        private readonly IUrlService _urlService;

        public FileAppService(IStorageService storage, IUrlService urlService)
        {
            _storage = storage;
            _urlService = urlService;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if(file is null || file.Length == 0)
            {
                throw new UserFriendlyException("No se ha proporcionado un archivo válido");
            }

            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{extension}";

                using var stream = file.OpenReadStream();
                var relativeUrl = await _storage.UploadFileAsync(stream, fileName, folder);
                return _urlService.GetAbsoluteUrl(relativeUrl);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Ocurrió un error al procesar el archivo en el servidor. " + ex.Message);
            }
        }

        public async Task<bool> RemoveFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new UserFriendlyException("La URL del archivo no puede estar vacía.");

            try
            {
                await _storage.DeleteFileAsync(fileUrl);

                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("No se pudo eliminar el archivo del servidor. " + ex.Message);
            }
        }
    }
}
