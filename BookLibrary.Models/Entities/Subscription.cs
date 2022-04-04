using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Entities
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }//<---
        public string Name { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
        public DateTime ExpiryDate { get; set; }

        //Foreign Key for User Table 
        [Display(Name = "User")]
        public virtual int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User Users { get; set; }
    }
}
