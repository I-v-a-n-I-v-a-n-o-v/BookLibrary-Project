using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models.Entities;


namespace BookLibrary.Models.Books
{
    public class DetailsVM
    {
        public int BookId { get; set; }
        public string Heading { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Pages { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }
        public int CountOfLikes { get; set; }
        public string ContentComment { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public int CountOfOrderedBooks { get; set; }
        public int CountOfAllOrderedBooks { get; set; }

    }
}
