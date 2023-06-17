using Microsoft.AspNetCore.Mvc;

namespace UNDEFINED.Areas.Admin.Controllers
{
	public class HomeController : AdminBaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
