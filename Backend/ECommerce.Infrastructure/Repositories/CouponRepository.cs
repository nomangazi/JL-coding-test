using ECommerce.Core.Entities;

using ECommerce.Core.Interfaces;
using ECommerce.Core.Entities;

using ECommerce.Infrastructure.Data;
using ECommerce.Core.Interfaces;

using Microsoft.EntityFrameworkCore;
using ECommerce.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories

{
    namespace ECommerce.Infrastructure.Repositories

    public class CouponRepository : ICouponRepository
    {

    {    public class CouponRepository(AppDbContext context) : ICouponRepository

        private readonly AppDbContext _context;    {

        private readonly AppDbContext _context = context;

        public CouponRepository(AppDbContext context)

        {        public async Task<Coupon?> GetCouponByCodeAsync(string code)

            _context = context;        {

        }            return await _context.Coupons.Where(c => c.Code == code).FirstOrDefaultAsync(c => c.DeletedAt == null);

        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()

        {
            {

                return await _context.Coupons            return await _context.Coupons.Where(c => c.DeletedAt == null).ToListAsync();

                .FirstOrDefaultAsync(c => c.Code == code && c.DeletedAt == null);
            }

        }

        public async Task AddCouponAsync(Coupon coupon)

        public async Task<Coupon?> GetCouponByIdAsync(int id)
        {

            {
                await _context.Coupons.AddAsync(coupon);

                return await _context.Coupons            await _context.SaveChangesAsync();

                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
            }

        }

        public async Task DeleteCouponAsync(int couponId)

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {

            {
                var coupon = await _context.Coupons.FindAsync(couponId);

                return await _context.Coupons            if (coupon != null)

                .Where(c => c.DeletedAt == null)            {

                .OrderByDescending(c => c.CreatedAt)                coupon.DeletedAt = DateTime.UtcNow;

                .ToListAsync(); await _context.SaveChangesAsync();

                }
            }

        }

        public async Task<List<Coupon>> GetAutoAppliedCouponsAsync()

        {
            Task<bool> ICouponRepository.IsCouponCodeUniqueAsync(string code, int ? excludeCouponId)

            var now = DateTime.UtcNow;
            {

                return await _context.Coupons            throw new NotImplementedException();

                .Where(c => c.IsAutoApplied         }

                    && c.IsActive

                    && c.DeletedAt == null        Task ICouponRepository.UpdateCouponAsync(Coupon coupon)

                    && (c.StartDate == null || c.StartDate <= now)        {

                    && (c.ExpiryDate == null || c.ExpiryDate >= now))            throw new NotImplementedException();

                .ToListAsync();
            }

        }
    }

    }
    public async Task<Coupon> AddCouponAsync(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        return coupon;
    }

    public async Task UpdateCouponAsync(Coupon coupon)
    {
        coupon.UpdatedAt = DateTime.UtcNow;
        _context.Coupons.Update(coupon);
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
            .Where(cu => cu.Coupon.Code == couponCode && cu.UserId == userId)
            .CountAsync();
    }

    public async Task<CouponUsage> AddCouponUsageAsync(CouponUsage usage)
    {
        _context.CouponUsages.Add(usage);
        await _context.SaveChangesAsync();
        return usage;
    }
}
}
