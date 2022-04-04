using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Home
{
    public class LoginVM
    {
        [DisplayName("Username: ")]
        [MaxLength(90)]
        [Required(ErrorMessage = "❗This field is Required!")]
        public string Username { get; set; }


        [DisplayName("Password: ")]
        [Required(ErrorMessage = "❗This field is Required!")]
        [StringLength(40, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
