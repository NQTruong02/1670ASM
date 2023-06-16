using ASM_AppWeb_Bicycle_Shop.Infrastructure;
using ASM_AppWeb_Bicycle_Shop.Models.ViewModel;
using ASM_AppWeb_Bicycle_Shop.Models;
using ASM_Bicycle_Shops.Models;
using Microsoft.AspNetCore.Mvc;
using ASM_AppWeb_Bicycle_Shop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Hosting;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CartController(ASM_AppWeb_Bicycle_ShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            ViewBag.CartUserName = User.Identity.Name;
			ViewBag.Date= DateTime.Now.ToString("yyyy-MM-dd");
			return View(cartVM);
        }

        public async Task<IActionResult> Add(int id)
        {
            Product product = await _context.Product.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;  
            }

            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "The product has been added!";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(p => p.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CheckOut()
        {
            var name = User.Identity.Name;
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            foreach(CartItem item in cart)
            {
                Shop createData = new Shop();
                createData.ProductName = item.ProductName;
                createData.ProductPrice = item.Price;
                createData.Quantity = item.Quantity;
                createData.CustomerName = name;
                createData.Purchase_date = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Add(createData);
                    await _context.SaveChangesAsync();
                    return View();
                }
            }
            return View();
        }
    }
}
