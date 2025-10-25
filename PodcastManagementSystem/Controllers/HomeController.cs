using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Services;

namespace PodcastManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEpisodeService _episodeService;

        public HomeController(ApplicationDbContext context, IEpisodeService episodeService)
        {
            _context = context;
            _episodeService = episodeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalPodcasts = await _context.Podcasts.CountAsync();
            ViewBag.TotalEpisodes = await _context.Episodes.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            var popularEpisodes = await _episodeService.GetMostPopularEpisodesAsync();
            ViewBag.PopularEpisodes = popularEpisodes.Take(5).ToList();

            var recentEpisodes = await _episodeService.GetEpisodesByDateAsync();
            ViewBag.RecentEpisodes = recentEpisodes.Take(5).ToList();

            return View();
        }
    }
}