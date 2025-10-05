import { useEffect, useState } from "react";
import { ToastContainer, toast } from "react-toastify";
import { useCartStore } from "./store/cartStore";
import { useProductStore } from "./store/productStore";
import { useUserStore } from "./store/userStore";
import { formatCurrency } from "./lib/utils";
import { Button } from "./components/ui/button";
import { GlobalAuthModal } from "./components/GlobalAuthModal";
import type { Product } from "./types";

function App() {
  const { cart, loading: cartLoading, fetchCart, addItem, updateItem, removeItem, applyCoupon, removeCoupon } = useCartStore();
  const { initializeAuth, user, isAuthenticated } = useUserStore();

  const { products, loading: productsLoading, fetchProducts } = useProductStore();
  const [couponCode, setCouponCode] = useState("");

  useEffect(() => {
    initializeAuth();
    fetchCart();
    fetchProducts();
  }, [fetchCart, fetchProducts, initializeAuth]);

  const handleAddToCart = async (product: Product) => {
    try {
      await addItem({ productId: product.id, quantity: 1 });
      toast.success(`Added ${product.name} to cart`);
    } catch {
      toast.error("Failed to add item to cart");
    }
  };

  const handleUpdateQuantity = async (productId: number, quantity: number) => {
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
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900">E-Commerce Cart & Coupon System</h1>

          <div className="flex items-center space-x-4">
            {isAuthenticated && user ? (
              <div className="flex items-center space-x-3">
                <div className="text-right">
                  <p className="text-sm font-medium text-gray-900">{user.name}</p>
                  <p className="text-xs text-gray-500">{user.email}</p>
                </div>
                <Button variant="outline" size="sm" onClick={() => useUserStore.getState().logout()}>
                  Logout
                </Button>
              </div>
            ) : (
              <Button variant="outline" onClick={() => useCartStore.getState().setShowAuthModal(true)}>
                Login
              </Button>
            )}
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Products Section */}
          <div className="lg:col-span-2">
            <h2 className="text-2xl font-bold mb-4">Products</h2>
            {productsLoading ? (
              <div className="text-center py-8">Loading products...</div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {products.map((product) => (
                  <div key={product.id} className="bg-white rounded-lg shadow p-6">
                    <div className="flex justify-between items-start mb-4">
                      <div>
                        <h3 className="text-lg font-semibold">{product.name}</h3>
                        <p className="text-gray-600 text-sm mt-1">{product.description}</p>
                        <p className="text-sm text-gray-500 mt-2">Stock: {product.stock}</p>
                      </div>
                      <span className="text-xl font-bold text-blue-600">{formatCurrency(product.price)}</span>
                    </div>
                    <Button onClick={() => handleAddToCart(product)} disabled={product.stock === 0 || cartLoading} className="w-full">
                      {product.stock === 0 ? "Out of Stock" : "Add to Cart"}
                    </Button>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Cart Section */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow p-6 sticky top-8">
              <h2 className="text-2xl font-bold mb-4">Shopping Cart</h2>

              {cartLoading && <div className="text-center py-4">Loading cart...</div>}

              {!cartLoading && cart && (
                <>
                  {cart.items.length === 0 ? (
                    <p className="text-gray-500 text-center py-8">Your cart is empty</p>
                  ) : (
                    <>
                      <div className="space-y-4 mb-6">
                        {cart.items.map((item) => (
                          <div key={item.id} className="flex justify-between items-center border-b pb-4">
                            <div className="flex-1">
                              <h4 className="font-medium">{item.productName}</h4>
                              <p className="text-sm text-gray-600">{formatCurrency(item.price)} each</p>
                              <div className="flex items-center gap-2 mt-2">
                                <Button onClick={() => handleUpdateQuantity(item.productId, item.quantity - 1)} size="sm" variant="outline">
                                  -
                                </Button>
                                <span className="px-3">{item.quantity}</span>
                                <Button onClick={() => handleUpdateQuantity(item.productId, item.quantity + 1)} size="sm" variant="outline">
                                  +
                                </Button>
                                <Button onClick={() => removeItem(item.productId)} size="sm" variant="destructive" className="ml-auto">
                                  Remove
                                </Button>
                              </div>
                            </div>
                            <div className="text-right ml-4">
                              <p className="font-semibold">{formatCurrency(item.subtotal)}</p>
                            </div>
                          </div>
                        ))}
                      </div>

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
                          <h3 className="font-semibold mb-2">Applied Coupons:</h3>
                          <div className="space-y-2">
                            {cart.appliedCoupons.map((coupon) => (
                              <div key={coupon.id} className="flex justify-between items-center bg-green-50 p-2 rounded">
                                <div>
                                  <span className="font-medium">{coupon.code}</span>
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
                      <div className="border-t pt-4 space-y-2">
                        <div className="flex justify-between text-gray-600">
                          <span>Subtotal:</span>
                          <span>{formatCurrency(cart.priceCalculation.totalBeforeDiscount)}</span>
                        </div>

                        {cart.priceCalculation.totalDiscount > 0 && (
                          <div className="flex justify-between text-green-600">
                            <span>Discount:</span>
                            <span>-{formatCurrency(cart.priceCalculation.totalDiscount)}</span>
                          </div>
                        )}

                        <div className="flex justify-between text-xl font-bold border-t pt-2">
                          <span>Total:</span>
                          <span>{formatCurrency(cart.priceCalculation.finalPayableAmount)}</span>
                        </div>
                      </div>

                      {/* Discount Breakdown */}
                      {cart.priceCalculation.discountDetails.length > 0 && (
                        <div className="mt-4 text-sm text-gray-600">
                          <p className="font-semibold mb-1">Discount Breakdown:</p>
                          {cart.priceCalculation.discountDetails.map((detail, index) => (
                            <p key={index}>
                              {detail.couponCode}: -{formatCurrency(detail.discountAmount)}
                            </p>
                          ))}
                        </div>
                      )}
                    </>
                  )}
                </>
              )}
            </div>
          </div>
        </div>
      </div>

      <GlobalAuthModal />
      <ToastContainer position="top-right" autoClose={3000} hideProgressBar={false} newestOnTop={false} closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover theme="light" />
    </div>
  );
}

export default App;
