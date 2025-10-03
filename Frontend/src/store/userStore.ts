import { create } from "zustand";
import { persist } from "zustand/middleware";
import { usersApi } from "../lib/api";
import type { User, UserCreateRequest, AuthState } from "../types";

interface UserStore extends AuthState {
  login: (email: string) => Promise<boolean>;
  register: (userData: UserCreateRequest) => Promise<boolean>;
  logout: () => void;
  clearError: () => void;
  initializeAuth: () => void;
}

export const useUserStore = create<UserStore>()(
  persist(
    (set, get) => ({
      user: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,

      initializeAuth: () => {
        const user = get().user;
        if (user) {
          set({ isAuthenticated: true });
        }
      },

      login: async (email: string): Promise<boolean> => {
        set({ isLoading: true, error: null });

        try {
          const user = await usersApi.login(email);

          if (user) {
            set({
              user,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });

            // Update cart store with user ID
            const { useCartStore } = await import("./cartStore");
            useCartStore.getState().setUserId(user.id.toString());

            return true;
          } else {
            set({
              isLoading: false,
              error: "User not found. Please check your email or register.",
            });
            return false;
          }
        } catch {
          set({
            isLoading: false,
            error: "Login failed. Please try again.",
          });
          return false;
        }
      },

      register: async (userData: UserCreateRequest): Promise<boolean> => {
        set({ isLoading: true, error: null });

        try {
          const newUser = await usersApi.register(userData);

          set({
            user: newUser,
            isAuthenticated: true,
            isLoading: false,
            error: null,
          });

          // Update cart store with user ID
          const { useCartStore } = await import("./cartStore");
          useCartStore.getState().setUserId(newUser.id.toString());

          return true;
        } catch (error: unknown) {
          let errorMessage = "Registration failed. Please try again.";

          // Type guard for axios error
          if (error && typeof error === "object" && "response" in error) {
            const axiosError = error as { response?: { data?: { message?: string }; status?: number } };
            if (axiosError.response?.data?.message) {
              errorMessage = axiosError.response.data.message;
            } else if (axiosError.response?.status === 400) {
              errorMessage = "Invalid user data. Please check your information.";
            } else if (
              axiosError.response?.status === 500 &&
              axiosError.response?.data?.message?.includes("Email already exists")
            ) {
              errorMessage = "Email already exists. Please use a different email or try logging in.";
            }
          }

          set({
            isLoading: false,
            error: errorMessage,
          });

          return false;
        }
      },

      logout: () => {
        set({
          user: null,
          isAuthenticated: false,
          error: null,
        });

        // Clear cart store
        import("./cartStore").then(({ useCartStore }) => {
          useCartStore.getState().setUserId("");
          useCartStore.getState().clearPendingAction();
        });
      },

      clearError: () => {
        set({ error: null });
      },
    }),
    {
      name: "user-auth", // Key for localStorage
      partialize: (state) => ({
        user: state.user,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);
