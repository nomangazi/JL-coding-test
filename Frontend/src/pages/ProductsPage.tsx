import { toast } from "react-toastify";
import { useCartStore } from "../store/cartStore";
import { useProductStore } from "../store/productStore";
import { formatCurrency } from "../lib/utils";
import { Button } from "../components/ui/button";
import type { Product } from "../types";

export function ProductsPage() {
  const { addItem, loading: cartLoading } = useCartStore();
  const { loading: productsLoading, getFilteredProducts, searchQuery, selectedCategory } = useProductStore();

  const filteredProducts = getFilteredProducts();

  const handleAddToCart = async (product: Product) => {
    try {
      const success = await addItem({ productId: product.id, quantity: 1 });
      if (success) {
        toast.success(`Added ${product.name} to cart`);
      }
    } catch {
      toast.error("Failed to add item to cart");
    }
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h2 className="text-2xl font-bold">Products</h2>
          {(searchQuery || selectedCategory) && (
            <p className="text-sm text-gray-600 mt-1">
              {filteredProducts.length} product{filteredProducts.length !== 1 ? "s" : ""} found
              {searchQuery && ` for "${searchQuery}"`}
              {selectedCategory && ` in ${selectedCategory}`}
            </p>
          )}
        </div>
      </div>
      {productsLoading ? (
        <div className="text-center py-8">Loading products...</div>
      ) : filteredProducts.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg">No products found</p>
          <p className="text-gray-400 text-sm mt-2">Try adjusting your search or filter</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredProducts.map((product) => (
            <div key={product.id} className="bg-white rounded-lg shadow p-6">
              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <div className="flex items-start justify-between gap-2">
                    <h3 className="text-lg font-semibold">{product.name}</h3>
                    <span className="text-xl font-bold text-blue-600 whitespace-nowrap">{formatCurrency(product.price)}</span>
                  </div>
                  <p className="text-xs text-blue-600 font-medium mt-1">{product.category}</p>
                  <p className="text-gray-600 text-sm mt-2">{product.description}</p>
                  <p className="text-sm text-gray-500 mt-2">Stock: {product.stock}</p>
                </div>
              </div>
              <Button onClick={() => handleAddToCart(product)} disabled={product.stock === 0 || cartLoading} className="w-full">
                {product.stock === 0 ? "Out of Stock" : "Add to Cart"}
              </Button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
