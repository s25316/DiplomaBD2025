// app/companies/[id]/createContractCondition/page.tsx
'use client';

import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';

type ContractParameter = {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
};

export default function CreateContractConditionPage() {
  const { data: session } = useSession();
  const { id: companyId } = useParams();
  const router = useRouter();

  const [salaryMin, setSalaryMin] = useState<number>(0);
  const [salaryMax, setSalaryMax] = useState<number>(0);
  const [hoursPerTerm, setHoursPerTerm] = useState<number>(0);
  const [isNegotiable, setIsNegotiable] = useState<boolean>(false);
  const [salaryTermId, setSalaryTermId] = useState<number>(3001);
  const [currencyId, setCurrencyId] = useState<number>(1);
  const [workModeIds, setWorkModeIds] = useState<number[]>([]);
  const [employmentTypeIds, setEmploymentTypeIds] = useState<number[]>([]);

  const [contractParameters, setContractParameters] = useState<ContractParameter[]>([]);

  useEffect(() => {
    const fetchContractParameters = async () => {
      if (!session?.user?.token) return;

      const res = await fetch('http://localhost:8080/api/Dictionaries/contractParameters', {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session.user.token}`,
        },
      });

      if (res.ok) {
        const data = await res.json();
        setContractParameters(data);
      }
    };

    fetchContractParameters();
  }, [session]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!session?.user?.token || !companyId) return;

    const payload = [
      {
        salaryMin,
        salaryMax,
        hoursPerTerm,
        isNegotiable,
        salaryTermId,
        currencyId,
        workModeIds,
        employmentTypeIds,
      },
    ];

    const res = await fetch(
      `http://localhost:8080/api/CompanyUser/companies/${companyId}/contractConditions`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session.user.token}`,
        },
        body: JSON.stringify(payload),
      }
    );

    if (res.ok) {
      alert('Contract condition created successfully.');
      router.push(`/companies/${companyId}`);
    } else {
      alert('Failed to create contract condition.');
    }
  };

  const getOptions = (typeId: number) =>
    contractParameters
      .filter((param) => param.contractParameterType.contractParameterTypeId === typeId)
      .map((param) => (
        <option key={param.contractParameterId} value={param.contractParameterId}>
          {param.name}
        </option>
      ));

  const getCheckboxes = (
    typeId: number,
    selectedIds: number[],
    setSelectedIds: (ids: number[]) => void
  ) =>
    {
      return (
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "0.5rem" }}>
          {contractParameters
      .filter((param) => param.contractParameterType.contractParameterTypeId === typeId)
      .map((param) => (
        <label key={param.contractParameterId}>
          <input
            type="checkbox"
            value={param.contractParameterId}
            checked={selectedIds.includes(param.contractParameterId)}
            onChange={(e) => {
              const value = parseInt(e.target.value);
              if (e.target.checked) {
                setSelectedIds([...selectedIds, value]);
              } else {
                setSelectedIds(selectedIds.filter((id) => id !== value));
              }
            }}
          />
          {param.name}
        </label>
      ))}
      </div>
      );
    };

  return (
    <div>
      <h1>Create Contract Condition</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            Salary Min:
            <input
              type="number"
              value={salaryMin}
              onChange={(e) => setSalaryMin(parseFloat(e.target.value))}
            />
          </label>
        </div>
        <div>
          <label>
            Salary Max:
            <input
              type="number"
              value={salaryMax}
              onChange={(e) => setSalaryMax(parseFloat(e.target.value))}
            />
          </label>
        </div>
        <div>
          <label>
            Hours Per Term:
            <input
              type="number"
              value={hoursPerTerm}
              onChange={(e) => setHoursPerTerm(parseFloat(e.target.value))}
            />
          </label>
        </div>
        <div>
          <label>
            Is Negotiable:
            <input
              type="checkbox"
              checked={isNegotiable}
              onChange={(e) => setIsNegotiable(e.target.checked)}
            />
          </label>
        </div>
        <div>
          <label>
            Salary Term:
            <select
              value={salaryTermId}
              onChange={(e) => setSalaryTermId(parseInt(e.target.value))}
            >
              {getOptions(3)}
            </select>
          </label>
        </div>
        <div>
          <label>
            Currency:
            <select
              value={currencyId}
              onChange={(e) => setCurrencyId(parseInt(e.target.value))}
            >
              {getOptions(4)}
            </select>
          </label>
        </div>
        <div>
          <h3 className="mt-4 font-semibold">Work Modes:</h3>
          {getCheckboxes(1, workModeIds, setWorkModeIds)}
        </div>
        <div>
        <h3 className="mt-4 font-semibold">Employment Types:</h3>
          {getCheckboxes(2, employmentTypeIds, setEmploymentTypeIds)}
        </div>
        <button type="submit">Create</button>
      </form>
    </div>
  );
}
