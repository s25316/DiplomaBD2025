'use client';

import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import React, { useCallback, useEffect, useRef, useState } from 'react';
import ContractConditionForm from '@/app/components/forms/ContractConditionForm'; // Import the form component
import { OuterContainer } from '@/app/components/layout/PageContainers';

interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

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

interface ApiErrorItem {
  item?: ContractConditionFormData;
  isCorrect: boolean;
  message: string;
}

const CreateContractConditionPage = () => {
  const { data: session } = useSession();
  const { id } = useParams() as { id: string }; 
  const router = useRouter();
  const topRef = useRef<HTMLDivElement>(null); // Ref do przewijania
  // State to store the array of contract parameters
  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  const [apiError, setApiError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<string[]>([]);

  const backUrl = process.env.NEXT_PUBLIC_API_URL


  const resetErrors =useCallback(() => {
    setApiError(null);
    setValidationErrors([]);
  }, []);
  // Przewiń do góry, gdy pojawi się błąd
  useEffect(() => {
    if (apiError || validationErrors.length > 0) {
      topRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }, [apiError, validationErrors]);

  useEffect(() => {
    const fetchParameters = async () => {
      if (!session?.user?.token) return;

      try {
        const res = await fetch(`${backUrl}/api/Dictionaries/contractParameters`, {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });

        if (!res.ok) {
          const errorText = await res.text();
          throw new Error(`Failed to fetch contract parameters: ${errorText}`);
        }

        const paramData: ContractParameter[] = await res.json(); // Explicitly type the response
        setParameters(paramData);
        // console.log("Fetched contract parameters:", paramData); 
      } catch (error) {
        console.error("Error fetching parameters:", error);
        if(error instanceof Error){
          setApiError(`Parameter loading Error: ${error.message}`);
        } else{
          setApiError(`Parameter loading Error: Uknown error.`);
        }
      }
    };
    fetchParameters();
  }, [session, backUrl]); // Depend on session to refetch when it changes

  const handleSubmit = async (form: ContractConditionFormData) => {
    resetErrors();

    if (!session?.user?.token) {
      setApiError("Authentication required to create contract condition.");
      return;
    }

    try {
      // console.log("Submitting form data:", form); 
      const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${session?.user.token}`,
        },
        body: JSON.stringify([form]), // API expects an array containing the form data for create
      });

      if (res.ok) {
        window.alert('Contract Condition Created Successfully!');
        router.replace(`/companies/${id}`); // Redirect to company details page
      } else {
        const errorText = await res.text();
        console.error("Failed to create contract condition:", errorText);

        try {
          const parsedError = JSON.parse(errorText);
          
          // Handle array of errors like: [{"item":..., "isCorrect":false,"message":"..."}]
          if (Array.isArray(parsedError) && parsedError.length > 0 && parsedError[0].message) {
            setValidationErrors(parsedError.map((err: ApiErrorItem) => err.message));
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
            setApiError(`Cannot create contract: ${errorText}`);
          }
        } catch (parseError) {
          console.error("Error parsing API response:", parseError);
          setApiError(`Cannot create contract: ${errorText}`);
        }

      }
    } catch (error) {
      console.error("Error submitting contract condition:", error);
      if(error instanceof Error)
      setApiError(`An error occurred: ${error.message}`);
      else{
          setApiError(`Uknown error.`);
      }
    }
  };

  // Render loading state if not authorized or parameters are not loaded
  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  if (parameters.length === 0 && session.user.token) return <div className="text-center py-4 text-blue-600">Loading contract parameters...</div>;

  return (
    <OuterContainer className="max-w-xl mx-auto p-6 mt-8 font-inter">
      <div ref={topRef} />
      <h1 className="text-3xl font-bold mb-6 text-center">Create Contract Condition</h1>
      
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
      
      <ContractConditionForm onSubmit={handleSubmit} parameters={parameters} submitText="Create" onFormChange={resetErrors}/>
    </OuterContainer>
  );
};

export default CreateContractConditionPage;