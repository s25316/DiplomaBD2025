'use client';
import React from 'react';

interface Props {
  value: number;
  onChange: (value: number) => void;
}

const SelectItemsPerPage = ({ value, onChange }: Props) => (
  <div className="mt-4 mb-2">
    <label className="mr-2 font-medium">Items per page:</label>
    <select
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
      className="border px-2 py-1 rounded"
    >
      {[10, 20, 50, 100].map((count) => (
        <option key={count} value={count}>{count}</option>
      ))}
    </select>
  </div>
);

export default SelectItemsPerPage;