using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WritingPlatformAPI.Authentication;

namespace WritingPlatformAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Work> Works{ get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkGenre> WorkGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<WorkGenre>(wg =>
            {
                wg.HasKey(k => new { k.WorkId, k.GenreId });

                wg.HasOne(pt => pt.Work)
                .WithMany(p => p.WorkGenres)
                .HasForeignKey(pt => pt.WorkId);

                wg.HasOne(pt => pt.Genre)
                .WithMany(t => t.GenreWorks)
                .HasForeignKey(pt => pt.GenreId);
            });
            builder.Entity<Genre>(g =>
            {
                g.HasIndex(n => n.Name).IsUnique();
            });
            base.OnModelCreating(builder);
        }
    }
}
