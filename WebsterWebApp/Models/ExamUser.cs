using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsterWebApp.Models
{
    [Table("ExamUser")]
    public class ExamUser
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamUserId { get; set; }
        public string UserId { get; set; }
        public int ExamId { get; set; }


    }
}
