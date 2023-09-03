using Microsoft.AspNetCore.Identity;

namespace Literati.Data
{
	public class ApplicationUser : IdentityUser
	{
		public bool IsBlocked { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
