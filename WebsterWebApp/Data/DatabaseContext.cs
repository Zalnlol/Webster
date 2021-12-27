using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Models.User> Users{ get; set; }
        public virtual DbSet<Models.CandidateList> CandidateLists { get; set; }
    }
}
