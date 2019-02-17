using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvApi.Models;

namespace TvApi.Core
{
    public interface IShowProvider 
    {
        Task<Dictionary<int, int>> GetTvShowsAsync(CancellationToken cancellationToken);
        Task<Show> GetTvShowAsync(int showId, CancellationToken cancellationToken);
    }
}
