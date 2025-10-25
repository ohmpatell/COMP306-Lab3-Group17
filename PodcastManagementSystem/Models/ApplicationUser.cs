using Microsoft.AspNetCore.Identity;

namespace PodcastManagementSystem.Models
{
    public enum UserRole
    {
        Podcaster,
        Listener,
        Admin
    }

    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
    }
}