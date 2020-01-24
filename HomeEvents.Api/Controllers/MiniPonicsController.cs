using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeEvents.Extensions;
using HomeEvents.Infrastructure;
using HomeEvents.MiniPonics;
using HomeEvents.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HomeEvents.Api.Controllers
{
    [Route("api/mini-ponics")]
    [ApiController]
    public class MiniPonicsController : ControllerBase
    {
        readonly IHomeEventsSettings settings;
        readonly IInternalRepository<MiniPonicsImageData> miniPonicsImageDataRepository;

        public MiniPonicsController(IHomeEventsSettings settings,
            IInternalRepository<MiniPonicsImageData> miniPonicsImageDataRepository)
        {
            this.settings = settings;
            this.miniPonicsImageDataRepository = miniPonicsImageDataRepository;
        }

        [Route("image")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> ImagePost()
        {
            try
            {
                var imagesBaseFilePath = settings.MiniPonicsImagesBaseFilePath;
                var file = Request.Form.Files[0];
                
                var now = DateTimeOffset.Now.TruncateToMinute();

                if (file.Length > 0)
                {
                    var fileExtension = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim()
                        .Split(new[] {'.'})
                        .LastOrDefault()
                        .ToString();
                    var fileName = $"{now:O}.{fileExtension}";
                    var directory = Path.Combine(
                        imagesBaseFilePath,
                        now.Year.ToString(),
                        now.Month.ToString(),
                        now.Day.ToString());
                    Directory.CreateDirectory(directory);
                    var fullPath = Path.Combine(directory, fileName);

                    await using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        await miniPonicsImageDataRepository.Save(new MiniPonicsImageData
                        {
                            EventDate = now,
                            ImageFilePath = fullPath
                        });
                    }

                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in {nameof(MiniPonicsController)}: {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}