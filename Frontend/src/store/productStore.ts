import { create } from "zustand";
import { productsApi } from "../lib/api";
import type { Product } from "../types";

interface ProductStore {
  products: Product[];
  loading: boolean;
  error: string | null;

  fetchProducts: () => Promise<void>;
}

export const useProductStore = create<ProductStore>((set) => ({
  products: [],
  loading: false,
  error: null,

  fetchProducts: async () => {
    set({ loading: true, error: null });
    try {
      const { data } = await productsApi.getAll();
      set({ products: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to fetch products", loading: false });
    }
  },
}));
