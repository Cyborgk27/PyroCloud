using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Common;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileAppService _fileAppService;

        public FilesController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost("upload/{folder}")]
        public async Task<IActionResult> Upload(IFormFile file, string folder)
        {
            var response = await _fileAppService.SaveFileAsync(file, folder);
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string fileUrl)
        {
            var response = await _fileAppService.RemoveFileAsync(fileUrl);
            return Ok(response);
        }
    }
}
