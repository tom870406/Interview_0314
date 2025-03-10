using Microsoft.EntityFrameworkCore;

namespace Interview_0314.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }  // 產品資料表
    }
}
