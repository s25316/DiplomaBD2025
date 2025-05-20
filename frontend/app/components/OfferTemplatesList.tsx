'use client'
import React from 'react';
import Link from 'next/link';
import { useRouter, useParams } from 'next/navigation';
import { useSession } from 'next-auth/react';

const OfferTemplatesList = ({
    templates,
    onDelete,
  }: {
    templates: any[];
    onDelete: (id: string) => void;
  }) => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id } = useParams();

  if (!templates.length) return <h2>No templates available</h2>;

  return (
    <>
      <h2 className="mt-6">Offer Templates:</h2>
      <ul>
        {templates.map((t) => (
          <li key={t.offerTemplateId} className="border p-3 rounded my-2">
            <Link href={`/companies/${id}/templates/${t.offerTemplateId}`}>
              <b>{t.name}</b>
            </Link>
            <div className="mt-2 flex gap-2">
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
          </li>
        ))}
      </ul>
    </>
  );
};

export default OfferTemplatesList;
