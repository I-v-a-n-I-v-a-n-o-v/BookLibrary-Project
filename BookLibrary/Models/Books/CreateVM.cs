using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models.Entities;

namespace BookLibrary.Models.Books
{
    public class CreateVM
    {

        [DisplayName("Author: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Author { get; set; }

        [DisplayName("Heading: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Heading { get; set; }

        [DisplayName("ReleaseDate: ")]
        // [Required(ErrorMessage = "*This field is Required!")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Pages: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int Pages { get; set; }

        [DisplayName("Quantity: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int Quantity { get; set; }
        public virtual Summary Summary { get; set; }
        public int GenreID { get; set; }
        public virtual ICollection<Genre> GenreCollection { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Image Image { get; set; }


    }
}
