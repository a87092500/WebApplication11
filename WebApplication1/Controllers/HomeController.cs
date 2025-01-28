using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // 模擬靜態資料
            var users = new List<User>
            {
                new User { Id = 1, Username = "Alice", Email = "alice@example.com" },
                new User { Id = 2, Username = "Bob", Email = "bob@example.com" },
                new User { Id = 3, Username = "Charlie", Email = "charlie@example.com" }
            };

            // 模擬異步行為
            await Task.CompletedTask;

            // 將靜態資料傳遞到視圖
            return View(users);
        }
    }
}
