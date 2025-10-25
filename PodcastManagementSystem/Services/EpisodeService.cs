using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly ApplicationDbContext _context;

        public EpisodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Episode>> GetAllEpisodesAsync()
        {
            return await _context.Episodes.Include(e => e.Podcast).ToListAsync();
        }

        public async Task<Episode> GetEpisodeByIdAsync(int id)
        {
            return await _context.Episodes.Include(e => e.Podcast).FirstOrDefaultAsync(e => e.EpisodeID == id);
        }

        public async Task<List<Episode>> GetEpisodesByTopicAsync(string topic)
        {
            return await _context.Episodes
                .Include(e => e.Podcast)
                .Where(e => e.Topic.Contains(topic))
                .ToListAsync();
        }

        public async Task<List<Episode>> GetEpisodesByHostAsync(string host)
        {
            return await _context.Episodes
                .Include(e => e.Podcast)
                .Where(e => e.Host.Contains(host))
                .ToListAsync();
        }

        public async Task<List<Episode>> GetMostPopularEpisodesAsync()
        {
            return await _context.Episodes
                .Include(e => e.Podcast)
                .OrderByDescending(e => e.PlayCount)
                .ToListAsync();
        }

        public async Task<List<Episode>> GetEpisodesByDateAsync()
        {
            return await _context.Episodes
                .Include(e => e.Podcast)
                .OrderByDescending(e => e.ReleaseDate)
                .ToListAsync();
        }

        public async Task AddEpisodeAsync(Episode episode)
        {
            episode.ReleaseDate = DateTime.Now;
            episode.PlayCount = 0;
            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEpisodeAsync(Episode episode)
        {
            _context.Episodes.Update(episode);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEpisodeAsync(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);
            if (episode != null)
            {
                _context.Episodes.Remove(episode);
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementPlayCountAsync(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);
            if (episode != null)
            {
                episode.PlayCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}