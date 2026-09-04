using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyHub.DTOs.Common;
using MyHub.Services.Document;
using MyHub.Services.FileStorage;
using MyHub.Utils;
using System.Diagnostics;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        private readonly DocumentService _documentService;

        public DocumentController(IFileStorage fileStorage, DocumentService documentService)
        {
            _fileStorage = fileStorage;
            _documentService = documentService;
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument(Guid profileId, IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest(ApiResponse.Fail("No file was uploaded or the file is empty"));
            }

            List<string> validExtensions = new() { ".jpg", ".png", ".pdf" };
            if (!validExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                return BadRequest(ApiResponse.Fail("File with invalid extension"));
            }

            long fileSize = file.Length;

            //1 megabyte
            const long MAXFILESIZE_BYTE = 1 * 1024 * 1024;
            if (fileSize > MAXFILESIZE_BYTE)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge,
                    $"Payload too large, max payload = {FileUtils.ByteToMegaByte(MAXFILESIZE_BYTE)} mega bytes");
            }

            var result = await _documentService.UploadAsync(profileId, file);
            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse.Fail("File upload failed"));
            }

            return Ok(ApiResponse.Ok($"File: {file.FileName} sucessfully written."));
        }

        [HttpGet("download-document")]
        public async Task<IActionResult> DownloadDocument()
        {
            return Ok();
        }

    }
}
