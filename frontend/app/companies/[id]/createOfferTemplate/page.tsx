'use client';
import React, { useEffect, useState } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { useSession } from 'next-auth/react';
import OfferTemplateForm from '@/app/components/forms/OfferTemplateForm';
import { OuterContainer } from '@/app/components/layout/PageContainers';
import { SkillWithRequired } from '@/app/components/forms/OfferForm';

// This interface defines the structure of a single skill object as received from the API
// interface Skill {
//   skillId: number; // Unique identifier for the skill (from API)
//   name: string;
//   skillType: {
//     skillTypeId: number;
//     name: string;
//   };
// }

// This interface defines how a selected skill is stored in the form's state
interface SkillSelection {
  skillId: number; // References the skillId from the Skill interface
  isRequired: boolean;
}

// This interface defines the form's state structure for a new offer template
interface NewTemplateFormState {
  name: string;
  description: string;
  skills: SkillSelection[]; // Array of selected skills for the form
}



export default function CreateOfferTemplate() {
  const { id } = useParams() as { id: string }; // Type 'id' as string
  const router = useRouter();
  const { data: session } = useSession();

  // type the 'skills' state as an array of Skill
  const [skills, setSkills] = useState<SkillWithRequired[]>([]);
  // type the 'form' state using NewTemplateFormState
  const [form, setForm] = useState<NewTemplateFormState>({
    name: '',
    description: '',
    skills: [], // Initialize with an empty array of SkillSelection
  });

  // Custom alert function
  const showCustomAlert = (message: string) => {
    console.log("ALERT:", message);
    alert(message); // Temporary: for Canvas environment demonstration
  };

  useEffect(() => {
    const fetchSkills = async () => {
      if (!session?.user?.token) return;

      try {
        const res = await fetch("http://localhost:8080/api/Dictionaries/skills", {
          headers: { Authorization: `Bearer ${session.user.token}` },
        });

        if (!res.ok) {
          const errorText = await res.text();
          throw new Error(`Failed to fetch skills: ${errorText}`);
        }

        const data: SkillWithRequired[] = await res.json(); // Explicitly type the API response
        
        // Ensure unique skills based on skillId. This helps prevent "Each child in a list should have a unique 'key' prop"
        // if the API happens to return duplicate skillId's, and also ensures `selectedSkills.find` works correctly.
        const uniqueSkills = Array.from(new Map(data.map(item => [item.skillId, item])).values());

        if (uniqueSkills.length !== data.length) {
            console.warn("WARNING: Duplicate skill IDs found in API response. Deduplicated list used.");
        }
        // --- END DEDUPLICATION ---

        setSkills(uniqueSkills);
        console.log("Fetched and set unique skills:", uniqueSkills); // Debug log
      } catch (error) {
        console.error("Error fetching skills:", error);
        if(error instanceof Error)
        showCustomAlert(`Failed to load skills: ${error.message}`);
      }
    };
    fetchSkills();
  }, [session]); // Depend on session to refetch when it changes

  // Function to handle changes in name or description fields
  const onChange = (field: 'name' | 'description', value: string) => {
    console.log(`Form change - Field: ${field}, Value: ${value}`); // Debug log
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  // Function to toggle skill selection
  const onSkillToggle = (skillId: number, isChecked: boolean) => {
    console.log(`onSkillToggle - Skill ID: ${skillId}, isChecked: ${isChecked}`); // Debug log
    setForm((prev) => {
      const updatedSkills = isChecked
        ? [...prev.skills, { skillId, isRequired: true }] // Add skillId and default isRequired to true
        : prev.skills.filter((s) => s.skillId !== skillId); // Remove skill by skillId
      console.log('Updated form.skills after toggle:', updatedSkills); // Debug log
      return { ...prev, skills: updatedSkills };
    });
  };

  // Function to toggle a selected skill's 'isRequired' status
  const onSkillRequiredToggle = (skillId: number, isRequired: boolean) => {
    console.log(`onSkillRequiredToggle - Skill ID: ${skillId}, isRequired: ${isRequired}`); // Debug log
    setForm((prev) => {
      const updatedSkills = prev.skills.map((s) =>
        s.skillId === skillId ? { ...s, isRequired } : s // Update isRequired for the matching skillId
      );
      console.log('Updated form.skills after required toggle:', updatedSkills); // Debug log
      return { ...prev, skills: updatedSkills };
    });
  };

  const handleSubmit = async () => {
    if (!session?.user?.token) {
      showCustomAlert("Authentication required to create template.");
      return;
    }

    try {
      console.log("Submitting form data:", form); // Debug log
      const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${session.user.token}`,
        },
        body: JSON.stringify([form]), // API expects an array containing the form data
      });

      if (res.ok) {
        showCustomAlert("Offer Template created successfully!");
        router.push(`/companies/${id}`);
      } else {
        const errorText = await res.text();
        console.error("Failed to create offer template:", errorText);
        showCustomAlert(`Failed to create offer template: ${errorText}`);
      }
    } catch (error) {
      console.error("Error submitting offer template:", error);
      if(error instanceof Error)
      showCustomAlert(`An error occurred while creating template: ${error.message}`);
    }
  };

  // Render loading state if session token is not available or skills are not loaded
  if (!session?.user?.token) return <div className='text-center py-4 text-red-600'>Unauthorized. Please log in.</div>;
  // Show loading for skills if they are not yet fetched, but session token exists
  if (skills.length === 0 && session.user.token) return <div className='text-center py-4 text-blue-600'>Loading skills...</div>;


  return (
    <OuterContainer className='max-w-xl mx-auto p-6 font-inter'>
      <h1 className='text-3xl font-bold mb-6 text-center'>Create Offer Template</h1>
      <OfferTemplateForm
        name={form.name}
        description={form.description}
        skills={skills} // All available skills
        selectedSkills={form.skills} // Currently selected skills
        onChange={onChange}
        onSkillToggle={onSkillToggle}
        onSkillRequiredToggle={onSkillRequiredToggle}
        onSubmit={handleSubmit}
        submitText="Create Template"
      />
    </OuterContainer>
  );
}
