using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Areas.Admin.Models
{
    [Table("tb_Candidate_List")]
    public class CandidateList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateId { get; set; }
        [Required]
        public int ExamId { get; set; }

        public string Email { get; set; }
    }
}
