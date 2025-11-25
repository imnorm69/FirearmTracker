using FirearmTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Context
{
    public class FirearmTrackerContext : DbContext
    {
        public FirearmTrackerContext(DbContextOptions<FirearmTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Firearm> Firearms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Ammunition> Ammunition { get; set; }
        public DbSet<FirearmAmmunitionLink> FirearmAmmunitionLinks { get; set; }
        public DbSet<AmmunitionTransaction> AmmunitionTransactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Firearm>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Manufacturer).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FirearmType).IsRequired();

                // Configure firearm properties
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.Caliber).HasMaxLength(100);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActivityType).IsRequired();
                entity.Property(e => e.ActivityDate).IsRequired();
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Notes).HasMaxLength(2000);
                entity.Property(e => e.AdditionalData).HasMaxLength(4000);

                // Configure relationship
                entity.HasOne(e => e.Firearm)
                      .WithMany()
                      .HasForeignKey(e => e.FirearmId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ContentType).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Configure relationships
                entity.HasOne(e => e.Activity)
                      .WithMany()
                      .HasForeignKey(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Firearm)
                      .WithMany()
                      .HasForeignKey(e => e.FirearmId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Manufacturer).HasMaxLength(200);
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
                entity.Property(e => e.SalePrice).HasPrecision(18, 2);  // Add this line
                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasOne(e => e.LinkedFirearm)
                      .WithMany()
                      .HasForeignKey(e => e.LinkedFirearmId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Ammunition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Caliber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Manufacturer).HasMaxLength(200);
                entity.Property(e => e.BulletType).HasMaxLength(100);
                entity.Property(e => e.LotNumber).HasMaxLength(100);
                entity.Property(e => e.StorageLocation).HasMaxLength(200);
                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<AmmunitionTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TransactionType).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.TransactionDate).IsRequired();
                entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
                entity.Property(e => e.SalePrice).HasPrecision(18, 2);
                entity.Property(e => e.BuyerName).HasMaxLength(200);
                entity.Property(e => e.Notes).HasMaxLength(2000);

                // Configure relationship with Ammunition
                entity.HasOne(e => e.Ammunition)
                      .WithMany(a => a.Transactions)
                      .HasForeignKey(e => e.AmmunitionId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure optional relationship with Activity (for range sessions)
                entity.HasOne(e => e.Activity)
                      .WithMany()
                      .HasForeignKey(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<FirearmAmmunitionLink>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).HasMaxLength(500);

                // Configure relationship with Firearm
                entity.HasOne(e => e.Firearm)
                      .WithMany()
                      .HasForeignKey(e => e.FirearmId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Ammunition
                entity.HasOne(e => e.Ammunition)
                      .WithMany(a => a.FirearmLinks)
                      .HasForeignKey(e => e.AmmunitionId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Prevent duplicate links - unique constraint
                entity.HasIndex(e => new { e.FirearmId, e.AmmunitionId })
                      .IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedDate).IsRequired();

                // Unique constraint on username
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}