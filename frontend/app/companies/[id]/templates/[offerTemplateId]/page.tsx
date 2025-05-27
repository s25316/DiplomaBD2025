import React from "react";
import { getServerSession } from "next-auth";
import { authOptions } from "@/app/api/auth/[...nextauth]/route";
import Link from "next/link";
import DeleteTemplateButton from "@/app/components/buttons/DeleteTemplateButton";

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

const TemplateDetails = async ({
  params,
}: {
  params: { id: string; offerTemplateId: string };
}) => {
  const session = await getServerSession(authOptions);
  const { id, offerTemplateId } = params;

  let template: OfferTemplate | null = null;

  if (session?.user.token) {
    const res = await fetch(
      `http://localhost:8080/api/CompanyUser/offerTemplates/${offerTemplateId}`,
      {
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        },
        cache: "no-store",
      }
    );

    if (res.ok) {
      const data = await res.json();
      const templateData = data.items[0]?.offerTemplate;

      if (templateData) {
        template = {
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
        };
      }
    }
  }

  if (!template) {
    return <p>Unable to load template details.</p>;
  }

  return (
    <div>
      <h1>Template Details</h1>
      <p><b>Name:</b> {template.name}</p>
      <p><b>Description:</b> {template.description}</p>
      <p><b>Created:</b> {new Date(template.created).toLocaleDateString()}</p>

      <h2 className="mt-4">Skills Required</h2>
      {template.skills.length > 0 ? (
        <ul className="list-disc pl-6">
          {template.skills.map((skill) => (
            <li key={skill.skillId}>
              <b>{skill.name}</b> - {skill.skillType.name}{" "}
              {skill.isRequired ? "(Required)" : "(Optional)"}
            </li>
          ))}
        </ul>
      ) : (
        <p>No skills assigned.</p>
      )}

      <Link
        href={`/companies/${id}/templates/${template.offerTemplateId}/edit`}
        className="inline-block mt-4 bg-blue-500 text-white px-4 py-2 rounded"
      >
        Edit Template
      </Link>

      {session?.user.token && (
        <DeleteTemplateButton
          offerTemplateId={template.offerTemplateId}
          companyId={id}
        />
      )}
    </div>
  );
};

export default TemplateDetails;