using ECommerce.Core.Entities;

namespace ECommerce.Core.DTOs
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public List<AppliedCouponDto> AppliedCoupons { get; set; } = new List<AppliedCouponDto>();
        public PriceCalculation PriceCalculation { get; set; } = new PriceCalculation();
    }
}