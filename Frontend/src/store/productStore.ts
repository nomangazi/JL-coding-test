import { create } from "zustand";
import { toast } from "react-toastify";
import { productsApi } from "../lib/api";
import type { Product } from "../types";

interface ProductStore {
  products: Product[];
  loading: boolean;
  error: string | null;
  searchQuery: string;
  selectedCategory: string;

  fetchProducts: () => Promise<void>;
  setSearchQuery: (query: string) => void;
  setSelectedCategory: (category: string) => void;
  getFilteredProducts: () => Product[];
}

export const useProductStore = create<ProductStore>((set, get) => ({
  products: [],
  loading: false,
  error: null,
  searchQuery: "",
  selectedCategory: "",

  fetchProducts: async () => {
    set({ loading: true, error: null });
    try {
      const { data } = await productsApi.getAll();
      set({ products: data, loading: false });
    } catch {
      const errorMsg = "Failed to fetch products";
      set({ error: errorMsg, loading: false });
      toast.error(errorMsg);
    }
  },

  setSearchQuery: (query: string) => {
    set({ searchQuery: query });
  },

  setSelectedCategory: (category: string) => {
    set({ selectedCategory: category });
  },

  getFilteredProducts: () => {
    const { products, searchQuery, selectedCategory } = get();
    return products.filter((product) => {
      const matchesSearch = searchQuery === "" || product.name.toLowerCase().includes(searchQuery.toLowerCase());
      const matchesCategory = selectedCategory === "" || product.category === selectedCategory;
      return matchesSearch && matchesCategory && product.isActive;
    });
  },
}));
