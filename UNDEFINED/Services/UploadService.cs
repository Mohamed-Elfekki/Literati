using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using UNDEFINED.Data;
using UNDEFINED.Models;

namespace UNDEFINED.Services
{
	public class UploadService : IUploadService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper mapper;

		public UploadService(ApplicationDbContext context, IMapper mapper)
		{
			this._db = context;
			this.mapper = mapper;
		}



		public async Task CreateAsync(PushUploadsViewModel model)
		{
			var mappedObj = mapper.Map<Upload>(model);
			await _db.Uploads.AddAsync(mappedObj);
			await _db.SaveChangesAsync();
		}


		public async Task<DisplayUploadsViewModel> FindAsync(string id)
		{
			var result = await _db.Uploads.FindAsync(id);
			if (result != null)
			{
				return mapper.Map<DisplayUploadsViewModel>(result);
			}
			return null;
		}

		public async Task DeleteAsync(string id, string UserId)
		{
			var result = await _db.Uploads.FirstOrDefaultAsync(u => u.Id == id && u.UserId == UserId);
			if (result != null)
			{
				_db.Uploads.Remove(result);
				await _db.SaveChangesAsync();
			}
		}

		public IQueryable<DisplayUploadsViewModel> GetBy(string userId)
		{
			var result = _db.Uploads.Where(u => u.User.Id == userId)
				.OrderByDescending(u => u.UploadDate)
				.ProjectTo<DisplayUploadsViewModel>(mapper.ConfigurationProvider);
			return result;
		}

		public async Task<DisplayUploadsViewModel> FindAsync(string id, string UserId)
		{
			var result = await _db.Uploads.FirstOrDefaultAsync(u => u.Id == id && u.UserId == UserId);
			if (result != null)
			{
				return mapper.Map<DisplayUploadsViewModel>(result);
			}
			return null;
		}

		public IQueryable<DisplayUploadsViewModel> GetAll()
		{
			var result = _db.Uploads
				.OrderByDescending(u => u.UploadDate)
				.ProjectTo<DisplayUploadsViewModel>(mapper.ConfigurationProvider);
			return result;
		}

		public async Task<int> GetUploadsCount()
		{
			return await _db.Uploads.CountAsync();
		}

		public IQueryable<DisplayUploadsViewModel> Search(string term)
		{
			var result = _db.Uploads
				.Where(u => u.Title.Contains(term))
				.OrderByDescending(u => u.UploadDate)
				.ProjectTo<DisplayUploadsViewModel>(mapper.ConfigurationProvider);
			return result;
		}

		public async Task<UpdateUploadsViewModel> FindUpdateAsync(string id, string UserId)
		{
			var result = await _db.Uploads.FirstOrDefaultAsync(u => u.Id == id && u.UserId == UserId);
			if (result != null)
			{
				return mapper.Map<UpdateUploadsViewModel>(result);
			}
			return null;
		}

		public async Task UpdateAsync(UpdateUploadsViewModel model)
		{
			var result = await _db.Uploads.FirstOrDefaultAsync(u => u.Id == model.Id && u.UserId == model.UserId);
			result.FilePath = model.FilePath;
			result.Title = model.Title;
			result.UploadDate = model.UploadDate;
			result.Description = model.Description;
			result.Fulldescription = model.Fulldescription;
			result.HasedFilePath = model.HasedFilePath;
			_db.Update(result);
			await _db.SaveChangesAsync();
		}
	}
}
