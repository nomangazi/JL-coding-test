using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IsAutoApplied = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MinimumCartItems = table.Column<int>(type: "integer", nullable: true),
                    MinimumTotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MaxTotalUses = table.Column<int>(type: "integer", nullable: true),
                    MaxUsesPerUser = table.Column<int>(type: "integer", nullable: true),
                    CurrentTotalUses = table.Column<int>(type: "integer", nullable: false),
                    ApplicableProductIdsJson = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PriceOffice = table.Column<decimal>(type: "numeric", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CouponId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppliedCoupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    CouponId = table.Column<int>(type: "integer", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAutoApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedCoupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedCoupons_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedCoupons_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "ApplicableProductIdsJson", "Code", "CreatedAt", "CurrentTotalUses", "DeletedAt", "Description", "DiscountType", "DiscountValue", "ExpiryDate", "IsActive", "IsAutoApplied", "MaxDiscountAmount", "MaxTotalUses", "MaxUsesPerUser", "MinimumCartItems", "MinimumTotalPrice", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "", "WELCOME50", new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(1877), 0, null, "Welcome bonus - $50 off your first order over $200", 1, 50m, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, false, null, null, 1, null, 200m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(1878) },
                    { 2, "", "AUTO15", new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4089), 0, null, "Automatic 15% off on orders over $100", 2, 15m, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, true, 75m, null, null, null, 100m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4090) },
                    { 3, "[1,2,3,6,7,8]", "TECH25", new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4227), 0, null, "25% off on all electronics - limited time!", 2, 25m, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, false, 250m, 500, 2, null, 50m, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4228) },
                    { 4, "", "FREESHIP", new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4232), 0, null, "Free shipping - $15 off on orders over $75", 1, 15m, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, false, null, null, 5, null, 75m, new DateTime(2024, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4232) },
                    { 5, "", "BULK30", new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4234), 0, null, "30% off when you buy 3 or more items", 2, 30m, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), true, false, 300m, 200, 1, 3, null, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4235) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "DeletedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "PriceOffice", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7632), null, "High-performance laptop for professionals", "", true, "Laptop", 1000m, 0m, 15, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7782) },
                    { 2, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7922), null, "Ergonomic wireless mouse with long battery life", "", true, "Wireless Mouse", 25m, 0m, 50, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7922) },
                    { 3, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7924), null, "RGB mechanical keyboard with tactile switches", "", true, "Mechanical Keyboard", 75m, 0m, 25, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7925) },
                    { 4, "Accessories", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7927), null, "Fast charging USB-C cable 6 feet long", "", true, "USB-C Cable", 15m, 0m, 100, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7927) },
                    { 5, "Accessories", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7929), null, "Adjustable aluminum laptop stand for better ergonomics", "", true, "Laptop Stand", 45m, 0m, 30, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7929) },
                    { 6, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7931), null, "27-inch 4K gaming monitor with 144Hz refresh rate", "", true, "Gaming Monitor", 350m, 0m, 12, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7931) },
                    { 7, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7933), null, "Latest flagship smartphone with 5G connectivity", "", true, "Smartphone", 800m, 0m, 20, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7933) },
                    { 8, "Electronics", new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7935), null, "Premium noise-cancelling wireless headphones", "", true, "Wireless Headphones", 150m, 0m, 35, new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7935) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppliedCoupons_CartId_CouponId",
                table: "AppliedCoupons",
                columns: new[] { "CartId", "CouponId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppliedCoupons_CouponId",
                table: "AppliedCoupons",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code",
                table: "Coupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_CouponId_UserId",
                table: "CouponUsages",
                columns: new[] { "CouponId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_UserId",
                table: "CouponUsages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedCoupons");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CouponUsages");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
