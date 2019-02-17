using System.Collections.Generic;
using System.Linq;

namespace TvApi.Infrastructure.Extensions
{
    public static class ShowListExtensions
    {
        public static IEnumerable<IEnumerable<KeyValuePair<int, int>>> SplitShows(
            this Dictionary<int, int> shows, 
            int chunkSize = 20)
        {
            var i = 0;
            do
            {
                yield return shows.Skip(i).Take(chunkSize);
                i += chunkSize;
            } while (i <= shows.Count);
        }
    }
}