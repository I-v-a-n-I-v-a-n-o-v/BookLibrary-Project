using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Heading { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Pages { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("GenreId")]
        public int? GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        [ForeignKey("SummaryId")]
        public int? SummaryId { get; set; }
        public virtual Summary Summary { get; set; }


        [ForeignKey("ImageId")]
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }


    }
}
