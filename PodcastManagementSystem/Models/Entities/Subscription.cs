using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PodcastManagementSystem.Models.Entities
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [Required]
        public int PodcastID { get; set; }

        public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("PodcastID")]
        public virtual Podcast? Podcast { get; set; }
    }
}