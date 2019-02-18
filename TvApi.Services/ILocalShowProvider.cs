using System.Collections.Generic;
using System.Threading.Tasks;
using TvApi.Models;

namespace TvApi.Core
{
    public interface ILocalShowProvider
    {
        Task<IEnumerable<Show>> GetPagedShows(int page, int itemsPerPage);
    }
}