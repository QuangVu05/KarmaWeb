using KarmaWebMVC.Data;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace KarmaWebMVC.Controllers
{
	[ViewComponent (Name ="_Product")]
	public class _ProductViewController : ViewComponent
	{
		private readonly KarmaWebMVCContext _context;

		public _ProductViewController(KarmaWebMVCContext context)
		{
			_context = context;
		}
		public Task<IViewComponentResult> InvokeAsync()
		{

			return null ;
		}


	}
}
