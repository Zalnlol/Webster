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

        [Required(ErrorMessage = "ExamName is required!")]
        public string ExamName { get; set; }
        [Required(ErrorMessage = "ExamName is required!")]
        public bool ExamType { get; set; }

        [Required(ErrorMessage = "ExamType title is required!")]
        public int PassPercent { get; set; }
        public string PassWord { get; set; }

        [Required(ErrorMessage = "FirstCountdown type is required!")]

        public TimeSpan FirstCountdown { get; set; }
        [Required(ErrorMessage = "SecondCountdown type is required!")]
        public TimeSpan SecondCountdown { get; set; }
        [Required(ErrorMessage = "ThirdCountdown type is required!")]
        public TimeSpan ThirdCountdown { get; set; }
        [Required(ErrorMessage = "StartDate type is required!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "FinishTime type is required!")]
        public DateTime FinishTime { get; set; }



    }
}
