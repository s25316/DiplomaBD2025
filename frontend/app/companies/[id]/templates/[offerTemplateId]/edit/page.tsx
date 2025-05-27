"use client";
import React, { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useSession } from "next-auth/react";
import OfferTemplateForm from "@/app/components/forms/OfferTemplateForm";

export default function EditOfferTemplate() {
  const { id, offerTemplateId } = useParams();
  const router = useRouter();
  const { data: session } = useSession();

  const [skills, setSkills] = useState([]);
  const [form, setForm] = useState({
    name: "",
    description: "",
    skills: [] as { skillId: number; isRequired: boolean }[],
  });

  useEffect(() => {
    if (!session?.user?.token) return;

    const headers = {
          Authorization: `Bearer ${session.user.token}`,
        };

    const fetchAll = async () => {
      const [skillsRes, templateRes] = await Promise.all([
        fetch(`http://localhost:8080/api/Dictionaries/skills`, {headers} ),
        fetch(`http://localhost:8080/api/CompanyUser/offerTemplates/${offerTemplateId}`, {headers}),
      ]);

      const skillsData = await skillsRes.json();
      setSkills(skillsData);

      const templateData = await templateRes.json();
      const offerTemplate = templateData.items[0]?.offerTemplate;

      if (offerTemplate) {
        setForm({
          name: offerTemplate.name,
          description: offerTemplate.description,
          skills: offerTemplate.skills.map((s: any) => ({
            skillId: s.skill.skillId,
            isRequired: s.isRequired,
          })),
        });
      }
    };

    fetchAll();
  }, [session, offerTemplateId]);

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

  const handleSubmit = async () => {
    if (!session?.user?.token) return;

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/offerTemplates/${offerTemplateId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(form),
    });

    if (res.ok) {
      alert("Template updated!");
      router.push(`/companies/${id}`);
    } else {
      const msg = await res.text();
      console.error(msg);
      alert("Update failed");
    }
  };

  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-semibold mb-4">Edit Offer Template</h1>
      <OfferTemplateForm
        name={form.name}
        description={form.description}
        skills={skills}
        selectedSkills={form.skills}
        onChange={onChange}
        onSkillToggle={onSkillToggle}
        onSkillRequiredToggle={onSkillRequiredToggle}
        onSubmit={handleSubmit}
        submitText="Save"
      />
    </div>
  );
}
