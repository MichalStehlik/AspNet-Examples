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
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace FilesUpload.Pages
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly int _squareSize = 64;
        private readonly int _sameAspectRatioHeigth = 128;


        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public ICollection<IFormFile> Upload { get; set; }
        public UploadModel(IWebHostEnvironment environment, ApplicationDbContext context, IConfiguration configuration)
        {
            _environment = environment;
            _context = context;
            _configuration = configuration;
            if (Int32.TryParse(_configuration["Thumbnails:SquareSize"], out _squareSize) == false) _squareSize = 64;
            if (Int32.TryParse(_configuration["Thumbnails:SameAspectRatioHeigth"], out _sameAspectRatioHeigth) == false) _sameAspectRatioHeigth = 128;
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
                if (uploadedFile.ContentType.StartsWith("image"))
                {
                    fileRecord.Thumbnails = new List<Thumbnail>();
                    MemoryStream ims = new MemoryStream();
                    MemoryStream oms1 = new MemoryStream();
                    MemoryStream oms2 = new MemoryStream();
                    uploadedFile.CopyTo(ims);
                    IImageFormat format;
                    using (Image image = Image.Load(ims.ToArray(), out format))
                    {
                        int largestSize = Math.Max(image.Height, image.Width);
                        if (image.Width > image.Height)
                        {
                            image.Mutate(x => x.Resize(0, _squareSize));
                        }
                        else
                        {
                            image.Mutate(x => x.Resize(_squareSize, 0));
                        }
                        image.Mutate(x => x.Crop(new Rectangle((image.Width - _squareSize) / 2, (image.Height - _squareSize) / 2, _squareSize, _squareSize)));
                        image.Save(oms1, format);
                        fileRecord.Thumbnails.Add(new Thumbnail { Type = ThumbnailType.Square, Blob = oms1.ToArray() });
                    }
                    using (Image image = Image.Load(ims.ToArray(), out format))
                    {
                        image.Mutate(x => x.Resize(0, _sameAspectRatioHeigth));
                        image.Save(oms2, format);
                        fileRecord.Thumbnails.Add(new Thumbnail { Type = ThumbnailType.SameAspectRatio, Blob = oms2.ToArray() });
                    }
                }
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
