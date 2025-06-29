using DiziVote.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiziVote.Services
{
    public class TMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TMDbService()
        {
            _httpClient = new HttpClient();
            _apiKey = App.Configuration["TMDbApiKey"];
        }

        private void CheckApiKey()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("Geçersiz TMDb API Anahtarı. Lütfen appsettings.Development.json dosyasındaki API anahtarınızı kontrol edin.");
            }
        }

        public async Task<List<TVShow>> SearchShowsAsync(string query)
        {
            CheckApiKey();
            var response = await _httpClient.GetStringAsync($"https://api.themoviedb.org/3/search/tv?api_key={_apiKey}&query={query}&language=tr-TR");
            var searchResult = JsonConvert.DeserializeObject<TMDbSearchResult>(response);
            return searchResult?.Results;
        }

        public async Task<List<TVShow>> GetPopularShowsAsync()
        {
            CheckApiKey();
            var response = await _httpClient.GetStringAsync($"https://api.themoviedb.org/3/tv/popular?api_key={_apiKey}&language=tr-TR");
            var searchResult = JsonConvert.DeserializeObject<TMDbSearchResult>(response);
            return searchResult?.Results;
        }

        public async Task<TVShow> GetShowDetailsAsync(int showId)
        {
            CheckApiKey();
            var response = await _httpClient.GetStringAsync($"https://api.themoviedb.org/3/tv/{showId}?api_key={_apiKey}&language=tr-TR");
            return JsonConvert.DeserializeObject<TVShow>(response);
        }
    }

    public class TMDbSearchResult
    {
        public List<TVShow> Results { get; set; }
    }
}
