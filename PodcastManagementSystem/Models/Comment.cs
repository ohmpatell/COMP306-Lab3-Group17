namespace PodcastManagementSystem.Models
{
    public class Comment
    {
        public string EpisodeID { get; set; }
        public string CommentID { get; set; }
        public string PodcastID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public string Timestamp { get; set; }
    }
}