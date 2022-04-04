using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


    }
}
