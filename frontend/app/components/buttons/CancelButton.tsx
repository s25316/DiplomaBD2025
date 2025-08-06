'use client';

import React from "react";
import { useRouter } from 'next/navigation';

interface CancelButtonProps {
  redirectTo?: string;
}

const CancelButton = ({ redirectTo }: CancelButtonProps) => {
  const router = useRouter();

  const handleGoBack = () => {
    if (redirectTo) {
      router.replace(redirectTo);
    } else {
      router.back(); 
      if (window.history.length <= 1) {
        router.push("/");
      }
    }
  };

  return (
    <button 
      onClick={handleGoBack} 
      className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition duration-300 ease-in-out shadow-md font-semibold"
    >
      Cancel
    </button>
  );
};

export default CancelButton;
