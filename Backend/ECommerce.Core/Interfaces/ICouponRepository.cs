using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<Coupon?> GetCouponByIdAsync(int id);
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<List<Coupon>> GetAutoAppliedCouponsAsync();
        Task<Coupon> AddCouponAsync(Coupon coupon);
        Task UpdateCouponAsync(Coupon coupon);
        Task DeleteCouponAsync(int couponId);
        Task<bool> IsCouponCodeUniqueAsync(string code, int? excludeCouponId = null);
        Task<int> GetUserCouponUsageCountAsync(string couponCode, int userId);
        Task<CouponUsage> AddCouponUsageAsync(CouponUsage usage);
    }
}
