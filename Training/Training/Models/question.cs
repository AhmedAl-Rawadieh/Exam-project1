using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Training.Models
{
    public class question
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int Mark { get; set; }

        // ForeignKey to exam
        [ForeignKey("Exam")]
        public int ExamID { get; set; }

        // Navigation property to Exam
        public virtual exam Exam { get; set; }
        public virtual List<answer> Answer { get; set; }

    }
}