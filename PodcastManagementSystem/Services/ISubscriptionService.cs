using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public interface ISubscriptionService
    {
        Task<bool> IsSubscribedAsync(string userId, int podcastId);
        Task SubscribeAsync(string userId, int podcastId);
        Task UnsubscribeAsync(string userId, int podcastId);
        Task<List<Subscription>> GetUserSubscriptionsAsync(string userId);
        Task<int> GetSubscriberCountAsync(int podcastId);
    }
}