using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KarmaWebMVC.Data;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsAdminController : Controller
    {
        private readonly KarmaWebMVCContext _context;

        public ProductsAdminController(KarmaWebMVCContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductsAdmin
        //public async Task<IActionResult> Index()
        //{
        //    var karmaWebMVCContext = _context.Product.Include(p => p.Brand).Include(p => p.Category).Include(p => p.ProductStatus).Include(p => p.Size);
        //    return View(await karmaWebMVCContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 3; // Số lượng sản phẩm trên mỗi trang

            // Tính toán tổng số trang
            var totalItems = await _context.Product.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy sản phẩm cho trang hiện tại
            var products = await _context.Product
                                          .Include(p => p.Brand)
                                          .Include(p => p.Category)
                                          .Include(p => p.ProductStatus)
                                          .Include(p => p.Size)
                                          .Skip((pageNumber - 1) * pageSize) // Bỏ qua sản phẩm của các trang trước
                                          .Take(pageSize) // Lấy số lượng sản phẩm của trang hiện tại
                                          .ToListAsync();

            // Trả về view với sản phẩm và thông tin phân trang
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(products);
        }
        // GET: Admin/ProductsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/ProductsAdmin/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandDescription");
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryDescription");
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatus, "ProductStatusId", "StatusName");
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName");
            return View();
        }

        // POST: Admin/ProductsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductQuantity,ProductImage,CategoryId,BrandId,SizeId,ProductStatusId,Date")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandDescription", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryDescription", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatus, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // GET: Admin/ProductsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandDescription", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryDescription", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatus, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // POST: Admin/ProductsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductQuantity,ProductImage,CategoryId,BrandId,SizeId,ProductStatusId,Date")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

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
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandDescription", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryDescription", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatus, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // GET: Admin/ProductsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
