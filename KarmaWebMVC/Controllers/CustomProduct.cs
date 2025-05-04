using Microsoft.AspNetCore.Mvc;

namespace KarmaWebMVC.Controllers
{
	public class CustomProduct : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
