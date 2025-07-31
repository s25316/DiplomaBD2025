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
    birthDate: '',
    isIndividual: false,
  });

  const [confirming, setConfirming] = useState(false);
  const [dateError, setDateError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);

  const showCustomAlert = (message: string, isError: boolean = false) => {
    let alertMessage = message;
    if (isError) {
      try {
        const errorJson = JSON.parse(message);
        if (errorJson.errors) {
          alertMessage = "Validation Errors:\n" + Object.entries(errorJson.errors).map(([key, value]) => `${key}: ${(value as string[]).join(", ")}`).join("\n");
        } else if (errorJson.title) {
          alertMessage = errorJson.title + (errorJson.detail ? `\n${errorJson.detail}` : "");
        } else {
          alertMessage = message;
        }
      } catch (e) {
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;

    const newValue = type === 'checkbox' ? checked : value;

    if (name === 'birthDate') {
      const selectedDate = new Date(value);
      const today = new Date();
      today.setHours(0, 0, 0, 0); 

      if (selectedDate > today) {
        setDateError('Birth date cannot be in the future.');
      } else {
        const ageDate = new Date(selectedDate.getFullYear() + 18, selectedDate.getMonth(), selectedDate.getDate());
        if (ageDate > today) {
          setDateError('You must be at least 18 years old.');
        } else {
          setDateError(null);
        }
      }
    }

    setForm({ ...form, [name]: newValue });
    setApiError(null); 
  };

  const submitConfirmed = async () => {
    setApiError(null);
    try {
      const res = await fetch('http://localhost:8080/api/User/baseData', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(form)
      });

      if (res.ok) {
        showCustomAlert('Base profile saved!');
        onSuccess();
      } else {
        const errorText = await res.text();
        if (res.status === 409) {
          setApiError('This email is already in use. Please use a different one.');
          showCustomAlert('Error: This email is already in use.', true);
        } else {
          try {
            const errorJson = JSON.parse(errorText);
            if (errorJson.title || errorJson.detail || errorJson.errors) {
              setApiError(errorJson.title || errorJson.detail || JSON.stringify(errorJson.errors));
              showCustomAlert(errorText, true);
            } else {
              setApiError('Failed to save base profile: An unknown error occurred.');
              showCustomAlert('Failed to save base profile: An unknown error occurred.', true);
            }
          } catch (parseError) {
            setApiError('Failed to save base profile. Please try again.');
            showCustomAlert(`Failed to save base profile: ${errorText}`, true);
          }
        }
        setConfirming(false);
      }
    } catch (err: any) {
      console.error("Error saving base profile:", err);
      setApiError(`An unexpected error occurred: ${err.message}`);
      showCustomAlert(`An unexpected error occurred: ${err.message}`, true);
      setConfirming(false);
    }
  };

  const handleSubmit = () => {
    if (!form.name || !form.surname || !form.contactEmail || !form.birthDate) {
      showCustomAlert('Please fill all required fields.', true);
      return;
    }
    if (dateError) {
      showCustomAlert(`Please correct the birth date: ${dateError}`, true);
      return;
    }
    if (apiError) { 
      showCustomAlert(`Please correct the previous error: ${apiError}`, true);
      return;
    }
    setConfirming(true);
  };

  return (
    <div className="max-w-md p-4 border rounded shadow bg-white dark:bg-gray-800 text-gray-900 dark:text-gray-100">
      <h2 className="text-lg font-bold mb-4 text-gray-800 dark:text-gray-100">Complete your basic profile</h2>
      
      <label className="flex items-center gap-2 mb-4 text-gray-700 dark:text-gray-300">
        <input 
          type="checkbox" 
          name="isIndividual" 
          checked={form.isIndividual} 
          onChange={handleChange}
          className='global-field-style'
        />
        Create an individual profile
      </label>

      <input 
        name="name" 
        placeholder="Name" 
        onChange={handleChange} 
        value={form.name} 
        className='global-field-style'
        required
      />
      <input 
        name="surname" 
        placeholder="Surname" 
        onChange={handleChange} 
        value={form.surname} 
        className='global-field-style'
        required
      />
      <input 
        name="contactEmail" 
        type="email"
        placeholder="Email" 
        onChange={handleChange} 
        value={form.contactEmail} 
        className='global-field-style'
        required
      />
      <input 
        type="date" 
        name="birthDate" 
        onChange={handleChange} 
        value={form.birthDate} 
        className='global-field-style'
        required
      />
      {dateError && <p className="text-red-500 text-sm mb-2">{dateError}</p>}
      {apiError && <p className="text-red-500 text-sm mb-2">{apiError}</p>}
      
      {!confirming ? (
        <button 
          onClick={handleSubmit} 
          className="inline-block bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4"
        >
          Submit
        </button>
      ) : (
        <div className="border border-yellow-400 bg-yellow-100 dark:bg-yellow-900 dark:border-yellow-700 p-3 rounded mt-4">
          <p className="text-sm mb-2 font-medium text-yellow-800 dark:text-yellow-200">
            Are you sure everything is correct? After saving, you <b>cannot change</b>:
          </p>
          <ul className="list-disc ml-5 text-sm text-yellow-700 dark:text-yellow-300">
            <li><b>Name:</b> {form.name || <i>(empty)</i>}</li>
            <li><b>Surname:</b> {form.surname || <i>(empty)</i>}</li>
            <br/>
            <li><b> {form.isIndividual ? 'Individual account' : 'Company account'}</b></li>
          </ul>
          <div className="mt-4 flex gap-3">
            <button 
              onClick={submitConfirmed} 
              className="inline-block bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold" 
            >
              Yes, Save
            </button>
            <button
              onClick={() => setConfirming(false)} 
              className="inline-block bg-gray-300 text-gray-800 px-5 py-2 rounded-lg hover:bg-gray-400 transition duration-300 ease-in-out shadow-md font-semibold"
            >
              No, Go Back
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default BaseProfileForm;
