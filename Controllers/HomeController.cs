using ASM_AppWeb_Bicycle_Shop.Data;
using ASM_AppWeb_Bicycle_Shop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;

        public HomeController(ASM_AppWeb_Bicycle_ShopContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            var data = _context.News.ToList();
            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}