using KarmaWebMVC.Data;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KarmaWebMVC.Controllers
{
	public class HomeController : Controller
	{
		//private readonly ILogger<HomeController> _logger;
		private readonly KarmaWebMVCContext _context;
		public HomeController(KarmaWebMVCContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var products=_context.Product.Include(p=> p.Brand).Include(p=> p.Category);
			return View(products.ToList());
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		


	}
}
