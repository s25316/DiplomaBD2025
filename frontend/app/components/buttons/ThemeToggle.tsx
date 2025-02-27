"use client";
import { useTheme } from "next-themes";
import React, { useEffect, useState } from "react";

const ThemeToggle = () => {
  const { theme, setTheme } = useTheme();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) {//Nie renderuje przycisku do momentu, aż komponent zostanie zamontowany po stronie klienta.
    return null;
  }

  return (
    <button
      className="border-2 border-foreground px-4 py-2 rounded-lg font-bold transition duration-300 ease-in-out"
      onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
    >
      {theme === "dark" ? "☀️ Light Mode" : "🌙 Dark Mode"}
    </button>
  );
};

export default ThemeToggle;
