using Literati.Models;

namespace Literati.Services
{
	public interface IUploadService
	{
		Task CreateAsync(PushUploadsViewModel model);

		IQueryable<DisplayUploadsViewModel> GetBy(string id);

		Task<DisplayUploadsViewModel> FindAsync(string id);

		Task<DisplayUploadsViewModel> FindAsync(string id, string UserId);

		Task DeleteAsync(string id, string UserId);

		IQueryable<DisplayUploadsViewModel> GetAll();

		Task<int> GetUploadsCount();

		IQueryable<DisplayUploadsViewModel> Search(string term);


		Task<UpdateUploadsViewModel> FindUpdateAsync(string id, string UserId);

		Task UpdateAsync(UpdateUploadsViewModel model);

	}
}
