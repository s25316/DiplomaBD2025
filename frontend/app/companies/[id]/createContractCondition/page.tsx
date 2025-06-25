'use client';

import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';
import ContractConditionForm from '@/app/components/forms/ContractConditionForm'; // Import the form component

// Interface for parameters like Currency, Salary Term, Work Mode, Employment Type
// as they are received from /api/Dictionaries/contractParameters
interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

// Interface for the data structure submitted by the form
interface ContractConditionFormData {
  salaryMin: number;
  salaryMax: number;
  hoursPerTerm: number;
  isNegotiable: boolean;
  salaryTermId: number;
  currencyId: number;
  workModeIds: number[];
  employmentTypeIds: number[];
}

const CreateContractConditionPage = () => {
  const { data: session } = useSession();
  const { id } = useParams() as { id: string }; // Type 'id' as string
  const router = useRouter();

  // State to store the array of contract parameters
  const [parameters, setParameters] = useState<ContractParameter[]>([]);

  // Custom alert function (replace with a proper UI modal in production)
  const showCustomAlert = (message: string) => {
    console.log("ALERT:", message);
    alert(message); // Temporary: for Canvas environment demonstration
  };

  useEffect(() => {
    const fetchParameters = async () => {
      if (!session?.user?.token) return;

      try {
        const res = await fetch('http://localhost:8080/api/Dictionaries/contractParameters', {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });

        if (!res.ok) {
          const errorText = await res.text();
          throw new Error(`Failed to fetch contract parameters: ${errorText}`);
        }

        const paramData: ContractParameter[] = await res.json(); // Explicitly type the response
        setParameters(paramData);
        console.log("Fetched contract parameters:", paramData); // Debug log
      } catch (error: any) {
        console.error("Error fetching parameters:", error);
        showCustomAlert(`Error loading contract parameters: ${error.message}`);
      }
    };
    fetchParameters();
  }, [session]); // Depend on session to refetch when it changes

  const handleSubmit = async (form: ContractConditionFormData) => {
    if (!session?.user?.token) {
      showCustomAlert("Authentication required to create contract condition.");
      return;
    }

    try {
      console.log("Submitting form data:", form); // Debug log
      const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session?.user.token}`,
        },
        body: JSON.stringify([form]), // API expects an array containing the form data for create
      });

      if (res.ok) {
        showCustomAlert('Contract Condition Created Successfully!');
        router.push(`/companies/${id}`); // Redirect to company details page
      } else {
        const errorText = await res.text();
        console.error("Failed to create contract condition:", errorText);
        showCustomAlert(`Failed to create contract condition: ${errorText}`);
      }
    } catch (error: any) {
      console.error("Error submitting contract condition:", error);
      showCustomAlert(`An error occurred: ${error.message}`);
    }
  };

  // Render loading state if not authorized or parameters are not loaded
  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  if (parameters.length === 0 && session.user.token) return <div className="text-center py-4 text-blue-600">Loading contract parameters...</div>;

  return (
    <div className="max-w-xl mx-auto p-6 mt-8 font-inter">
      <h1 className="text-3xl font-bold mb-6 text-gray-800 text-center">Create Contract Condition</h1>
      <ContractConditionForm onSubmit={handleSubmit} parameters={parameters} submitText="Create" />
    </div>
  );
};

export default CreateContractConditionPage;
