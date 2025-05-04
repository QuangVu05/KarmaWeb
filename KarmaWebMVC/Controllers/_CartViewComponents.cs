using KarmaWebMVC.Helpers;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebMVC.Controllers
{
    [ViewComponent(Name = "_Cart")]
    public class _CartViewComponents:ViewComponent
    {
        public IViewComponentResult Invoke() {
         var _cartItem = HttpContext.Session.Get<List<CartController>>(MySetting.CART_KEY) ?? new List<CartController>();
            return View("_Cart", _cartItem);
        }
    }
}
