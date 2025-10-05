using ECommerce.Core.Entities;

namespace ECommerce.Tests;

public class CouponCalculationTests
{
    [Fact]
    public void FixedDiscount_ShouldCalculateCorrectly()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "FIXED10",
            DiscountType = DiscountType.Fixed,
            DiscountValue = 10
        };

        decimal cartTotal = 100;

        // Act
        decimal discount = coupon.DiscountType == DiscountType.Fixed
            ? coupon.DiscountValue
            : cartTotal * (coupon.DiscountValue / 100);

        // Assert
        Assert.Equal(10, discount);
    }

    [Fact]
    public void PercentageDiscount_ShouldCalculateCorrectly()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PERCENT20",
            DiscountType = DiscountType.Percentage,
            DiscountValue = 20
        };

        decimal cartTotal = 100;

        // Act
        decimal discount = coupon.DiscountType == DiscountType.Fixed
            ? coupon.DiscountValue
            : cartTotal * (coupon.DiscountValue / 100);

        // Assert
        Assert.Equal(20, discount);
    }

    [Fact]
    public void Cart_TotalCalculation_ShouldBeCorrect()
    {
        // Arrange
        var cart = new Cart
        {
            Id = 1,
            UserId = 1,
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, Price = 50 },
                new CartItem { ProductId = 2, Quantity = 1, Price = 30 }
            }
        };

        // Act
        var total = cart.TotalBeforeDiscount;
        var itemCount = cart.TotalItems;

        // Assert
        Assert.Equal(130, total); // (2 * 50) + (1 * 30)
        Assert.Equal(3, itemCount); // 2 + 1
    }

    [Fact]
    public void CouponValidation_ExpiredCoupon_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "EXPIRED",
            ExpiryDate = DateTime.UtcNow.AddDays(-1),
            IsActive = true
        };

        // Act
        var isExpired = coupon.ExpiryDate.HasValue && DateTime.UtcNow > coupon.ExpiryDate.Value;

        // Assert
        Assert.True(isExpired);
    }

    [Fact]
    public void CouponValidation_FutureCoupon_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "FUTURE",
            StartDate = DateTime.UtcNow.AddDays(1),
            IsActive = true
        };

        // Act
        var isNotYetValid = coupon.StartDate.HasValue && DateTime.UtcNow < coupon.StartDate.Value;

        // Assert
        Assert.True(isNotYetValid);
    }

    [Fact]
    public void PriceCalculation_WithDiscount_ShouldCalculateCorrectly()
    {
        // Arrange
        decimal totalBeforeDiscount = 100;
        decimal discount = 20;

        // Act
        decimal finalAmount = totalBeforeDiscount - discount;

        // Assert
        Assert.Equal(80, finalAmount);
    }

    // ========================================
    // PERCENTAGE DISCOUNT WITH MAX CAP TESTS
    // ========================================

    [Fact]
    public void PercentageDiscount_WithMaxCap_ShouldApplyCap()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PERCENT20",
            DiscountType = DiscountType.Percentage,
            DiscountValue = 20,
            MaxDiscountAmount = 15 // Cap at $15
        };

        decimal cartTotal = 100; // 20% would be $20, but capped at $15

        // Act
        decimal percentDiscount = cartTotal * (coupon.DiscountValue / 100);
        decimal discount = coupon.MaxDiscountAmount.HasValue
            ? Math.Min(percentDiscount, coupon.MaxDiscountAmount.Value)
            : percentDiscount;

        // Assert
        Assert.Equal(15, discount); // Should be $15 (capped), not $20
    }

    [Fact]
    public void PercentageDiscount_BelowMaxCap_ShouldUseCalculatedAmount()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PERCENT10",
            DiscountType = DiscountType.Percentage,
            DiscountValue = 10,
            MaxDiscountAmount = 50 // Cap at $50
        };

        decimal cartTotal = 100; // 10% = $10, below cap

        // Act
        decimal percentDiscount = cartTotal * (coupon.DiscountValue / 100);
        decimal discount = coupon.MaxDiscountAmount.HasValue
            ? Math.Min(percentDiscount, coupon.MaxDiscountAmount.Value)
            : percentDiscount;

        // Assert
        Assert.Equal(10, discount); // Should be $10 (calculated), not $50 (cap)
    }

    // ========================================
    // MINIMUM CART VALUE VALIDATION TESTS
    // ========================================

    [Fact]
    public void CouponValidation_MinimumCartValue_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "MIN50",
            MinimumTotalPrice = 50,
            IsActive = true
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, Price = 30 } // Total: $60
            }
        };

        // Act
        var meetsMinimum = cart.TotalBeforeDiscount >= coupon.MinimumTotalPrice.Value;

        // Assert
        Assert.True(meetsMinimum);
    }

    [Fact]
    public void CouponValidation_BelowMinimumCartValue_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "MIN100",
            MinimumTotalPrice = 100,
            IsActive = true
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 1, Price = 50 } // Total: $50
            }
        };

        // Act
        var meetsMinimum = cart.TotalBeforeDiscount >= coupon.MinimumTotalPrice.Value;

        // Assert
        Assert.False(meetsMinimum);
    }

    // ========================================
    // MINIMUM CART ITEMS VALIDATION TESTS
    // ========================================

    [Fact]
    public void CouponValidation_MinimumCartItems_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "MIN3ITEMS",
            MinimumCartItems = 3,
            IsActive = true
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, Price = 20 },
                new CartItem { ProductId = 2, Quantity = 1, Price = 30 }
            }
        };

        // Act
        var meetsMinimum = cart.TotalItems >= coupon.MinimumCartItems.Value;

        // Assert
        Assert.True(meetsMinimum); // 2 + 1 = 3 items
    }

    [Fact]
    public void CouponValidation_BelowMinimumCartItems_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "MIN5ITEMS",
            MinimumCartItems = 5,
            IsActive = true
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 1, Price = 20 },
                new CartItem { ProductId = 2, Quantity = 1, Price = 30 }
            }
        };

        // Act
        var meetsMinimum = cart.TotalItems >= coupon.MinimumCartItems.Value;

        // Assert
        Assert.False(meetsMinimum); // Only 2 items
    }

    // ========================================
    // USAGE LIMIT TESTS
    // ========================================

    [Fact]
    public void CouponValidation_MaxTotalUses_NotReached_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "LIMITED100",
            MaxTotalUses = 100,
            CurrentTotalUses = 50,
            IsActive = true
        };

        // Act
        var canBeUsed = !coupon.MaxTotalUses.HasValue ||
                        coupon.CurrentTotalUses < coupon.MaxTotalUses.Value;

        // Assert
        Assert.True(canBeUsed);
    }

    [Fact]
    public void CouponValidation_MaxTotalUses_Reached_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "LIMITED100",
            MaxTotalUses = 100,
            CurrentTotalUses = 100,
            IsActive = true
        };

        // Act
        var canBeUsed = !coupon.MaxTotalUses.HasValue ||
                        coupon.CurrentTotalUses < coupon.MaxTotalUses.Value;

        // Assert
        Assert.False(canBeUsed);
    }

    [Fact]
    public void CouponValidation_MaxTotalUses_Exceeded_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "LIMITED50",
            MaxTotalUses = 50,
            CurrentTotalUses = 75, // Over limit
            IsActive = true
        };

        // Act
        var canBeUsed = !coupon.MaxTotalUses.HasValue ||
                        coupon.CurrentTotalUses < coupon.MaxTotalUses.Value;

        // Assert
        Assert.False(canBeUsed);
    }

    // ========================================
    // ACTIVE STATUS TESTS
    // ========================================

    [Fact]
    public void CouponValidation_InactiveCoupon_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "INACTIVE",
            IsActive = false
        };

        // Act & Assert
        Assert.False(coupon.IsActive);
    }

    [Fact]
    public void CouponValidation_ActiveCoupon_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "ACTIVE",
            IsActive = true
        };

        // Act & Assert
        Assert.True(coupon.IsActive);
    }

    // ========================================
    // AUTO-APPLIED COUPON TESTS
    // ========================================

    [Fact]
    public void AutoAppliedCoupon_Flag_ShouldBeSet()
    {
        // Arrange & Act
        var coupon = new Coupon
        {
            Code = "AUTO50",
            IsAutoApplied = true,
            DiscountType = DiscountType.Fixed,
            DiscountValue = 5,
            MinimumTotalPrice = 50
        };

        // Assert
        Assert.True(coupon.IsAutoApplied);
    }

    [Fact]
    public void AutoAppliedCoupon_WithMinimum_ShouldValidateCartTotal()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "AUTO100",
            IsAutoApplied = true,
            MinimumTotalPrice = 100,
            IsActive = true
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 3, Price = 40 } // Total: $120
            }
        };

        // Act
        var isEligible = coupon.IsAutoApplied &&
                        coupon.IsActive &&
                        (!coupon.MinimumTotalPrice.HasValue ||
                         cart.TotalBeforeDiscount >= coupon.MinimumTotalPrice.Value);

        // Assert
        Assert.True(isEligible);
    }

    // ========================================
    // MULTIPLE COUPONS (STACKING) TESTS
    // ========================================

    [Fact]
    public void MultipleCoupons_ShouldStackCorrectly()
    {
        // Arrange
        var cart = new Cart
        {
            Id = 1,
            UserId = 1,
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, Price = 50 } // $100
            }
        };

        var coupon1Discount = 10m; // SAVE10 - Fixed $10
        var coupon2Discount = 20m; // PERCENT20 - 20% of $100 = $20

        // Act
        var totalDiscount = coupon1Discount + coupon2Discount;
        var finalTotal = cart.TotalBeforeDiscount - totalDiscount;

        // Assert
        Assert.Equal(100, cart.TotalBeforeDiscount);
        Assert.Equal(30, totalDiscount);
        Assert.Equal(70, finalTotal);
    }

    [Fact]
    public void MultipleCoupons_ShouldNotExceedCartTotal()
    {
        // Arrange
        var cartTotal = 50m;
        var discount1 = 30m;
        var discount2 = 25m;
        var totalDiscount = discount1 + discount2; // $55, exceeds cart total

        // Act
        var cappedDiscount = Math.Min(totalDiscount, cartTotal);
        var finalTotal = Math.Max(0, cartTotal - cappedDiscount);

        // Assert
        Assert.Equal(50, cappedDiscount); // Capped at cart total
        Assert.Equal(0, finalTotal); // Can't go below $0
    }

    // ========================================
    // PRODUCT-SPECIFIC COUPON TESTS
    // ========================================

    [Fact]
    public void ProductSpecificCoupon_ApplicableProducts_ShouldSetCorrectly()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "ELECTRONICS20",
            ApplicableProductIdsJson = string.Empty
        };

        var productIds = new List<int> { 1, 2, 3 };

        // Act
        coupon.SetApplicableProductIds(productIds);
        var retrievedIds = coupon.GetApplicableProductIds();

        // Assert
        Assert.Equal(3, retrievedIds.Count);
        Assert.Contains(1, retrievedIds);
        Assert.Contains(2, retrievedIds);
        Assert.Contains(3, retrievedIds);
    }

    [Fact]
    public void ProductSpecificCoupon_EmptyList_ShouldApplyToAll()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "ALLPRODUCTS",
            ApplicableProductIdsJson = string.Empty
        };

        // Act
        var applicableProducts = coupon.GetApplicableProductIds();

        // Assert
        Assert.Empty(applicableProducts);
    }

    [Fact]
    public void ProductSpecificCoupon_CartHasApplicableProduct_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PRODUCT123",
            ApplicableProductIdsJson = "[1,2,3]"
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 2, Quantity = 1, Price = 50 }
            }
        };

        var applicableProducts = coupon.GetApplicableProductIds();

        // Act
        var hasApplicableProduct = cart.Items.Any(item =>
            applicableProducts.Contains(item.ProductId));

        // Assert
        Assert.True(hasApplicableProduct);
    }

    [Fact]
    public void ProductSpecificCoupon_CartHasNoApplicableProduct_ShouldBeInvalid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PRODUCT123",
            ApplicableProductIdsJson = "[1,2,3]"
        };

        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 5, Quantity = 1, Price = 50 } // Product 5 not in list
            }
        };

        var applicableProducts = coupon.GetApplicableProductIds();

        // Act
        var hasApplicableProduct = applicableProducts.Any() &&
                                  cart.Items.Any(item => applicableProducts.Contains(item.ProductId));

        // Assert
        Assert.False(hasApplicableProduct);
    }

    // ========================================
    // DATE RANGE VALIDATION TESTS
    // ========================================

    [Fact]
    public void CouponValidation_WithinDateRange_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "SEASONAL",
            StartDate = DateTime.UtcNow.AddDays(-5),
            ExpiryDate = DateTime.UtcNow.AddDays(5),
            IsActive = true
        };

        var now = DateTime.UtcNow;

        // Act
        var isWithinRange = (!coupon.StartDate.HasValue || now >= coupon.StartDate.Value) &&
                           (!coupon.ExpiryDate.HasValue || now <= coupon.ExpiryDate.Value);

        // Assert
        Assert.True(isWithinRange);
    }

    [Fact]
    public void CouponValidation_NoDateRestrictions_ShouldBeValid()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "ANYTIME",
            StartDate = null,
            ExpiryDate = null,
            IsActive = true
        };

        var now = DateTime.UtcNow;

        // Act
        var isWithinRange = (!coupon.StartDate.HasValue || now >= coupon.StartDate.Value) &&
                           (!coupon.ExpiryDate.HasValue || now <= coupon.ExpiryDate.Value);

        // Assert
        Assert.True(isWithinRange);
    }

    // ========================================
    // CART ITEM CALCULATION TESTS
    // ========================================

    [Fact]
    public void CartItem_Subtotal_ShouldCalculateCorrectly()
    {
        // Arrange
        var cartItem = new CartItem
        {
            ProductId = 1,
            Quantity = 5,
            Price = 19.99m
        };

        // Act
        var subtotal = cartItem.Subtotal;

        // Assert
        Assert.Equal(99.95m, subtotal);
    }

    [Fact]
    public void Cart_MultipleItems_ShouldCalculateTotalCorrectly()
    {
        // Arrange
        var cart = new Cart
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, Price = 25.50m },  // $51.00
                new CartItem { ProductId = 2, Quantity = 3, Price = 10.00m },  // $30.00
                new CartItem { ProductId = 3, Quantity = 1, Price = 49.99m }   // $49.99
            }
        };

        // Act
        var total = cart.TotalBeforeDiscount;
        var itemCount = cart.TotalItems;

        // Assert
        Assert.Equal(130.99m, total);
        Assert.Equal(6, itemCount); // 2 + 3 + 1
    }

    // ========================================
    // EDGE CASE TESTS
    // ========================================

    [Fact]
    public void PercentageDiscount_100Percent_ShouldMakeCartFree()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "FREE100",
            DiscountType = DiscountType.Percentage,
            DiscountValue = 100
        };

        decimal cartTotal = 150;

        // Act
        decimal discount = cartTotal * (coupon.DiscountValue / 100);
        decimal finalTotal = Math.Max(0, cartTotal - discount);

        // Assert
        Assert.Equal(150, discount);
        Assert.Equal(0, finalTotal);
    }

    [Fact]
    public void FixedDiscount_ExceedsCartTotal_ShouldCapAtCartTotal()
    {
        // Arrange
        decimal cartTotal = 30m;
        decimal fixedDiscount = 50m; // Exceeds cart total

        // Act
        decimal appliedDiscount = Math.Min(fixedDiscount, cartTotal);
        decimal finalTotal = Math.Max(0, cartTotal - appliedDiscount);

        // Assert
        Assert.Equal(30m, appliedDiscount); // Capped at cart total
        Assert.Equal(0m, finalTotal);
    }

    [Fact]
    public void EmptyCart_ShouldHaveZeroTotal()
    {
        // Arrange
        var cart = new Cart
        {
            Items = new List<CartItem>()
        };

        // Act & Assert
        Assert.Equal(0, cart.TotalBeforeDiscount);
        Assert.Equal(0, cart.TotalItems);
    }

    [Fact]
    public void CouponDiscount_ShouldRoundToTwoDecimals()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "PERCENT15",
            DiscountType = DiscountType.Percentage,
            DiscountValue = 15
        };

        decimal cartTotal = 47.77m;

        // Act
        decimal discount = cartTotal * (coupon.DiscountValue / 100);
        decimal roundedDiscount = Math.Round(discount, 2);

        // Assert
        Assert.Equal(7.17m, roundedDiscount); // 15% of 47.77 = 7.1655, rounded to 7.17
    }

    [Fact]
    public void CouponValidation_CurrentTotalUsesIncrement_ShouldWork()
    {
        // Arrange
        var coupon = new Coupon
        {
            Code = "INCREMENTTEST",
            CurrentTotalUses = 10,
            MaxTotalUses = 100
        };

        // Act
        coupon.CurrentTotalUses++;

        // Assert
        Assert.Equal(11, coupon.CurrentTotalUses);
    }
}
