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

  const [confirming, setConfirming] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const submitConfirmed = async () => {
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
  const handleSubmit = () => {
    setConfirming(true);
  };

  return (
    <div className="max-w-md p-4 border rounded shadow">
      <h2 className="text-lg font-bold mb-2">Complete your basic profile</h2>
      <input name="name" placeholder="Name" onChange={handleChange} value={form.name} className="mb-2 w-full border p-1" />
      <input name="surname" placeholder="Surname" onChange={handleChange} value={form.surname} className="mb-2 w-full border p-1" />
      <input name="contactEmail" placeholder="Email" onChange={handleChange} value={form.contactEmail} className="mb-2 w-full border p-1" />
      <input type="date" name="birthDate" onChange={handleChange} value={form.birthDate} className="mb-2 w-full border p-1" />
      
      {!confirming ? (
        <button onClick={handleSubmit} className="bg-blue-600 text-white px-4 py-2 rounded">Submit</button>
      ) : (
        <div className="border border-yellow-400 bg-yellow-100 p-3 rounded mt-4">
          <p className="text-sm mb-2 font-medium text-yellow-800">
            Are you sure everything is correct? After saving, you <b>cannot change</b>:
          </p>
          <ul className="list-disc ml-5 text-sm text-yellow-700">
            <li><b>Name:</b> {form.name || <i>(empty)</i>}</li>
            <li><b>Surname:</b> {form.surname || <i>(empty)</i>}</li>
          </ul>
          <div className="mt-4 flex gap-3">
            <button onClick={submitConfirmed} className="bg-green-600 text-white px-4 py-1 rounded" >
              Yes, Save
            </button>
            <button
              onClick={() => setConfirming(false)} className="bg-gray-300 text-gray-800 px-4 py-1 rounded">
              No, Go Back
            </button>
          </div>
        </div>
      )}
      </div>
  );
};

export default BaseProfileForm;