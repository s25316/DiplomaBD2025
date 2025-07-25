'use client';
import React from 'react';
import CancelButton from '../buttons/CancelButton';

// This interface defines the structure of a single skill object as received from the API
interface Skill {
  skillId: number; // Unique identifier for the skill (from API)
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
}

// This interface defines how a selected skill is stored in the form's state
interface SkillSelection {
  skillId: number; // References the skillId from the Skill interface
  isRequired: boolean;
}

// This interface defines the props for the OfferTemplateForm component
interface OfferTemplateFormProps {
  name: string;
  description: string;
  skills: Skill[]; // Array of all available skills passed down from parent
  selectedSkills: SkillSelection[]; // Array of skills currently selected in the form
  onChange: (field: 'name' | 'description', value: string) => void;
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
  submitText = 'Submit',
}: OfferTemplateFormProps) => {

  // Group skills by their skillType for display purposes
  // 'skill' in this reduce function is of type 'Skill' from the local interface
  const groupedSkills = skills.reduce((acc, skill) => {
    const group = skill.skillType.name;
    if (!acc[group]) acc[group] = [];
    acc[group].push(skill);
    return acc;
  }, {} as Record<string, Skill[]>); // Ensure the accumulator is correctly typed as Record<string, Skill[]>

  return (
    <div className='flex flex-col gap-4 p-4 border rounded-lg'>
      <h3 className='text-xl font-semibold text-gray-800 mb-2'>OfferTemplate Details</h3>
      <label htmlFor='templateName' className='font-semibold text-gray-700 dark:text-gray-300'>Template Name</label>
      <input
        id='templateName'
        type='text'
        name='name'
        value={name}
        placeholder="e.g., Software Engineer Intern Template"
        onChange={(e) => onChange('name', e.target.value)}
        required
        className='border border-gray-300 rounded-md p-1'
      />

      <label htmlFor='templateDescription' className='font-semibold text-gray-700 dark:text-gray-300'>Description</label>
      <textarea
        id='templateDescription'
        name='description'
        value={description}
        placeholder="Detailed description of the offer template..."
        onChange={(e) => onChange('description', e.target.value)}
        required
        rows={4}
        className='border border-gray-300 rounded-md p-1'
      />

      <h3 className='text-xl font-semibold text-gray-800 mt-4 mb-2 dark:text-gray-300'>Select Skills</h3>
      {Object.entries(groupedSkills).length > 0 ? (
        Object.entries(groupedSkills).map(([type, group]) => (
          <div key={type} className='mb-4'> {/* Key for the skill type group div */}
            <fieldset className='border border-gray-300 p-3 rounded-md'>
            <legend className='text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2'>{type}</legend>
            <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3'>
              {group.map((skill: Skill) => { // 'skill' here is of type 'Skill'
                // Find if the current 'skill' is in the 'selectedSkills' array
                // We compare skill.skillId (from the full skill object) with s.skillId (from the selectedSkills array item)
                const selected = selectedSkills.find((s: SkillSelection) => s.skillId === skill.skillId);
                
                return (
                  // Use skill.skillId as the unique key for each individual skill item.
                  // This is CRUCIAL for React's reconciliation to correctly identify and update elements.
                  <div key={skill.skillId} className='flex items-center justify-between border-gray-100'>
                    <label className='flex items-center gap-2 text-gray-700 dark:text-gray-300 cursor-pointer'>
                      <input
                        type='checkbox'
                        checked={!!selected} // '!!selected' converts 'SkillSelection | undefined' to 'boolean'
                        // When checkbox state changes, call onSkillToggle with the specific skill.skillId
                        onChange={(e) => onSkillToggle(skill.skillId, e.target.checked)}
                        className='form-checkbox h-5 w-5 text-blue-600 rounded-md'
                      />
                      <span className='font-medium'>{skill.name}</span>
                    </label>

                    {selected && ( // Only show "Required" checkbox if the skill is currently selected
                      <label className='flex items-center gap-1 text-sm text-gray-600 dark:text-gray-300 cursor-pointer'>
                        <input
                          type='checkbox'
                          checked={selected.isRequired} // Use the 'isRequired' status from the 'selected' object
                          // When 'Required' checkbox state changes, call onSkillRequiredToggle with skill.skillId
                          onChange={(e) => onSkillRequiredToggle(skill.skillId, e.target.checked)}
                          className='form-checkbox h-4 w-4 text-green-600 rounded-md'
                        />
                        Required
                      </label>
                    )}
                  </div>
                );
              })}
            </div>
            </fieldset>
          </div>
        ))
      ) : (
        <p className='text-gray-500 italic'>No skills available to select.</p>
      )}


      <CancelButton/>
      <button
        type='button'
        onClick={onSubmit}
        className='bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4'
      >
        {submitText}
      </button>
    </div>
  );
};

export default OfferTemplateForm;
