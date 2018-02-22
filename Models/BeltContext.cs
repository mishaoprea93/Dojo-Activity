using Microsoft.EntityFrameworkCore;
using belt_exam.Models;

namespace belt_exam.Models
{
    public class BeltContext : DbContext
    {
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        
        public DbSet<Activity> activities{get;set;}
        public DbSet<Join> joins{get;set;}
        
    }
}
