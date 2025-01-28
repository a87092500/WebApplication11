using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    // 對應資料庫的 Users 資料表
    public DbSet<User> Users { get; set; }
}
