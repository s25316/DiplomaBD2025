"use client";
import React, { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useSession } from "next-auth/react";

interface Skill {
  skillId: number;
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
}

const TemplateDetails = () => {
  const { id, offerTemplateId } = useParams();
  const router = useRouter();
  const { data: session } = useSession();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [skills, setSkills] = useState<Skill[]>([]);
  const [selectedSkills, setSelectedSkills] = useState<{ skillId: number; isRequired: boolean }[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      if (!session?.user?.token) return;

      try {
        // Get all skills
        const skillsRes = await fetch("http://localhost:8080/api/Dictionaries/skills", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });
        const skillsData = await skillsRes.json();
        setSkills(skillsData);

        // Get offer template
        const templateRes = await fetch(
          `http://localhost:8080/api/CompanyUser/offerTemplates/${offerTemplateId}`,
          {
            headers: { Authorization: `Bearer ${session.user.token}` },
            cache: "no-store",
          }
        );
        const templateJson = await templateRes.json();
        const offerTemplate = templateJson.items[0]?.offerTemplate;

        if (offerTemplate) {
          setName(offerTemplate.name);
          setDescription(offerTemplate.description);
          const mappedSkills = offerTemplate.skills.map((s: any) => ({
            skillId: s.skill.skillId,
            isRequired: s.isRequired,
          }));
          setSelectedSkills(mappedSkills);
        }
      } catch (error) {
        console.error("Error loading data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [session]);

  const handleSkillToggle = (skillId: number, isChecked: boolean) => {
    setSelectedSkills((prev) =>
      isChecked
        ? [...prev, { skillId, isRequired: true }]
        : prev.filter((s) => s.skillId !== skillId)
    );
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    const payload = [{
      name,
      description,
      skills: selectedSkills,
    }];

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      alert("Template updated!");
      router.push(`/companies/${id}/templates/${offerTemplateId}`);
    } else {
      const msg = await res.text();
      console.error(msg);
      alert("Update failed");
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-semibold mb-4">Edit Template</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Template Name"
          className="border p-2"
          required
        />

        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Description"
          className="border p-2"
          required
        />

      <h3>Select Skills</h3>
      {Object.entries(
        skills.reduce((acc, skill) => {
          const typeName = skill.skillType.name;
          if (!acc[typeName]) acc[typeName] = [];
          acc[typeName].push(skill);
          return acc;
        }, {} as Record<string, typeof skills>)
      ).map(([typeName, skillsInGroup]) => (
        <div key={typeName} className="mb-4">
          <h4 className="text-lg font-semibold mb-2">{typeName}</h4>
          <div className="grid grid-cols-2 gap-2">
            {skillsInGroup.map((skill) => (
              <label key={skill.skillId} className="flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={selectedSkills.some((s) => s.skillId === skill.skillId)}
                  onChange={(e) => handleSkillToggle(skill.skillId, e.target.checked)}
                />
                {skill.name}
              </label>
            ))}
          </div>
        </div>
      ))}


        <button type="submit" className="bg-blue-600 text-white p-2 rounded">
          Save Changes
        </button>
      </form>
    </div>
  );
};

export default TemplateDetails;
