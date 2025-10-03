namespace ECommerce.Core.DTOs
{
    public class ApplyCouponRequest
    {
        public string Code { get; set; } = string.Empty;
        public int UserId { get; set; }
        public decimal CartTotal { get; set; }
        public int ItemCount { get; set; }
    }
}