using Microsoft.Build.Framework;

namespace Literati.Models
{

	public class PushUploadsViewModel
	{
		[Required]
		public IFormFile File { get; set; }
		public string? FilePath { get; set; }
		public string? HasedFilePath { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string Fulldescription { get; set; }
		public string? UserId { get; set; }

	}

	public class DisplayUploadsViewModel
	{
		public string? FilePath { get; set; }
		public string? HasedFilePath { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Fulldescription { get; set; }
		public DateTime UploadDate { get; set; }
		public string Id { get; set; }
	}


	public class UpdateUploadsViewModel
	{
		[Required]
		public IFormFile? File { get; set; }
		public string? FilePath { get; set; }
		public string? HasedFilePath { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string Fulldescription { get; set; }


		public DateTime UploadDate { get; set; }
		public string? UserId { get; set; }
		public string Id { get; set; }
	}
}
