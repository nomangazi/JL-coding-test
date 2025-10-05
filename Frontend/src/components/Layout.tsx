import { Outlet, Link, useLocation } from "react-router-dom";
import { useEffect } from "react";
import { useCartStore } from "../store/cartStore";
import { useUserStore } from "../store/userStore";
import { Button } from "../components/ui/button";
import { GlobalAuthModal } from "../components/GlobalAuthModal";
import { ShoppingCart, Store } from "lucide-react";

export function Layout() {
  const { cart, fetchCart } = useCartStore();
  const { initializeAuth, user, isAuthenticated } = useUserStore();
  const location = useLocation();

  useEffect(() => {
    initializeAuth();
    fetchCart();
  }, [fetchCart, initializeAuth]);

  const cartItemCount = cart?.items.reduce((sum, item) => sum + item.quantity, 0) || 0;

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header / Navigation */}
      <header className="bg-white shadow-sm sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            {/* Logo and Nav */}
            <div className="flex items-center space-x-8">
              <Link to="/" className="flex items-center space-x-2 text-xl font-bold text-gray-900 hover:text-blue-600 transition">
                <Store className="w-6 h-6" />
                <span>E-Commerce</span>
              </Link>

              <nav className="hidden md:flex space-x-4">
                <Link to="/" className={`px-3 py-2 rounded-md text-sm font-medium transition ${location.pathname === "/" ? "bg-blue-100 text-blue-700" : "text-gray-700 hover:bg-gray-100"}`}>
                  Products
                </Link>
                <Link to="/cart" className={`px-3 py-2 rounded-md text-sm font-medium transition ${location.pathname === "/cart" ? "bg-blue-100 text-blue-700" : "text-gray-700 hover:bg-gray-100"}`}>
                  Cart
                </Link>
              </nav>
            </div>

            {/* User Actions */}
            <div className="flex items-center space-x-4">
              {/* Cart Icon */}
              <Link to="/cart" className="relative p-2 text-gray-700 hover:text-blue-600 hover:bg-gray-100 rounded-lg transition">
                <ShoppingCart className="w-6 h-6" />
                {cartItemCount > 0 && <span className="absolute -top-1 -right-1 bg-blue-600 text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center">{cartItemCount}</span>}
              </Link>

              {/* User Auth */}
              {isAuthenticated && user ? (
                <div className="flex items-center space-x-3">
                  <div className="text-right hidden sm:block">
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
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <Outlet />
      </main>

      {/* Global Modals */}
      <GlobalAuthModal />
    </div>
  );
}
