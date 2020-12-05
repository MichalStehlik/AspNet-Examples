using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FilesUpload.Data;
using FilesUpload.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilesUpload.Pages
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private ApplicationDbContext _context;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public ICollection<IFormFile> Upload { get; set; }
        public UploadModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            int successfulProcessing = 0;
            int failedProcessing = 0;
            foreach (var uploadedFile in Upload)
            {
                var fileRecord = new StoredFile
                {
                    OriginalName = uploadedFile.FileName,
                    UploaderId = userId,
                    UploadedAt = DateTime.Now,
                    ContentType = uploadedFile.ContentType
                };
                try
                {
                    _context.Files.Add(fileRecord);
                    await _context.SaveChangesAsync();
                    var file = Path.Combine(_environment.ContentRootPath, "Uploads", fileRecord.Id.ToString());
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    };
                    successfulProcessing++;
                }
                catch
                {
                    failedProcessing++;
                }
                if (failedProcessing == 0)
                {
                    SuccessMessage = "All files has been uploaded successfuly.";
                }
                else
                {
                    ErrorMessage = "There were " + failedProcessing + " errors during uploading and processing of files.";
                }
            }
            return RedirectToPage("/Index");
        }
    }
}
