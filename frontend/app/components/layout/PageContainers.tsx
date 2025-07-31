import React from 'react';

interface ContainerProps {
  children: React.ReactNode;
  className?: string; // Pozwala na dodanie dodatkowych klas
  maxWidth?: 'max-w-4xl' | 'max-w-6xl'; // Opcjonalna prop do kontroli szerokości
}

/**
 * Komponent do głównego kontenera strony.
 * Zapewnia spójne tło, obramowanie, cień i marginesy.
 */
export const OuterContainer: React.FC<ContainerProps> = ({ children, className, maxWidth = 'max-w-6xl' }) => {
  return (
    <div className={`${maxWidth} mx-auto p-6 bg-white dark:bg-gray-900 rounded-lg shadow-xl mt-8 font-inter text-gray-900 dark:text-gray-100 ${className || ''}`}>
      {children}
    </div>
  );
};

/**
 * Komponent do wewnętrznych sekcji treści na stronie.
 * Zapewnia spójne tło, obramowanie, cień wewnętrzny i marginesy.
 */
export const InnerSection: React.FC<ContainerProps> = ({ children, className }) => {
  return (
    // Dodano border i border-gray-300 dla trybu jasnego, dark:border-gray-700 dla trybu ciemnego
    <div className={`bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6 border border-gray-300 dark:border-gray-700 ${className || ''}`}>
      {children}
    </div>
  );
};
