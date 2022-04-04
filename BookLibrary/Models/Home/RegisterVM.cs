using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Home
{
    public class RegisterVM
    {
        [DisplayName("FirstName: ")]
        [MaxLength(35)]
        public string FirstName { get; set; }

        [DisplayName("LastName: ")]
        [MaxLength(35)]
        public string LastName { get; set; }

        
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
