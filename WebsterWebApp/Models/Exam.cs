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

        [Required(ErrorMessage = "PassPercent title is required!")]
        [Range(0,100,ErrorMessage ="Pass from 0 to 1000 !!")]

        public int PassPercent { get; set; }
        public string ExamPass { get; set; }

        [Required(ErrorMessage = "FirstCountdown type is required!")]
        // [RegularExpression(@"[0-5]{1}[\d]:[0-5]{1}[\d]{1}", ErrorMessage ="Format mm:ss")]
        public TimeSpan FirstCountdown { get; set; }
        [Required(ErrorMessage = "SecondCountdown type is required!")]
        // [RegularExpression(@"/[0-9]{2}:[0-5]{1}[\d]{1}/", ErrorMessage = "Format mm:ss")]
        public TimeSpan SecondCountdown { get; set; }
        [Required(ErrorMessage = "ThirdCountdown type is required!")]
        // [RegularExpression(@"/[0-9]{2}:[0-5]{1}[\d]{1}/", ErrorMessage = "Format mm:ss")]
        public TimeSpan ThirdCountdown { get; set; }
        [Required(ErrorMessage = "StartDate type is required!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "FinishTime type is required!")]
        public DateTime FinishTime { get; set; }



    }
}
