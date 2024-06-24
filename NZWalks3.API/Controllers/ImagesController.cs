using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks3.API.DTOs;
using NZWalks3.API.Model;
using NZWalks3.API.Repository;

namespace NZWalks3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto requestDto)
        {
            ValidationOnUpload(requestDto);

            if (ModelState.IsValid)
            {
                //Convert DTO to Model 
                var imageDomainModel = new Image
                {
                    File = requestDto.File,
                    FileDescription=requestDto.Discription,
                    FileName=requestDto.FileName,
                    FileExtension= Path.GetExtension(requestDto.File.FileName),
                    FilSizeBytes=requestDto.File.Length,
                };

                //User repository to upload image
                await imageRepository.UploadImage(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }

        private void ValidationOnUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtension.Contains(Path.GetExtension(requestDto.File.FileName)))
            {
                ModelState.AddModelError("File", "Unsported File Extension");
            }

            if(requestDto.File.Length>10485760)
            {
                ModelState.AddModelError("file", "File size more than 10 MB, Please upload a smaller size file");
            }
        }

    }
}
