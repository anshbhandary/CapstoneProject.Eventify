using Microsoft.EntityFrameworkCore;
using VendorAPI.Models;

namespace VendorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorPackage> VendorPackages { get; set; }

        public DbSet<ServiceSelection> ServiceSelections { get; set; }

        public DbSet<Quotation> Quotations { get; set; }

        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

        public DbSet<Booking> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quotation>()
                .Property(q => q.TotalPrice)
                .HasPrecision(18, 2); // 18 digits in total, 2 after the decimal

            modelBuilder.Entity<Quotation>()
                .Property(q => q.Discount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Vendor>().HasData(new Vendor
            {
                Id = 1,
                BusinessName = "zzz",
                Email = "asd@epic.com",
                PasswordHash = "nothing@epic420",
            });
        }
    }
}
