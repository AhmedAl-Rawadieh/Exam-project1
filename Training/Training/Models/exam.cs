using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Models
{
    public class exam
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual List<question> questions { get; set; }
    }
}