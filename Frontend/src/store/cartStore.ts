import { create } from "zustand";
import { toast } from "react-toastify";
import { cartApi } from "../lib/api";
import type { CartResponse, AddCartItemRequest, UpdateCartItemRequest } from "../types";

interface CartStore {
  cart: CartResponse | null;
  loading: boolean;
  error: string | null;
  userId: string;
  showAuthModal: boolean;
  pendingCartAction: (() => Promise<boolean>) | null;

  setUserId: (userId: string) => void;
  fetchCart: () => Promise<void>;
  addItem: (request: AddCartItemRequest, requireAuth?: boolean) => Promise<boolean>;
  updateItem: (productId: number, request: UpdateCartItemRequest) => Promise<void>;
  removeItem: (productId: number) => Promise<void>;
  applyCoupon: (code: string) => Promise<void>;
  removeCoupon: (couponCode: string) => Promise<void>;
  clearCart: () => Promise<void>;
  clearCartData: () => void;
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
      // Clear cart and set loading to false if no valid userId
      set({ cart: null, loading: false });
      return;
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.getCart(state.userId);
      set({ cart: data, loading: false });
    } catch {
      const errorMsg = "Failed to fetch cart";
      set({ error: errorMsg, loading: false });
      toast.error(errorMsg);
    }
  },

  addItem: async (request: AddCartItemRequest, requireAuth = true) => {
    const state = get();

    // Check if user is authenticated when required or if userId is invalid
    if (
      requireAuth &&
      (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0)
    ) {
      // Store the pending action
      const pendingAction = () => get().addItem(request, false);
      set({ showAuthModal: true, pendingCartAction: pendingAction });
      return false; // Return false when auth is required
    }

    // Double-check userId validity before making API call
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg, loading: false });
      toast.error(errorMsg);
      return false; // Return false on error
    }

    set({ loading: true, error: null });
    try {
      const { data } = await cartApi.addItem(state.userId, request);
      set({ cart: data, loading: false });
      return true; // Return true on success
    } catch (error: unknown) {
      set({ error: "Failed to add item", loading: false });
      throw error;
    }
  },

  updateItem: async (productId: number, request: UpdateCartItemRequest) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg });
      toast.error(errorMsg);
      return;
    }

    // Don't set global loading to prevent full page reload appearance
    set({ error: null });
    try {
      const { data } = await cartApi.updateItem(state.userId, productId, request);
      set({ cart: data });
    } catch (error: unknown) {
      set({ error: "Failed to update item" });
      throw error;
    }
  },

  removeItem: async (productId: number) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg });
      toast.error(errorMsg);
      return;
    }

    // Don't set global loading to prevent full page reload appearance
    set({ error: null });
    try {
      const { data } = await cartApi.removeItem(state.userId, productId);
      set({ cart: data });
    } catch (error: unknown) {
      set({ error: "Failed to remove item" });
      throw error;
    }
  },

  applyCoupon: async (code: string) => {
    const state = get();
    // Check if userId is valid
    if (!state.userId || state.userId === "" || isNaN(Number(state.userId)) || Number(state.userId) <= 0) {
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg });
      toast.error(errorMsg);
      return;
    }

    // Keep loading state for coupon operations as they affect the entire cart
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
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg });
      toast.error(errorMsg);
      return;
    }

    // Keep loading state for coupon operations as they affect the entire cart
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
      const errorMsg = "Invalid user session. Please login first.";
      set({ error: errorMsg, loading: false });
      toast.error(errorMsg);
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

  clearCartData: () => set({ cart: null, loading: false, error: null }),

  setShowAuthModal: (show: boolean) => set({ showAuthModal: show }),

  executePendingAction: async () => {
    const { pendingCartAction } = get();
    if (pendingCartAction) {
      const success = await pendingCartAction();
      set({ pendingCartAction: null });
      if (success) {
        toast.success("Item added to cart successfully!");
      }
    }
  },

  clearPendingAction: () => set({ pendingCartAction: null }),
}));
