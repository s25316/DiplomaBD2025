'use client';

import React, { useState } from 'react';
import { InnerSection } from '../layout/PageContainers';

export interface Skill {
  isRequired: boolean,
  skill: {
    skillId: number,
    name: string,
    skillType: {
      skillTypeId: number,
      name: string
    }
  }

}

export interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

export interface ContractConditionFormData {
  salaryMin: number;
  salaryMax: number;
  hoursPerTerm: number;
  isNegotiable: boolean;
  isPaid: boolean;
  salaryTermId: number;
  currencyId: number;
  workModeIds: number[];
  employmentTypeIds: number[];
}

interface ContractConditionFormProps {
  onSubmit: (form: ContractConditionFormData) => Promise<void>;
  initialData?: ContractConditionFormData | null;
  parameters: ContractParameter[];
  submitText?: string;
}

const ContractConditionForm = ({ onSubmit, parameters, initialData, submitText = "Submit" }: ContractConditionFormProps) => {

  const [form, setForm] = useState<ContractConditionFormData>(
    initialData ?? {
      salaryMin: 0,
      salaryMax: 0,
      hoursPerTerm: 0,
      isNegotiable: false,
      isPaid: false,
      salaryTermId: 3001,
      currencyId: 1,
      workModeIds: [],
      employmentTypeIds: [],
    }
  );

  // React.useEffect(() => {
  //   console.log('ContractConditionForm Initialized with Form:', form);
  //   console.log('ContractConditionForm Parameters:', parameters);
  // }, [form, parameters]);


  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const target = e.target;
    const name = target.name;

    let value: string | number | boolean;
    if (target.type === 'checkbox') {
      value = (target as HTMLInputElement).checked;
    } else if (target.type === 'number') { // For number inputs (salaryMin, salaryMax, hoursPerTerm)
      let numValue = Number(target.value);

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
    <InnerSection className="flex flex-col gap-4 p-4 border rounded-lg">
      <h3 className="font-bold mb-2">Contract Details</h3>

      <label className="font-semibold text-gray-700 dark:text-gray-300">Min Salary:  </label>
      <input
        type="number"
        name="salaryMin"
        value={form.salaryMin}
        onChange={handleChange}
        placeholder="Salary Min"
        className="global-field-style"
        min="0" // Added for browser-level validation
      />


      <label className="font-semibold text-gray-700 dark:text-gray-300">Max Salary: </label>
      <input
        type="number"
        name="salaryMax"
        value={form.salaryMax}
        onChange={handleChange}
        placeholder="Salary Max"
        className="global-field-style"
        min="0"
      />


      <label className="font-semibold text-gray-700 dark:text-gray-300"></label>
      Salary Term:
      <select
        name="salaryTermId"
        value={form.salaryTermId}
        onChange={handleChange}
        className="global-field-style"
      >
        {getOptions('Salary Term')}
      </select>


      <label className="font-semibold text-gray-700 dark:text-gray-300">Hours per term: </label>
      <input
        type="number"
        name="hoursPerTerm"
        value={form.hoursPerTerm}
        onChange={handleChange}
        placeholder="Hours"
        className="global-field-style"
        min="0"
      />


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
          className="global-field-style"
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


      <button
        type="button"
        className="bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4"
        onClick={() => onSubmit(form)}
      >
        {submitText}
      </button>
    </InnerSection>
  );
}

export default ContractConditionForm;