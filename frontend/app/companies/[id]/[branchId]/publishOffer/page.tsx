"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useParams, useRouter } from "next/navigation";

interface OfferTemplate {
  offerTemplateId: string;
  name: string;
  description: string;
  skills: {
    isRequired: boolean;
    skill: {
      skillId: number;
      name: string;
      skillType: {
        skillTypeId: number;
        name: string;
      };
    };
  }[];
}

interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}
interface ExistingContractCondition {
  contractConditionId: string;
  hoursPerTerm: number;
  salaryMin: number;
  salaryMax: number;
  isNegotiable: boolean;
  salaryTerm: { name: string };
  currency: { name: string };
  workModes: {name: string}[];
  employmentTypes: {name: string}[];
}

const PublishOffer = () => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id, branchId } = useParams();

  const [templates, setTemplates] = useState<OfferTemplate[]>([]);
  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  const [existingConditions, setExistingConditions] = useState<ExistingContractCondition[]>([]);
  const [includeConditions, setIncludeConditions] = useState(false);
  const [selectedConditionId, setSelectedConditionId] = useState<string>("");

  const [form, setForm] = useState({
    offerTemplateId: "",
    publicationStart: "",
    publicationEnd: "",
    employmentLength: 0,
    websiteUrl: "",
    conditionIds: [] as string[],
  });

  const [conditions, setConditions] = useState({
    salaryMin: 0,
    salaryMax: 0,
    hoursPerTerm: 40,
    isNegotiable: false,
    salaryTermId: 3001,
    currencyId: 1,
    workModeIds: [] as number[],
    employmentTypeIds: [] as number[],
  });

  useEffect(() => {
    if (!session?.user?.token || !id) return;
  
    const headers = {
      Authorization: `Bearer ${session.user.token}`,
    };
  
    const fetchData = async () => {
      const [templateRes, paramRes, condRes] = await Promise.all([
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, { headers }),
        fetch(`http://localhost:8080/api/Dictionaries/contractParameters`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions`, { headers }), // fallback if no /companies/{id}/contractConditions
      ]);
  
      if (templateRes.ok) {
        const data = await templateRes.json();
        setTemplates(data.items.map((item: any) => item.offerTemplate)); // lub item, zależnie od struktury
      }
  
      if (paramRes.ok) {
        const data = await paramRes.json();
        setParameters(data);
      }
  
      if (condRes.ok) {
        const data = await condRes.json();
        const filtered = data.items
          .map((item: any) => item.contractCondition)
          .filter((c: any) => c.companyId === id);
        setExistingConditions(filtered);
      }
    };
  
    fetchData();
  }, [session, id]);
  

  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleConditionChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const target = e.target;
    const name = target.name;
    const val = target instanceof HTMLInputElement && target.type === "checkbox"
      ? target.checked
      : Number(target.value) || target.value;

    setConditions((prev) => ({
      ...prev,
      [name]: val,
    }));
  };

  const handleArrayCheckbox = (field: "workModeIds" | "employmentTypeIds", id: number, isChecked: boolean) => {
    setConditions((prev) => {
      const updated = isChecked
        ? [...prev[field], id]
        : prev[field].filter((v) => v !== id);
      return { ...prev, [field]: updated };
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    let conditionId: string | null = selectedConditionId || null;

    if (includeConditions && !selectedConditionId) {
      const res = await fetch(
        `http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${session.user.token}`,
          },
          body: JSON.stringify([conditions]),
        }
      );

      if (res.ok) {
        const result = await res.json();
        conditionId = result[0].item.contractConditionId;
      } else {
        alert("Failed to create contract condition.");
        return;
      }
    }

    const payload = [{
      ...form,
      branchId: branchId as string,
      conditionIds: conditionId ? [conditionId] : [],
    }];

    // const offerRes = await fetch("http://localhost:8080/api/CompanyUser/companies/offers", {
    //   method: "POST",
    //   headers: {
    //     "Content-Type": "application/json",
    //     Authorization: `Bearer ${session.user.token}`,
    //   },
    //   body: JSON.stringify(payload),
    // });

    // if (offerRes.ok) {
    //   alert("Offer published!");
    //   router.push(`/companies/${id}/${branchId}`);
    // } else {
    //   alert("Failed to publish offer.");
    // }
    const offerRes = await fetch("http://localhost:8080/api/CompanyUser/companies/offers", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(payload),
    });
    
    if (offerRes.ok) {
      alert("Offer published!");
      router.push(`/companies/${id}/${branchId}`);
    } else {
      const errorText = await offerRes.text();
      console.error("Error response:", errorText);
      alert("Failed to publish offer. Details in console.");
    }
    
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Publish Offer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">

        {/* Existing Conditions */}
        <label>Use Existing Condition:</label>
        <select value={selectedConditionId} onChange={(e) => setSelectedConditionId(e.target.value)} className="p-1 border rounded">
          <option value="">-- None --</option>
          {existingConditions.map(cond => (
            <option key={cond.contractConditionId} value={cond.contractConditionId}>
              {`[${cond.hoursPerTerm}h] ${cond.salaryMin}-${cond.salaryMax} ${cond.currency.name}`}
            </option>
          ))}
        </select>

        {/* Add New Condition */}
        <label>
          <input
            type="checkbox"
            checked={includeConditions}
            onChange={(e) => setIncludeConditions(e.target.checked)}
            disabled={!!selectedConditionId}
          /> Create New Contract Condition
        </label>

        {includeConditions && (
          <>
            <hr />
            <h3 className="text-lg font-semibold mt-2">New Contract Condition</h3>
            <input type="number" name="salaryMin" placeholder="Min Salary" onChange={handleConditionChange} className="p-1 border rounded" />
            <input type="number" name="salaryMax" placeholder="Max Salary" onChange={handleConditionChange} className="p-1 border rounded" />
            <input type="number" name="hoursPerTerm" placeholder="Hours per Term" onChange={handleConditionChange} className="p-1 border rounded" />
            <label>
              <input type="checkbox" name="isNegotiable" onChange={handleConditionChange} /> Negotiable Salary
            </label>

            <label>Currency:</label>
            <select name="currencyId" onChange={handleConditionChange}>
              {parameters.filter(p => p.contractParameterType.name === "Currency").map(p => (
                <option key={p.contractParameterId} value={p.contractParameterId}>{p.name}</option>
              ))}
            </select>

            <label>Salary Term:</label>
            <select name="salaryTermId" onChange={handleConditionChange}>
              {parameters.filter(p => p.contractParameterType.name === "Salary Term").map(p => (
                <option key={p.contractParameterId} value={p.contractParameterId}>{p.name}</option>
              ))}
            </select>

            <div className="mt-4">
              <label className="block mb-1 font-medium">Work Modes:</label>
              <div className="grid grid-cols-2 gap-2">
                {parameters.filter(p => p.contractParameterType.name === "Work Mode").map((p) => (
                  <label key={p.contractParameterId} className="flex gap-2 items-center">
                    <input
                      type="checkbox"
                      onChange={(e) => handleArrayCheckbox("workModeIds", p.contractParameterId, e.target.checked)}
                    /> {p.name}
                  </label>
                ))}
              </div>
            </div>

            <div className="mt-4">
              <label className="block mb-1 font-medium">Employment Types:</label>
              <div className="grid grid-cols-2 gap-2">
                {parameters.filter(p => p.contractParameterType.name === "Employment Type").map((p) => (
                  <label key={p.contractParameterId} className="flex gap-2 items-center">
                    <input
                      type="checkbox"
                      onChange={(e) => handleArrayCheckbox("employmentTypeIds", p.contractParameterId, e.target.checked)}
                    /> {p.name}
                  </label>
                ))}
              </div>
            </div>
          </>
        )}
        {/* Contract Condition Preview */}
        {selectedConditionId && (
          <div>
            <h4 className="font-semibold mb-2">Selected Contract Details</h4>
            {(() => {
              const cond = existingConditions.find(c => c.contractConditionId === selectedConditionId);
              if (!cond) return <p className="text-sm text-gray-500">Loading...</p>;
              return (
                <ul className="text-sm list-disc list-inside text-gray-700">
                  <li><b>Hours/Term:</b> {cond.hoursPerTerm}</li>
                  <li><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency.name}</li>
                  <li><b>Negotiable:</b> {cond.isNegotiable ? "Yes" : "No"}</li>
                  <li><b>Salary Term:</b> {cond.salaryTerm.name}</li>
                  <li>
                    <b>Work Modes:</b> {cond.workModes.map((wm) => wm.name).join(", ")}
                  </li>
                  <li>
                      <b>Employment Types:</b> {cond.employmentTypes.map((et) => et.name).join(", ")}
                  </li>

                </ul>
              );
            })()}
          </div>
        )}


        {/* Remaining fields */}
        <div className="flex items-center gap-2">
          <label className="w-40">Offer Template:</label>
          <select name="offerTemplateId" onChange={handleFormChange} required className="flex-1 p-1 border rounded">
            <option value="">Select template</option>
            {templates.map((t) => (
              <option key={t.offerTemplateId} value={t.offerTemplateId}>{t.name}</option>
            ))}
          </select>
        </div>
        {/* Template Preview */}
        {form.offerTemplateId && (
        <div>
          <h4 className="font-semibold mb-2">Selected Template</h4>
          {(() => {
            const selectedTemplate = templates.find(t => t.offerTemplateId === form.offerTemplateId);
            if (!selectedTemplate) return <p className="text-sm text-gray-500">Loading...</p>;
            return (
              <ul className="text-sm list-disc list-inside text-gray-700">
                <p><b>Name:</b> {selectedTemplate.name}</p>
                <p><b>Description:</b> {selectedTemplate.description}</p>
                <p><b>Skills:</b></p>
                <ul className="list-disc pl-6">
                  {selectedTemplate.skills.map((s, index) => (
                    <li key={index}>
                      {s.skill.name} ({s.skill.skillType.name}) {s.isRequired ? "(required)" : ""}
                    </li>
                  ))}
                </ul>
              </ul>
            );
          })()}
        </div>
)}



        <div className="flex items-center gap-2">
          <label className="w-40">Publication Start:</label>
          <input type="datetime-local" name="publicationStart" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>
        <div className="flex items-center gap-2">
          <label className="w-40">Publication End:</label>
          <input type="datetime-local" name="publicationEnd" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>

        <div className="flex items-center gap-2">
          <label className="w-40">Employment Length (months):</label>
          <input type="number" name="employmentLength" onChange={handleFormChange} className="flex-1 p-1 border rounded" />
        </div>

        <div className="flex items-center gap-2">
          <label className="w-40">Website URL:</label>
          <input type="text" name="websiteUrl" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>

        <button type="submit" className="mt-4 bg-blue-600 text-white p-2 rounded">Publish Offer</button>
      </form>
    </div>
  );
};

export default PublishOffer;
