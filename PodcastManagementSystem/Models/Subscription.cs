namespace PodcastManagementSystem.Models
{
    public class Subscription
    {
        public int SubscriptionID { get; set; }
        public string UserID { get; set; }
        public int PodcastID { get; set; }
        public DateTime SubscribedDate { get; set; }

        public ApplicationUser User { get; set; }
        public Podcast Podcast { get; set; }
    }
}