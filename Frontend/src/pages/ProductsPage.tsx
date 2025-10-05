import { useEffect } from "react";
import { toast } from "react-toastify";
import { useCartStore } from "../store/cartStore";
import { useProductStore } from "../store/productStore";
import { formatCurrency } from "../lib/utils";
import { Button } from "../components/ui/button";
import type { Product } from "../types";

export function ProductsPage() {
  const { addItem, loading: cartLoading } = useCartStore();
  const { products, loading: productsLoading, fetchProducts } = useProductStore();

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

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
      <h2 className="text-2xl font-bold mb-6">Products</h2>
      {productsLoading ? (
        <div className="text-center py-8">Loading products...</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
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
  );
}
