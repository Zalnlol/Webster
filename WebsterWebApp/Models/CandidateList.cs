using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Models
{
    [Table("CandidateList")]
    public class CandidateList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateListId { get; set; }
        [Required]
        public string Exam { get; set; }

        public string Email { get; set; }
    }
}
