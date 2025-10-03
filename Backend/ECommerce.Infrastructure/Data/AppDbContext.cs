using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Entities;

namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<AppliedCoupon> AppliedCoupons { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(e => e.Carts)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.CouponUsages)
                    .WithOne(cu => cu.User)
                    .HasForeignKey(cu => cu.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cart configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.HasIndex(e => e.UserId);

                entity.HasMany(e => e.Items)
                    .WithOne()
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.AppliedCoupons)
                    .WithOne(ac => ac.Cart)
                    .HasForeignKey(ac => ac.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            // Coupon configuration
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaxDiscountAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinimumTotalPrice).HasColumnType("decimal(18,2)");

                entity.HasMany(e => e.AppliedCoupons)
                    .WithOne(ac => ac.Coupon)
                    .HasForeignKey(ac => ac.CouponId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.CouponUsages)
                    .WithOne(cu => cu.Coupon)
                    .HasForeignKey(cu => cu.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // AppliedCoupon configuration
            modelBuilder.Entity<AppliedCoupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CartId, e.CouponId }).IsUnique();
            });

            // CouponUsage configuration
            modelBuilder.Entity<CouponUsage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.HasIndex(e => new { e.CouponId, e.UserId });
            });

            // Seed data - temporarily disabled for initial migration
            // SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "High-performance laptop",
                    Price = 1000,
                    Category = "Electronics",
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse",
                    Price = 25,
                    Category = "Electronics",
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Mechanical Keyboard",
                    Description = "RGB mechanical keyboard",
                    Price = 75,
                    Category = "Electronics",
                    IsActive = true
                },
                new Product
                {
                    Id = 4,
                    Name = "USB-C Cable",
                    Description = "Fast charging USB-C cable",
                    Price = 15,
                    Category = "Accessories",
                    IsActive = true
                },
                new Product
                {
                    Id = 5,
                    Name = "Laptop Stand",
                    Description = "Adjustable aluminum laptop stand",
                    Price = 45,
                    Category = "Accessories",
                    IsActive = true
                }
            );

            // Seed Coupons
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    Id = 1,
                    Code = "SAVE100",
                    Description = "Save $100 on orders over $500",
                    DiscountType = DiscountType.Fixed,
                    DiscountValue = 100,
                    IsAutoApplied = false,
                    StartDate = new DateTime(2024, 9, 1),
                    ExpiryDate = new DateTime(2025, 9, 1),
                    MinimumTotalPrice = 500,
                    MaxTotalUses = 1000,
                    MaxUsesPerUser = 5,
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                },
                new Coupon
                {
                    Id = 2,
                    Code = "AUTO10",
                    Description = "Automatic 10% off on 2+ items",
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 10,
                    MaxDiscountAmount = 50,
                    IsAutoApplied = true,
                    StartDate = new DateTime(2024, 9, 1),
                    ExpiryDate = new DateTime(2025, 9, 1),
                    MinimumCartItems = 2,
                    MaxTotalUses = null, // Unlimited
                    MaxUsesPerUser = null, // Unlimited
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                },
                new Coupon
                {
                    Id = 3,
                    Code = "ELECTRONICS20",
                    Description = "20% off on electronics (Product IDs 1,2,3)",
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 20,
                    MaxDiscountAmount = 200,
                    IsAutoApplied = false,
                    StartDate = new DateTime(2024, 9, 20),
                    ExpiryDate = new DateTime(2025, 3, 1),
                    MinimumTotalPrice = 100,
                    MaxTotalUses = 500,
                    MaxUsesPerUser = 3,
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = "[1,2,3]" // Only for electronics products
                },
                new Coupon
                {
                    Id = 4,
                    Code = "FREESHIP",
                    Description = "Free shipping - $15 off",
                    DiscountType = DiscountType.Fixed,
                    DiscountValue = 15,
                    IsAutoApplied = false,
                    StartDate = new DateTime(2024, 9, 25),
                    ExpiryDate = new DateTime(2025, 12, 1),
                    MinimumTotalPrice = 50,
                    MaxTotalUses = null,
                    MaxUsesPerUser = 10,
                    CurrentTotalUses = 0,
                    IsActive = true,
                }
            );
        }
    }
}