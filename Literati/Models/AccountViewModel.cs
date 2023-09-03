using System.ComponentModel.DataAnnotations;

namespace Literati.Models
{
	public class LoginViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		public string Username { get; set; }

		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}

	public class ChangePasswordViewModel
	{
		[Required]
		public string CurrentPassword { get; set; }
		[Required]
		public string NewPassword { get; set; }
		[Compare("NewPassword")]
		public string ConfirmNewPassword { get; set; }

	}

	public class ConfirmEmailViewModel
	{
		[Required]
		public string Token { get; set; }


		[Required]
		public string UserId { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

	public class VerifyResetPasswordViewModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required]
		public string NewPassword { get; set; }

		[Compare("NewPassword")]
		public string ConfirmNewPassword { get; set; }

		[Required]
		public string Token { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
