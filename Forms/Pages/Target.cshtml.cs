using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forms.Pages
{
    public class TargetModel : PageModel
    {
        [BindProperty]
        [Display(Name = "Jméno")]
        [Required]
        public string Firstname { get; set; }

        [BindProperty]
        [Display(Name = "Pøíjmení")]
        public string Lastname { get; set; }
        public void OnGet()
        {
        }

        public ActionResult OnPost(/*string firstname*/)
        {
            if (ModelState.IsValid) // je formuláø validní?
            {
                return Page(); // zobraz stránku, která patøí k tomuto PageModelu
            }
            return NotFound();
        }

    }
}
