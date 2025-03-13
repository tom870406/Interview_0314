using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Interview_0314.Models;

namespace Interview_0314.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }  // 產品資料表
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }        
        public DbSet<Player> Player { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Position)
                .WithMany()
                .HasForeignKey(p => p.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
