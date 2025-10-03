using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.API.Services
{
    public class CartService : Core.Services.ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly Core.Services.ICouponService _couponService;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICouponRepository couponRepository,
            Core.Services.ICouponService couponService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _couponService = couponService;
        }

        public async Task<CartResponse> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            // Apply auto-applied coupons
            await ApplyAutoAppliedCouponsInternalAsync(cart);

            var priceCalculation = CalculatePricing(cart);

            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> AddItemToCartAsync(int userId, AddCartItemRequest request)
        {
            if (request.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (!product.IsActive)
            {
                throw new Exception("Product is not available");
            }

            if (product.Stock < request.Quantity)
            {
                throw new Exception("Insufficient stock");
            }

            // Check if item already exists in cart
            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = product.Price; // Update price
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price,
                    Product = product
                };
                await _cartRepository.AddCartItemAsync(cartItem);
            }

            await _cartRepository.UpdateCartAsync(cart);

            // Revalidate and apply auto-coupons after cart changes
            await ApplyAutoAppliedCouponsInternalAsync(cart);

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> UpdateCartItemAsync(int userId, int productId, UpdateCartItemRequest request)
        {
            if (request.Quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null)
            {
                throw new Exception("Item not found in cart");
            }

            if (request.Quantity == 0)
            {
                await _cartRepository.DeleteCartItemAsync(cartItem.Id);
            }
            else
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product != null && product.Stock < request.Quantity)
                {
                    throw new Exception("Insufficient stock");
                }

                cartItem.Quantity = request.Quantity;
                await _cartRepository.UpdateCartItemAsync(cartItem);
            }

            await _cartRepository.UpdateCartAsync(cart);

            // Revalidate coupons after cart changes
            await RevalidateAppliedCouponsAsync(cart);
            await ApplyAutoAppliedCouponsInternalAsync(cart);

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> RemoveItemFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem != null)
            {
                await _cartRepository.DeleteCartItemAsync(cartItem.Id);
                await _cartRepository.UpdateCartAsync(cart);

                // Revalidate coupons
                await RevalidateAppliedCouponsAsync(cart);
                await ApplyAutoAppliedCouponsInternalAsync(cart);
            }

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> ApplyCouponAsync(int userId, string couponCode)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            // Check if coupon is already applied
            if (cart.AppliedCoupons.Any(ac => ac.Coupon.Code == couponCode))
            {
                throw new Exception("Coupon is already applied");
            }

            // Validate coupon
            var validationResult = await _couponService.ValidateCouponAsync(couponCode, userId, cart);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.Message);
            }

            var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode);
            if (coupon == null)
            {
                throw new Exception("Coupon not found");
            }

            // Apply coupon
            var appliedCoupon = new AppliedCoupon
            {
                CartId = cart.Id,
                CouponId = coupon.Id,
                Coupon = coupon,
                AppliedAt = DateTime.UtcNow
            };

            await _cartRepository.AddAppliedCouponAsync(appliedCoupon);

            // Record usage
            var usage = new CouponUsage
            {
                CouponId = coupon.Id,
                UserId = userId,
                UsedAt = DateTime.UtcNow
            };
            await _couponRepository.AddCouponUsageAsync(usage);

            // Update coupon usage count
            coupon.CurrentTotalUses++;
            await _couponRepository.UpdateCouponAsync(coupon);

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> RemoveCouponAsync(int userId, string couponCode)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            await _cartRepository.RemoveAppliedCouponAsync(cart.Id, couponCode);

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task<CartResponse> ApplyAutoAppliedCouponsAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            await ApplyAutoAppliedCouponsInternalAsync(cart);

            var priceCalculation = CalculatePricing(cart);
            return MapToCartResponse(cart, priceCalculation);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartRepository.ClearCartAsync(cart.Id);
            }
        }

        public PriceCalculation CalculatePricing(Cart cart)
        {
            var totalBeforeDiscount = cart.Items.Sum(i => i.Subtotal);
            var discountDetails = new List<AppliedDiscountDetail>();
            decimal totalDiscount = 0;

            foreach (var appliedCoupon in cart.AppliedCoupons)
            {
                var discount = _couponService.CalculateDiscount(
                    appliedCoupon.Coupon,
                    totalBeforeDiscount,
                    cart.Items.ToList());

                if (discount > 0)
                {
                    discountDetails.Add(new AppliedDiscountDetail
                    {
                        CouponCode = appliedCoupon.Coupon.Code,
                        DiscountAmount = discount,
                        DiscountType = appliedCoupon.Coupon.DiscountType
                    });
                    totalDiscount += discount;
                }
            }

            var finalAmount = totalBeforeDiscount - totalDiscount;
            if (finalAmount < 0) finalAmount = 0;

            return new PriceCalculation
            {
                TotalBeforeDiscount = totalBeforeDiscount,
                TotalDiscount = totalDiscount,
                FinalPayableAmount = finalAmount,
                DiscountDetails = discountDetails
            };
        }

        private async Task ApplyAutoAppliedCouponsInternalAsync(Cart cart)
        {
            var autoAppliedCoupons = await _couponService.GetAutoAppliedCouponsAsync(cart.UserId, cart);

            foreach (var coupon in autoAppliedCoupons)
            {
                // Check if already applied
                if (cart.AppliedCoupons.Any(ac => ac.CouponId == coupon.Id))
                {
                    continue;
                }

                // Validate the coupon
                var validationResult = await _couponService.ValidateCouponAsync(coupon.Code, cart.UserId, cart);
                if (validationResult.IsValid)
                {
                    var appliedCoupon = new AppliedCoupon
                    {
                        CartId = cart.Id,
                        CouponId = coupon.Id,
                        Coupon = coupon,
                        AppliedAt = DateTime.UtcNow
                    };

                    await _cartRepository.AddAppliedCouponAsync(appliedCoupon);
                }
            }
        }

        private async Task RevalidateAppliedCouponsAsync(Cart cart)
        {
            var invalidCoupons = new List<string>();

            foreach (var appliedCoupon in cart.AppliedCoupons.ToList())
            {
                var validationResult = await _couponService.ValidateCouponAsync(
                    appliedCoupon.Coupon.Code,
                    cart.UserId,
                    cart);

                if (!validationResult.IsValid)
                {
                    invalidCoupons.Add(appliedCoupon.Coupon.Code);
                }
            }

            // Remove invalid coupons
            foreach (var code in invalidCoupons)
            {
                await _cartRepository.RemoveAppliedCouponAsync(cart.Id, code);
            }
        }

        private CartResponse MapToCartResponse(Cart cart, PriceCalculation priceCalculation)
        {
            return new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    Price = i.Price,
                    Quantity = i.Quantity,
                    Subtotal = i.Subtotal
                }).ToList(),
                AppliedCoupons = cart.AppliedCoupons.Select(ac => new AppliedCouponDto
                {
                    Id = ac.Id,
                    Code = ac.Coupon.Code,
                    Description = ac.Coupon.Description,
                    IsAutoApplied = ac.Coupon.IsAutoApplied,
                    AppliedAt = ac.AppliedAt,
                    DiscountType = ac.Coupon.DiscountType,
                    DiscountValue = ac.Coupon.DiscountValue
                }).ToList(),
                PriceCalculation = priceCalculation
            };
        }
    }
}
