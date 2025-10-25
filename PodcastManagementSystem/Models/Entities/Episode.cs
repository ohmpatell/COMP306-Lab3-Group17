using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PodcastManagementSystem.Models.Entities
{
    public class Episode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EpisodeID { get; set; }

        [Required]
        public int PodcastID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

        public int Duration { get; set; } // in minutes

        public int PlayCount { get; set; } = 0;

        [StringLength(500)]
        public string? AudioFileURL { get; set; }

        [StringLength(500)]
        public string? VideoFileURL { get; set; }

        [StringLength(100)]
        public string? Topic { get; set; }

        [StringLength(100)]
        public string? Host { get; set; }

        // Navigation property
        [ForeignKey("PodcastID")]
        public virtual Podcast? Podcast { get; set; }
    }
}