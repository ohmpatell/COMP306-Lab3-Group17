using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;
using PodcastManagementSystem.Services;
using System.Security.Claims;

namespace PodcastManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEpisodeService _episodeService;
        private readonly ICommentService _commentService;

        public HomeController(ApplicationDbContext context, IEpisodeService episodeService, ICommentService commentService)
        {
            _context = context;
            _episodeService = episodeService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);

            ViewBag.UserRole = currentUser.Role;
            ViewBag.TotalPodcasts = await _context.Podcasts.CountAsync();
            ViewBag.TotalEpisodes = await _context.Episodes.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            if (currentUser.Role == UserRole.Podcaster || currentUser.Role == UserRole.Admin)
            {
                var userPodcasts = currentUser.Role == UserRole.Admin
                    ? await _context.Podcasts.Include(p => p.Episodes).ToListAsync()
                    : await _context.Podcasts.Include(p => p.Episodes).Where(p => p.CreatorID == currentUserId).ToListAsync();

                ViewBag.MyPodcasts = userPodcasts;

                var episodeIds = userPodcasts.SelectMany(p => p.Episodes.Select(e => e.EpisodeID)).ToList();
                var userEpisodes = await _context.Episodes
                    .Include(e => e.Podcast)
                    .Where(e => episodeIds.Contains(e.EpisodeID))
                    .ToListAsync();

                ViewBag.TopEpisodesByViews = userEpisodes.OrderByDescending(e => e.PlayCount).Take(5).ToList();

                ViewBag.TotalViews = userEpisodes.Sum(e => e.PlayCount);

                var commentCounts = new Dictionary<int, int>();
                foreach (var episode in userEpisodes)
                {
                    try
                    {
                        var comments = await _commentService.GetCommentsByEpisodeAsync(episode.EpisodeID.ToString());
                        commentCounts[episode.EpisodeID] = comments.Count;
                    }
                    catch
                    {
                        commentCounts[episode.EpisodeID] = 0;
                    }
                }
                ViewBag.CommentCounts = commentCounts;

                ViewBag.TopEpisodesByComments = userEpisodes
                    .OrderByDescending(e => commentCounts.ContainsKey(e.EpisodeID) ? commentCounts[e.EpisodeID] : 0)
                    .Take(5)
                    .ToList();

                ViewBag.RecentEpisodes = userEpisodes.OrderByDescending(e => e.ReleaseDate).Take(5).ToList();

                var subscriberCounts = new Dictionary<int, int>();
                foreach (var podcast in userPodcasts)
                {
                    subscriberCounts[podcast.PodcastID] = await _context.Subscriptions.CountAsync(s => s.PodcastID == podcast.PodcastID);
                }
                ViewBag.SubscriberCounts = subscriberCounts;

                ViewBag.TotalSubscribers = subscriberCounts.Values.Sum();
            }
            else
            {
                var popularEpisodes = await _episodeService.GetMostPopularEpisodesAsync();
                ViewBag.PopularEpisodes = popularEpisodes.Take(5).ToList();

                var recentEpisodes = await _episodeService.GetEpisodesByDateAsync();
                ViewBag.RecentEpisodes = recentEpisodes.Take(5).ToList();
            }

            return View();
        }
    }
}