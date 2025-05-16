import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import { getServerSession } from 'next-auth';
import React from 'react';



const OfferDetails = async ({
    // params: { id, branchId, offerId },
    params: { offerId },

  }: {
    // params: { id: string; branchId: string; offerId: string };
    params: {  offerId: string };
  }) => {
    const session = await getServerSession(authOptions);
  
    if (!session?.user?.token) {
      return <div>Unauthorized</div>;
    }
  
  if (!session?.user?.token) {
    return <div>Unauthorized</div>;
  }

  const res = await fetch(`http://localhost:8080/api/CompanyUser/offers/${offerId}`, {
    headers: {
      Authorization: `Bearer ${session.user.token}`,
    },
  });

  if (!res.ok) return <div>Failed to fetch offer details</div>;

  const data = await res.json();
  const offer = data.items?.[0];

  if (!offer) return <div>Offer not found</div>;

  return (
    <div className="max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Offer Details</h1>

      <p><b>Status:</b> {offer.status}</p>
      <p><b>Publication:</b> {new Date(offer.publicationStart).toLocaleDateString()} – {new Date(offer.publicationEnd).toLocaleDateString()}</p>
      <p><b>Employment Length:</b> {offer.employmentLength} months</p>
      <p><b>Website:</b> <a href={offer.websiteUrl} className="text-blue-600 underline" target="_blank">{offer.websiteUrl}</a></p>

      {/* <hr className="my-4" />
      <h2 className="text-xl font-semibold">Template</h2>
      <p><b>Name:</b> {offer.offerTemplate.name}</p>
      <p><b>Description:</b> {offer.offerTemplate.description}</p>

      <h3 className="font-medium mt-2">Skills:</h3>
      <ul className="list-disc ml-6">
        {offer.offerTemplate.skills.map((s: any, i: number) => (
          <li key={i}>
            {s.skill.name} ({s.skill.skillType.name}) {s.isRequired ? '(required)' : ''}
          </li>
        ))}
      </ul>

      {offer.contractConditions?.length > 0 && (
        <>
          <hr className="my-4" />
          <h2 className="text-xl font-semibold">Contract Conditions</h2>
          {offer.contractConditions.map((cond: any, index: number) => (
            <div key={index} className="mb-4 border p-3 rounded">
              <p><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency.name} ({cond.salaryTerm.name})</p>
              <p><b>Hours/Term:</b> {cond.hoursPerTerm}</p>
              <p><b>Negotiable:</b> {cond.isNegotiable ? 'Yes' : 'No'}</p>
              <p><b>Work Modes:</b> {cond.workModes?.map((w: any) => w.name).join(', ')}</p>
              <p><b>Employment Types:</b> {cond.employmentTypes?.map((e: any) => e.name).join(', ')}</p>
            </div>
          ))}
        </>
      )} */}
    </div>
  );
};

export default OfferDetails;
