// "use client";
// import React, { useEffect, useState } from "react";
// import { useSession } from "next-auth/react";
// import { useParams, useRouter } from "next/navigation";

// interface ContractCondition {
//   contractConditionId: string;
//   salaryMin: number;
//   salaryMax: number;
//   hoursPerTerm: number;
//   isNegotiable: boolean;
//   salaryTermId: number;
//   currencyId: number;
//   workModeIds: number[];
//   employmentTypeIds: number[];
// }

// const EditContractCondition = () => {
//   const { data: session } = useSession();
//   const router = useRouter();
//   const { id, contractConditionId } = useParams();

//   const [condition, setCondition] = useState<ContractCondition>({
//     contractConditionId: "",
//     salaryMin: 0,
//     salaryMax: 0,
//     hoursPerTerm: 0,
//     isNegotiable: false,
//     salaryTermId: 0,
//     currencyId: 0,
//     workModeIds: [],
//     employmentTypeIds: [],
//   });

//   useEffect(() => {
//     if (!session?.user?.token || !contractConditionId) return;

//     const fetchCondition = async () => {
//       const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
//         headers: {
//           Authorization: `Bearer ${session.user.token}`,
//         },
//       });

//       if (res.ok) {
//         const data = await res.json();
//         setCondition(data.item);
//       } else {
//         alert("Failed to fetch contract condition.");
//       }
//     };

//     fetchCondition();
//   }, [session, contractConditionId]);

//   const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
//     const { name, value, type } = e.target;
//     const val = type === "checkbox" ? (e.target as HTMLInputElement).checked : value;
//     setCondition((prev) => ({
//       ...prev,
//       [name]: type === "number" ? Number(val) : val,
//     }));
//   };

//   const handleSubmit = async (e: React.FormEvent) => {
//     e.preventDefault();
//     if (!session?.user?.token) return;

//     const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
//       method: "PUT",
//       headers: {
//         "Content-Type": "application/json",
//         Authorization: `Bearer ${session.user.token}`,
//       },
//       body: JSON.stringify(condition),
//     });

//     if (res.ok) {
//       alert("Contract condition updated successfully!");
//       router.push(`/companies/${id}`);
//     } else {
//       alert("Failed to update contract condition.");
//     }
//   };

//   return (
//     <div className="max-w-xl mx-auto">
//       <h1 className="text-2xl font-bold mb-4">Edit Contract Condition</h1>
//       <form onSubmit={handleSubmit} className="flex flex-col gap-4">
//         <label>
//           Salary Min:
//           <input type="number" name="salaryMin" value={condition.salaryMin} onChange={handleChange} />
//         </label>
//         <label>
//           Salary Max:
//           <input type="number" name="salaryMax" value={condition.salaryMax} onChange={handleChange} />
//         </label>
//         <label>
//           Hours per Term:
//           <input type="number" name="hoursPerTerm" value={condition.hoursPerTerm} onChange={handleChange} />
//         </label>
//         <label>
//           Negotiable:
//           <input type="checkbox" name="isNegotiable" checked={condition.isNegotiable} onChange={handleChange} />
//         </label>
//         <label>
//           Salary Term ID:
//           <input type="number" name="salaryTermId" value={condition.salaryTermId} onChange={handleChange} />
//         </label>
//         <label>
//           Currency ID:
//           <input type="number" name="currencyId" value={condition.currencyId} onChange={handleChange} />
//         </label>
//         <label>
//           Work Mode IDs (comma separated):
//           <input
//             type="text"
//             name="workModeIds"
//             value={condition.workModeIds.join(",")}
//             onChange={(e) =>
//               setCondition((prev) => ({
//                 ...prev,
//                 workModeIds: e.target.value.split(",").map((id) => Number(id.trim())),
//               }))
//             }
//           />
//         </label>
//         <label>
//           Employment Type IDs (comma separated):
//           <input
//             type="text"
//             name="employmentTypeIds"
//             value={condition.employmentTypeIds.join(",")}
//             onChange={(e) =>
//               setCondition((prev) => ({
//                 ...prev,
//                 employmentTypeIds: e.target.value.split(",").map((id) => Number(id.trim())),
//               }))
//             }
//           />
//         </label>
//         <button type="submit" className="mt-4 bg-blue-600 text-white p-2 rounded">
//           Update Contract Condition
//         </button>
//       </form>
//     </div>
//   );
// };

// export default EditContractCondition;

'use client';

import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';

interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

interface ContractCondition {
  contractConditionId: string;
  salaryMin: number;
  salaryMax: number;
  hoursPerTerm: number;
  isNegotiable: boolean;
  salaryTermId: number;
  currencyId: number;
  workModeIds: number[];
  employmentTypeIds: number[];
}

const EditContractCondition = () => {
    const { data: session } = useSession();
    const router = useRouter();
    const { id, contractConditionId } = useParams();
  
    const [parameters, setParameters] = useState<ContractParameter[]>([]);
    const [condition, setCondition] = useState<ContractCondition>({
      contractConditionId: '',
      salaryMin: 0,
      salaryMax: 0,
      hoursPerTerm: 0,
      isNegotiable: false,
      salaryTermId: 0,
      currencyId: 0,
      workModeIds: [],
      employmentTypeIds: [],
    });
    useEffect(() => {
        if (!session?.user?.token) return;
    
        const fetchData = async () => {
          try {
            // Fetch contract parameters
            const paramRes = await fetch('http://localhost:8080/api/Dictionaries/contractParameters', {
              headers: { Authorization: `Bearer ${session.user.token}` },
            });
            if (paramRes.ok) {
              const data = await paramRes.json();
              setParameters(data);
            }
    
            // Fetch existing contract condition
            const conditionRes = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
              headers: { Authorization: `Bearer ${session.user.token}` },
            });
            if (conditionRes.ok) {
              const data = await conditionRes.json();
              const item = data.item;
              setCondition({
                contractConditionId: item.contractConditionId,
                salaryMin: item.salaryMin,
                salaryMax: item.salaryMax,
                hoursPerTerm: item.hoursPerTerm,
                isNegotiable: item.isNegotiable,
                salaryTermId: item.salaryTermId,
                currencyId: item.currencyId,
                workModeIds: item.workModeIds,
                employmentTypeIds: item.employmentTypeIds,
              });
            }
          } catch (error) {
            console.error('Error fetching data:', error);
          }
        };
    
        fetchData();
      }, [session, contractConditionId]);
      
      const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const target = e.target;
      
        const name = target.name;
      
        const value =
          target instanceof HTMLInputElement && target.type === 'checkbox'
            ? target.checked
            : Number(target.value) || target.value;
      
        setCondition((prev) => ({
          ...prev,
          [name]: value,
        }));
      };
      
    
      const handleArrayCheckbox = (field: 'workModeIds' | 'employmentTypeIds', id: number, isChecked: boolean) => {
        setCondition((prev) => {
          const updated = isChecked
            ? [...prev[field], id]
            : prev[field].filter((v) => v !== id);
          return { ...prev, [field]: updated };
        });
      };
      const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!session?.user?.token) return;
    
        try {
          const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/contractConditions/${contractConditionId}`, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
              Authorization: `Bearer ${session.user.token}`,
            },
            body: JSON.stringify({
              salaryMin: condition.salaryMin,
              salaryMax: condition.salaryMax,
              hoursPerTerm: condition.hoursPerTerm,
              isNegotiable: condition.isNegotiable,
              salaryTermId: condition.salaryTermId,
              currencyId: condition.currencyId,
              workModeIds: condition.workModeIds,
              employmentTypeIds: condition.employmentTypeIds,
            }),
          });
    
          if (res.ok) {
            alert('Contract condition updated successfully!');
            router.push(`/companies/${id}`);
          } else {
            const errorData = await res.json();
            alert(`Failed to update contract condition: ${errorData.message || 'Unknown error'}`);
          }
        } catch (error) {
          console.error('Error updating contract condition:', error);
          alert('An error occurred while updating the contract condition.');
        }
      };
      return (
        <div className="max-w-xl mx-auto">
          <h1 className="text-2xl font-bold mb-4">Edit Contract Condition</h1>
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            {/* Salary and Hours */}
            <input
              type="number"
              name="salaryMin"
              value={condition.salaryMin}
              onChange={handleInputChange}
              placeholder="Min Salary"
              className="p-1 border rounded"
            />
            <input
              type="number"
              name="salaryMax"
              value={condition.salaryMax}
              onChange={handleInputChange}
              placeholder="Max Salary"
              className="p-1 border rounded"
            />
            <input
              type="number"
              name="hoursPerTerm"
              value={condition.hoursPerTerm}
              onChange={handleInputChange}
              placeholder="Hours per Term"
              className="p-1 border rounded"
            />
    
            {/* Negotiable */}
            <label>
              <input
                type="checkbox"
                name="isNegotiable"
                checked={condition.isNegotiable}
                onChange={handleInputChange}
              />{' '}
              Negotiable Salary
            </label>
    
            {/* Currency */}
            <label>Currency:</label>
            <select name="currencyId" value={condition.currencyId} onChange={handleInputChange}>
              {parameters
                .filter((p) => p.contractParameterType.name === 'Currency')
                .map((p) => (
                  <option key={p.contractParameterId} value={p.contractParameterId}>
                    {p.name}
                  </option>
                ))}
            </select>
    
            {/* Salary Term */}
            <label>Salary Term:</label>
            <select name="salaryTermId" value={condition.salaryTermId} onChange={handleInputChange}>
              {parameters
                .filter((p) => p.contractParameterType.name === 'Salary Term')
                .map((p) => (
                  <option key={p.contractParameterId} value={p.contractParameterId}>
                    {p.name}
                  </option>
                ))}
            </select>
    
            {/* Work Modes */}
            <label>Work Modes:</label>
            {parameters
              .filter((p) => p.contractParameterType.name === 'Work Mode')
              .map((p) => (
                <label key={p.contractParameterId}>
                  <input
                    type="checkbox"
                    checked={condition.workModeIds.includes(p.contractParameterId)}
                    onChange={(e) =>
                      handleArrayCheckbox('workModeIds', p.contractParameterId, e.target.checked)
                    }
                  />{' '}
                  {p.name}
                </label>
              ))}
    
            {/* Employment Types */}
            <label>Employment Types:</label>
            {parameters
              .filter((p) => p.contractParameterType.name === 'Employment Type')
              .map((p) => (
                <label key={p.contractParameterId}>
                  <input
                    type="checkbox"
                    checked={condition.employmentTypeIds.includes(p.contractParameterId)}
                    onChange={(e) =>
                      handleArrayCheckbox('employmentTypeIds', p.contractParameterId, e.target.checked)
                    }
                  />{' '}
                  {p.name}
                </label>
              ))}
    
            <button type="submit" className="mt-4 bg-blue-600 text-white p-2 rounded">
              Save Changes
            </button>
          </form>
        </div>
      );
    };
    
    export default EditContractCondition;
    