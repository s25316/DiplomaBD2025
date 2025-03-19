"use client";
import React, { useEffect, useState } from "react";
import { useRouter, useParams } from "next/navigation";
import { useSession } from "next-auth/react";

interface Skill {
  skillId: number;
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
  isRequired: boolean;
}

interface OfferTemplate {
  offerTemplateId: string;
  name: string;
  description: string;
  created: string;
  skills: Skill[];
}

const TemplateDetails = () => {
  const { id, offerTemplateId } = useParams(); // Pobiera companyId i offerTemplateId z URL-a
  const { data: session } = useSession();
  const router = useRouter();
  const [template, setTemplate] = useState<OfferTemplate | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchTemplateDetails = async () => {
      if (!session?.user?.token) return;

      try {
        const res = await fetch(`http://localhost:8080/api/CompanyUser/offerTemplates/${offerTemplateId}`, {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
          },
        });

        if (res.ok) {
          const data = await res.json();
          const templateData = data.items[0]?.offerTemplate;

          if (templateData) {
            setTemplate({
              offerTemplateId: templateData.offerTemplateId,
              name: templateData.name,
              description: templateData.description,
              created: templateData.created,
              skills: templateData.skills.map((skillData: any) => ({
                skillId: skillData.skill.skillId,
                name: skillData.skill.name,
                skillType: skillData.skill.skillType,
                isRequired: skillData.isRequired,
              })),
            });
          }
        }
      } catch (error) {
        console.error("Error fetching template details:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchTemplateDetails();
  }, [session, offerTemplateId]);

  if (loading) return <p>Loading template details...</p>;
  if (!template) return <p>Error loading template.</p>;

  return (
    <div>
      <h1>Template Details</h1>
      <p><b>Name:</b> {template.name}</p>
      <p><b>Description:</b> {template.description}</p>
      <p><b>Created:</b> {new Date(template.created).toLocaleDateString()}</p>

      <h2>Skills Required</h2>
      {template.skills.length > 0 ? (
        <ul>
          {template.skills.map((skill) => (
            <li key={skill.skillId}>
              <b>{skill.name}</b> - {skill.skillType.name} {skill.isRequired ? "(Required)" : "(Optional)"}
            </li>
          ))}
        </ul>
      ) : (
        <p>No skills assigned.</p>
      )}

      {/* Przycisk do publikacji oferty */}
      {/* <button
        onClick={() => router.push(`/companies/${id}/templates/${offerTemplateId}/offerPublish`)}
      >
        Publish Offer
      </button> */}
    </div>
  );
};

export default TemplateDetails;
