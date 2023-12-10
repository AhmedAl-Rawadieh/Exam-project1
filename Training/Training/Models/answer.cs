using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Models
{
    public class answer
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Text { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

 


        // ForeignKey to Question
        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        // Navigation property to Question
        public virtual question Question { get; set; }

      
    }

    
}