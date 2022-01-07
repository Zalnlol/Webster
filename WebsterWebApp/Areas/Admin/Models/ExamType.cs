using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsterWebApp.Areas.Admin.Models
{
    [Table("tb_ExamType")]
    public class ExamType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamTypeId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
    }
}
