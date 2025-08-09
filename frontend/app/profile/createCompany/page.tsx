"use client";
import React, { useState } from "react";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import { InnerSection, OuterContainer } from "@/app/components/layout/PageContainers";
import CancelButton from "@/app/components/buttons/CancelButton";

const CreateCompany = () => {
  const { data: session } = useSession();
  const [form, setForm] = useState({
    name: "",
    description: "",
    regon: "",
    nip: "",
    krs: "",
    websiteUrl: "https://",
  });

  const [errors, setErrors] = useState<string[]>([]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };


  const validateFields = () => {
    const errs: string[] = [];

    if (!/^\d{10}$/.test(form.regon)) {
      errs.push("REGON must contain exactly 10 digits.");
    }

    if (!/^(\d{9}|\d{14})$/.test(form.nip)) {
      errs.push("NIP must contain exactly 9 or 14 digits.");
    }

    if (!/^\d{10}$/.test(form.krs)) {
      errs.push("KRS must contain exactly 10 digits.");
    }

    if (!/^https:\/\/.+\..+/.test(form.websiteUrl)) {
      errs.push("Website URL must be a valid 'https://www.example.com' format.");
    }

    return errs;
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrors([]);

    if (!session?.user.token) {
      alert("You must be logged in to create a company.");
      return;
    }

    const validationErrors = validateFields();
    if (validationErrors.length > 0) {
      setErrors(validationErrors);
      return;
    }

    const res = await fetch("http://localhost:8080/api/CompanyUser/Companies", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(form),
    });

    if (res.ok) {
      const data = await res.json();
      alert(`Company created! ID: ${data.companyId}`);
      redirect("/profile");
    } else {
      const errorText = await res.text();
      alert("Failed to create company.\n" + errorText);
    }
  };

  return (
    <OuterContainer className="max-w-xl mx-auto">
      <h1 className="text-3xl font-bold mb-6 text-center">Create Company</h1>
      <InnerSection>
        {/* Błędy walidacji */}
        {errors.length > 0 && (
          <div className= "text-red-500">
            <ul>
              {errors.map((err, idx) => (
                <li key={idx}>• {err}</li>
              ))}
            </ul>
          </div>
        )}

        <form onSubmit={handleSubmit} className="flex flex-col gap-4 mt-4">

        <div className="flex items-center gap-2">
          <label htmlFor="name" className="w-32">Name:</label>
          <input type="text" id="name" name="name" onChange={handleChange} required className='global-field-style' />
        </div>

        <div className="flex items-center gap-2">
          <label htmlFor="description" className="w-32">Description:</label>
          <textarea id="description" name="description" rows={8} onChange={handleChange} required className='global-field-style' />
        </div>

        <div className="flex items-center gap-2">
          <label htmlFor="regon" className="w-32">REGON:</label>
          <input type="text" id="regon" name="regon" onChange={handleChange} required className='global-field-style' />
        </div>

        <div className="flex items-center gap-2">
          <label htmlFor="nip" className="w-32">NIP:</label>
          <input type="text" id="nip" name="nip" onChange={handleChange} required className='global-field-style' />
        </div>

        <div className="flex items-center gap-2">
          <label htmlFor="krs" className="w-32">KRS:</label>
          <input type="text" id="krs" name="krs" onChange={handleChange} required className='global-field-style' />
        </div>

        <div className="flex items-center gap-2">
          <label htmlFor="websiteUrl" className="w-32">Website:</label>
          <input type="text" id="websiteUrl" name="websiteUrl" onChange={handleChange} required className='global-field-style' />
        </div>

        <button type="submit" className="mt-4 p-2 bg-blue-600 text-white rounded">Create</button>
        </form>
        <br/>
        <CancelButton/>
      </InnerSection>
    </OuterContainer>
  );
};

export default CreateCompany;