using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TvApi.Controllers;
using TvApi.Core;
using TvApi.Infrastructure;

namespace TvApi.Tests.Internals
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IShowRepository>(_ => Mocks.GetShowRepository(200).Object);
            services.AddScoped<ILocalShowProvider, LocalShowProvider>();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddApplicationPart(typeof(ShowsController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


    }
}