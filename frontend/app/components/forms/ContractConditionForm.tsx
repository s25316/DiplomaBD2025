'use client';

import React, { useState } from 'react';
import CancelButton from '../buttons/CancelButton';

// Interface for parameters like Currency, Salary Term, Work Mode, Employment Type
// as they are received from /api/Dictionaries/contractParameters
export interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

// Interface for the data structure submitted by the form
export interface ContractConditionFormData {
  salaryMin: number;
  salaryMax: number;
  hoursPerTerm: number;
  isNegotiable: boolean;
  salaryTermId: number;
  currencyId: number;
  workModeIds: number[];
  employmentTypeIds: number[];
}

// Props interface for the ContractConditionForm component
interface ContractConditionFormProps {
  onSubmit: (form: ContractConditionFormData) => Promise<void>;
  initialData?: ContractConditionFormData | null; // Can be null if no initial data
  parameters: ContractParameter[]; // This component expects an array of ContractParameter
  submitText?: string;
}

const ContractConditionForm = ({ onSubmit, parameters, initialData, submitText = "Submit" }: ContractConditionFormProps) => {
  // Initialize form state with initialData or default values
  const [form, setForm] = useState<ContractConditionFormData>(
    initialData ?? { // Use nullish coalescing operator (??) for default values
      salaryMin: 0,
      salaryMax: 0,
      hoursPerTerm: 0,
      isNegotiable: false,
      salaryTermId: 3001, // Default to a known ID, adjust if necessary
      currencyId: 1,      // Default to a known ID, adjust if necessary
      workModeIds: [],
      employmentTypeIds: [],
    }
  );

  // Debugging: Log initial form state and parameters
  React.useEffect(() => {
    console.log('ContractConditionForm Initialized with Form:', form);
    console.log('ContractConditionForm Parameters:', parameters);
  }, [form, parameters]);


  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const target = e.target;
    const name = target.name;

    let value: string | number | boolean;
    if (target.type === 'checkbox') { // For checkbox inputs
      value = (target as HTMLInputElement).checked;
    } else if (target.type === 'number') { // For number inputs (salaryMin, salaryMax, hoursPerTerm)
      let numValue = Number(target.value);
      // Ensure number is not negative
      if (numValue < 0) {
        numValue = 0; // Set to 0 if negative
      }
      value = numValue;
    } else if (name.endsWith('Id')) { // For select inputs representing IDs
      value = Number(target.value);
    } else { // For other text-based inputs (if any)
      value = target.value;
    }

    setForm(prev => ({
      ...prev,
      [name]: value,
    }));
    console.log(`handleChange - Field: ${name}, Value: ${value}`); // Debug log
  };


  const handleMultiCheckbox = (field: 'workModeIds' | 'employmentTypeIds', id: number, checked: boolean) => {
    setForm(prev => {
      const updated = checked
        ? [...prev[field], id] // Add ID if checked
        : prev[field].filter(i => i !== id); // Remove ID if unchecked
      console.log(`handleMultiCheckbox - Field: ${field}, ID: ${id}, Checked: ${checked}, Updated:`, updated); // Debug log
      return { ...prev, [field]: updated };
    });
  };

  // Helper function to filter and map parameters for select options
  const getOptions = (type: string) =>
    parameters
      .filter(p => p.contractParameterType.name === type)
      .map(p => (
        <option key={p.contractParameterId} value={p.contractParameterId}>
          {p.name}
        </option>
      ));

  // Helper function to render checkboxes for Work Modes and Employment Types
  const getCheckboxes = (type: string, field: 'workModeIds' | 'employmentTypeIds') => {
    const filtered = parameters.filter(p => p.contractParameterType.name === type);

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 mt-2">
        {filtered.map(p => (
            <label key={p.contractParameterId} className="flex items-center gap-2 text-gray-700 dark:text-gray-300">
            <input
                type="checkbox"
                // Check if the current parameter's ID is included in the form's selected IDs for this field
                checked={form[field].includes(p.contractParameterId)}
                onChange={(e) =>
                handleMultiCheckbox(field, p.contractParameterId, e.target.checked)
                }
                className="form-checkbox h-4 w-4 text-blue-600 rounded"
            />
            <span>{p.name}</span>
            </label>
        ))}
        </div>
    );
  };


  return (
    <div className="flex flex-col gap-4 p-4 border rounded-lg">
      <h3 className="text-xl font-semibold text-gray-800 mb-2">Contract Details</h3>
      
      <label className="font-semibold text-gray-700 dark:text-gray-300">Min Salary: 
      <input 
          type="number" 
          name="salaryMin" 
          value={form.salaryMin} 
          onChange={handleChange} 
          placeholder="Salary Min" 
          className="border border-gray-300 rounded-md p-1"
          min="0" // Added for browser-level validation
      />
      </label>

      <label className="font-semibold text-gray-700 dark:text-gray-300">Max Salary: 
      <input 
          type="number" 
          name="salaryMax" 
          value={form.salaryMax} 
          onChange={handleChange} 
          placeholder="Salary Max" 
          className="border border-gray-300 rounded-md p-1"
          min="0" // Added for browser-level validation
      />
      </label>

      <label className="font-semibold text-gray-700 dark:text-gray-300">
        Salary Term:
        <select 
            name="salaryTermId" 
            value={form.salaryTermId} 
            onChange={handleChange}
            className="border border-gray-300 rounded-md p-1"
        >
          {getOptions('Salary Term')}
        </select>
      </label>

      <label className="font-semibold text-gray-700 dark:text-gray-300">Hours per term:
      <input 
          type="number" 
          name="hoursPerTerm" 
          value={form.hoursPerTerm} 
          onChange={handleChange} 
          placeholder="Hours" 
          className="border border-gray-300 rounded-md p-2"
          min="0" // Added for browser-level validation
      />
      </label>

      <label className="flex items-center gap-2 text-gray-700 dark:text-gray-300">
        <input 
            type="checkbox" 
            name="isNegotiable" 
            checked={form.isNegotiable} 
            onChange={handleChange} 
            className="form-checkbox h-5 w-5 text-blue-600 rounded"
        /> 
        <span>Negotiable</span>
      </label>

      <label className="font-semibold text-gray-700 dark:text-gray-300">
        Currency:
        <select 
            name="currencyId" 
            value={form.currencyId} 
            onChange={handleChange}
            className="border border-gray-300 rounded-md p-1"
        >
          {getOptions('Currency')}
        </select>
      </label>

      <fieldset className="border border-gray-300 p-3 rounded-md">
        <legend className="font-semibold text-gray-800 px-2 dark:text-gray-300">Work Modes</legend>
        {getCheckboxes('Work Mode', 'workModeIds')}
      </fieldset>

      <fieldset className="border border-gray-300 p-3 rounded-md">
        <legend className="font-semibold text-gray-800 px-2 dark:text-gray-300">Employment Types</legend>
        {getCheckboxes('Employment Type', 'employmentTypeIds')}
      </fieldset>

      <CancelButton/>
      <button
        type="button" // Use type="button" to prevent default form submission
        className="bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4"
        onClick={() => onSubmit(form)} // Call onSubmit with the current form state
      >
        {submitText}
      </button>
    </div>
  );
}

export default ContractConditionForm;