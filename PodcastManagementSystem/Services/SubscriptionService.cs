using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSubscribedAsync(string userId, int podcastId)
        {
            return await _context.Subscriptions
                .AnyAsync(s => s.UserID == userId && s.PodcastID == podcastId);
        }

        public async Task SubscribeAsync(string userId, int podcastId)
        {
            if (await IsSubscribedAsync(userId, podcastId))
                return;

            var subscription = new Subscription
            {
                UserID = userId,
                PodcastID = podcastId,
                SubscribedDate = DateTime.Now
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeAsync(string userId, int podcastId)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserID == userId && s.PodcastID == podcastId);

            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Subscription>> GetUserSubscriptionsAsync(string userId)
        {
            return await _context.Subscriptions
                .Include(s => s.Podcast)
                    .ThenInclude(p => p.Episodes)
                .Include(s => s.User)
                .Where(s => s.UserID == userId)
                .OrderByDescending(s => s.SubscribedDate)
                .ToListAsync();
        }

        public async Task<int> GetSubscriberCountAsync(int podcastId)
        {
            return await _context.Subscriptions
                .Where(s => s.PodcastID == podcastId)
                .CountAsync();
        }
    }
}