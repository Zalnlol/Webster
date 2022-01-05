using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsterWebApp.Models
{
    [Table("tb_ExamType")]
    public class ExamType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamTypeId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }


    }
}
