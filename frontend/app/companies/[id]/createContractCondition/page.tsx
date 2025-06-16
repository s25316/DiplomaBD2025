'use client';
import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';
import ContractConditionForm, { ContractConditionFormData, ContractParameter } from '@/app/components/forms/ContractConditionForm';

const CreateContractConditionPage = () => {
  const { data: session } = useSession();
  const { id } = useParams();
  const router = useRouter();
  const [parameters, setParameters] = useState<ContractParameter[]>([]);

  useEffect(() => {
    if (!session?.user?.token) return;
    fetch('http://localhost:8080/api/Dictionaries/contractParameters', {
      headers: { Authorization: `Bearer ${session.user.token}` },
    })
      .then(res => res.json())
      .then(setParameters);
  }, [session]);

  const handleSubmit = async (form: ContractConditionFormData) => {
    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify([form]),
    });

    if (res.ok) {
      alert('Created');
      router.push(`/companies/${id}`);
    } else {
      alert('Creation failed');
    }
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1>Create Contract Condition</h1>
      <ContractConditionForm onSubmit={handleSubmit} parameters={parameters} submitText="Create" />
    </div>
  );
};

export default CreateContractConditionPage;
