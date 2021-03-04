using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        [Display(Name="Jméno")]
        [Required]    
        public string Firstname { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Příjmení")]
        public string Lastname { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public ActionResult OnGet()
        {
            if (!(String.IsNullOrEmpty(Firstname) || String.IsNullOrEmpty(Lastname)))
            {
                return RedirectToPage("Result", new { firstname = Firstname });
            }
            Firstname = "?";
            return Page();
        }

        public ActionResult OnPost(/*string firstname*/)
        {
            //Firstname = Request.Form["firstname"];
            //Firstname = firstname;
            if (!ModelState.IsValid) // je formulář validní?
            {
                return Page(); // zobraz stránku, která patří k tomuto PageModelu
            }
            return RedirectToPage("Result", new { firstname = Firstname }); // přesměruj se na jinou stránku (Result) a předej jí nějaké parametry
        }
    }
}
