using Forms2.Model;
using Forms2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forms2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public UserDataIM Input { get; set; } = new UserDataIM();

        public List<SelectListItem> Classnames { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Classnames = new List<SelectListItem>
            {
                new SelectListItem { Text = "P1", Value = "p2020"},
                new SelectListItem { Text = "P2", Value = "p2019"},
                new SelectListItem { Text = "P3", Value = "p2018"},
                new SelectListItem { Text = "P4", Value = "p2017"}
            };
        }

        public void OnGet()
        {
            Input.Firstname = "Alfons";
            Input.Agreement = true;
            Input.Classname = "p2019";
            Input.Gender = Gender.Female;
        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
