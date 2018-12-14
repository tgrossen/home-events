using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using common.DarkSky;
using common.Infrastructure;

namespace api.Controllers
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
        public ActionResult<string> GetLatest()
        {
            return Ok(darkSkyLogRepository.GetLatest());
        }

        [HttpPost]
        public ActionResult Post([FromBody] DarkSkyLog forecast)
        {
            darkSkyLogRepository.Save(forecast);
            return NoContent();
        }
    }
}
