using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Entities
{
    public class Summary
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
