using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvApi.Core;
using TvApi.Models;

namespace TvApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly ILocalShowProvider _localShowProvider;

        public ShowsController(ILocalShowProvider localShowProvider)
        {
            _localShowProvider = localShowProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> Get(
            int page = 0, 
            int count = 100)
        {
            var shows = await _localShowProvider
                .GetPagedShows(page, count);

            if (!shows.Any())
                return NotFound();

            return Ok(shows);
        }
    }
}
