"use client";
import React from "react";

interface Skill {
  skillId: number;
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
}

interface SkillSelection {
  skillId: number;
  isRequired: boolean;
}

interface OfferTemplateFormProps {
  name: string;
  description: string;
  skills: Skill[];
  selectedSkills: SkillSelection[];
  onChange: (field: "name" | "description", value: string) => void;
  onSkillToggle: (skillId: number, isChecked: boolean) => void;
  onSkillRequiredToggle: (skillId: number, isRequired: boolean) => void;
  onSubmit: () => void;
  submitText?: string;
}

const OfferTemplateForm = ({
  name,
  description,
  skills,
  selectedSkills,
  onChange,
  onSkillToggle,
  onSkillRequiredToggle,
  onSubmit,
  submitText = "Submit",
}: OfferTemplateFormProps) => {
  const groupedSkills = skills.reduce((acc, skill) => {
    const group = skill.skillType.name;
    if (!acc[group]) acc[group] = [];
    acc[group].push(skill);
    return acc;
  }, {} as Record<string, Skill[]>);

  return (
    <div className="flex flex-col gap-4">
      <input
        type="text"
        name="name"
        value={name}
        placeholder="Template Name"
        onChange={(e) => onChange("name", e.target.value)}
        required
      />

      <textarea
        name="description"
        value={description}
        placeholder="Description"
        onChange={(e) => onChange("description", e.target.value)}
        required
      />

      <h3>Select Skills</h3>
      {Object.entries(groupedSkills).map(([type, group]) => (
        <div key={type}>
          <h4 className="text-lg font-semibold mb-2">{type}</h4>
          <div className="grid grid-cols-2 gap-2">
            {group.map((skill) => {
              const selected = selectedSkills.find((s) => s.skillId === skill.skillId);
              return (
                <div key={skill.skillId} className="flex items-center gap-4 mb-2">
                <label className="flex items-center gap-2">
                    <input
                    type="checkbox"
                    checked={!!selected}
                    onChange={(e) => onSkillToggle(skill.skillId, e.target.checked)}
                    />
                    {skill.name}
                </label>

                {selected && (
                    <label className="flex items-center gap-1 text-sm">
                    <input
                        type="checkbox"
                        checked={selected.isRequired}
                        onChange={(e) =>
                        onSkillRequiredToggle(skill.skillId, e.target.checked)
                        }
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
      <button type="button" onClick={onSubmit} className="bg-blue-600 text-white p-2 rounded">
        {submitText}
      </button>
    </div>
  );
};

export default OfferTemplateForm;
