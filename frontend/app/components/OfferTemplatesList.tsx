'use client'
import React from 'react';
import Link from 'next/link';
import { useRouter, useParams } from 'next/navigation';
import { useSession } from 'next-auth/react';
import { InnerSection } from './layout/PageContainers';

interface Template{
  offerTemplateId: string;
  name: string;
}

const OfferTemplatesList = ({
    templates,
    onDelete,
  }: {
    templates: Template[];
    onDelete: (id: string) => void;
  }) => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id } = useParams();

  if (!templates.length) return <h2>No templates available</h2>;

  return (
    <>
      <ul>
        {templates.map((t) => (
          <li key={t.offerTemplateId}>
            <InnerSection className="border p-2 my-2 max-w-md">
            <div className="flex justify-between items-center">
            
            <Link href={`/companies/${id}/templates/${t.offerTemplateId}`}>
              <b>{t.name}</b>
            </Link>
            <div className="flex gap">
              <button
                onClick={() => router.push(`/companies/${id}/templates/${t.offerTemplateId}/edit`)}
                className="bg-blue-500 text-white px-2 py-1 rounded"
              >Edit</button>
              
              <button
              onClick={async () => {
                const confirmDelete = confirm("Are you sure you want to delete this template?");
                if (!confirmDelete || !session?.user.token) return;
                
                const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/offerTemplates/${t.offerTemplateId}`, {
                  method: "DELETE",
                  headers: {
                    Authorization: `Bearer ${session.user.token}`,
                  },
                });

                if (res.ok) {
                  onDelete(t.offerTemplateId);
                } else {
                  alert("Failed to delete.");
                }
              }}
                className="ml-4 bg-red-500 text-white px-2 py-1 rounded"> Delete
              </button>
            </div>
            </div>
            </InnerSection>
          </li>
        ))}
      </ul>
    </>
  );
};

export default OfferTemplatesList;
