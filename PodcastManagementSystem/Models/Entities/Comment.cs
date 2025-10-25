namespace PodcastManagementSystem.Models.Entities
{
    // DynamoDB model - no EF attributes needed
    public class Comment
    {
        public string EpisodeID { get; set; } = string.Empty;
        public string CommentID { get; set; } = string.Empty;
        public string PodcastID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}