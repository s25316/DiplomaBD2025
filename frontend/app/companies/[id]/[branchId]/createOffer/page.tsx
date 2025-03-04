"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter, useParams } from "next/navigation";

const CreateOffer = () => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id, branchId } = useParams(); // Pobieramy companyId i branchId z URL-a

  const offerTemplateId = "4036595E-6BAB-4FC8-9266-C2FD7D915191"; // Wpisane na sztywno

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

  // Pobieranie danych z API (tryby pracy i typy zatrudnienia)
  useEffect(() => {
    const fetchData = async () => {
      if (!session?.user?.token) return;
      try {
        const resWorkModes = await fetch("http://localhost:8080/api/Dictionaries/offer/workModes", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        const resEmploymentTypes = await fetch("http://localhost:8080/api/Dictionaries/offer/employmentTypes", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });

        setWorkModes(await resWorkModes.json());
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

    const res = await fetch(
      `http://localhost:8080/api/User/companies/${id}/offers/templates/${offerTemplateId}/offers`,
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
      router.push(`/companies/${id}/branches/${branchId}`); // Przekierowanie po sukcesie
    } else {
      alert("Failed to create offer.");
    }
  };

  return (
    <div>
      <h1>Create Offer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
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
        <input type="text" name="websiteUrl" onChange={handleChange} required />

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

        <button type="submit">Create Offer</button>
      </form>
    </div>
  );
};

export default CreateOffer;
