
using Npgsql;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

// builder を宣言
var builder = WebApplication.CreateBuilder(args);
// 設定ファイルの読み込み
builder.Configuration
       //.SetBasePath("/app") // 容器内の基礎パス（必要に応じて削除可能）
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// 讀取 ConnectionStrings 的設定
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// 使用 PostgreSQL Provider 註冊 DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString)
);
// 啟用 Controller 和 Views 支援
builder.Services.AddControllersWithViews();
var app = builder.Build();
// 錯誤處理設定
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
// 測試路由 /test
app.MapGet("/test", async () =>
{
    try
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        var cmd = new NpgsqlCommand("SELECT Id, Username, Email FROM Users", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        var sb = new StringBuilder();
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32(0);
            var username = reader.GetString(1);
            var email = reader.GetString(2);
            sb.AppendLine($"Id={id}, Username={username}, Email={email}");
        }
        return sb.Length > 0 ? sb.ToString() : "查無任何記錄！";
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "資料庫查詢失敗");
        return "系統錯誤，請稍後再試。";
    }
});
// 測試路由 /testjson
app.MapGet("/testjson", async () =>
{
    try
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        var cmd = new NpgsqlCommand("SELECT Id, Username, Email FROM Users", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        var results = new List<dynamic>();
        while (await reader.ReadAsync())
        {
            results.Add(new
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }
        return Results.Json(results);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "資料庫查詢失敗");
        return Results.Problem("系統錯誤，請稍後再試。");
    }
});
// 預設路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();