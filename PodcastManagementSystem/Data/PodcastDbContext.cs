using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models.Entities;
using System.Runtime.Intrinsics.Arm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PodcastManagementSystem.Data
{
    public class PodcastDbContext : DbContext
    {
        public PodcastDbContext(DbContextOptions<PodcastDbContext> options)
            : base(options)
        {
        }

        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table configurations
            modelBuilder.Entity<Podcast>(entity =>
            {
                entity.HasKey(e => e.PodcastID);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.CreatorID).IsRequired();
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.HasKey(e => e.EpisodeID);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Topic).HasMaxLength(100);
                entity.Property(e => e.Host).HasMaxLength(100);

                entity.HasOne(e => e.Podcast)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(e => e.PodcastID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.SubscriptionID);

                entity.HasOne(s => s.Podcast)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(s => s.PodcastID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
