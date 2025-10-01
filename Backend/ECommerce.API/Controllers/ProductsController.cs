using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct(Products product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("test-connection")]
        public async Task<ActionResult> TestConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    return Ok(new { message = "PostgreSQL connection successful!", timestamp = DateTime.UtcNow });
                }
                else
                {
                    return StatusCode(500, new { message = "PostgreSQL connection failed!", timestamp = DateTime.UtcNow });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "PostgreSQL connection failed!", error = ex.Message });
            }
        }
    }
}