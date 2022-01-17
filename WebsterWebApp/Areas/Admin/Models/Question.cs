using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsterWebApp.Areas.Admin.Models
{
    [Table("tb_Question")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Subject is required!")]
        public String Subject { get; set; }

        public String QuestionTitle { get; set; }

        [Required(ErrorMessage = "Question type is required!")]
        public bool QuestionType { get; set; }

        public String Photo { get; set; }
    }
}
