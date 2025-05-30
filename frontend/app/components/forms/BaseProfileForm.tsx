'use client';
import React, { useState } from 'react';

interface Props {
  onSuccess: () => void;
  token: string;
}

const BaseProfileForm = ({ onSuccess, token }: Props) => {
  const [form, setForm] = useState({
    name: '',
    surname: '',
    contactEmail: '',
    birthDate: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async () => {
    const res = await fetch('http://localhost:8080/api/User/baseData', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(form)
    });

    if (res.ok) {
      alert('Base profile saved');
      onSuccess();
    } else {
      alert('Failed to save base profile');
    }
  };

  return (
    <div className="max-w-md p-4 border rounded shadow">
      <h2 className="text-lg font-bold mb-2">Complete your basic profile</h2>
      <input name="name" placeholder="Name" onChange={handleChange} value={form.name} className="mb-2 w-full border p-1" />
      <input name="surname" placeholder="Surname" onChange={handleChange} value={form.surname} className="mb-2 w-full border p-1" />
      <input name="contactEmail" placeholder="Email" onChange={handleChange} value={form.contactEmail} className="mb-2 w-full border p-1" />
      <input type="date" name="birthDate" onChange={handleChange} value={form.birthDate} className="mb-2 w-full border p-1" />
      <button onClick={handleSubmit} className="bg-blue-600 text-white px-4 py-2 rounded">Submit</button>
    </div>
  );
};

export default BaseProfileForm;