using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class MyDbContext : DbContext
    {
        // 建構式，使用 DbContextOptions<MyDbContext> 作為參數
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        // 你的資料表對應 (Entity) - 例如 Users
        public DbSet<User> Users { get; set; } = null!;
    }

    
}
