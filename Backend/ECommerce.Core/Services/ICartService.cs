using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Services
{
    public interface ICartService
    {
        Task<CartResponse> GetCartAsync(int userId);
        Task<CartResponse> AddItemToCartAsync(int userId, AddCartItemRequest request);
        Task<CartResponse> UpdateCartItemAsync(int userId, int productId, UpdateCartItemRequest request);
        Task<CartResponse> RemoveItemFromCartAsync(int userId, int productId);
        Task<CartResponse> ApplyCouponAsync(int userId, string couponCode);
        Task<CartResponse> RemoveCouponAsync(int userId, string couponCode);
        Task<CartResponse> ApplyAutoAppliedCouponsAsync(int userId);
        Task ClearCartAsync(int userId);
        PriceCalculation CalculatePricing(Cart cart);
    }
}
