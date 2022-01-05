using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Areas.Admin.Models
{
    [Table("tb_Mail")]
    public class MailContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ToMail { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachments { get; set; }
    }
}
