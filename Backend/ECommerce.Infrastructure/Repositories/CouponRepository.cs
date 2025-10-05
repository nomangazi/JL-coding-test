using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code.ToUpper() == code.ToUpper() && c.DeletedAt == null);
        }

        public async Task<Coupon?> GetCouponByIdAsync(int id)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupons
                .Where(c => c.DeletedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Coupon>> GetAutoAppliedCouponsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Coupons
                .Where(c => c.IsAutoApplied
                    && c.IsActive
                    && c.DeletedAt == null
                    && (c.StartDate == null || c.StartDate <= now)
                    && (c.ExpiryDate == null || c.ExpiryDate >= now))
                .ToListAsync();
        }

        public async Task<Coupon> AddCouponAsync(Coupon coupon)
        {
            // Check if code already exists
            // Check if code already exists (case-insensitive)
            bool exists = await _context.Coupons
                .AnyAsync(c => c.Code.ToUpper() == coupon.Code.ToUpper() && c.DeletedAt == null);
            if (exists)
            {
                throw new Exception("Coupon code already exists");
            }
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            coupon.UpdatedAt = DateTime.UtcNow;
            _context.Entry(coupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCouponAsync(int couponId)
        {
            var coupon = await _context.Coupons.FindAsync(couponId);
            if (coupon != null)
            {
                coupon.DeletedAt = DateTime.UtcNow;
                coupon.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCouponCodeUniqueAsync(string code, int? excludeCouponId = null)
        {
            var query = _context.Coupons.Where(c => c.Code == code && c.DeletedAt == null);

            if (excludeCouponId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCouponId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<int> GetUserCouponUsageCountAsync(string couponCode, int userId)
        {
            return await _context.CouponUsages
                .Include(cu => cu.Coupon)
                .Where(cu => cu.Coupon!.Code == couponCode && cu.UserId == userId)
                .CountAsync();
        }

        public async Task<CouponUsage> AddCouponUsageAsync(CouponUsage usage)
        {// Check if user already used this coupon
            bool alreadyUsed = await _context.CouponUsages
                .AnyAsync(cu => cu.CouponId == usage.CouponId && cu.UserId == usage.UserId);

            if (alreadyUsed)
                throw new Exception("User has already used this coupon");

            _context.CouponUsages.Add(usage);
            await _context.SaveChangesAsync();
            return usage;
        }
    }
}
