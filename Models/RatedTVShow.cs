using System.ComponentModel.DataAnnotations;

namespace DiziVote.Models
{
    public class RatedTVShow
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PosterUrl { get; set; }
        public int UserRating { get; set; }
    }
}
