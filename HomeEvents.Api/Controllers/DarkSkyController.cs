using System.Threading.Tasks;
using HomeEvents.DarkSky;
using HomeEvents.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HomeEvents.Api.Controllers
{
    [Route("api/dark-sky")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IInternalRepository<DarkSkyLog> darkSkyLogRepository;

        public ValuesController(IInternalRepository<DarkSkyLog> darkSkyLogRepository) {
            this.darkSkyLogRepository = darkSkyLogRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetLatest()
        {
            return Ok(await darkSkyLogRepository.GetLatest());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DarkSkyLog forecast)
        {
            await darkSkyLogRepository.Save(forecast);
            return NoContent();
        }
    }
}
