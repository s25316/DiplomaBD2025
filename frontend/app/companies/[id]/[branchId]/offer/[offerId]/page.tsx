'use client';

import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

interface Company {
  name: string;
}

interface Branch {
  name: string;
}

interface Offer {
  status: string;
  statusId: number;
  publicationStart: string;
  publicationEnd: string;
  employmentLength: number;
  websiteUrl: string;
}

interface SkillInfo {
  skill: {
    name: string;
    skillType: {
      name: string;
    };
  };
  isRequired: boolean;
}

interface OfferTemplate {
  name: string;
  description: string;
  skills: SkillInfo[];
}

interface ContractCondition {
  salaryMin: number;
  salaryMax: number;
  currency: { name: string };
  salaryTerm: { name: string };
  hoursPerTerm: number;
  isNegotiable: boolean;
  workModes: { name: string }[];
  employmentTypes: { name: string }[];
}

interface OfferDetailsData {
  company: Company;
  branch: Branch;
  offer: Offer;
  offerTemplate: OfferTemplate;
  contractConditions: ContractCondition[];
}

const OfferDetails = () => {
  const { id, branchId, offerId } = useParams() as {
    id: string;
    branchId: string;
    offerId: string;
  };

  const { data: session } = useSession();
  const router = useRouter();

  const [offerDetails, setOfferDetails] = useState<OfferDetailsData | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    if (!session?.user?.token || !offerId) return;

    const fetchOfferDetails = async () => {
      try {
        const res = await fetch(`http://localhost:8080/api/CompanyUser/offers/${offerId}`, {
          headers: {
            'Authorization': `Bearer ${session.user.token}`,
          },
        });

        if (!res.ok) {
          throw new Error('Failed to fetch offer details');
        }

        const json = await res.json();
        const item = json.items?.[0];

        if (!item) {
          throw new Error('No offer data found in the API response');
        }

        setOfferDetails(item);
      } catch (err: any) {
        setError(err.message);
      }
    };

    fetchOfferDetails();
  }, [session, offerId]);

  if (!session?.user?.token) return <div>Unauthorized</div>;
  if (error) return <div>Error: {error}</div>;

  const handleDelete = async () => {
    if (!window.confirm('Are you sure you want to delete this offer?')) {
      return;
    }

    setIsDeleting(true);
    setError(null);

    try {
      const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/offers/${offerId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${session.user.token}`,
        },
      });

      if (!res.ok) {
        throw new Error('Failed to delete the offer.');
      }

      alert('Offer deleted successfully!');
      router.push(`/companies/${id}/${branchId}`);

    } catch (err: any) {
      setError(err.message);
    } finally {
      setIsDeleting(false);
    }
  };

  if (!session?.user?.token) return <div className="text-center p-4">Unauthorized</div>;
  if (error) return <div className="text-center p-4 text-red-600">Error: {error}</div>;
  if (!offerDetails) return <div className="text-center p-4">Loading...</div>;

  const { company, branch, offer, offerTemplate, contractConditions } = offerDetails;

  return (
    <div className="max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Offer Details</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        <p>
          <span className="font-semibold">Company:</span>{' '}
          <Link href={`/companies/${id}`}>
            {company?.name || 'N/A'}
          </Link>
        </p>
        <p>
          <span className="font-semibold">Branch:</span>{' '}
          <Link href={`/companies/${id}/${branchId}`}>
            {branch?.name || 'N/A'}
          </Link>
        </p>
        <p><span className="font-semibold">Status:</span> {offer.status}</p>
        <p><span className="font-semibold">Employment Length:</span> {offer.employmentLength} months</p>
        <p className="md:col-span-2"><span className="font-semibold">Publication:</span> {new Date(offer.publicationStart).toLocaleString()} – {new Date(offer.publicationEnd).toLocaleString()}</p>
        <p className="md:col-span-2"><span className="font-semibold">Website:</span> <a href={offer.websiteUrl} className="text-blue-600 underline" target="_blank" rel="noopener noreferrer">{offer.websiteUrl}</a></p>
      </div>

      <hr className="my-6" />

      <div>
        <h2 className="text-2xl font-semibold mb-3 text-gray-700">Template Details</h2>
        <p><span className="font-semibold">Name:</span> {offerTemplate.name}</p>
        <p><span className="font-semibold">Description:</span> {offerTemplate.description}</p>
        <h3 className="font-semibold mt-4 mb-2">Required Skills:</h3>
        <ul className="list-disc list-inside space-y-1">
          {offerTemplate.skills.map((s, i) => (
            <li key={i}>
              {s.skill.name} ({s.skill.skillType.name}) {s.isRequired && <span className="font-bold text-sm"> (required)</span>}
            </li>
          ))}
        </ul>
      </div>

      <hr className="my-6" />

      <div>
        <h2 className="text-2xl font-semibold mb-3 text-gray-700">Contract Conditions</h2>
        {contractConditions?.length > 0 ? (
          <div className="space-y-4">
            {contractConditions.map((cond, index) => (
              <div key={index} className="mb-4 border p-4 rounded-md">
                <p><span className="font-semibold">Salary:</span> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name} ({cond.salaryTerm?.name})</p>
                <p><span className="font-semibold">Hours/Term:</span> {cond.hoursPerTerm}</p>
                <p><span className="font-semibold">Negotiable:</span> {cond.isNegotiable ? 'Yes' : 'No'}</p>
                <p><span className="font-semibold">Work Modes:</span> {cond.workModes?.map((w) => w.name).join(', ')}</p>
                <p><span className="font-semibold">Employment Types:</span> {cond.employmentTypes?.map((e) => e.name).join(', ')}</p>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 italic">No contract conditions specified.</p>
        )}
      </div>

      {offer.statusId !== 1 && (
        <div className="mt-8 flex items-center gap-4">
          <Link
            href={`/companies/${id}/${branchId}/offer/${offerId}/edit`}
            className="inline-block bg-blue-600  py-2 rounded-md hover:bg-blue-700 transition-colors"
          >
            Edit Offer
          </Link>
          <button
            onClick={handleDelete}
            disabled={isDeleting}
            className="bg-red-600 py-2 rounded-md hover:bg-red-700 transition-colors disabled:bg-gray-400"
          >
            {isDeleting ? 'Deleting...' : 'Delete Offer'}
          </button>
        </div>
      )}
    </div>
  );
};

export default OfferDetails;