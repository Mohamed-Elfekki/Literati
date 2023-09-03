using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using Literati.Data;
using Literati.Models;
using Literati.Services;

namespace Literati.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUploadService uploadService;

		public HomeController(ILogger<HomeController> logger, IUploadService uploadService)
		{
			_logger = logger;
			this.uploadService = uploadService;
		}

		#region Not Used
		/*[HttpGet]
			public IActionResult Index()
			{
				var result = _db.Uploads.OrderByDescending(x => x.UploadDate).ToList()
				.Select(u => new DisplayUploadsViewModel
				{
					Id = u.Id,
					FilePath = u.FilePath,
					HasedFilePath = u.HasedFilePath,
					Title = u.Title,
					Description = u.Description,
					Fulldescription = u.Fulldescription,
					UploadDate = u.UploadDate,
				})
				.Take(4);
				//ViewBag.Popular = result;
				return View(result);
			}*/
		#endregion

		#region Index

		[HttpGet]
		public async Task<IActionResult> Index(int requiredPage = 1)
		{
			var result = uploadService.GetAll();

			var model = await GetPagedData(result, requiredPage);
			return View(model);
		}


		private async Task<List<DisplayUploadsViewModel>> GetPagedData(IQueryable<DisplayUploadsViewModel> result, int requiredPage = 1)
		{
			const int pageSize = 4;
			decimal rowsCount = await uploadService.GetUploadsCount();

			var pagesCount = Math.Ceiling(rowsCount / pageSize);

			if (requiredPage > pagesCount)
			{
				requiredPage = 1;
			}
			if (requiredPage <= 0)
			{
				requiredPage = 1;
			}

			int skipCount = (requiredPage - 1) * pageSize;

			//select count(*) from Uploads ;
			var pagedData = await result
				.Skip(skipCount)
				.Take(pageSize)
				.ToListAsync();
			ViewBag.CurrentPage = requiredPage;
			ViewBag.PagesCount = pagesCount;

			return pagedData;
		}

		#endregion

		#region Result

		[HttpGet]
		public IActionResult Result()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Result(string term)
		{
			var result = uploadService.Search(term);
			if (result.FirstOrDefault() ==null)
			{
				TempData["Success"] = "Sorry No Content Available, Try Another Term!";
			}
			return View(result);
		}

		#endregion

		#region Privacy
		[HttpGet]
		public IActionResult Privacy()
		{
			return View();
		}
		#endregion

		#region Error
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		#endregion
	}
}