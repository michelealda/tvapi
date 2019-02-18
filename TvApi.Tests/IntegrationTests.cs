using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using TvApi.Models;
using TvApi.Tests.Internals;
using Xunit;

namespace TvApi.Tests
{
    public class IntegrationTests
    {
        private readonly HttpClient _client;

        public IntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = server.CreateClient();
        }

        private async Task<IEnumerable<Show>> GetShows(int page = 0, int pageCount = 100)
        {
            var response = await _client.GetAsync($"api/shows?page={page}&count={pageCount}");
            response.EnsureSuccessStatusCode();
            return JsonConvert
                .DeserializeObject<IEnumerable<Show>>(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ShouldReturnsPaginatedShows()
            => (await GetShows())
                .Should()
                .HaveCount(100);

        [Fact]
        public async Task ShouldRespectPageSize()
            => (await GetShows(pageCount:5))
                .Should()
                .HaveCount(5);

        [Fact]
        public async Task UnknownPageShouldReturnNotFound()
            => (await _client.GetAsync("api/shows?page=100"))
                .StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
    }
}