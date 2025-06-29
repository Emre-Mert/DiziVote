using Newtonsoft.Json;

namespace DiziVote.Models
{
    public class TVShow
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        public string PosterUrl => $"https://image.tmdb.org/t/p/w500{PosterPath}";
    }
}
