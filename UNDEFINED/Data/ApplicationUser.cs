using Microsoft.AspNetCore.Identity;

namespace UNDEFINED.Data
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
