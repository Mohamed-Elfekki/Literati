using Microsoft.AspNetCore.Identity;

namespace Literati.Data
{
    public class Upload
    {
        public Upload()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }
        public string Id { get; set; }
        public string FilePath { get; set; }

        public string HasedFilePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Fulldescription { get; set; }
        public DateTime UploadDate { get; set; }

        //Relation One:Many
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
