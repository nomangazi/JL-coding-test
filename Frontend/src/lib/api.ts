import axios from "axios";
import type {
  Product,
  CartResponse,
  AddCartItemRequest,
  UpdateCartItemRequest,
  ApplyCouponRequest,
  Coupon,
} from "../types";

const API_BASE_URL = "http://localhost:5090/api";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Products API
export const productsApi = {
  getAll: () => api.get<Product[]>("/products"),
  getById: (id: number) => api.get<Product>(`/products/${id}`),
  create: (product: Omit<Product, "id">) => api.post<Product>("/products", product),
  update: (id: number, product: Partial<Product>) => api.put<Product>(`/products/${id}`, product),
  delete: (id: number) => api.delete(`/products/${id}`),
};

// Cart API
export const cartApi = {
  getCart: (userId: string) => api.get<CartResponse>(`/cart/${userId}`),
  addItem: (userId: string, request: AddCartItemRequest) => api.post<CartResponse>(`/cart/${userId}/items`, request),
  updateItem: (userId: string, productId: number, request: UpdateCartItemRequest) =>
    api.put<CartResponse>(`/cart/${userId}/items/${productId}`, request),
  removeItem: (userId: string, productId: number) => api.delete<CartResponse>(`/cart/${userId}/items/${productId}`),
  applyCoupon: (userId: string, request: ApplyCouponRequest) =>
    api.post<CartResponse>(`/cart/${userId}/coupons`, request),
  removeCoupon: (userId: string, couponCode: string) =>
    api.delete<CartResponse>(`/cart/${userId}/coupons/${couponCode}`),
  clearCart: (userId: string) => api.delete<CartResponse>(`/cart/${userId}/clear`),
};

// Coupons API
export const couponsApi = {
  getAll: (params?: { isActive?: boolean; isAutoApplied?: boolean }) => api.get<Coupon[]>("/coupons", { params }),
  getById: (id: number) => api.get<Coupon>(`/coupons/${id}`),
  getByCode: (code: string) => api.get<Coupon>(`/coupons/by-code/${code}`),
  create: (coupon: Omit<Coupon, "id">) => api.post<Coupon>("/coupons", coupon),
  update: (id: number, coupon: Partial<Coupon>) => api.put<Coupon>(`/coupons/${id}`, coupon),
  delete: (id: number) => api.delete(`/coupons/${id}`),
};
