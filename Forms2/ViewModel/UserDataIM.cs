using Forms2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forms2.ViewModel
{
    public class UserDataIM
    {
        [Required(ErrorMessage = "Pole jméno musí být vyplněno.")]
        [Display(Name = "Jméno")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Pole souhlas musí být vyplněno.")]
        [Display(Name = "Souhlas")]
        public bool Agreement { get; set; }
        [Required]
        public string Classname { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}
