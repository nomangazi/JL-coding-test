using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Data
{
    class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
    }
}