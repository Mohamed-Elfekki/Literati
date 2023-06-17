using UNDEFINED.Areas.Admin.Models;
namespace UNDEFINED.Areas.Admin.Services
{
    public interface IUserService
    {
        IQueryable<AdminUserViewModel> GetAll();
        IQueryable<AdminUserViewModel> GetBlockedUsers();

        Task<OperationResult> ToggleBlockedUseerAsync(string UserId);

        Task<int> UserRegistrationCountAsync();

        Task<int> UserRegistrationCountAsync(int month);

        Task IntializeAsync();
    }
}
