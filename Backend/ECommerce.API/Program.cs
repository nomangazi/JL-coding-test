using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper (for DTO mapping)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// CORS configuration for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Dependency Injection - Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IUserService, ECommerce.API.Services.UserService>();
builder.Services.AddScoped<ICartService, ECommerce.API.Services.CartService>();
builder.Services.AddScoped<ICouponService, ECommerce.API.Services.CouponService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

// Apply pending migrations and create database if not exists
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Applies all pending migrations

    // Seed users if none exist
    if (!dbContext.Users.Any())
    {
        Console.WriteLine("No users found. Seeding initial users...");
        dbContext.Users.AddRange(
            new User
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "555-0101",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "555-0102",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "555-0103",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Users seeded successfully!");
    }
    else
    {
        var userCount = await dbContext.Users.CountAsync();
        Console.WriteLine($"Found {userCount} existing users in database.");
    }
}

// Map controllers
app.MapControllers();

app.Run();
