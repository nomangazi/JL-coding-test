using ECommerce.Core.DTOs;

using ECommerce.Core.Entities;
using ECommerce.Core.Entities;

using ECommerce.Core.Interfaces;
using ECommerce.Core.Interfaces;



namespace ECommerce.API.Servicesnamespace ECommerce.Core.Services

{{

    public class CouponService : Core.Services.ICouponService    public class CouponService : ICouponService

{    {

        private readonly ICouponRepository _couponRepository; private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository) => _couponRepository = couponRepository;

    public CouponService(ICouponRepository couponRepository)

    {        public async Task<Coupon?> ApplyCouponAsync(string code, int userId, decimal cartTotal, int itemCount)

            _couponRepository = couponRepository;        {

        }            var coupon = await _couponRepository.GetCouponByCodeAsync(code);

            if (coupon == null)

        public async Task<List<Coupon>> GetAllCouponsAsync()
    {

        {
            return null;

            var coupons = await _couponRepository.GetAllCouponsAsync();
        }

        return coupons.ToList();            // Validate coupon conditions

    }            // Date time validity

            if (DateTime.UtcNow<coupon.StartDate || DateTime.UtcNow> coupon.ExpiryDate) return null;

        public async Task<Coupon?> GetCouponByCodeAsync(string code)

    {            // Minimum cart total

        return await _couponRepository.GetCouponByCodeAsync(code);
    }

}
}

}
        public async Task<Coupon?> GetCouponByIdAsync(int id)
{
    return await _couponRepository.GetCouponByIdAsync(id);
}

public async Task<Coupon> CreateCouponAsync(CouponCreateRequest request)
{
    // Validate unique code
    var isUnique = await _couponRepository.IsCouponCodeUniqueAsync(request.Code);
    if (!isUnique)
    {
        throw new Exception("Coupon code already exists");
    }

    var coupon = new Coupon
    {
        Code = request.Code.ToUpper().Trim(),
        Description = request.Description,
        DiscountType = (DiscountType)request.DiscountType,
        DiscountValue = request.DiscountValue,
        MaxDiscountAmount = request.MaxDiscountAmount,
        IsAutoApplied = request.IsAutoApplied,
        StartDate = request.StartDate,
        ExpiryDate = request.ExpiryDate,
        MinimumCartItems = request.MinimumCartItems,
        MinimumTotalPrice = request.MinimumTotalPrice,
        MaxTotalUses = request.MaxTotalUses,
        MaxUsesPerUser = request.MaxUsesPerUser,
        IsActive = request.IsActive,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    if (request.ApplicableProductIds != null && request.ApplicableProductIds.Any())
    {
        coupon.SetApplicableProductIds(request.ApplicableProductIds);
    }

    return await _couponRepository.AddCouponAsync(coupon);
}

public async Task<Coupon> UpdateCouponAsync(int id, CouponUpdateRequest request)
{
    var coupon = await _couponRepository.GetCouponByIdAsync(id);
    if (coupon == null)
    {
        throw new Exception("Coupon not found");
    }

    if (request.Description != null) coupon.Description = request.Description;
    if (request.DiscountValue.HasValue) coupon.DiscountValue = request.DiscountValue.Value;
    if (request.MaxDiscountAmount.HasValue) coupon.MaxDiscountAmount = request.MaxDiscountAmount;
    if (request.IsAutoApplied.HasValue) coupon.IsAutoApplied = request.IsAutoApplied.Value;
    if (request.StartDate.HasValue) coupon.StartDate = request.StartDate;
    if (request.ExpiryDate.HasValue) coupon.ExpiryDate = request.ExpiryDate;
    if (request.MinimumCartItems.HasValue) coupon.MinimumCartItems = request.MinimumCartItems;
    if (request.MinimumTotalPrice.HasValue) coupon.MinimumTotalPrice = request.MinimumTotalPrice;
    if (request.MaxTotalUses.HasValue) coupon.MaxTotalUses = request.MaxTotalUses;
    if (request.MaxUsesPerUser.HasValue) coupon.MaxUsesPerUser = request.MaxUsesPerUser;
    if (request.IsActive.HasValue) coupon.IsActive = request.IsActive.Value;

    if (request.ApplicableProductIds != null)
    {
        coupon.SetApplicableProductIds(request.ApplicableProductIds);
    }

    await _couponRepository.UpdateCouponAsync(coupon);
    return coupon;
}

public async Task DeleteCouponAsync(int id)
{
    await _couponRepository.DeleteCouponAsync(id);
}

public async Task<CouponValidationResult> ValidateCouponAsync(string code, int userId, Cart cart)
{
    var coupon = await _couponRepository.GetCouponByCodeAsync(code);

    if (coupon == null)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Coupon not found"
        };
    }

    if (!coupon.IsActive)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Coupon is not active"
        };
    }

    // Check date validity
    var now = DateTime.UtcNow;
    if (coupon.StartDate.HasValue && now < coupon.StartDate.Value)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Coupon is not yet valid"
        };
    }

    if (coupon.ExpiryDate.HasValue && now > coupon.ExpiryDate.Value)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Coupon has expired"
        };
    }

    // Check minimum cart items
    if (coupon.MinimumCartItems.HasValue && cart.TotalItems < coupon.MinimumCartItems.Value)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = $"Minimum {coupon.MinimumCartItems} items required"
        };
    }

    // Check minimum total price
    if (coupon.MinimumTotalPrice.HasValue && cart.TotalBeforeDiscount < coupon.MinimumTotalPrice.Value)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = $"Minimum cart total of ${coupon.MinimumTotalPrice:F2} required"
        };
    }

    // Check max total uses
    if (coupon.MaxTotalUses.HasValue && coupon.CurrentTotalUses >= coupon.MaxTotalUses.Value)
    {
        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Coupon usage limit reached"
        };
    }

    // Check max uses per user
    if (coupon.MaxUsesPerUser.HasValue)
    {
        var userUsageCount = await _couponRepository.GetUserCouponUsageCountAsync(code, userId);
        if (userUsageCount >= coupon.MaxUsesPerUser.Value)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                Message = "You have reached the usage limit for this coupon"
            };
        }
    }

    // Check product restrictions
    var applicableProductIds = coupon.GetApplicableProductIds();
    if (applicableProductIds.Any())
    {
        var hasApplicableProduct = cart.Items.Any(i => applicableProductIds.Contains(i.ProductId));
        if (!hasApplicableProduct)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                Message = "This coupon is not applicable to the products in your cart"
            };
        }
    }

    var discountAmount = CalculateDiscount(coupon, cart.TotalBeforeDiscount, cart.Items.ToList());

    return new CouponValidationResult
    {
        IsValid = true,
        Message = "Coupon is valid",
        DiscountAmount = discountAmount
    };
}

public async Task<List<Coupon>> GetAutoAppliedCouponsAsync(int userId, Cart cart)
{
    var autoAppliedCoupons = await _couponRepository.GetAutoAppliedCouponsAsync();
    var validCoupons = new List<Coupon>();

    foreach (var coupon in autoAppliedCoupons)
    {
        var validationResult = await ValidateCouponAsync(coupon.Code, userId, cart);
        if (validationResult.IsValid)
        {
            validCoupons.Add(coupon);
        }
    }

    return validCoupons;
}

public decimal CalculateDiscount(Coupon coupon, decimal cartTotal, List<CartItem> items)
{
    decimal discount = 0;

    // If product-specific, calculate only for applicable products
    var applicableProductIds = coupon.GetApplicableProductIds();
    decimal applicableTotal = cartTotal;

    if (applicableProductIds.Any())
    {
        applicableTotal = items
            .Where(i => applicableProductIds.Contains(i.ProductId))
            .Sum(i => i.Subtotal);
    }

    if (coupon.DiscountType == DiscountType.Fixed)
    {
        discount = coupon.DiscountValue;
    }
    else if (coupon.DiscountType == DiscountType.Percentage)
    {
        discount = applicableTotal * (coupon.DiscountValue / 100);
    }

    // Apply max discount limit
    if (coupon.MaxDiscountAmount.HasValue && discount > coupon.MaxDiscountAmount.Value)
    {
        discount = coupon.MaxDiscountAmount.Value;
    }

    // Ensure discount doesn't exceed applicable total
    if (discount > applicableTotal)
    {
        discount = applicableTotal;
    }

    return Math.Round(discount, 2);
}
    }
}
