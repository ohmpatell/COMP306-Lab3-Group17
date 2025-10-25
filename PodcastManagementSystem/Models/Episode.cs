using System.ComponentModel.DataAnnotations;

namespace PodcastManagementSystem.Models
{
    public class Episode
    {
        public int EpisodeID { get; set; }

        [Required]
        public int PodcastID { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Duration { get; set; }

        public int PlayCount { get; set; }

        public string AudioFileURL { get; set; }

        public string VideoFileURL { get; set; }

        public string Topic { get; set; }

        public string Host { get; set; }

        public Podcast Podcast { get; set; }
    }
}