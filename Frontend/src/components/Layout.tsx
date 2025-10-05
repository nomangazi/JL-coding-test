import { Outlet, Link, useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import { useCartStore } from "../store/cartStore";
import { useUserStore } from "../store/userStore";
import { useProductStore } from "../store/productStore";
import { Button } from "../components/ui/button";
import { GlobalAuthModal } from "../components/GlobalAuthModal";
import { ShoppingCart, Store, Search } from "lucide-react";

export function Layout() {
  const { cart, fetchCart } = useCartStore();
  const { initializeAuth, user, isAuthenticated } = useUserStore();
  const { searchQuery, selectedCategory, setSearchQuery, setSelectedCategory, fetchProducts } = useProductStore();
  const location = useLocation();
  const [localSearch, setLocalSearch] = useState(searchQuery);

  useEffect(() => {
    initializeAuth();
    fetchCart();
    fetchProducts();
  }, [fetchCart, initializeAuth, fetchProducts]);

  useEffect(() => {
    setLocalSearch(searchQuery);
  }, [searchQuery]);

  const cartItemCount = cart?.items.reduce((sum, item) => sum + item.quantity, 0) || 0;

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSearchQuery(localSearch);
  };

  const handleCategoryChange = (category: string) => {
    setSelectedCategory(category);
  };

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
              </nav>
            </div>

            {/* Search Bar - Desktop */}
            {location.pathname === "/" && (
              <form onSubmit={handleSearchSubmit} className="hidden lg:flex items-center flex-1 max-w-md mx-8">
                <div className="relative w-full">
                  <input
                    type="text"
                    value={localSearch}
                    onChange={(e) => setLocalSearch(e.target.value)}
                    placeholder="Search products..."
                    className="w-full px-4 py-2 pl-10 pr-4 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                  />
                  <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
                </div>
              </form>
            )}

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

          {/* Search Bar & Category Filter - Mobile */}
          {location.pathname === "/" && (
            <div className="lg:hidden pb-4 pt-2 space-y-3">
              <form onSubmit={handleSearchSubmit} className="relative">
                <input
                  type="text"
                  value={localSearch}
                  onChange={(e) => setLocalSearch(e.target.value)}
                  placeholder="Search products..."
                  className="w-full px-4 py-2 pl-10 pr-4 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                />
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
              </form>
              <div className="flex gap-2 overflow-x-auto pb-2">
                <button
                  onClick={() => handleCategoryChange("")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium whitespace-nowrap transition ${
                    selectedCategory === "" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                  }`}
                >
                  All
                </button>
                <button
                  onClick={() => handleCategoryChange("Electronics")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium whitespace-nowrap transition ${
                    selectedCategory === "Electronics" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                  }`}
                >
                  Electronics
                </button>
                <button
                  onClick={() => handleCategoryChange("Clothing")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium whitespace-nowrap transition ${
                    selectedCategory === "Clothing" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                  }`}
                >
                  Clothing
                </button>
                <button
                  onClick={() => handleCategoryChange("Home")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium whitespace-nowrap transition ${
                    selectedCategory === "Home" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                  }`}
                >
                  Home
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Category Filter - Desktop */}
        {location.pathname === "/" && (
          <div className="hidden lg:block border-t border-gray-200">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
              <div className="flex gap-2 py-3">
                <button
                  onClick={() => handleCategoryChange("")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${selectedCategory === "" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"}`}
                >
                  All Categories
                </button>
                <button
                  onClick={() => handleCategoryChange("Electronics")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${selectedCategory === "Electronics" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"}`}
                >
                  Electronics
                </button>
                <button
                  onClick={() => handleCategoryChange("Clothing")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${selectedCategory === "Clothing" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"}`}
                >
                  Clothing
                </button>
                <button
                  onClick={() => handleCategoryChange("Home")}
                  className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${selectedCategory === "Home" ? "bg-blue-600 text-white" : "bg-gray-100 text-gray-700 hover:bg-gray-200"}`}
                >
                  Home
                </button>
              </div>
            </div>
          </div>
        )}
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
