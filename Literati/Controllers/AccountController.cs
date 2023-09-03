using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using Literati.Data;
using Literati.Models;
using Literati.Settings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Literati.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IMapper mapper;
		private readonly IMailingService _mailingService;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMapper mapper, IMailingService mailingService)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.mapper = mapper;
			this._mailingService = mailingService;
		}

		#region Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = model.Username,
					Email = model.Email,
					CreatedDate = DateTime.Now,
				};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					//create Link
					var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
					var url = Url.Action("ConfirmEmail", "Account", new { token = token, userId = user.Id }, Request.Scheme);
					//Send Email
					StringBuilder body = new StringBuilder();
					body.AppendLine("UNDEFINED Application: Email Confirmation");
					body.AppendFormat("to confirm your email, you should <a href='{0}'> click here </a>", url);
					var subject = "Email Confirmation";
					await _mailingService.SendEmailAsync(user.Email, subject, body.ToString());

					return RedirectToAction("RequireEmailConfirm");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			TempData["Error"] = "Invalid Username or Password!";
			return View(model);
		}

		[HttpGet]
		public IActionResult RequireEmailConfirm()
		{
			return View();
		}


		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByIdAsync(model.UserId);
				if (user != null)
				{
					if (!user.EmailConfirmed)
					{
						var result = await userManager.ConfirmEmailAsync(user, model.Token);
						if (result.Succeeded)
						{
							TempData["Success"] = "Your Email already confirmed";
							return RedirectToAction("Login");
						}
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
					else
					{

						TempData["Success"] = "Your Email already confirmed";
					}

				}

			}
			return View();
		}

		#endregion

		#region Login-Logout

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{

			if (ModelState.IsValid)
			{
				var existedUser = await userManager.FindByNameAsync(model.Username);
				if (existedUser == null)
				{
					TempData["Error"] = "Invalid Username or Password!";
					return View(model);
				}

				if (!existedUser.IsBlocked)
				{
					var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
					}
					else if (result.IsNotAllowed)
					{
						TempData["Error"] = "Require Email Confirmation!";
					}
				}
				else
				{
					TempData["Error"] = "This Account has been Blocked!";
				}

			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{

			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Profile

		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			var result = await userManager.GetUserAsync(User);
			if (result != null)
			{
				var model = mapper.Map<UserViewModel>(result);
				return View(model);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> Profile(UserViewModel model)
		{
			if (ModelState.IsValid)
			{
				var CurrentUser = await userManager.GetUserAsync(User);
				if (CurrentUser != null)
				{
					CurrentUser.UserName = model.UserName;
					var result = await userManager.UpdateAsync(CurrentUser);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				else
				{
					return NotFound();
				}
			}
			return View(model);
		}

		#endregion

		#region Change Password
		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			var CurrentUser = await userManager.GetUserAsync(User);


			if (CurrentUser != null)
			{
				if (ModelState.IsValid)
				{
					var result = await userManager.ChangePasswordAsync(CurrentUser, model.CurrentPassword, model.NewPassword);
					if (result.Succeeded)
					{
						await signInManager.SignOutAsync();
						return RedirectToAction("Login", "Account");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				else
				{
					return NotFound();
				}
			}
			return View("Profile", mapper.Map<UserViewModel>(CurrentUser));
		}

		#endregion

		#region Forgot Password

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var existedUser = await userManager.FindByEmailAsync(model.Email);
				if (existedUser != null)
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(existedUser);
					var url = Url.Action("ResetPassword", "Account", new { token, model.Email }, Request.Scheme);
					//Send Email
					StringBuilder body = new StringBuilder();
					body.AppendLine("UNDEFINED Application: Reset Password");
					body.AppendFormat("to Reset your Password, Please <a href='{0}'> click here! </a>", url);
					var subject = "Reset Password";
					await _mailingService.SendEmailAsync(model.Email, subject, body.ToString());

					TempData["Success"] = "please check your Email for Recovery!";
					return RedirectToAction("Login", "Account");
				}
				else
				{
					TempData["Error"] = "Email Not Existed!";
					return View();
				}
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ResetPassword(VerifyResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var existedUser = await userManager.FindByEmailAsync(model.Email);
				if (existedUser != null)
				{
					var isValid = await userManager.VerifyUserTokenAsync(existedUser, TokenOptions.DefaultProvider, "ResetPassword", model.Token);
					if (isValid)
					{
						return View();
					}
					else
					{
						TempData["Error"] = "Token is  Not Valid!";
					}
				}

			}
			return RedirectToAction("Login", "Account");
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var existedUser = await userManager.FindByEmailAsync(model.Email);
				if (existedUser != null)
				{
					var result = await userManager.ResetPasswordAsync(existedUser, model.Token, model.NewPassword);
					if (result.Succeeded)
					{
						TempData["Error"] = "Reset Password Completed Successfully!";
						return RedirectToAction("Login", "Account");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

				}
			}
			return View(model);
		}

		#endregion

	}
}
