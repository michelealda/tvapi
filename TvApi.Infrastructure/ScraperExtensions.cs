using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using TvApi.Core;

namespace TvApi.Infrastructure
{
    public static class ScraperExtensions
    {
        public static void ConfigureScraper(this IServiceCollection services)
        {
            services
                .AddHttpClient<IRemoteShowProvider, RemoteShowProvider>()
                .AddPolicyHandler(RetryOn429TooManyRequests);

            services.AddHostedService<ShowScraperService>();
        }

        private static RetryPolicy<HttpResponseMessage> RetryOn429TooManyRequests
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == (HttpStatusCode)429)
                .WaitAndRetryForeverAsync(
                    sleepDurationProvider: (i, response, context) =>
                    {
                        var retryHeader = response.Result?.Headers.RetryAfter;
                        if (retryHeader == null)
                            return TimeSpan.FromSeconds(5 * i);

                        var delay = retryHeader.Delta;
                        return delay ?? TimeSpan.FromSeconds(5 * i);

                    },
                    onRetryAsync: (_, __, ___) => Task.CompletedTask);
    }
}