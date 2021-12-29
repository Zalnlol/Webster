using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsterWebApp.Areas.Admin.Models
{
    [Table("tb_Answer")]
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Answer content is required!")]
        public String AnswerContent { get; set; }

        [Required(ErrorMessage = "Is correct answer is required!")]
        public bool IsCorrectAnswer { get; set; }

        public String Photo { get; set; }
    }
}
