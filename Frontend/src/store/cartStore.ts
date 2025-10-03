import { create } from "zustand";
import { cartApi } from "../lib/api";
import type { CartResponse, AddCartItemRequest, UpdateCartItemRequest } from "../types";

interface CartStore {
  cart: CartResponse | null;
  loading: boolean;
  error: string | null;
  userId: string;

  setUserId: (userId: string) => void;
  fetchCart: () => Promise<void>;
  addItem: (request: AddCartItemRequest) => Promise<void>;
  updateItem: (productId: number, request: UpdateCartItemRequest) => Promise<void>;
  removeItem: (productId: number) => Promise<void>;
  applyCoupon: (code: string) => Promise<void>;
  removeCoupon: (couponCode: string) => Promise<void>;
  clearCart: () => Promise<void>;
}

export const useCartStore = create<CartStore>((set, get) => ({
  cart: null,
  loading: false,
  error: null,
  userId: "user-1", // Default user ID

  setUserId: (userId: string) => set({ userId }),

  fetchCart: async () => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.getCart(get().userId);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to fetch cart", loading: false });
    }
  },

  addItem: async (request: AddCartItemRequest) => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.addItem(get().userId, request);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to add item", loading: false });
      throw error;
    }
  },

  updateItem: async (productId: number, request: UpdateCartItemRequest) => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.updateItem(get().userId, productId, request);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to update item", loading: false });
      throw error;
    }
  },

  removeItem: async (productId: number) => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.removeItem(get().userId, productId);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to remove item", loading: false });
      throw error;
    }
  },

  applyCoupon: async (code: string) => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.applyCoupon(get().userId, { couponCode: code });
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to apply coupon", loading: false });
      throw error;
    }
  },

  removeCoupon: async (couponCode: string) => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.removeCoupon(get().userId, couponCode);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to remove coupon", loading: false });
      throw error;
    }
  },

  clearCart: async () => {
    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.clearCart(get().userId);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to clear cart", loading: false });
      throw error;
    }
  },
}));
