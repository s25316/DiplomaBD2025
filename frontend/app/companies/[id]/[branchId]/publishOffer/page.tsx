"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter, useParams } from "next/navigation";

const PublishOffer = () => {
  const { data: session } = useSession({
      required: true,
      onUnauthenticated() {
        const router = useRouter()
        router.back()
      },
    });
  const router = useRouter();
  const { id, branchId } = useParams(); // Pobieram companyId i branchId z URL-a

  // Stan dla dostępnych template
  const [templates, setTemplates] = useState<{ offerTemplateId: string; name: string }[]>([]);
  const [selectedTemplate, setSelectedTemplate] = useState("");

  const [workModes, setWorkModes] = useState<{ workModeId: number; name: string }[]>([]);
  const [employmentTypes, setEmploymentTypes] = useState<{ employmentTypeId: number; name: string }[]>([]);

  const [form, setForm] = useState({
    branchId: branchId as string,
    publicationStart: "",
    publicationEnd: "",
    workBeginDate: "",
    workEndDate: "",
    salaryRangeMin: 0,
    salaryRangeMax: 0,
    salaryTermId: 1, // Upewnij się, że to prawidłowe ID
    currencyId: 1, // Upewnij się, że to prawidłowe ID
    isNegotiated: false,
    websiteUrl: "",
    employmentTypes: [] as number[],
    workModes: [] as number[],
  });

  // Pobieranie danych z API
  useEffect(() => {
    if (!session?.user?.token) return;

    const fetchData = async () => {
      try {
        // Pobranie listy offer templates
        const resTemplates = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        setTemplates(await resTemplates.json().then((data) => data.offerTemplates));

        // Pobranie trybów pracy
        const resWorkModes = await fetch("http://localhost:8080/api/Dictionaries/offer/workModes", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        setWorkModes(await resWorkModes.json());

        // Pobranie typów zatrudnienia
        const resEmploymentTypes = await fetch("http://localhost:8080/api/Dictionaries/offer/employmentTypes", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        setEmploymentTypes(await resEmploymentTypes.json());
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [session]);

  // Aktualizacja formularza
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  // Aktualizacja checkboxów
  const handleCheckboxChange = (type: "workModes" | "employmentTypes", id: number, isChecked: boolean) => {
    setForm((prev) => {
      const updated = isChecked ? [...prev[type], id] : prev[type].filter((item) => item !== id);
      return { ...prev, [type]: updated };
    });
  };

  // Wysyłanie formularza
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!session?.user?.token) {
      alert("You must be logged in.");
      return;
    }

    if (!selectedTemplate) {
      alert("Please select an offer template.");
      return;
    }

    const res = await fetch(
      `http://localhost:8080/api/User/companies/${id}/offers/templates/${selectedTemplate}/offers`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${session.user.token}`,
        },
        body: JSON.stringify([form]), // API wymaga tablicy
      }
    );

    if (res.ok) {
      alert("Offer created successfully!");
      router.push(`/companies/${id}/${branchId}`); // Przekierowanie po sukcesie
    } else {
      alert("Failed to create offer.");
    }
  };

  return (
    <div>
      <h1>Publish Offer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        {/* Wybór template */}
        <label>Offer Template:</label>
        <select value={selectedTemplate} onChange={(e) => setSelectedTemplate(e.target.value)} required>
          <option value="">Select a template</option>
          {templates.map((template) => (
            <option key={template.offerTemplateId} value={template.offerTemplateId}>
              {template.name}
            </option>
          ))}
        </select>

        <label>Publication Start:</label>
        <input type="datetime-local" name="publicationStart" onChange={handleChange} required />

        <label>Publication End:</label>
        <input type="datetime-local" name="publicationEnd" onChange={handleChange} required />

        <label>Work Begin Date:</label>
        <input type="datetime-local" name="workBeginDate" onChange={handleChange} required />

        <label>Work End Date:</label>
        <input type="datetime-local" name="workEndDate" onChange={handleChange} required />


        <label>Salary Min:</label>
        <input type="number" name="salaryRangeMin" onChange={handleChange} required />

        <label>Salary Max:</label>
        <input type="number" name="salaryRangeMax" onChange={handleChange} required />

        <label>Negotiable Salary:</label>
        <input
          type="checkbox"
          checked={form.isNegotiated}
          onChange={() => setForm({ ...form, isNegotiated: !form.isNegotiated })}
        />

        <label>Website URL:</label>
        <input type="text" name="websiteUrl"  placeholder="https://www.strona.pl" onChange={handleChange} required />

        <h3>Work Modes</h3>
        {workModes.map((mode) => (
          <label key={mode.workModeId}>
            <input
              type="checkbox"
              value={mode.workModeId}
              onChange={(e) => handleCheckboxChange("workModes", mode.workModeId, e.target.checked)}
            />
            {mode.name}
          </label>
        ))}

        <h3>Employment Types</h3>
        {employmentTypes.map((type) => (
          <label key={type.employmentTypeId}>
            <input
              type="checkbox"
              value={type.employmentTypeId}
              onChange={(e) => handleCheckboxChange("employmentTypes", type.employmentTypeId, e.target.checked)}
            />
            {type.name}
          </label>
        ))}
        <button type="submit">Publish Offer</button>
      </form>
    </div>
  );
};

export default PublishOffer;
