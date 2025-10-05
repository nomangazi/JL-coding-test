import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useCartStore } from "../store/cartStore";
import { useUserStore } from "../store/userStore";
import { formatCurrency } from "../lib/utils";
import { Button } from "../components/ui/button";
import { Skeleton } from "../components/ui/skeleton";
import { ShoppingCart, Loader2 } from "lucide-react";

export function CartPage() {
  const { cart, loading: cartLoading, fetchCart, updateItem, removeItem, applyCoupon, removeCoupon } = useCartStore();
  const { isAuthenticated } = useUserStore();
  const [couponCode, setCouponCode] = useState("");
  const [updatingItems, setUpdatingItems] = useState<Set<number>>(new Set());

  useEffect(() => {
    fetchCart();
  }, [fetchCart]);

  const handleUpdateQuantity = async (productId: number, quantity: number) => {
    // Add item to updating set
    setUpdatingItems((prev) => new Set(prev).add(productId));

    try {
      if (quantity === 0) {
        await removeItem(productId);
        toast.success("Item removed from cart");
      } else {
        await updateItem(productId, { quantity });
        toast.success("Quantity updated");
      }
    } catch {
      toast.error("Failed to update quantity");
    } finally {
      // Remove item from updating set
      setUpdatingItems((prev) => {
        const newSet = new Set(prev);
        newSet.delete(productId);
        return newSet;
      });
    }
  };

  const handleApplyCoupon = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!couponCode.trim()) return;

    try {
      await applyCoupon(couponCode.toUpperCase());
      toast.success(`Coupon ${couponCode} applied successfully!`);
      setCouponCode("");
    } catch (error: unknown) {
      const errorMessage =
        error && typeof error === "object" && "response" in error
          ? (error as { response?: { data?: { message?: string } } }).response?.data?.message
          : error instanceof Error
          ? error.message
          : "Failed to apply coupon";
      toast.error(errorMessage || "Failed to apply coupon");
    }
  };

  const handleRemoveCoupon = async (code: string) => {
    try {
      await removeCoupon(code);
      toast.success(`Coupon ${code} removed`);
    } catch {
      toast.error("Failed to remove coupon");
    }
  };

  return (
    <div>
      <h2 className="text-2xl font-bold mb-6 flex items-center gap-2">
        <ShoppingCart className="w-7 h-7" />
        Shopping Cart
      </h2>

      {cartLoading && !cart && (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Cart Items Skeleton */}
          <div className="lg:col-span-2 space-y-4">
            {[1, 2, 3].map((i) => (
              <div key={i} className="bg-white rounded-lg shadow p-6">
                <div className="flex items-start gap-4">
                  <div className="flex-1 space-y-3">
                    <Skeleton className="h-6 w-3/4" />
                    <Skeleton className="h-4 w-1/4" />
                    <Skeleton className="h-10 w-48" />
                  </div>
                  <Skeleton className="h-8 w-24" />
                </div>
              </div>
            ))}
          </div>

          {/* Order Summary Skeleton */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow p-6">
              <Skeleton className="h-7 w-40 mb-4" />
              <div className="space-y-4">
                <Skeleton className="h-10 w-full" />
                <Skeleton className="h-20 w-full" />
                <Skeleton className="h-32 w-full" />
              </div>
            </div>
          </div>
        </div>
      )}

      {!isAuthenticated && !cartLoading && (
        <div className="bg-white rounded-lg shadow p-12 text-center">
          <ShoppingCart className="w-16 h-16 mx-auto text-gray-300 mb-4" />
          <p className="text-gray-500 text-lg mb-4">Please login to view your cart</p>
          <Button onClick={() => useCartStore.getState().setShowAuthModal(true)}>Login</Button>
        </div>
      )}

      {isAuthenticated && cart && (
        <>
          {cart.items.length === 0 ? (
            <div className="bg-white rounded-lg shadow p-12 text-center">
              <ShoppingCart className="w-16 h-16 mx-auto text-gray-300 mb-4" />
              <p className="text-gray-500 text-lg mb-4">Your cart is empty</p>
              <p className="text-gray-400 text-sm">Add some products to get started!</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
              {/* Cart Items */}
              <div className="lg:col-span-2 space-y-4">
                {cart.items.map((item) => {
                  const isUpdating = updatingItems.has(item.productId);
                  return (
                    <div key={item.id} className={`bg-white rounded-lg shadow p-6 transition-opacity ${isUpdating ? "opacity-60 pointer-events-none" : "opacity-100"}`}>
                      <div className="flex items-start gap-4">
                        <div className="flex-1">
                          <h3 className="text-lg font-semibold mb-1 flex items-center gap-2">
                            {item.productName}
                            {isUpdating && <Loader2 className="w-4 h-4 animate-spin text-blue-600" />}
                          </h3>
                          <p className="text-gray-600 mb-3">{formatCurrency(item.price)} each</p>

                          <div className="flex items-center gap-3">
                            <div className="flex items-center gap-2 border rounded-lg">
                              <Button onClick={() => handleUpdateQuantity(item.productId, item.quantity - 1)} size="sm" variant="ghost" className="h-10 w-10" disabled={isUpdating}>
                                -
                              </Button>
                              <span className="px-4 font-medium min-w-[2rem] text-center">{item.quantity}</span>
                              <Button onClick={() => handleUpdateQuantity(item.productId, item.quantity + 1)} size="sm" variant="ghost" className="h-10 w-10" disabled={isUpdating}>
                                +
                              </Button>
                            </div>

                            <Button onClick={() => handleUpdateQuantity(item.productId, 0)} size="sm" variant="destructive" disabled={isUpdating}>
                              Remove
                            </Button>
                          </div>
                        </div>

                        <div className="text-right">
                          <p className="text-xl font-bold text-gray-900">{formatCurrency(item.subtotal)}</p>
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>

              {/* Order Summary */}
              <div className="lg:col-span-1">
                <div className={`bg-white rounded-lg shadow p-6 sticky top-8 transition-opacity ${cartLoading ? "opacity-60" : "opacity-100"}`}>
                  <h3 className="text-xl font-bold mb-4 flex items-center gap-2">
                    Order Summary
                    {cartLoading && <Loader2 className="w-5 h-5 animate-spin text-blue-600" />}
                  </h3>

                  {/* Coupon Input */}
                  <div className="mb-6">
                    <form onSubmit={handleApplyCoupon} className="flex gap-2">
                      <input
                        type="text"
                        value={couponCode}
                        onChange={(e) => setCouponCode(e.target.value)}
                        placeholder="Enter coupon code"
                        className="flex-1 px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                      />
                      <Button type="submit" disabled={!couponCode.trim() || cartLoading}>
                        Apply
                      </Button>
                    </form>
                  </div>

                  {/* Applied Coupons */}
                  {cart.appliedCoupons.length > 0 && (
                    <div className="mb-6">
                      <h4 className="font-semibold mb-2 text-sm text-gray-700">Applied Coupons:</h4>
                      <div className="space-y-2">
                        {cart.appliedCoupons.map((coupon) => (
                          <div key={coupon.id} className="flex justify-between items-center bg-green-50 p-3 rounded-lg">
                            <div>
                              <span className="font-medium text-sm">{coupon.code}</span>
                              {coupon.isAutoApplied && <span className="ml-2 text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded">Auto-applied</span>}
                            </div>
                            {!coupon.isAutoApplied && (
                              <Button onClick={() => handleRemoveCoupon(coupon.code)} size="sm" variant="ghost">
                                Remove
                              </Button>
                            )}
                          </div>
                        ))}
                      </div>
                    </div>
                  )}

                  {/* Price Summary */}
                  <div className="border-t pt-4 space-y-3">
                    <div className="flex justify-between text-gray-600">
                      <span>Subtotal:</span>
                      <span className="font-medium">{formatCurrency(cart.priceCalculation.totalBeforeDiscount)}</span>
                    </div>

                    {cart.priceCalculation.totalDiscount > 0 && (
                      <div className="flex justify-between text-green-600">
                        <span>Discount:</span>
                        <span className="font-medium">-{formatCurrency(cart.priceCalculation.totalDiscount)}</span>
                      </div>
                    )}

                    <div className="flex justify-between text-xl font-bold border-t pt-3">
                      <span>Total:</span>
                      <span>{formatCurrency(cart.priceCalculation.finalPayableAmount)}</span>
                    </div>
                  </div>

                  {/* Discount Breakdown */}
                  {cart.priceCalculation.discountDetails.length > 0 && (
                    <div className="mt-4 pt-4 border-t">
                      <p className="font-semibold text-sm text-gray-700 mb-2">Discount Breakdown:</p>
                      <div className="space-y-1">
                        {cart.priceCalculation.discountDetails.map((detail, index) => (
                          <p key={index} className="text-sm text-gray-600">
                            {detail.couponCode}: <span className="text-green-600 font-medium">-{formatCurrency(detail.discountAmount)}</span>
                          </p>
                        ))}
                      </div>
                    </div>
                  )}

                  <Button className="w-full mt-6" size="lg">
                    Proceed to Checkout
                  </Button>
                </div>
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}
