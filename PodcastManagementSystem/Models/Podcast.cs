using System.ComponentModel.DataAnnotations;

namespace PodcastManagementSystem.Models
{
    public class Podcast
    {
        public int PodcastID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string CreatorID { get; set; }

        public DateTime CreatedDate { get; set; }

        public ApplicationUser Creator { get; set; }
        public ICollection<Episode> Episodes { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}