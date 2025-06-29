using DiziVote.Models;
using Microsoft.EntityFrameworkCore;

namespace DiziVote.Data
{
    public class DiziVoteDbContext : DbContext
    {
        public DbSet<RatedTVShow> RatedShows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DiziVote.db");
        }
    }
}
