using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Literati.Models;
using Literati.Services;

namespace Literati.Areas.Admin.Controllers
{
	public class UploadController : AdminBaseController
	{
		private readonly IUploadService uploadService;
		private readonly IWebHostEnvironment env;

		public UploadController(IUploadService uploadService, IWebHostEnvironment env)
		{
			this.uploadService = uploadService;
			this.env = env;
		}


		#region UserId
		private string UserId
		{
			get
			{
				return User.FindFirstValue(ClaimTypes.NameIdentifier);
			}
		}
		#endregion

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(PushUploadsViewModel model)
		{
			if (ModelState.IsValid)
			{
				var newName = Guid.NewGuid().ToString();
				var extention = Path.GetExtension(model.File.FileName);

				var fileName = string.Concat(newName, extention);
				var root = env.WebRootPath;
				var path = Path.Combine(root, "Uploads", fileName);

				using (var fs = System.IO.File.Create(path))
				{
					await model.File.CopyToAsync(fs);
				}
				await uploadService.CreateAsync(new PushUploadsViewModel
				{
					FilePath = model.File.FileName,
					HasedFilePath = fileName,
					Title = model.Title,
					Description = model.Description,
					Fulldescription = model.Fulldescription,
					UserId = UserId
				});
				return RedirectToAction("Index", "User");

			}


			return View(model);
		}
		#endregion

		#region Private
		[HttpGet]
		public IActionResult Private()
		{
			var result = uploadService.GetBy(UserId);
			if (result.FirstOrDefault() == null)
			{
				TempData["Success"] = "Sorry No Content Available!  PUSH Some.";
			}
			return View(result);
		}
		#endregion

		#region Delete
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await uploadService.FindAsync(id, UserId);
			if (result == null)
			{
				return NotFound();
			}
			await uploadService.DeleteAsync(id, UserId);
			return RedirectToAction("Index", "User");
		}
		#endregion

		#region Info
		[HttpGet]
		public async Task<IActionResult> Info(string id)
		{
			var result = await uploadService.FindAsync(id);

			if (result == null)
			{
				return NotFound();
			}
			return View(result);
		}
		#endregion

		#region Update

		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			var result = await uploadService.FindUpdateAsync(id, UserId);

			if (result == null)
			{
				return NotFound();
			}
			return View(result);
		}
		[HttpPost]

		public async Task<IActionResult> Update(UpdateUploadsViewModel model)
		{
			if (ModelState.IsValid)
			{
				var newName = Guid.NewGuid().ToString();
				var extention = Path.GetExtension(model.File.FileName);

				var fileName = string.Concat(newName, extention);
				var root = env.WebRootPath;
				var path = Path.Combine(root, "Uploads", fileName);

				using (var fs = System.IO.File.Create(path))
				{
					await model.File.CopyToAsync(fs);
				}
				await uploadService.UpdateAsync(new UpdateUploadsViewModel
				{
					FilePath = model.File.FileName,
					HasedFilePath = fileName,
					Title = model.Title,
					Description = model.Description,
					Fulldescription = model.Fulldescription,
					UploadDate = DateTime.Now,
					UserId = UserId,
					Id = model.Id
				});
				return RedirectToAction("Index", "User");
			}
			return View(model);

		}

		#endregion
	}
}
