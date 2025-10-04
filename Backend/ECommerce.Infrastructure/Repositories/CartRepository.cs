using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            var cartData = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .Include(c => c.AppliedCoupons)
                .ThenInclude(ac => ac.Coupon)
            .AsSplitQuery() // Use split query to avoid cartesian explosion
            .FirstOrDefaultAsync(c => c.UserId == userId);

            // System.Console.WriteLine("1++++++++++++++++++++++++++++++");
            // System.Console.WriteLine($"User ID: {userId}");
            // System.Console.WriteLine(JsonSerializer.Serialize(cartData, new JsonSerializerOptions
            // {
            //     ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //     WriteIndented = true
            // }));
            // System.Console.WriteLine("2++++++++++++++++++++++++++++++");

            return cartData;

        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .Include(c => c.AppliedCoupons)
                    .ThenInclude(ac => ac.Coupon)
                .AsSplitQuery() // Use split query to avoid cartesian explosion
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart> CreateCartAsync(int userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AppliedCoupon>> GetAppliedCouponsAsync(int cartId)
        {
            return await _context.AppliedCoupons
                .Include(ac => ac.Coupon)
                .Where(ac => ac.CartId == cartId)
                .ToListAsync();
        }

        public async Task<AppliedCoupon> AddAppliedCouponAsync(AppliedCoupon appliedCoupon)
        {
            try
            {
                Console.WriteLine($"18++++++++++++++++ Before Adding {appliedCoupon.CouponId}" + appliedCoupon.CouponId);
                _context.AppliedCoupons.Add(appliedCoupon);
                await _context.SaveChangesAsync();
                Console.WriteLine($"19++++++++++++++++ After Adding {appliedCoupon.CouponId}" + appliedCoupon.CouponId);
                return appliedCoupon;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"20+++++++++++++++++++++++: {ex.Message}");
                Console.WriteLine($"21+++++++++++++++++++++++: {ex.InnerException}");
                Console.WriteLine($"22+++++++++++++++++++++++: {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveAppliedCouponAsync(int cartId, string couponCode)
        {
            var appliedCoupon = await _context.AppliedCoupons
                .Include(ac => ac.Coupon)
                .FirstOrDefaultAsync(ac => ac.CartId == cartId && ac.Coupon!.Code == couponCode);

            if (appliedCoupon != null)
            {
                _context.AppliedCoupons.Remove(appliedCoupon);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            var appliedCoupons = await _context.AppliedCoupons.Where(ac => ac.CartId == cartId).ToListAsync();
            _context.AppliedCoupons.RemoveRange(appliedCoupons);

            await _context.SaveChangesAsync();
        }
    }
}
