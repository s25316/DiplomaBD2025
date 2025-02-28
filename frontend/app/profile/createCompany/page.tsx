"use client";
import React, { useState } from "react";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";

const CreateCompany = () => {
  const { data: session } = useSession();
  const [form, setForm] = useState({
    name: "",
    description: "",
    regon: "",
    nip: "",
    krs: "",
    websiteUrl: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!session?.user.token) {
      alert("You must be logged in to create a company.");
      return;
    }

    const res = await fetch("http://localhost:8080/api/User/Companies", {
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
      alert("Failed to create company.");
    }
  };

  return (
    <div>
      <h1>Create Company</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <input type="text" name="name" placeholder="Company Name" onChange={handleChange} required />
        <input type="text" name="description" placeholder="Description" onChange={handleChange} required />
        <input type="text" name="regon" placeholder="REGON" onChange={handleChange} required />
        <input type="text" name="nip" placeholder="NIP" onChange={handleChange} required />
        <input type="text" name="krs" placeholder="KRS" onChange={handleChange} required />
        <input type="text" name="websiteUrl" placeholder="Website URL" onChange={handleChange} required />
        <button type="submit">Create</button>
      </form>
    </div>
  );
};

export default CreateCompany;
