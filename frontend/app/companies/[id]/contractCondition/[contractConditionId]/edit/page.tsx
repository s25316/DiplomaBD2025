'use client';

import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import {useRef, useCallback, useEffect, useState } from 'react';
import ContractConditionForm from '@/app/components/forms/ContractConditionForm'; // Import the form component
import { OuterContainer } from '@/app/components/layout/PageContainers';

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
  isPaid: boolean;
}

// API response item for a single Contract Condition (if nested like items[0]?.contractCondition)
interface ContractConditionApiItem {
  contractCondition: ContractConditionDataApi;
  // Other potential fields from API, e.g., company data
  company: Company;
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
  isPaid: boolean;
}


const EditContractConditionPage = () => {
  const { data: session } = useSession();
  const { id, contractConditionId } = useParams() as { id: string; contractConditionId: string };
  const router = useRouter();

  // State to store the array of available contract parameters
  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  // State to store the initial data for the form, can be null initially
  const [initialData, setInitialData] = useState<ContractConditionFormData | null>(null);
  const topRef = useRef<HTMLDivElement>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<string[]>([]);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const resetErrors = useCallback(() => {
    setApiError(null);
    setValidationErrors([]);
  }, []);
  useEffect(() => {
    if (apiError || validationErrors.length > 0) {
      topRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }, [apiError, validationErrors]);

  useEffect(() => {
    if (!session?.user?.token || !contractConditionId) return;

    const headers = { Authorization: `Bearer ${session.user.token}` };

    const fetchAll = async () => {
      try {
        const [paramRes, condRes] = await Promise.all([
          fetch(`${backUrl}/api/Dictionaries/contractParameters`, { headers }),
          fetch(`${backUrl}/api/CompanyUser/contractConditions/${contractConditionId}`, { headers }),
        ]);

        if (!paramRes.ok) {
          const errorText = await paramRes.text();
          throw new Error(`Failed to fetch contract parameters: ${errorText}`);
        }
        const paramData: ContractParameter[] = await paramRes.json();
        setParameters(paramData);
        // console.log("Fetched contract parameters (Edit):", paramData); 

        if (!condRes.ok) {
          const errorText = await condRes.text();
          throw new Error(`Failed to fetch contract condition details: ${errorText}`);
        }
        const condData: { items: ContractConditionApiItem[] } = await condRes.json();
        const item = condData.items?.[0]?.contractCondition;

        if (!item) {
          alert("Contract condition not found or malformed data.");
          router.replace(`/companies/${id}`); // Redirect if not found
          return;
        }

        // Map the API data (ContractConditionDataApi) to the form's expected structure (ContractConditionFormData)
        setInitialData({
          salaryMin: item.salaryMin,
          salaryMax: item.salaryMax,
          hoursPerTerm: item.hoursPerTerm,
          isNegotiable: item.isNegotiable,
          salaryTermId: item.salaryTerm?.contractParameterId, // Extract ID
          currencyId: item.currency?.contractParameterId,     // Extract ID
          workModeIds: item.workModes.map((w: ArrayNestedContractParameter) => w.contractParameterId), // Extract IDs
          employmentTypeIds: item.employmentTypes.map((e: ArrayNestedContractParameter) => e.contractParameterId), // Extract IDs
          isPaid: item.isPaid
        });
        // console.log("Fetched and set initial form data (Edit):", { ...item, mappedToForm: true }); 

      } catch (error: unknown) {
        console.error("Error fetching data:", error);
        if(error instanceof Error)
        alert(`Error loading data: ${error.message}`);
      }
    };

    fetchAll();
  }, [session, contractConditionId, id, router]); // Added 'id' to dependencies for router.push

  const handleSubmit = async (form: ContractConditionFormData) => {
    resetErrors();
    
    if (!session?.user?.token) {
      alert("Authentication required to update contract condition.");
      return;
    }

    try {
      // console.log("Submitting form data (Edit):", form); 
      const res = await fetch(`${backUrl}/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session?.user.token}`,
        },
        body: JSON.stringify(form), // For PUT, usually send the object directly, not as an array
      });

      if (res.ok) {
        alert('Contract Condition Updated Successfully!');
        router.replace(`/companies/${id}`);
      } else {
        const errorText = await res.text();
        console.error("Failed to update contract condition:", errorText);
        try {
          const parsedError = JSON.parse(errorText);
          
          // Handle array of errors like: [{"item":..., "isCorrect":false,"message":"..."}]
          if (parsedError.message) {
            setValidationErrors([parsedError.message]);
            setApiError("Please note:");
          }
          // Handle general errors with 'errors' object (like in CreateCompany)
          else if (parsedError.errors) {
            const errorsArray: string[] = [];
            for (const key in parsedError.errors) {
              if (Object.prototype.hasOwnProperty.call(parsedError.errors, key)) {
                const messages = parsedError.errors[key];
                if (Array.isArray(messages)) {
                  messages.forEach(msg => errorsArray.push(`${key}: ${msg}`));
                }
              }
            }
            setValidationErrors(errorsArray);
            setApiError("Please note:");
          } 
          // Handle single error messages (title or detail)
          else if (parsedError.title || parsedError.detail) {
            setApiError(parsedError.title || parsedError.detail);
          } 
          // Fallback for unexpected JSON error format
          else {
            setApiError(`An error occurred: ${errorText}`);
          }
        } catch (parseError) {
          console.error("Error parsing API response:", parseError);
          setApiError(`An unexpected error occurred. Could not parse the server response.`);
        }
      }
    } catch (error: unknown) {
      console.error("Error updating contract condition:", error);
      if(error instanceof Error)
        setApiError(`An error occurred: ${error.message}`);
      else{
        setApiError(`Uknown error.`);
      }
    }
  };

  // Render loading state if not authorized or data is not yet loaded
  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  // Show loading if parameters are not fetched or initialData is not set yet
  if (parameters.length === 0 || initialData === null) return <div className="text-center py-4 text-blue-600">Loading contract condition data...</div>;

  return (
    <OuterContainer className="max-w-xl mx-auto p-6 mt-8 font-inter">
      <div ref={topRef} />
      <h1 className="text-3xl font-bold mb-6 text-center">Edit Contract Condition</h1>
      
      {(apiError || validationErrors.length > 0) && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-6 dark:bg-red-800 dark:border-red-700 dark:text-red-100">
          <strong className="font-bold">Error:</strong>
          {apiError && <p className="mt-2 text-sm">{apiError}</p>}
          {validationErrors.length > 0 && (
            <ul className="list-disc list-inside mt-3 text-sm">
              {validationErrors.map((err, idx) => (
                <li key={idx}>{err}</li>
              ))}
            </ul>
          )}
        </div>
      )}
      {/* Render form only when initialData is available */}
      <ContractConditionForm
        initialData={initialData}
        onSubmit={handleSubmit}
        parameters={parameters}
        submitText="Save Changes"
        onFormChange={resetErrors}
      />
    </OuterContainer>
  );
};

export default EditContractConditionPage;