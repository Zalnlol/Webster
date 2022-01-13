using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(250)]
        public string Avatar { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<CandidateList> Candidate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public virtual DbSet<CandidateList> Candidate { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamType> ExamTypes { get; set; }
        public virtual DbSet<ExamUser> ExamUsers { get; set; }
        public virtual DbSet<ResultsModel> Results { get; set; }





        public virtual DbSet<MailContact> Mails { get; set; }





        public DbSet<WebsterWebApp.Models.RegistrationModel> RegistrationModel { get; set; }

    }
}
