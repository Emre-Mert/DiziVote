using DiziVote.Data;
using DiziVote.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiziVote.Services
{
    public class DatabaseService
    {
        private readonly DiziVoteDbContext _context;

        public DatabaseService(DiziVoteDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<RatedTVShow>> GetRatedShowsAsync()
        {
            return await _context.RatedShows.ToListAsync();
        }

        public async Task AddOrUpdateRatingAsync(RatedTVShow show)
        {
            var existingShow = await _context.RatedShows.FindAsync(show.Id);
            if (existingShow != null)
            {
                existingShow.UserRating = show.UserRating;
            }
            else
            {
                _context.RatedShows.Add(show);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetRatingForShowAsync(int showId)
        {
            var show = await _context.RatedShows.FindAsync(showId);
            return show?.UserRating ?? 0;
        }

        public async Task DeleteRatingAsync(int showId)
        {
            var show = await _context.RatedShows.FindAsync(showId);
            if (show != null)
            {
                _context.RatedShows.Remove(show);
                await _context.SaveChangesAsync();
            }
        }
    }
}
