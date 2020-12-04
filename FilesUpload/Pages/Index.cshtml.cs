using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace FileUpload.Pages
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;
        
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public List<string> Files { get; set; } = new List<string>();
        
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
        }
        
        public void OnGet()
        {
            var fullNames = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Uploads")).ToList();
            foreach (var fn in fullNames)
            {
                Files.Add(Path.GetFileName(fn));
            }
        }
        
        public IActionResult OnGetDownload(string filename)
        {
            var fullName = Path.Combine(_environment.ContentRootPath, "Uploads", filename);
            if (System.IO.File.Exists(fullName))
            {
                return PhysicalFile(fullName, MediaTypeNames.Application.Octet, filename);
            }
            else
            {
                ErrorMessage = "There is no such file.";
                return RedirectToPage();
            }
        }
    }
}
