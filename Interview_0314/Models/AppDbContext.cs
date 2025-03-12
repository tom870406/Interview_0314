using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Interview_0314.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }  // 產品資料表
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
