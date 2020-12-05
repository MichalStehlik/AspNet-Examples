using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilesUpload.Models
{
    public class StoredFile
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UploaderId")]
        public IdentityUser Uploader { get; set; }
        [Required]
        public string UploaderId { get; set; }
        [Required]
        public DateTime UploadedAt { get; set; }
        [Required]
        public string OriginalName { get; set; }
        [Required]
        public string ContentType { get; set; }
        public ICollection<Thumbnail> Thumbnails { get; set; }
    }
}
