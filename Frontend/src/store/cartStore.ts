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
    // Only fetch cart if userId is a valid non-empty string and represents a positive integer
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      return;
    }

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

    // Check if user is authenticated when required or if userId is invalid
    if (requireAuth && (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0)) {
      // Store the pending action
      const pendingAction = () => get().addItem(request, false);
      set({ showAuthModal: true, pendingCartAction: pendingAction });
      return;
    }

    // Double-check userId validity before making API call
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
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
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.updateItem(state.userId, productId, request);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to update item", loading: false });
      throw error;
    }
  },

  removeItem: async (productId: number) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.removeItem(state.userId, productId);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to remove item", loading: false });
      throw error;
    }
  },

  applyCoupon: async (code: string) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.applyCoupon(state.userId, { couponCode: code });
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to apply coupon", loading: false });
      throw error;
    }
  },

  removeCoupon: async (couponCode: string) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.removeCoupon(state.userId, couponCode);
      set({ cart: data, loading: false });
    } catch (error: unknown) {
      set({ error: "Failed to remove coupon", loading: false });
      throw error;
    }
  },

  clearCart: async () => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      set({ error: "Invalid user session. Please login first.", loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.clearCart(state.userId);
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
