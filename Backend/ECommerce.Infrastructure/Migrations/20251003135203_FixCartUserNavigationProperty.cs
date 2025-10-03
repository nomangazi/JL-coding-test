using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCartUserNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(3648), new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(3650) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6279), new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6280) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6427), new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6428) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6432), new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6432) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6436), new DateTime(2025, 10, 3, 13, 52, 3, 337, DateTimeKind.Utc).AddTicks(6436) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7688), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7860) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7984), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7985) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7987), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7987) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7989), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7989) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7991), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(7992) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8007), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8007) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8009), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8009) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8011), new DateTime(2025, 10, 3, 13, 52, 3, 336, DateTimeKind.Utc).AddTicks(8011) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(1877), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(1878) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4089), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4090) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4227), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4228) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4232), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4232) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4234), new DateTime(2025, 10, 3, 13, 45, 3, 726, DateTimeKind.Utc).AddTicks(4235) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7632), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7782) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7922), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7922) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7924), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7925) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7927) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7929), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7929) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7931), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7931) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7933), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7933) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7935), new DateTime(2025, 10, 3, 13, 45, 3, 725, DateTimeKind.Utc).AddTicks(7935) });
        }
    }
}
