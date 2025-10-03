using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;

namespace ECommerce.API.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<List<Coupon>> GetAllCouponsAsync()
        {
            var coupons = await _couponRepository.GetAllCouponsAsync();
            return coupons.ToList();
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            return await _couponRepository.GetCouponByCodeAsync(code);
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

            // Update properties only if provided
            if (!string.IsNullOrEmpty(request.Description))
                coupon.Description = request.Description;

            if (request.DiscountValue.HasValue)
                coupon.DiscountValue = request.DiscountValue.Value;

            if (request.MaxDiscountAmount.HasValue)
                coupon.MaxDiscountAmount = request.MaxDiscountAmount;

            if (request.IsAutoApplied.HasValue)
                coupon.IsAutoApplied = request.IsAutoApplied.Value;

            coupon.StartDate = request.StartDate;
            coupon.ExpiryDate = request.ExpiryDate;
            coupon.MinimumCartItems = request.MinimumCartItems;
            coupon.MinimumTotalPrice = request.MinimumTotalPrice;
            coupon.MaxTotalUses = request.MaxTotalUses;
            coupon.MaxUsesPerUser = request.MaxUsesPerUser;

            if (request.IsActive.HasValue)
                coupon.IsActive = request.IsActive.Value;

            coupon.UpdatedAt = DateTime.UtcNow;

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

            // Check if coupon is active
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
            if (coupon.MinimumCartItems.HasValue && cart.Items.Count < coupon.MinimumCartItems.Value)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    Message = $"Minimum {coupon.MinimumCartItems.Value} items required"
                };
            }

            // Check minimum total price
            var cartTotal = cart.Items.Sum(i => i.Quantity * i.Product.Price);
            if (coupon.MinimumTotalPrice.HasValue && cartTotal < coupon.MinimumTotalPrice.Value)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    Message = $"Minimum cart total of ${coupon.MinimumTotalPrice.Value} required"
                };
            }

            // Check usage limits
            if (coupon.MaxTotalUses.HasValue && coupon.CurrentTotalUses >= coupon.MaxTotalUses.Value)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    Message = "Coupon usage limit reached"
                };
            }

            // Check per-user usage limit
            if (coupon.MaxUsesPerUser.HasValue)
            {
                var userUsageCount = await _couponRepository.GetUserCouponUsageCountAsync(code, userId);
                if (userUsageCount >= coupon.MaxUsesPerUser.Value)
                {
                    return new CouponValidationResult
                    {
                        IsValid = false,
                        Message = "Personal usage limit reached for this coupon"
                    };
                }
            }

            // Calculate discount
            var discountAmount = CalculateDiscount(coupon, cartTotal, cart.Items);

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
            decimal discount;

            // Filter applicable items if product restrictions exist
            var applicableProductIds = coupon.GetApplicableProductIds();
            var applicableItems = applicableProductIds.Any()
                ? items.Where(i => applicableProductIds.Contains(i.ProductId)).ToList()
                : items;

            if (!applicableItems.Any())
            {
                return 0;
            }

            var applicableTotal = applicableItems.Sum(i => i.Quantity * i.Product.Price);

            if (coupon.DiscountType == DiscountType.Fixed)
            {
                discount = coupon.DiscountValue;
            }
            else // Percentage
            {
                discount = applicableTotal * (coupon.DiscountValue / 100);
            }

            // Apply maximum discount limit if specified
            if (coupon.MaxDiscountAmount.HasValue && discount > coupon.MaxDiscountAmount.Value)
            {
                discount = coupon.MaxDiscountAmount.Value;
            }

            // Ensure discount doesn't exceed applicable total
            return Math.Min(discount, applicableTotal);
        }
    }
}