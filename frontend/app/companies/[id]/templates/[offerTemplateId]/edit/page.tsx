'use client';
import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
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

// This interface defines the structure of a skill within an offer template as returned by the API
interface SkillInTemplateApi { // Naming convention 'Api' to denote it's from API response
    skill: {
        skillId: number;
        name: string;
        skillType: {
            skillTypeId: number;
            name: string;
        };
    };
    isRequired: boolean;
}

// This interface defines the structure of the offer template data nested in the API response
interface OfferTemplateDataApi { // Naming convention 'Api' to show it's from API response
    offerTemplateId: string;
    name: string;
    description: string;
    skills: SkillInTemplateApi[]; 
}

// API response item for a single Offer Template (if nested like items[0]?.offerTemplate)
interface OfferTemplateApiItem {
  offerTemplate: OfferTemplateDataApi;
  company: Company;
}

// This interface defines the form's state structure for editing an offer template
interface EditTemplateFormState {
  name: string;
  description: string;
  skills: SkillSelection[]; 
}



export default function EditOfferTemplate() {

  const { id, offerTemplateId } = useParams() as { id: string; offerTemplateId: string };
  const router = useRouter();
  const { data: session } = useSession();

  const [skills, setSkills] = useState<SkillWithRequired[]>([]); 

  const [form, setForm] = useState<EditTemplateFormState | null>(null);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const showCustomAlert = (message: string) => {
    console.log("ALERT:", message);
    alert(message);
  };

  useEffect(() => {
    if (!session?.user?.token || !offerTemplateId) return;

    const headers = { Authorization: `Bearer ${session.user.token}` };

    const fetchAll = async () => {
      try {
        const [skillsRes, templateRes] = await Promise.all([
          fetch(`${backUrl}/api/Dictionaries/skills`, { headers }),
          fetch(`${backUrl}/api/CompanyUser/offerTemplates/${offerTemplateId}`, { headers }),
        ]);

        if (!skillsRes.ok) {
          const errorText = await skillsRes.text();
          throw new Error(`Failed to fetch skills: ${errorText}`);
        }
        const skillsData: SkillWithRequired[] = await skillsRes.json();

        // Ensure unique skills based on skillId for available skills list
        const uniqueSkills = Array.from(new Map(skillsData.map(item => [item.skillId, item])).values());
        if (uniqueSkills.length !== skillsData.length) {
            console.warn("WARNING: Duplicate skill IDs found in API response for Edit. Deduplicated list used.");
        }
        // --- END DEDUPLICATION ---

        uniqueSkills.sort((a, b) => a.name.localeCompare(b.name));
        
        setSkills(uniqueSkills);
        console.log("Fetched and set unique skills (Edit):", uniqueSkills); // Debug log

        if (!templateRes.ok) {
          const errorText = await templateRes.text();
          throw new Error(`Failed to fetch offer template details: ${errorText}`);
        }
        const templateApiData: { items: OfferTemplateApiItem[] } = await templateRes.json();
        const offerTemplate = templateApiData.items?.[0]?.offerTemplate;

        if (!offerTemplate) {
          showCustomAlert("Offer template not found.");
          router.replace(`/companies/${id}`); // Redirect if template not found
          return;
        }

        // Map API's SkillInTemplateApi structure to our local SkillSelection form state structure
        // This is where we extract the skillId from the nested 'skill' object in the API response
        const initialSelectedSkills: SkillSelection[] = offerTemplate.skills.map((s: SkillInTemplateApi) => ({
            skillId: s.skill.skillId, // Correctly use s.skill.skillId for selected skills
            isRequired: s.isRequired,
        }));

        setForm({
          name: offerTemplate.name,
          description: offerTemplate.description,
          skills: initialSelectedSkills,
        });
        console.log("Fetched and set form data (Edit):", { ...offerTemplate, skills: initialSelectedSkills }); // Debug log

      } catch (error) {
        console.error("Error fetching data:", error);
        if(error instanceof Error)
        showCustomAlert(`Error loading offer template data: ${error.message}`);
      }
    };

    fetchAll();
  }, [session, offerTemplateId, id, router]); // Added 'id' to dependencies for router.push

  // Function to handle changes in name or description fields
  const onChange = (field: 'name' | 'description', value: string) =>
    setForm((prev) => {
        console.log(`Form change (Edit) - Field: ${field}, Value: ${value}`); // Debug log
        return prev ? { ...prev, [field]: value } : null; // Ensure prev is not null
    });

  // Function to toggle skill selection
  const onSkillToggle = (skillId: number, isChecked: boolean) =>
    setForm((prev) => {
      if (!prev) return null; // If form is null, return null (shouldn't happen if loading state is handled)
      const updatedSkills = isChecked
        ? [...prev.skills, { skillId, isRequired: true }]
        : prev.skills.filter((s) => s.skillId !== skillId);
      console.log('Updated form.skills after toggle (Edit):', updatedSkills); // Debug log
      return { ...prev, skills: updatedSkills };
    });

  // Function to toggle a selected skill's 'isRequired' status
  const onSkillRequiredToggle = (skillId: number, isRequired: boolean) =>
    setForm((prev) => {
      if (!prev) return null; // If form is null, return null
      const updatedSkills = prev.skills.map((s) =>
        s.skillId === skillId ? { ...s, isRequired } : s
      );
      console.log('Updated form.skills after required toggle (Edit):', updatedSkills); // Debug log
      return { ...prev, skills: updatedSkills };
    });

  const handleSubmit = async () => {
    if (!session?.user?.token || !form) { // Check if form data is available
      showCustomAlert("Authentication required or form data missing.");
      return;
    }

    try {
      // Prepare payload: ensure skills are in the correct format (SkillSelection) for API
      const payload = {
        name: form.name,
        description: form.description,
        skills: form.skills.map(s => ({
          skillId: s.skillId, // Use skillId from SkillSelection (which is our form state)
          isRequired: s.isRequired,
        }))
      };
      console.log("Submitting form data (Edit):", payload); // Debug log

      const res = await fetch(`${backUrl}/api/CompanyUser/companies/offerTemplates/${offerTemplateId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${session.user.token}`,
        },
        body: JSON.stringify(payload), // Send the prepared payload
      });

      if (res.ok) {
        showCustomAlert("Offer Template updated successfully!");
        router.replace(`/companies/${id}/templates/${offerTemplateId}`); // Redirect to company details page after update
      } else {
        const errorText = await res.text();
        console.error("Failed to update offer template:", errorText);
        showCustomAlert(`Failed to update template: ${errorText}`);
      }
    } catch (error) {
      console.error("Error submitting offer template update:", error);
      if(error instanceof Error)
      showCustomAlert(`An error occurred while updating template: ${error.message}`);
    }
  };

  // Render loading state if session token is not available or form data hasn't loaded yet
  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  // Show loading if 'form' is null (initial state) or 'skills' are not yet fetched
  if (!form || skills.length === 0) return <div className="text-center py-4 text-blue-600">Loading template data...</div>;

  return (
    <OuterContainer className="max-w-xl mx-auto p-6 mt-8 font-inter">
      <h1 className="text-3xl font-bold mb-6 text-center">Edit Offer Template</h1>
      <OfferTemplateForm
        name={form.name}
        description={form.description}
        skills={skills}
        selectedSkills={form.skills}
        onChange={onChange}
        onSkillToggle={onSkillToggle}
        onSkillRequiredToggle={onSkillRequiredToggle}
        onSubmit={handleSubmit}
        submitText="Save Changes"
      />
    </OuterContainer>
  );
}
