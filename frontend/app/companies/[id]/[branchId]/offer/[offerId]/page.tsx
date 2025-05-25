'use client';

import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

const OfferDetails = () => {
  const { id, branchId, offerId } = useParams() as {
    id: string;
    branchId: string;
    offerId: string;
  };

  const { data: session } = useSession();
  const router = useRouter();

  const [data, setData] = useState<any>(null);
  const [error, setError] = useState<string | null>(null);


  useEffect(() => {
    if (!session?.user?.token || !offerId) return;

    fetch(`http://localhost:8080/api/CompanyUser/offers/${offerId}`, {
      headers: {
        Authorization: `Bearer ${session.user.token}`,
      },
    })
      .then(res => {
        if (!res.ok) throw new Error('Failed to fetch offer');
        return res.json();
      })
      .then(json => {
        const item = json.items?.[0];
        if (!item) throw new Error('No offer found');
        setData(item);
      })
      .catch(err => setError(err.message));
  }, [session, offerId]);

  if (!session?.user?.token) return <div>Unauthorized</div>;
  if (error) return <div>Error: {error}</div>;
  if (!data) return <div>Loading...</div>;

  const { offer, offerTemplate, contractConditions } = data;

  const handleDelete = async () => {
    const confirmed = confirm('Are you sure you want to delete this offer?');
    if (!confirmed) return;

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/offers/${offerId}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${session.user.token}`,
      },
    });

    if (res.ok) {
      alert('Offer deleted');
      router.push(`/companies/${id}/${branchId}`);
    } else {
      alert('Failed to delete offer');
    }
  };

  return (
    <div className="max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Offer Details</h1>

      <p><b>Status:</b> {offer.status}</p>
      

      <p><b>Publication:</b> {new Date(offer.publicationStart).toLocaleString()} – {new Date(offer.publicationEnd).toLocaleString()}</p>
      <p><b>Employment Length:</b> {offer.employmentLength} months</p>
      <p><b>Website:</b> <a href={offer.websiteUrl} className="text-blue-600 underline" target="_blank" rel="noopener noreferrer">{offer.websiteUrl}</a></p>

      <hr className="my-4" />

      <h2 className="text-xl font-semibold">Template</h2>
      <p><b>Name:</b> {offerTemplate.name}</p>
      <p><b>Description:</b> {offerTemplate.description}</p>

      <h3 className="font-medium mt-2">Skills:</h3>
      <ul className="list-disc ml-6">
        {offerTemplate.skills.map((s: any, i: number) => (
          <li key={i}>
            {s.skill.name} ({s.skill.skillType.name}) {s.isRequired ? '(required)' : ''}
          </li>
        ))}
      </ul>
      
      <br/>

      {contractConditions?.length > 0 ? (
        <>
          <hr className="my-4" />
          <h2 className="text-xl font-semibold">Contract Conditions</h2>
          {contractConditions.map((cond: any, index: number) => (
            <div key={index} className="mb-4 border p-3 rounded">
              <p><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name} ({cond.salaryTerm?.name})</p>
              <p><b>Hours/Term:</b> {cond.hoursPerTerm}</p>
              <p><b>Negotiable:</b> {cond.isNegotiable ? 'Yes' : 'No'}</p>
              <p><b>Work Modes:</b> {cond.workModes?.map((w: any) => w.name).join(', ')}</p>
              <p><b>Employment Types:</b> {cond.employmentTypes?.map((e: any) => e.name).join(', ')}</p>
            </div>
          ))}
        </>
      ) : (
         <h2 className="text-xl font-semibold">Contract Conditions: Non</h2> )
      }




      {offer.statusId != 1 && (
        <>
      <Link
        href={`/companies/${id}/${branchId}/offer/${offerId}/edit`}
        className="inline-block mt-6 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Edit Offer
      </Link>
        <button
        onClick={handleDelete}
        className="mt-4 ml-4 bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
      >
        Delete Offer
      </button>
      </>
      )}      

    </div>
  );
};

export default OfferDetails;
