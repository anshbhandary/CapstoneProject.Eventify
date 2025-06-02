using AdminAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminAPI.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        public DbSet<Admin> admins { get; set; }
        public DbSet<Approval> approvals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
