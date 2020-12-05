using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesUpload.ViewModels
{
    public class StoredFileListViewModel
    {
        public Guid Id { get; set; }
        public IdentityUser Uploader { get; set; }
        public string UploaderId { get; set; }
        public DateTime UploadedAt { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public int ThumbnailCount { get; set; }
    }
}
