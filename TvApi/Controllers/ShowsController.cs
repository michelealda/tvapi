using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TvApi.Core;
using TvApi.Models;

namespace TvApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowRepository _showRepository;

        public ShowsController(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Show>> Get(int page = 0, int count = 100)
        {
            var pagedShows = _showRepository
                .GetAll()
                .Skip(page * count)
                .Take(count)
                .ToList();

            if (!pagedShows.Any())
                return NotFound();

            return Ok(pagedShows);
        }
    }
}
