'use client';
import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';
import ContractConditionForm, { ContractConditionFormData, ContractParameter } from '@/app/components/forms/ContractConditionForm';

const EditContractConditionPage = () => {
  const { data: session } = useSession();
  const { id, contractConditionId } = useParams();
  const router = useRouter();

  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  const [initialData, setInitialData] = useState<ContractConditionFormData>();

  useEffect(() => {
    if (!session?.user?.token) return;

    const headers = { Authorization: `Bearer ${session.user.token}` };

    const fetchAll = async () => {
      const [paramRes, condRes] = await Promise.all([
        fetch('http://localhost:8080/api/Dictionaries/contractParameters', { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/contractConditions/${contractConditionId}`, { headers }),
      ]);

      if (paramRes.ok) {
        const paramData = await paramRes.json();
        setParameters(paramData);
      }

      if (condRes.ok) {
        const condData = await condRes.json();
        const item = condData.items[0].contractCondition;
        setInitialData({
          salaryMin: item.salaryMin,
          salaryMax: item.salaryMax,
          hoursPerTerm: item.hoursPerTerm,
          isNegotiable: item.isNegotiable,
          salaryTermId: item.salaryTerm.contractParameterId,
          currencyId: item.currency.contractParameterId,
          workModeIds: item.workModes.map((w: any) => w.contractParameterId),
          employmentTypeIds: item.employmentTypes.map((e: any) => e.contractParameterId),
        });
      }
    };

    fetchAll();
  }, [session, contractConditionId]);

  const handleSubmit = async (form: ContractConditionFormData) => {
    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify(form),
    });

    if (res.ok) {
      alert('Updated');
      router.push(`/companies/${id}`);
    } else {
      alert('Update failed');
    }
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1>Edit Contract Condition</h1>
      {initialData && (
        <ContractConditionForm
          initialData={initialData}
          onSubmit={handleSubmit}
          parameters={parameters}
          submitText="Save Changes"
        />
      )}
    </div>
  );
};

export default EditContractConditionPage;
