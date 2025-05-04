using KarmaWebMVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebMVC.Controllers
{
	[ViewComponent (Name ="_Category")]
	public class _CategoryViewComponents: ViewComponent
	{
		private readonly KarmaWebMVCContext _context;

		public _CategoryViewComponents(KarmaWebMVCContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke() {
			var _category = _context.Category.ToList();
			return View("_Category",_category);
		}

	}
}
