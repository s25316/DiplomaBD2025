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

// Interface for a nested dictionary item inside ContractConditionData (e.g., salaryTerm, currency)
interface NestedContractParameter {
    contractParameterId: number;
    name: string;
}

// Interface for Work Mode and Employment Type nested arrays in ContractConditionData
interface ArrayNestedContractParameter {
    contractParameterId: number;
    name: string;
}

// Interface for the full Contract Condition object as returned by the API for details
interface ContractConditionDataApi {
  contractConditionId: string; // Assuming string ID for existing conditions
  salaryMin: number;
  salaryMax: number;
  hoursPerTerm: number;
  isNegotiable: boolean;
  salaryTerm: NestedContractParameter; // Full nested object from API
  currency: NestedContractParameter;     // Full nested object from API
  workModes: ArrayNestedContractParameter[]; // Array of nested objects from API
  employmentTypes: ArrayNestedContractParameter[]; // Array of nested objects from API
  companyId?: string; // Optional: if company ID is part of the API response
}

// API response item for a single Contract Condition (if nested like items[0]?.contractCondition)
interface ContractConditionApiItem {
  contractCondition: ContractConditionDataApi;
  // Other potential fields from API, e.g., company data
  company?: any;
}

// Interface for the data structure submitted by the form (simplified for payload)
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

const EditContractConditionPage = () => {
  const { data: session } = useSession();
  const { id, contractConditionId } = useParams() as { id: string; contractConditionId: string };
  const router = useRouter();

  // State to store the array of available contract parameters
  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  // State to store the initial data for the form, can be null initially
  const [initialData, setInitialData] = useState<ContractConditionFormData | null>(null);

  // Custom alert function (replace with a proper UI modal in production)
  const showCustomAlert = (message: string) => {
    console.log("ALERT:", message);
    alert(message); // Temporary: for Canvas environment demonstration
  };

  useEffect(() => {
    if (!session?.user?.token || !contractConditionId) return;

    const headers = { Authorization: `Bearer ${session.user.token}` };

    const fetchAll = async () => {
      try {
        const [paramRes, condRes] = await Promise.all([
          fetch('http://localhost:8080/api/Dictionaries/contractParameters', { headers }),
          fetch(`http://localhost:8080/api/CompanyUser/contractConditions/${contractConditionId}`, { headers }),
        ]);

        if (!paramRes.ok) {
          const errorText = await paramRes.text();
          throw new Error(`Failed to fetch contract parameters: ${errorText}`);
        }
        const paramData: ContractParameter[] = await paramRes.json();
        setParameters(paramData);
        console.log("Fetched contract parameters (Edit):", paramData); // Debug log

        if (!condRes.ok) {
          const errorText = await condRes.text();
          throw new Error(`Failed to fetch contract condition details: ${errorText}`);
        }
        const condData: { items: ContractConditionApiItem[] } = await condRes.json();
        const item = condData.items?.[0]?.contractCondition;

        if (!item) {
          showCustomAlert("Contract condition not found or malformed data.");
          router.push(`/companies/${id}`); // Redirect if not found
          return;
        }

        // Map the API data (ContractConditionDataApi) to the form's expected structure (ContractConditionFormData)
        setInitialData({
          salaryMin: item.salaryMin,
          salaryMax: item.salaryMax,
          hoursPerTerm: item.hoursPerTerm,
          isNegotiable: item.isNegotiable,
          salaryTermId: item.salaryTerm.contractParameterId, // Extract ID
          currencyId: item.currency.contractParameterId,     // Extract ID
          workModeIds: item.workModes.map((w: ArrayNestedContractParameter) => w.contractParameterId), // Extract IDs
          employmentTypeIds: item.employmentTypes.map((e: ArrayNestedContractParameter) => e.contractParameterId), // Extract IDs
        });
        console.log("Fetched and set initial form data (Edit):", { ...item, mappedToForm: true }); // Debug log

      } catch (error: any) {
        console.error("Error fetching data:", error);
        showCustomAlert(`Error loading data: ${error.message}`);
      }
    };

    fetchAll();
  }, [session, contractConditionId, id]); // Added 'id' to dependencies for router.push

  const handleSubmit = async (form: ContractConditionFormData) => {
    if (!session?.user?.token) {
      showCustomAlert("Authentication required to update contract condition.");
      return;
    }

    try {
      console.log("Submitting form data (Edit):", form); // Debug log
      const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session?.user.token}`,
        },
        body: JSON.stringify(form), // For PUT, usually send the object directly, not as an array
      });

      if (res.ok) {
        showCustomAlert('Contract Condition Updated Successfully!');
        router.push(`/companies/${id}`); // Redirect to company details page
      } else {
        const errorText = await res.text();
        console.error("Failed to update contract condition:", errorText);
        showCustomAlert(`Failed to update contract condition: ${errorText}`);
      }
    } catch (error: any) {
      console.error("Error updating contract condition:", error);
      showCustomAlert(`An error occurred while updating the contract condition: ${error.message}`);
    }
  };

  // Render loading state if not authorized or data is not yet loaded
  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  // Show loading if parameters are not fetched or initialData is not set yet
  if (parameters.length === 0 || initialData === null) return <div className="text-center py-4 text-blue-600">Loading contract condition data...</div>;

  return (
    <div className="max-w-xl mx-auto p-6 mt-8 font-inter">
      <h1 className="text-3xl font-bold mb-6 text-gray-800 text-center">Edit Contract Condition</h1>
      {/* Render form only when initialData is available */}
      <ContractConditionForm
        initialData={initialData}
        onSubmit={handleSubmit}
        parameters={parameters}
        submitText="Save Changes"
      />
    </div>
  );
};

export default EditContractConditionPage;
