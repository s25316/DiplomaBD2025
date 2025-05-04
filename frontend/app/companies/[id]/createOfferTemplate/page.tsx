"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter, useParams } from "next/navigation";

type Skill = {
  skillId: number;
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
};

const CreateOfferTemplate = () => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id } = useParams(); // Pobieram companyId z URL-a

  const [skills, setSkills] = useState<Skill[]>([]);
  const [form, setForm] = useState({
    name: "",
    description: "",
    skills: [] as { skillId: number; isRequired: boolean }[],
  });

  // Pobieranie listy umiejętności
  useEffect(() => {
    const fetchSkills = async () => {
      if (!session?.user?.token) return;
      try {
        const res = await fetch("http://localhost:8080/api/Dictionaries/skills", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        const data = await res.json();
        setSkills(data);
      } catch (error) {
        console.error("Error fetching skills:", error);
      }
    };
    fetchSkills();
  }, [session]);

  // Aktualizacja formularza
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  // Aktualizacja umiejętności
  const handleSkillChange = (skillId: number, isChecked: boolean) => {
    setForm((prev) => {
      const updatedSkills = isChecked
        ? [...prev.skills, { skillId, isRequired: true }]
        : prev.skills.filter((s) => s.skillId !== skillId);
      return { ...prev, skills: updatedSkills };
    });
  };

  // Wysyłanie formularza
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!session?.user?.token) {
      alert("You must be logged in.");
      return;
    }

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify([form]), // API wymaga tablicy
    });

    if (res.ok) {
      alert("Offer template created!");
      router.push(`/companies/${id}`); // Przekierowanie po sukcesie
    } else {
      alert("Failed to create offer template.");
    }
  };

  return (
    <div>
      <h1>Create Offer Template</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <input type="text" name="name" placeholder="Template Name" onChange={handleChange} required />
        <textarea name="description" placeholder="Description" onChange={handleChange} required />
        
        <h3>Select Skills</h3>
        {Object.entries(
          skills.reduce((acc, skill) => {
            const type = skill.skillType.name;
            if (!acc[type]) acc[type] = [];
            acc[type].push(skill);
            return acc;
          }, {} as Record<string, Skill[]>)
        ).map(([typeName, groupedSkills]) => (
          <div key={typeName} className="mb-4">
            <h4 className="text-lg font-semibold mb-2">{typeName}</h4>
            <div className="grid grid-cols-2 gap-2">
            {groupedSkills.map((skill) => {
              const selected = form.skills.find((s) => s.skillId === skill.skillId);
              return (
                <div key={skill.skillId} className="flex items-center gap-4 mb-2">
                  <label className="flex items-center gap-2">
                    <input
                      type="checkbox"
                      checked={!!selected}
                      onChange={(e) => handleSkillChange(skill.skillId, e.target.checked)}
                    />
                    {skill.name}
                  </label>
                  {selected && (
                    <label className="flex items-center gap-1 text-sm">
                      <input
                        type="checkbox"
                        checked={selected.isRequired}
                        onChange={(e) => {
                          setForm((prev) => ({
                            ...prev,
                            skills: prev.skills.map((s) =>
                              s.skillId === skill.skillId
                                ? { ...s, isRequired: e.target.checked }
                                : s
                            ),
                          }));
                        }}
                      />
                      Required
                    </label>
                  )}
                </div>
              );
            })}
            </div>
          </div>
        ))}


        <button type="submit">Create</button>
      </form>
    </div>
  );
};

export default CreateOfferTemplate;