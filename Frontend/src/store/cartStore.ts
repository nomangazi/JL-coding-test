import { create } from "zustand";
import { cartApi } from "../lib/api";
import type { CartResponse, AddCartItemRequest, UpdateCartItemRequest } from "../types";

interface CartStore {
  cart: CartResponse | null;
  loading: boolean;
  error: string | null;
  userId: string;
  showAuthModal: boolean;
  pendingCartAction: (() => Promise<void>) | null;

  setUserId: (userId: string) => void;
  fetchCart: () => Promise<void>;
  addItem: (request: AddCartItemRequest, requireAuth?: boolean) => Promise<void>;
  updateItem: (productId: number, request: UpdateCartItemRequest) => Promise<void>;
  removeItem: (productId: number) => Promise<void>;
  applyCoupon: (code: string) => Promise<void>;
  removeCoupon: (couponCode: string) => Promise<void>;
  clearCart: () => Promise<void>;
  setShowAuthModal: (show: boolean) => void;
  executePendingAction: () => Promise<void>;
  clearPendingAction: () => void;
}

export const useCartStore = create<CartStore>((set, get) => ({
  cart: null,
  loading: false,
  error: null,
  userId: "",
  showAuthModal: false,
  pendingCartAction: null,

  setUserId: (userId: string) => set({ userId }),

  fetchCart: async () => {
    const state = get();
    if (!state.userId) return;

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.getCart(state.userId);
      set({ cart: data, loading: false });
    } catch {
      set({ error: "Failed to fetch cart", loading: false });
    }
  },

  addItem: async (request: AddCartItemRequest, requireAuth = true) => {
    const state = get();

    // Check if user is authenticated when required
    if (requireAuth && !state.userId) {
      // Store the pending action
      const pendingAction = () => get().addItem(request, false);
      set({ showAuthModal: true, pendingCartAction: pendingAction });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.addItem(state.userId, request);
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

  setShowAuthModal: (show: boolean) => set({ showAuthModal: show }),

  executePendingAction: async () => {
    const { pendingCartAction } = get();
    if (pendingCartAction) {
      await pendingCartAction();
      set({ pendingCartAction: null });
    }
  },

  clearPendingAction: () => set({ pendingCartAction: null }),
}));
