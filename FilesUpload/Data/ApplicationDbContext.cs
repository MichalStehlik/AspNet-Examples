using FilesUpload.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilesUpload.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<StoredFile> Files { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Thumbnail>().HasKey(t => new { t.FileId, t.Type });
        }
    }
}
