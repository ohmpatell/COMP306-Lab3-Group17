using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsByEpisodeAsync(string episodeId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(string episodeId, string commentId);
    }
}