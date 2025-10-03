import React, { useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "./ui/dialog";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { Label } from "./ui/label";
import { useUserStore } from "../store/userStore";
import { useCartStore } from "../store/cartStore";
import { Loader2 } from "lucide-react";

interface AuthModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess?: () => void;
}

export const AuthModal: React.FC<AuthModalProps> = ({ isOpen, onClose, onSuccess }) => {
  const [isLogin, setIsLogin] = useState(true);
  const [formData, setFormData] = useState({
    email: "",
    name: "",
    phone: "",
  });

  const { login, register, isLoading, error, clearError } = useUserStore();
  const { executePendingAction } = useCartStore();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    clearError();

    if (isLogin) {
      const success = await login(formData.email);
      if (success) {
        handleSuccess();
      }
    } else {
      if (!formData.name || !formData.email || !formData.phone) {
        return;
      }

      const success = await register({
        name: formData.name,
        email: formData.email,
        phone: formData.phone,
      });

      if (success) {
        handleSuccess();
      }
    }
  };

  const handleSuccess = async () => {
    setFormData({ email: "", name: "", phone: "" });
    onClose();

    // Execute any pending cart action after successful authentication
    await executePendingAction();

    onSuccess?.();
  };

  const toggleMode = () => {
    setIsLogin(!isLogin);
    clearError();
    setFormData({ email: "", name: "", phone: "" });
  };

  const handleOpenChange = (open: boolean) => {
    if (!open) {
      clearError();
      setFormData({ email: "", name: "", phone: "" });
      onClose();
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className="sm:max-w-[425px] bg-white">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-center">{isLogin ? "Sign In" : "Create Account"}</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          {error && <div className="p-3 text-sm text-red-600 bg-red-50 border border-red-200 rounded-md">{error}</div>}

          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input id="email" name="email" type="email" value={formData.email} onChange={handleInputChange} placeholder="Enter your email" required className="w-full" />
          </div>

          {!isLogin && (
            <>
              <div className="space-y-2">
                <Label htmlFor="name">Full Name</Label>
                <Input id="name" name="name" type="text" value={formData.name} onChange={handleInputChange} placeholder="Enter your full name" required className="w-full" />
              </div>

              <div className="space-y-2">
                <Label htmlFor="phone">Phone Number</Label>
                <Input id="phone" name="phone" type="tel" value={formData.phone} onChange={handleInputChange} placeholder="Enter your phone number" required className="w-full" />
              </div>
            </>
          )}

          <Button type="submit" className="w-full" disabled={isLoading}>
            {isLoading ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                {isLogin ? "Signing In..." : "Creating Account..."}
              </>
            ) : isLogin ? (
              "Sign In"
            ) : (
              "Create Account"
            )}
          </Button>
        </form>

        <div className="text-center space-y-2">
          <p className="text-sm text-gray-600">{isLogin ? "Don't have an account?" : "Already have an account?"}</p>
          <Button type="button" variant="ghost" onClick={toggleMode} className="w-full" disabled={isLoading}>
            {isLogin ? "Create New Account" : "Sign In Instead"}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
};
