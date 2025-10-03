using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Services
{
    public interface ICouponService
    {
        Task<List<Coupon>> GetAllCouponsAsync();
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<Coupon?> GetCouponByIdAsync(int id);
        Task<Coupon> CreateCouponAsync(CouponCreateRequest request);
        Task<Coupon> UpdateCouponAsync(int id, CouponUpdateRequest request);
        Task DeleteCouponAsync(int id);
        Task<CouponValidationResult> ValidateCouponAsync(string code, int userId, Cart cart);
        Task<List<Coupon>> GetAutoAppliedCouponsAsync(int userId, Cart cart);
        decimal CalculateDiscount(Coupon coupon, decimal cartTotal, List<CartItem> items);
    }
}