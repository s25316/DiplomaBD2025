'use client';
import React, { useState } from 'react';

interface Props {
  onSubmit: (form: ContractConditionFormData) => Promise<void>;
  initialData?: ContractConditionFormData;
  parameters: ContractParameter[];
  submitText?: string;
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
  salaryTermId: number;
  currencyId: number;
  workModeIds: number[];
  employmentTypeIds: number[];
}

const ContractConditionForm = ({ onSubmit, parameters, initialData, submitText = "Submit" }: Props) => {
  const [form, setForm] = useState<ContractConditionFormData>(
    initialData ?? {
      salaryMin: 0,
      salaryMax: 0,
      hoursPerTerm: 0,
      isNegotiable: false,
      salaryTermId: 3001,
      currencyId: 1,
      workModeIds: [],
      employmentTypeIds: [],
    }
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
  const target = e.target;
  const name = target.name;

  let value: string | number | boolean;
  if (target instanceof HTMLInputElement && target.type === 'checkbox') {
    value = target.checked;
  } else {
    value = Number(target.value);
  }

  setForm(prev => ({
    ...prev,
    [name]: value,
  }));
};


  const handleMultiCheckbox = (field: 'workModeIds' | 'employmentTypeIds', id: number, checked: boolean) => {
    setForm(prev => {
      const updated = checked
        ? [...prev[field], id]
        : prev[field].filter(i => i !== id);
      return { ...prev, [field]: updated };
    });
  };

  const getOptions = (type: string) =>
    parameters
      .filter(p => p.contractParameterType.name === type)
      .map(p => (
        <option key={p.contractParameterId} value={p.contractParameterId}>
          {p.name}
        </option>
      ));

    const getCheckboxes = (type: string, field: 'workModeIds' | 'employmentTypeIds') => {
    const filtered = parameters.filter(p => p.contractParameterType.name === type);

    return (
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "0.5rem" }}>
        {filtered.map(p => (
            <label key={p.contractParameterId} >
            <input
                type="checkbox"
                checked={form[field].includes(p.contractParameterId)}
                onChange={(e) =>
                handleMultiCheckbox(field, p.contractParameterId, e.target.checked)
                }
                className="accent-blue-500"
            />
            <span>{p.name}</span>
            </label>
        ))}
        </div>
    );
    };


return (
  <div className="flex flex-col gap-4">
    <br/>
    <label>Min salary: 
    <input type="number" name="salaryMin" value={form.salaryMin} onChange={handleChange} placeholder="Salary Min" />
    </label>

    <label>Max salary: 
    <input type="number" name="salaryMax" value={form.salaryMax} onChange={handleChange} placeholder="Salary Max" />
    </label>

    <label>
      Salary Term:
      <select name="salaryTermId" value={form.salaryTermId} onChange={handleChange}>
        {getOptions('Salary Term')}
      </select>
    </label>

    <label>Hours per term:
    <input type="number" name="hoursPerTerm" value={form.hoursPerTerm} onChange={handleChange} placeholder="Hours" />
    </label>

    <label>
      <input type="checkbox" name="isNegotiable" checked={form.isNegotiable} onChange={handleChange} /> Negotiable
    </label>

    <label>
      Currency:
      <select name="currencyId" value={form.currencyId} onChange={handleChange}>
        {getOptions('Currency')}
      </select>
    </label>

    <fieldset>
      <legend>Work Modes</legend>
      {getCheckboxes('Work Mode', 'workModeIds')}
    </fieldset>

    <fieldset>
      <legend>Employment Types</legend>
      {getCheckboxes('Employment Type', 'employmentTypeIds')}
    </fieldset>

    <button
      type="button"
      className="bg-blue-600 text-white p-2 rounded"
      onClick={() => onSubmit(form)}
    >
      {submitText}
    </button>
  </div>
);
}


export default ContractConditionForm;
