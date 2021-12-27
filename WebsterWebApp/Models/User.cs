using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Models
{
    [Table("Account")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public string Avatar { get; set; }
        [StringLength(50)]
        public string Role { get; set; }

        [ForeignKey("Email")]
        public virtual ICollection<CandidateList> CandidateLists { get; set; }
    }
}
