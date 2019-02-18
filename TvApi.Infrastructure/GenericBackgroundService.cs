using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TvApi.Infrastructure
{
    public abstract class GenericBackgroundService : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource _cts;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            _cts.Cancel();
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteAsync(_cts.Token);
            return _executingTask.IsCompleted
                ? _executingTask
                : Task.CompletedTask;
        }

        protected virtual Task ExecuteAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

    }
}