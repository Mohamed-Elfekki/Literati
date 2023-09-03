using Literati.Areas.Admin.Models;
namespace Literati.Areas.Admin.Services
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
