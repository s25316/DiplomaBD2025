'use client';
import React from 'react';

interface Props {
  page: number;
  onPrev: () => void;
  onNext: () => void;
  isNextDisabled: boolean;
}

const Pagination = ({ page, onPrev, onNext, isNextDisabled }: Props) => (
  <div className="mt-4 flex gap-2">
    <button onClick={onPrev} disabled={page === 1}>
      Previous
    </button>
    <span>Page {page}</span>
    <button onClick={onNext} disabled={isNextDisabled}>
      Next
    </button>
  </div>
);

export default Pagination;