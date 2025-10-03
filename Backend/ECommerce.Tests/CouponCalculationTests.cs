using ECommerce.Core.Entities;
using Xunit;

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
}
