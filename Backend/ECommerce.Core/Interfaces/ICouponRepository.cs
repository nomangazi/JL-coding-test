using ECommerce.Core.Entities;



namespace ECommerce.Core.Interfacesusing ECommerce.Core.Entities;

{

    public interface ICouponRepositorynamespace ECommerce.Core.Interfaces

    {{

        Task<Coupon?> GetCouponByCodeAsync(string code); public interface ICouponRepository

        Task<Coupon?> GetCouponByIdAsync(int id);    {

        Task<IEnumerable<Coupon>> GetAllCouponsAsync(); Task<Coupon?> GetCouponByCodeAsync(string code);

Task<List<Coupon>> GetAutoAppliedCouponsAsync(); Task<IEnumerable<Coupon>> GetAllCouponsAsync();

Task<Coupon> AddCouponAsync(Coupon coupon); Task AddCouponAsync(Coupon coupon);

Task UpdateCouponAsync(Coupon coupon); Task UpdateCouponAsync(Coupon coupon);

Task DeleteCouponAsync(int couponId); Task DeleteCouponAsync(int couponId);

Task<bool> IsCouponCodeUniqueAsync(string code, int? excludeCouponId = null); Task<bool> IsCouponCodeUniqueAsync(string code, int? excludeCouponId = null);

Task<int> GetUserCouponUsageCountAsync(string couponCode, int userId);
}

Task<CouponUsage> AddCouponUsageAsync(CouponUsage usage);}
    }
}
