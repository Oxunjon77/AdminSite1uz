using EntityLayer.Concreat;
using Microsoft.EntityFrameworkCore;

namespace DataAsseccLayer.Concreat
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Photos> Photos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .HasOne(n => n.Users)
                .WithMany(u => u.news)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Photos>()
                .HasOne(p => p.News)
                .WithMany(n => n.Images)
                .HasForeignKey(p => p.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasKey(u => u.id);

            modelBuilder.Entity<News>()
                .Navigation(n => n.Users)
                .AutoInclude();
        }
    }
}
