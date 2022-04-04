using BookLibrary.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Books
{
    public class IndexVM
    {
        public virtual List<Book> BookCollection { get; set; }
        public virtual List<Summary> Summary { get; set; }
    }
}
