using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvApi.Core;

namespace TvApi.Infrastructure
{
    public class ShowScraperService : GenericBackgroundService
    {
        private readonly IRemoteShowProvider _remoteShowProvider;
        private readonly IShowRepository _showRepository;


        public ShowScraperService(IRemoteShowProvider remoteShowProvider,
            IShowRepository showRepository
            )
        {
            _remoteShowProvider = remoteShowProvider;
            _showRepository = showRepository;
        }
        
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var allShowIds = await _remoteShowProvider
                .GetTvShowsAsync(cancellationToken);

            allShowIds.ToList()
                .ForEach(async s =>
                {
                    var fullShow = await _remoteShowProvider
                        .GetTvShowAsync(s.Key, cancellationToken);

                    if (fullShow != null)
                    {
                        _showRepository.Insert(fullShow);
                    }
                });
        }
    }
}