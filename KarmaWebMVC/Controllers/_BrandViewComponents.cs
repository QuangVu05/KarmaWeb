using KarmaWebMVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebMVC.Controllers
{
	[ViewComponent (Name ="_Brand")]
	public class _BrandViewComponents: ViewComponent
	{
		private readonly KarmaWebMVCContext _context;

		public _BrandViewComponents(KarmaWebMVCContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke() {
			var _brand = _context.Brand.ToList();
			return View("_Brand",_brand);
		}

	}
}
