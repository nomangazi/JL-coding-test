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

            // Seed data
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "High-performance laptop for professionals",
                    Price = 1000,
                    Category = "Electronics",
                    Stock = 15,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with long battery life",
                    Price = 25,
                    Category = "Electronics",
                    Stock = 50,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Name = "Mechanical Keyboard",
                    Description = "RGB mechanical keyboard with tactile switches",
                    Price = 75,
                    Category = "Electronics",
                    Stock = 25,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 4,
                    Name = "USB-C Cable",
                    Description = "Fast charging USB-C cable 6 feet long",
                    Price = 15,
                    Category = "Accessories",
                    Stock = 100,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 5,
                    Name = "Laptop Stand",
                    Description = "Adjustable aluminum laptop stand for better ergonomics",
                    Price = 45,
                    Category = "Accessories",
                    Stock = 30,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 6,
                    Name = "Gaming Monitor",
                    Description = "27-inch 4K gaming monitor with 144Hz refresh rate",
                    Price = 350,
                    Category = "Electronics",
                    Stock = 12,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 7,
                    Name = "Smartphone",
                    Description = "Latest flagship smartphone with 5G connectivity",
                    Price = 800,
                    Category = "Electronics",
                    Stock = 20,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 8,
                    Name = "Wireless Headphones",
                    Description = "Premium noise-cancelling wireless headphones",
                    Price = 150,
                    Category = "Electronics",
                    Stock = 35,
                    IsActive = true,
                    ImageUrl = "",
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                }
            );

            // Seed Coupons
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    Id = 1,
                    Code = "WELCOME50",
                    Description = "Welcome bonus - $50 off your first order over $200",
                    DiscountType = DiscountType.Fixed,
                    DiscountValue = 50,
                    IsAutoApplied = false,
                    StartDate = DateTime.SpecifyKind(new DateTime(2024, 9, 1), DateTimeKind.Utc),
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 12, 31), DateTimeKind.Utc),
                    MinimumTotalPrice = 200,
                    MaxTotalUses = null, // Unlimited
                    MaxUsesPerUser = 1, // First time users only
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                },
                new Coupon
                {
                    Id = 2,
                    Code = "AUTO15",
                    Description = "Automatic 15% off on orders over $100",
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 15,
                    MaxDiscountAmount = 75,
                    IsAutoApplied = true,
                    StartDate = DateTime.SpecifyKind(new DateTime(2024, 9, 1), DateTimeKind.Utc),
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 12, 31), DateTimeKind.Utc),
                    MinimumTotalPrice = 100,
                    MaxTotalUses = null, // Unlimited
                    MaxUsesPerUser = null, // Unlimited
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                },
                new Coupon
                {
                    Id = 3,
                    Code = "TECH25",
                    Description = "25% off on all electronics - limited time!",
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 25,
                    MaxDiscountAmount = 250,
                    IsAutoApplied = false,
                    StartDate = DateTime.SpecifyKind(new DateTime(2024, 10, 1), DateTimeKind.Utc),
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 3, 31), DateTimeKind.Utc),
                    MinimumTotalPrice = 50,
                    MaxTotalUses = 500,
                    MaxUsesPerUser = 2,
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = "[1,2,3,6,7,8]" // Electronics products
                },
                new Coupon
                {
                    Id = 4,
                    Code = "FREESHIP",
                    Description = "Free shipping - $15 off on orders over $75",
                    DiscountType = DiscountType.Fixed,
                    DiscountValue = 15,
                    IsAutoApplied = false,
                    StartDate = DateTime.SpecifyKind(new DateTime(2024, 9, 25), DateTimeKind.Utc),
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 12, 31), DateTimeKind.Utc),
                    MinimumTotalPrice = 75,
                    MaxTotalUses = null,
                    MaxUsesPerUser = 5,
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                },
                new Coupon
                {
                    Id = 5,
                    Code = "BULK30",
                    Description = "30% off when you buy 3 or more items",
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 30,
                    MaxDiscountAmount = 300,
                    IsAutoApplied = false,
                    StartDate = DateTime.SpecifyKind(new DateTime(2024, 10, 1), DateTimeKind.Utc),
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 6, 30), DateTimeKind.Utc),
                    MinimumCartItems = 3,
                    MaxTotalUses = 200,
                    MaxUsesPerUser = 1,
                    CurrentTotalUses = 0,
                    IsActive = true,
                    ApplicableProductIdsJson = string.Empty // Applies to all products
                }
            );
        }
    }
}