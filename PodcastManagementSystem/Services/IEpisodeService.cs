using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public interface IEpisodeService
    {
        Task<List<Episode>> GetAllEpisodesAsync();
        Task<Episode> GetEpisodeByIdAsync(int id);
        Task<List<Episode>> GetEpisodesByTopicAsync(string topic);
        Task<List<Episode>> GetEpisodesByHostAsync(string host);
        Task<List<Episode>> GetMostPopularEpisodesAsync();
        Task<List<Episode>> GetEpisodesByDateAsync();
        Task AddEpisodeAsync(Episode episode);
        Task UpdateEpisodeAsync(Episode episode);
        Task DeleteEpisodeAsync(int id);
        Task IncrementPlayCountAsync(int id);
    }
}