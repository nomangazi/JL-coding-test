import React from "react";
import { AuthModal } from "./AuthModal";
import { useCartStore } from "../store/cartStore";

export const GlobalAuthModal: React.FC = () => {
  const { showAuthModal, setShowAuthModal } = useCartStore();

  return <AuthModal isOpen={showAuthModal} onClose={() => setShowAuthModal(false)} />;
};
