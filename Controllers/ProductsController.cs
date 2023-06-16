using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_AppWeb_Bicycle_Shop.Data;
using ASM_Bicycle_Shops.Models;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Reflection;
using ClosedXML.Excel;
using ASM_AppWeb_Bicycle_Shop.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ASM_AppWeb_Bicycle_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ASM_AppWeb_Bicycle_ShopContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(ASM_AppWeb_Bicycle_ShopContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Admin/ManagerProducts/Index
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Index")]
        public async Task<IActionResult> Index()
        {
            var data = await _context.Product.ToListAsync();
            if (data != null & data.Count != 0)
            {
                ViewBag.mess = "True";
            }
            else
            {
                ViewBag.mess = "False";
            }
            return _context.Product != null ?
                          View(data) :
                          Problem("Entity set 'ASM_Bicycle_ShopsContext.Product'  is null.");
        }

        // GET: Products/ListProduct
        public async Task<IActionResult> ListProduct(string searchString)
        {
            var data = await _context.Product.ToListAsync();
            if (data == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var product = from m in data
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.ProductName!.Contains(searchString));
                return View(product);
            }
            else
            {
                return _context.Product != null ?
                                  View(data) :
                                  Problem("Entity set 'ASM_Bicycle_ShopsContext.Product'  is null.");
            }
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/ProductDetails/5
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/ManagerProducts/Create
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Create")]
        public IActionResult Create()
        {
            ViewBag.category = new SelectList(GetCategory());
            return View();
        }

        // POST: Admin/ManagerProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Create")]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductPriceSale,ProductDecription,CategoryName,Status,ImageFile")] Product product)
        {
            string uniqueFileName = null;
            if (product.ImageFile != null)
            {
                string ImageUpLoadFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadImg");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;

                string filepath = Path.Combine(ImageUpLoadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);
                }
                product.EmpPhotoPath = "~/wwwroot/UploadImg";
                product.EmpFileName = uniqueFileName;

                if (ModelState.IsValid)
                {
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.category = new SelectList(GetCategory());
            return View(product);
        }

        // GET: Admin/ManagerProducts/Edit/5
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.category = new SelectList(GetCategory());
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductPriceSale,ProductDecription,CategoryName,Status,ImageFile")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            string uniqueFileName = null;
            if (product.ImageFile != null)
            {
                string ImageUpLoadFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadImg");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;

                string filepath = Path.Combine(ImageUpLoadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);
                }
                product.EmpPhotoPath = "~/wwwroot/UploadImg";
                product.EmpFileName = uniqueFileName;


                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.ProductId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.category = new SelectList(GetCategory());
            return View(product);
        }

        // GET: Admin/ManagerProducts/Delete/5
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("Admin/ManagerProducts/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ASM_AppWeb_Bicycle_ShopContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        public List<string> GetCategory()
        {
            List<CategoryProduct> categoryProducts = _context.CategoryProduct.ToList<CategoryProduct>();
            List<string> categoryName = new List<string>();
            foreach (var i in categoryProducts)
            {
                categoryName.Add(i.CategoryProductName);
            }
            return categoryName;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ExportExcel()
        {
            try
            {
                var data = _context.Product.ToList();
                if (data != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ToConvertDataTable(data.ToList()));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            string fileName = $"Product_{DateTime.Now.ToString("dd/MM/yyyy")}.xlsx";
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet", fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        public DataTable ToConvertDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] propInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propInfo)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var value = new object[propInfo.Length];
                for (int i = 0; i < propInfo.Length; i++)
                {
                    value[i] = propInfo[i].GetValue(item, null);
                }
                dt.Rows.Add(value);
            }
            return dt;
        }
    }
}
