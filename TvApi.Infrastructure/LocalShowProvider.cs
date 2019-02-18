using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvApi.Core;
using TvApi.Models;

namespace TvApi.Infrastructure
{
    public class LocalShowProvider : ILocalShowProvider
    {
        private readonly IShowRepository _showRepository;

        public LocalShowProvider(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public Task<IEnumerable<Show>> GetPagedShows(int page, int itemsPerPage)
        {
            var pagedShows = _showRepository
                .GetAll()
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage);

            return Task.FromResult(pagedShows);
        }
    }
}