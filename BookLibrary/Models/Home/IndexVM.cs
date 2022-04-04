using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models.Entities;

namespace BookLibrary.Models.Home
{
    public class IndexVM
    {
        public virtual  List<Book> BookCollection { get; set; }
    }
}
