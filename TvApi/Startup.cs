using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using TvApi.Core;
using TvApi.Infrastructure;

namespace TvApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logger => logger.AddConsole());

            services.AddScoped<IShowRepository, ShowRepository>();
            services.AddScoped<ILocalShowProvider, LocalShowProvider>();
            
            services.AddHttpClient<IRemoteShowProvider, RemoteShowProvider>()
                .AddPolicyHandler(RetryOn429TooManyRequests);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHostedService<ShowScraperService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static RetryPolicy<HttpResponseMessage> RetryOn429TooManyRequests
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryForeverAsync(sleepDurationProvider: i => TimeSpan.FromSeconds(5 * i));

    }
}
