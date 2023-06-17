using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UNDEFINED.Data;

namespace UNDEFINED
{
	public class UploadProfile : Profile
	{
		public UploadProfile()
		{
			CreateMap<Models.PushUploadsViewModel, Data.Upload>()
				.ForMember(u => u.Id, op => op.Ignore())
				.ForMember(u => u.UploadDate, op => op.Ignore());
			CreateMap<Data.Upload, Models.DisplayUploadsViewModel>();

			CreateMap<Models.UpdateUploadsViewModel, Data.Upload>()
			   .ForMember(u => u.Id, op => op.Ignore())
			   .ForMember(u => u.UploadDate, op => op.Ignore());
			CreateMap<Data.Upload, Models.UpdateUploadsViewModel>();
		}

	}
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<ApplicationUser, Models.UserViewModel>();

			CreateMap<ApplicationUser, Areas.Admin.Models.AdminUserViewModel>()
				.ForMember(u => u.UserId, op => op.MapFrom(u => u.Id));
		}

	}
}
