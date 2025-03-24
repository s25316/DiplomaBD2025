"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useParams, useRouter } from "next/navigation";

interface OfferTemplate {
  offerTemplateId: string;
  name: string;
}

interface ContractParameter {
  contractParameterId: number;
  name: string;
  contractParameterType: {
    contractParameterTypeId: number;
    name: string;
  };
}

const PublishOffer = () => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id, branchId } = useParams();

  const [templates, setTemplates] = useState<OfferTemplate[]>([]);
  const [parameters, setParameters] = useState<ContractParameter[]>([]);
  const [includeConditions, setIncludeConditions] = useState(false);
  const [createdConditionId, setCreatedConditionId] = useState<string | null>(null);

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
    if (!session?.user?.token) return;

    const fetchData = async () => {
      const [templateRes, paramRes] = await Promise.all([
        fetch("http://localhost:8080/api/CompanyUser/offerTemplates", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        }),
        fetch("http://localhost:8080/api/Dictionaries/contractParameters", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        }),
      ]);

      if (templateRes.ok) {
        const data = await templateRes.json();
        const mapped = data.items.map((item: any) => ({
          offerTemplateId: item.offerTemplate.offerTemplateId,
          name: item.offerTemplate.name,
        }));
        setTemplates(mapped);
      }

      if (paramRes.ok) {
        const data = await paramRes.json();
        setParameters(data);
      }
    };

    fetchData();
  }, [session]);

  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleConditionChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const target = e.target as HTMLInputElement | HTMLSelectElement;
    const name = target.name;
    const value = target.value;

const val =
  target instanceof HTMLInputElement && target.type === "checkbox"
    ? target.checked
    : Number(value) || value;

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

    let conditionId: string | null = null;

    // Warunkowe tworzenie contractConditions
    if (includeConditions) {
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
        alert("Failed to create contract conditions.");
        return;
      }
    }

    const payload = [
      {
        ...form,
        branchId: branchId as string,
        conditionIds: conditionId ? [conditionId] : [],
      },
    ];

    const offerRes = await fetch("http://localhost:8080/api/CompanyUser/companies/offers", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(payload),
    });

    if (offerRes.ok) {
      alert("Offer published successfully!");
      router.push(`/companies/${id}/${branchId}`);
    } else {
      const err = await offerRes.text();
      console.error(err);
      alert("Failed to publish offer.");
    }
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Publish Offer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        {/* Template Select */}
        <div className="flex items-center gap-2">
          <label className="w-40">Offer Template:</label>
          <select name="offerTemplateId" onChange={handleFormChange} required className="flex-1 p-1 border rounded">
            <option value="">Select template</option>
            {templates.map((t) => (
              <option key={t.offerTemplateId} value={t.offerTemplateId}>
                {t.name}
              </option>
            ))}
          </select>
        </div>

        {/* Publication Dates */}
        <div className="flex items-center gap-2">
          <label className="w-40">Publication Start:</label>
          <input type="datetime-local" name="publicationStart" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>
        <div className="flex items-center gap-2">
          <label className="w-40">Publication End:</label>
          <input type="datetime-local" name="publicationEnd" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>

        {/* Employment */}
        <div className="flex items-center gap-2">
          <label className="w-40">Employment Length (months):</label>
          <input type="number" name="employmentLength" onChange={handleFormChange} className="flex-1 p-1 border rounded" />
        </div>

        {/* Website */}
        <div className="flex items-center gap-2">
          <label className="w-40">Website URL:</label>
          <input type="text" name="websiteUrl" onChange={handleFormChange} required className="flex-1 p-1 border rounded" />
        </div>

        {/* Toggle contract conditions */}
        <label>
          <input type="checkbox" checked={includeConditions} onChange={() => setIncludeConditions(!includeConditions)} /> Add contract conditions
        </label>

        {includeConditions && (
          <>
            <hr />
            <h3 className="text-lg font-medium">Contract Conditions</h3>

            {/* Salary + Hours */}
            <input type="number" name="salaryMin" placeholder="Min Salary" onChange={handleConditionChange} className="p-1 border rounded" />
            <input type="number" name="salaryMax" placeholder="Max Salary" onChange={handleConditionChange} className="p-1 border rounded" />
            <input type="number" name="hoursPerTerm" placeholder="Hours per Term" onChange={handleConditionChange} className="p-1 border rounded" />

            <label>
              <input type="checkbox" name="isNegotiable" onChange={handleConditionChange} /> Negotiable Salary
            </label>

            {/* Currency */}
            <label>Currency:</label>
            <select name="currencyId" onChange={handleConditionChange}>
              {parameters.filter(p => p.contractParameterType.name === "Currency").map(p => (
                <option key={p.contractParameterId} value={p.contractParameterId}>{p.name}</option>
              ))}
            </select>

            {/* Salary Term */}
            <label>Salary Term:</label>
            <select name="salaryTermId" onChange={handleConditionChange}>
              {parameters.filter(p => p.contractParameterType.name === "Salary Term").map(p => (
                <option key={p.contractParameterId} value={p.contractParameterId}>{p.name}</option>
              ))}
            </select>

            {/* Work Modes */}
            <label>Work Modes:</label>
            {parameters.filter(p => p.contractParameterType.name === "Work Mode").map((p) => (
              <label key={p.contractParameterId}>
                <input
                  type="checkbox"
                  onChange={(e) => handleArrayCheckbox("workModeIds", p.contractParameterId, e.target.checked)}
                />{" "}
                {p.name}
              </label>
            ))}

            {/* Employment Types */}
            <label>Employment Types:</label>
            {parameters.filter(p => p.contractParameterType.name === "Employment Type").map((p) => (
              <label key={p.contractParameterId}>
                <input
                  type="checkbox"
                  onChange={(e) => handleArrayCheckbox("employmentTypeIds", p.contractParameterId, e.target.checked)}
                />{" "}
                {p.name}
              </label>
            ))}
          </>
        )}

        <button type="submit" className="mt-4 bg-blue-600 text-white p-2 rounded">Publish Offer</button>
      </form>
    </div>
  );
};

export default PublishOffer;
