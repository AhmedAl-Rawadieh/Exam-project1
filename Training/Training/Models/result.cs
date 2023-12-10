using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Models
{
    public class result
    {
        [Key]
        public int? ID { get; set; }

        // ForeignKey to Exam
        [ForeignKey("Exam")]
        public int ExamID { get; set; }

        // Navigation property to Exam
        public virtual exam Exam { get; set; }

        // ForeignKey to User
        [ForeignKey("User")]
        public int? UserID { get; set; }

        // Navigation property to User
        public virtual users User { get; set; }

        // ForeignKey to Question
        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        // Navigation property to Question
        public virtual question Question { get; set; }

        // ForeignKey to Answer
        [ForeignKey("Answer")]
        public int AnswerID { get; set; }

        // Navigation property to Answer
        public virtual answer Answer { get; set; }

        public int FinalMark { get; set; }
    }
}