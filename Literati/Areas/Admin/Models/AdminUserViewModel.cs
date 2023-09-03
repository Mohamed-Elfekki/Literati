namespace Literati.Areas.Admin.Models
{
    public class AdminUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }


        public bool IsBlocked { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
