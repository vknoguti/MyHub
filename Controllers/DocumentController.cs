using Microsoft.AspNetCore.Mvc;
using MyHub.DTOs.Common;
using MyHub.Services.FileStorage;
using MyHub.Utils;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument(Guid ProfileId, IFormFile file)
        {
            if(file is null || file.Length == 0)
            {
                return BadRequest(ApiResponse.Fail("No file was uploaded or the file is empty"));
            }


            List<string> validExtensions = new List<string>() {".jpg", ".png", ".pdf"};
            if (!validExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                return BadRequest(ApiResponse.Fail("File with invalid extension"));
            }

            long fileSize = file.Length;

            const long MAXFILESIZE_BYTE = 1 * 1024 * 1024;
            if(fileSize > MAXFILESIZE_BYTE)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, 
                    $"Payload too large, max payload = {FileUtils.ByteToMegaByte(MAXFILESIZE_BYTE)} mega bytes");
            }



            //IMPLMENTAR AQUI A CHAMADA AO SERVICE DE FILESTORAGE
            return Ok();
        }

    }
}
