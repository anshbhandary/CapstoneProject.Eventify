using EventManagingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagingAPI.Data
{
    public class EventManagingContext : DbContext
    {
        public EventManagingContext(DbContextOptions<EventManagingContext> options) : base(options)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ManagedEvent> ManagedEvents { get; set; }
        public DbSet<ItemRequired> ItemRequirements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ManagedEvent entity
            modelBuilder.Entity<ManagedEvent>(entity =>
            {
                entity.HasKey(e => e.ManagedEventId);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false);

                // Relationships
                entity.HasMany(e => e.ToDoItems)
                    .WithOne(t => t.ManagedEvent)
                    .HasForeignKey(t => t.ManagedEventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ItemRequirements)
                    .WithOne(r => r.ManagedEvent)
                    .HasForeignKey(r => r.ManagedEventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ToDoItem entity
            modelBuilder.Entity<ToDoItem>(entity =>
            {
                entity.HasKey(t => t.ToDoItemId);

                entity.Property(t => t.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(t => t.IsCompleted)
                    .HasDefaultValue(false);

                entity.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(t => t.UpdatedAt)
                    .IsRequired(false);
            });

            // Configure RequiredItem entity
            modelBuilder.Entity<ItemRequired>(entity =>
            {
                entity.HasKey(r => r.RequiredItemId);

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.Description)
                    .HasMaxLength(500);

                entity.Property(r => r.Quantity)
                    .HasDefaultValue(1);

                entity.Property(r => r.IsAcquired)
                    .HasDefaultValue(false);

                entity.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(r => r.UpdatedAt)
                    .IsRequired(false);
            });
        }
    }
}