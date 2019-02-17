using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvApi.Core;
using TvApi.Infrastructure.Dto;
using TvApi.Infrastructure.Mappers;
using TvApi.Models;

namespace TvApi.Infrastructure
{
    public class OnlineShowProvider : IShowProvider
    {
        private readonly HttpClient _httpClient;

        public OnlineShowProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"http://api.tvmaze.com/");
        }


        public Task<Dictionary<int, int>> GetTvShowsAsync(CancellationToken cancellationToken)
            => GetContentAsync<Dictionary<int, int>>("updates/shows", cancellationToken);
        

        public async Task<Show> GetTvShowAsync(int showId, CancellationToken cancellationToken)
        {
            var dto = await GetContentAsync<TvShowDto>($"shows/{showId}?embed=cast", cancellationToken);

            return dto?.MapToShow();
        }


        private async Task<T> GetContentAsync<T>(string endpoint, CancellationToken cancellationToken)
            where T : class
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                return Deserialize<T>(stream);
            }
        }

        private static T Deserialize<T>(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return new JsonSerializer()
                        .Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
