using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using WebsterWebApp.Areas.Admin.Models;
using WebsterWebApp.Models;

namespace WebsterWebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        //[ForeignKey("UserId")]
        //public virtual ICollection<> {get;set;}
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        //public virtual DbSet<Areas.Admin.Models.User> User { get; set; }

        public virtual DbSet<Areas.Admin.Models.CandidateList> CandidateLists { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<MailContact> Mails { get; set; }

    }
}
