using KarmaWebMVC.Data;
using KarmaWebMVC.Helpers;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly KarmaWebMVCContext _context;

        public CartController(KarmaWebMVCContext context)
        {
            _context = context;
        }
        public List<Cart> Cart => HttpContext.Session.Get<List<Cart>>(MySetting.CART_KEY) ?? new List<Cart>();
        public IActionResult Index()
        {

            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity=1)
        {
            var cart = Cart;

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var item = cart.SingleOrDefault(p => p.CartId == id);
            if (item == null)
            {
                // Tìm sản phẩm trong cơ sở dữ liệu
                var product = _context.Product.SingleOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return NotFound(); // Nếu không tìm thấy sản phẩm, trả về NotFound
                }

                // Tạo mới một đối tượng Cart và thêm vào giỏ hàng
                item = new Cart
                {
                    CartId = product.ProductId, // Sử dụng ProductId từ sản phẩm
                    CartName = product.ProductName, // Sử dụng ProductName từ sản phẩm
                    CartPrice = product.ProductPrice ?? 0, // Sử dụng ProductPrice
                    CartImage = product.ProductImage ?? string.Empty, // Sử dụng ProductImage
                    CartQuantity = quantity
                };
                cart.Add(item);
            }
            else
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                item.CartQuantity += quantity;
            }

            // Cập nhật lại giỏ hàng vào session
            HttpContext.Session.Set(MySetting.CART_KEY, cart);

            return RedirectToAction("Index"); // Chuyển hướng về trang giỏ hàng
        }
        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = Cart;

            var item = cart.SingleOrDefault(p => p.CartId == id);
            if (item != null)
            {
                // Cập nhật số lượng sản phẩm
                if (quantity > 0)
                {
                    item.CartQuantity = quantity;
                }
                else
                {
                    // Nếu số lượng <= 0, xóa sản phẩm khỏi giỏ hàng
                    cart.Remove(item);
                }
            }

            // Lưu lại giỏ hàng vào session
            HttpContext.Session.Set(MySetting.CART_KEY, cart);

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveCart(int id)
        {
            var cart = Cart;

            var item = cart.SingleOrDefault(p => p.CartId == id);
            if (item != null)
            {
                // Xóa sản phẩm khỏi giỏ hàng
                cart.Remove(item);
            }

            // Lưu lại giỏ hàng vào session
            HttpContext.Session.Set(MySetting.CART_KEY, cart);

            return RedirectToAction("Index");
        }
    }
}
    

