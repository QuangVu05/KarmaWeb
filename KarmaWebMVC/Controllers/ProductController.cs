using KarmaWebMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebMVC.Controllers
{
    public class ProductController : Controller
    {
		private readonly KarmaWebMVCContext _context;

		public ProductController(KarmaWebMVCContext context)
		{
			_context = context;
		}
		public IActionResult Index(string query, string brandSlug, string categorySlug)
		{// Lấy tất cả sản phẩm bao gồm thông tin Brand, Category và Size
			var products = _context.Product
				.Include(p => p.Brand)
				.Include(p => p.Category)
				.Include(p => p.Size)
				.AsQueryable(); // Chuyển đổi thành IQueryable để có thể áp dụng điều kiện

			
			// Kiểm tra xem có từ khóa tìm kiếm không
			if (!string.IsNullOrEmpty(query))
			{
				// Lọc theo tên sản phẩm và tên nhãn hàng
				products = products.Where(p =>
					p.ProductName.ToLower().Contains(query.ToLower()) ||
					p.Brand.BrandName.ToLower().Contains(query.ToLower()));
			}
            // Kiểm tra xem có thương hiệu nào được chọn không
            if (!string.IsNullOrEmpty(brandSlug))
            {
                // Lọc theo thương hiệu
                products = products.Where(p => p.BrandId.ToString() == brandSlug);
            }

            // Kiểm tra xem có danh mục nào được chọn không
            if (!string.IsNullOrEmpty(categorySlug))
            {
                // Lọc theo danh mục
                products = products.Where(p => p.CategoryId.ToString() == categorySlug);
            }


            return View(products.ToList());
		}

        public IActionResult Detail(int id)
        {
            // Lấy thông tin sản phẩm theo id
            var product = _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Brand)
                .Include(p => p.Size)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu sản phẩm không tồn tại
            }

            
            return View(product); // Nếu không cần sản phẩm liên quan, chỉ trả về sản phẩm
        }
    }
}
