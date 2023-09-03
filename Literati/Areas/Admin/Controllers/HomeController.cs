using Microsoft.AspNetCore.Mvc;

namespace Literati.Areas.Admin.Controllers
{
	public class HomeController : AdminBaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
