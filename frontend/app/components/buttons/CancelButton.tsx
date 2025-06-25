"use client";

import React from "react";

const CancelButton = () => {
  const handleGoBack = () => {
    if (window.history.length > 1) {
      window.history.back();
    } else {
      window.location.href = "/";
    }
  };

  return (
    <button onClick={handleGoBack} className="bg-red-500 text-white px-2 py-1 rounded">
      Cancel
    </button>
  );
};

export default CancelButton;
