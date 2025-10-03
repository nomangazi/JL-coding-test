// Types matching backend DTOs

export type DiscountType = "Fixed" | "Percentage";

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
}

export interface CartItem {
  id: number;
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  subtotal: number;
}

export interface AppliedCoupon {
  id: number;
  code: string;
  discountType: DiscountType;
  discountValue: number;
  isAutoApplied: boolean;
}

export interface DiscountDetail {
  couponCode: string;
  discountAmount: number;
}

export interface PriceCalculation {
  totalBeforeDiscount: number;
  totalDiscount: number;
  finalPayableAmount: number;
  discountDetails: DiscountDetail[];
}

export interface CartResponse {
  id: number;
  userId: string;
  items: CartItem[];
  appliedCoupons: AppliedCoupon[];
  priceCalculation: PriceCalculation;
}

export interface AddCartItemRequest {
  productId: number;
  quantity: number;
}

export interface UpdateCartItemRequest {
  quantity: number;
}

export interface ApplyCouponRequest {
  couponCode: string;
}

export interface Coupon {
  id: number;
  code: string;
  description: string;
  discountType: DiscountType;
  discountValue: number;
  maxDiscountAmount?: number;
  minimumCartAmount?: number;
  validFrom?: string;
  validUntil?: string;
  maxUsageCount?: number;
  maxUsagePerUser?: number;
  isActive: boolean;
  isAutoApplied: boolean;
  applicableProductIds?: number[];
}
