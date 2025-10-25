using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;
using PodcastManagementSystem.Services;
using System.Security.Claims;

namespace PodcastManagementSystem.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly IEpisodeService _episodeService;
        private readonly ICommentService _commentService;
        private readonly IS3Service _s3Service;
        private readonly ApplicationDbContext _context;

        public EpisodesController(IEpisodeService episodeService, ICommentService commentService, IS3Service s3Service, ApplicationDbContext context)
        {
            _episodeService = episodeService;
            _commentService = commentService;
            _s3Service = s3Service;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTopic, string searchHost, string sortBy)
        {
            List<Episode> episodes;

            if (!string.IsNullOrEmpty(searchTopic))
            {
                episodes = await _episodeService.GetEpisodesByTopicAsync(searchTopic);
            }
            else if (!string.IsNullOrEmpty(searchHost))
            {
                episodes = await _episodeService.GetEpisodesByHostAsync(searchHost);
            }
            else if (sortBy == "popular")
            {
                episodes = await _episodeService.GetMostPopularEpisodesAsync();
            }
            else if (sortBy == "date")
            {
                episodes = await _episodeService.GetEpisodesByDateAsync();
            }
            else
            {
                episodes = await _episodeService.GetAllEpisodesAsync();
            }

            ViewBag.SearchTopic = searchTopic;
            ViewBag.SearchHost = searchHost;
            ViewBag.SortBy = sortBy;

            return View(episodes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            await _episodeService.IncrementPlayCountAsync(id);

            var comments = await _commentService.GetCommentsByEpisodeAsync(id.ToString());
            ViewBag.Comments = comments;

            return View(episode);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _context.Users.FindAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            IQueryable<Podcast> podcasts = _context.Podcasts;

            if (currentUser.Role == UserRole.Podcaster)
            {
                podcasts = podcasts.Where(p => p.CreatorID == currentUser.Id);
            }

            ViewBag.Podcasts = new SelectList(await podcasts.ToListAsync(), "PodcastID", "Title");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            var currentUser = await _context.Users.FindAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = currentUser.Role == UserRole.Admin;
            var canEdit = isAdmin || episode.Podcast.CreatorID == currentUser.Id;

            if (!canEdit)
            {
                return Forbid();
            }

            IQueryable<Podcast> podcasts = _context.Podcasts;

            if (currentUser.Role == UserRole.Podcaster)
            {
                podcasts = podcasts.Where(p => p.CreatorID == currentUser.Id);
            }

            ViewBag.Podcasts = new SelectList(await podcasts.ToListAsync(), "PodcastID", "Title", episode.PodcastID);
            return View(episode);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Create(Episode episode, IFormFile audioFile, IFormFile videoFile)
        {
            if (audioFile != null)
            {
                episode.AudioFileURL = await _s3Service.UploadFileAsync(audioFile, "audio");
            }

            if (videoFile != null)
            {
                episode.VideoFileURL = await _s3Service.UploadFileAsync(videoFile, "video");
            }

            await _episodeService.AddEpisodeAsync(episode);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Edit(int id, Episode episode, IFormFile audioFile, IFormFile videoFile)
        {
            if (id != episode.EpisodeID)
            {
                return NotFound();
            }

            var existingEpisode = await _episodeService.GetEpisodeByIdAsync(id);

            if (audioFile != null)
            {
                if (!string.IsNullOrEmpty(existingEpisode.AudioFileURL))
                {
                    await _s3Service.DeleteFileAsync(existingEpisode.AudioFileURL);
                }
                episode.AudioFileURL = await _s3Service.UploadFileAsync(audioFile, "audio");
            }
            else
            {
                episode.AudioFileURL = existingEpisode.AudioFileURL;
            }

            if (videoFile != null)
            {
                if (!string.IsNullOrEmpty(existingEpisode.VideoFileURL))
                {
                    await _s3Service.DeleteFileAsync(existingEpisode.VideoFileURL);
                }
                episode.VideoFileURL = await _s3Service.UploadFileAsync(videoFile, "video");
            }
            else
            {
                episode.VideoFileURL = existingEpisode.VideoFileURL;
            }

            episode.PlayCount = existingEpisode.PlayCount;
            episode.ReleaseDate = existingEpisode.ReleaseDate;

            await _episodeService.UpdateEpisodeAsync(episode);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(id);

            if (!string.IsNullOrEmpty(episode.AudioFileURL))
            {
                await _s3Service.DeleteFileAsync(episode.AudioFileURL);
            }

            if (!string.IsNullOrEmpty(episode.VideoFileURL))
            {
                await _s3Service.DeleteFileAsync(episode.VideoFileURL);
            }

            await _episodeService.DeleteEpisodeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(int episodeId, string commentText)
        {
            var episode = await _episodeService.GetEpisodeByIdAsync(episodeId);

            var comment = new Comment
            {
                EpisodeID = episodeId.ToString(),
                PodcastID = episode.PodcastID.ToString(),
                UserID = User.Identity.Name,
                Username = User.Identity.Name,
                Text = commentText
            };

            await _commentService.AddCommentAsync(comment);

            return RedirectToAction(nameof(Details), new { id = episodeId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditComment(string episodeId, string commentId, string commentText)
        {
            var comment = await _commentService.GetCommentAsync(episodeId, commentId);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);

            if (comment == null)
            {
                return NotFound();
            }

            // Admin can edit any comment, users can only edit their own
            if (currentUser.Role != UserRole.Admin && comment.UserID != User.Identity.Name)
            {
                TempData["Error"] = "You can only edit your own comments";
                return RedirectToAction(nameof(Details), new { id = episodeId });
            }

            // Check 24-hour window for non-admin users
            if (currentUser.Role != UserRole.Admin)
            {
                var commentTime = DateTime.Parse(comment.Timestamp);
                if (DateTime.UtcNow - commentTime > TimeSpan.FromHours(24))
                {
                    TempData["Error"] = "Cannot edit comment after 24 hours";
                    return RedirectToAction(nameof(Details), new { id = episodeId });
                }
            }

            comment.Text = commentText;
            await _commentService.UpdateCommentAsync(comment);

            return RedirectToAction(nameof(Details), new { id = episodeId });
        }
    }
}