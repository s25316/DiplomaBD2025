"use client";
import React, { useEffect, useState } from "react";
import { useRouter, useParams } from "next/navigation";
import { useSession } from "next-auth/react";
import OfferTemplateForm from "@/app/components/forms/OfferTemplateForm";

export default function CreateOfferTemplate() {
  const { id } = useParams();
  const router = useRouter();
  const { data: session } = useSession();

  const [skills, setSkills] = useState([]);
  const [form, setForm] = useState({
    name: "",
    description: "",
    skills: [] as { skillId: number; isRequired: boolean }[],
  });

  useEffect(() => {
    const fetchSkills = async () => {
      if (!session?.user?.token) return;
      const res = await fetch("http://localhost:8080/api/Dictionaries/skills", {
        headers: { Authorization: `Bearer ${session.user.token}` },
      });
      const data = await res.json();
      setSkills(data);
    };
    fetchSkills();
  }, [session]);

  const onChange = (field: "name" | "description", value: string) =>
    setForm((prev) => ({ ...prev, [field]: value }));

  const onSkillToggle = (skillId: number, isChecked: boolean) =>
    setForm((prev) => ({
      ...prev,
      skills: isChecked
        ? [...prev.skills, { skillId, isRequired: true }]
        : prev.skills.filter((s) => s.skillId !== skillId),
    }));

  const onSkillRequiredToggle = (skillId: number, isRequired: boolean) =>
    setForm((prev) => ({
      ...prev,
      skills: prev.skills.map((s) =>
        s.skillId === skillId ? { ...s, isRequired } : s
      ),
    }));

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify([form]),
    });

    if (res.ok) {
      alert("Template created!");
      router.push(`/companies/${id}`);
    } else {
      alert("Failed to create.");
    }
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-semibold mb-4">Create Offer Template</h1>
      <OfferTemplateForm
        name={form.name}
        description={form.description}
        skills={skills}
        selectedSkills={form.skills}
        onChange={onChange}
        onSkillToggle={onSkillToggle}
        onSkillRequiredToggle={onSkillRequiredToggle}
        onSubmit={handleSubmit}
        submitText="Create"
      />
    </div>
  );
}
