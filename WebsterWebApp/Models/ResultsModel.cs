using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebsterWebApp.Models
{
    [Table("tb_Result")]
    public class ResultsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ResultId { get; set; }
        public string IdUser { get; set; }
        public int ExamId { get; set; }
        public int GKScore { get; set; }
        public int MathScore { get; set; }
        public int TechScore { get; set; }
        public DateTime ExamDate { get; set; }
        public int TimeGK { get; set; }
        public int TimeMath { get; set; }
        public int TimeTech { get; set; }
        public bool IsPass { get; set; }


    }
}
