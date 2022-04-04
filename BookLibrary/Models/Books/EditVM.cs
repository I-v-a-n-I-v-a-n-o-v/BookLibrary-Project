using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models.Entities;

namespace BookLibrary.Models.Books
{
    public class EditVM : CreateVM
    {
        [Key]
        public int Id { get; set; }
        

    }
}
