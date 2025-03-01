"use client";

import React from "react";

const ReturnButton = () => {
  const handleGoBack = () => {
    if (window.history.length > 1) {
      window.history.back(); // Powrót do poprzedniej strony
    } else {
      window.location.href = "/"; // Jeśli nie ma historii, wróć na stronę główną
    }
  };

  return (
    <button onClick={handleGoBack} className="border-2 border-foreground px-4 py-2 rounded-lg font-bold transition duration-300 ease-in-out">
      Return
    </button>
  );
};

export default ReturnButton;
