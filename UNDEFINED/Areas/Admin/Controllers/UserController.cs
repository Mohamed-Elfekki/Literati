using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNDEFINED.Areas.Admin.Services;

namespace UNDEFINED.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

		#region Index
		[HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await userService.GetAll()
                .ToListAsync();
            return View(result);
        }
		#endregion

		#region Block

		[HttpGet]
        public async Task<IActionResult> Blocked()
        {
            var result = await userService.GetBlockedUsers()
                .ToListAsync();
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> Block(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var result = await userService.ToggleBlockedUseerAsync(userId);
                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
                return RedirectToAction("Index", "User");
            }
            TempData["Error"] = "User Id Not Found!";
            return RedirectToAction("Index", "User");
        }

		#endregion

		#region UserCount
		public async Task<IActionResult> UserCount()
        {
            var totalUserCount = await userService.UserRegistrationCountAsync();
            var month = DateTime.Today.Month;

            var monthUsersCount = await userService.UserRegistrationCountAsync(month);

            return Json(new { total = totalUserCount, month = monthUsersCount });
        }
		#endregion
	}
}
