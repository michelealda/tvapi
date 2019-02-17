using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TvApi.Core;
using TvApi.Infrastructure.Extensions;

namespace TvApi.Infrastructure
{
    public class ShowScraper : IHostedService
    {
        private readonly IShowProvider _showProvider;
        private readonly IShowRepository _showRepository;

        public ShowScraper(IShowProvider showProvider,
            IShowRepository showRepository
            )
        {
            _showProvider = showProvider;
            _showRepository = showRepository;
        }

        private Task _executingTask;
        private CancellationTokenSource _cts;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method
            _cts.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingTask = ExecuteAsync(_cts.Token);

            // If the task is completed then return it, otherwise it's running
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;


        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var allShows = await _showProvider
                .GetTvShowsAsync(cancellationToken);

            foreach (var shows in allShows.SplitShows())
            {
                foreach (var kv in shows)
                {
                 var show =  await _showProvider.GetTvShowAsync(kv.Key, cancellationToken);
                 if (show != null)
                 {
                     _showRepository.Insert(show);
                 }
                }

                //To avoid too many request delay 20 calls every 5 seconds
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}