using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebsterWebApp.Models
{
    [Table("tb_Exam")]
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string ExamPass { get; set; }
        public bool ExamType { get; set; }
        public int PassPercent { get; set; }
        public TimeSpan FirstCountdown { get; set; }
        public TimeSpan SecondCountdown { get; set; }
        public TimeSpan ThirdCountdown { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishTime { get; set; }



    }
}
