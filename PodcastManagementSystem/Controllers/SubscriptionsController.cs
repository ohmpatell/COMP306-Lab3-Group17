using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodcastManagementSystem.Services;
using System.Security.Claims;

namespace PodcastManagementSystem.Controllers
{
    [Authorize]
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(userId);
            return View(subscriptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(int podcastId, string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _subscriptionService.SubscribeAsync(userId, podcastId);

            TempData["Success"] = "Successfully subscribed to podcast!";

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Details", "Podcasts", new { id = podcastId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(int podcastId, string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _subscriptionService.UnsubscribeAsync(userId, podcastId);

            TempData["Success"] = "Successfully unsubscribed from podcast!";

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Details", "Podcasts", new { id = podcastId });
        }
    }
}