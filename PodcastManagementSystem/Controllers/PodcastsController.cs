using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;
using System.Security.Claims;

namespace PodcastManagementSystem.Controllers
{
    public class PodcastsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PodcastsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var podcasts = await _context.Podcasts
                .Include(p => p.Creator)
                .Include(p => p.Episodes)
                .ToListAsync();
            return View(podcasts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var podcast = await _context.Podcasts
                .Include(p => p.Creator)
                .Include(p => p.Episodes)
                .FirstOrDefaultAsync(p => p.PodcastID == id);

            if (podcast == null)
            {
                return NotFound();
            }

            return View(podcast);
        }

        [Authorize]
        public IActionResult Create()
        {
            var currentUser = _context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (currentUser.Role != UserRole.Podcaster && currentUser.Role != UserRole.Admin)
            {
                return Forbid();
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Podcast podcast)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);

            if (currentUser.Role != UserRole.Podcaster && currentUser.Role != UserRole.Admin)
            {
                return Forbid();
            }

            podcast.CreatorID = currentUserId;
            podcast.CreatedDate = DateTime.Now;

            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);
            var isAdmin = currentUser.Role == UserRole.Admin;
            var canEdit = isAdmin || podcast.CreatorID == currentUserId;

            if (!canEdit)
            {
                return Forbid();
            }

            return View(podcast);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Podcast podcast)
        {
            if (id != podcast.PodcastID)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);
            var existingPodcast = await _context.Podcasts.FindAsync(id);
            var isAdmin = currentUser.Role == UserRole.Admin;
            var canEdit = isAdmin || existingPodcast.CreatorID == currentUserId;

            if (!canEdit)
            {
                return Forbid();
            }

            existingPodcast.Title = podcast.Title;
            existingPodcast.Description = podcast.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var podcast = await _context.Podcasts
                .Include(p => p.Creator)
                .Include(p => p.Episodes)
                .FirstOrDefaultAsync(p => p.PodcastID == id);

            if (podcast == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);
            var isAdmin = currentUser.Role == UserRole.Admin;
            var canDelete = isAdmin || podcast.CreatorID == currentUserId;

            if (!canDelete)
            {
                return Forbid();
            }

            return View(podcast);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);
            var isAdmin = currentUser.Role == UserRole.Admin;
            var canDelete = isAdmin || podcast.CreatorID == currentUserId;

            if (!canDelete)
            {
                return Forbid();
            }

            _context.Podcasts.Remove(podcast);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}