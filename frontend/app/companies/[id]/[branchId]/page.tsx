'use client';

import React, { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

interface BranchDetailsType {
  name: string;
  desciption: string;
  created: string;
  address: {
    countryName: string;
    stateName: string;
    cityName: string;
    streetName?: string;
    houseNumber: string;
    apartmentNumber?: string;
    postCode: string;
  };
}

interface OfferFull {
  offer: {
    offerId: string;
    publicationStart: string;
    publicationEnd: string;
    employmentLength: number;
    websiteUrl: string;
    statusId: number;
    status: string;
    branchId: string;
  };
  offerTemplate: {
    name: string;
  };
  contractConditions: any[];
}
const STATUS_MAP: Record<number, string> = {
  0: "Not Published",
  1: "Expired",
  2: "Active",
  3: "Scheduled",
};

const BranchDetails = () => {
  const { id, branchId } = useParams() as { id: string; branchId: string };
  const { data: session } = useSession();

  const [branchData, setBranchData] = useState<BranchDetailsType | null>(null);
  const [offers, setOffers] = useState<OfferFull[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!session?.user?.token || !branchId) return;

    const headers = {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${session.user.token}`,
    };

    const fetchData = async () => {
      try {
        const [branchRes, offersRes] = await Promise.all([
          fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}`, { headers }),
          fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}/offers`, { headers }),
        ]);

        if (!branchRes.ok) throw new Error('Failed to fetch branch details');
        const branchJson = await branchRes.json();
        setBranchData(branchJson.items[0]?.branch);

        if (!offersRes.ok) throw new Error('Failed to fetch offers');
        const offersJson = await offersRes.json();
        setOffers(offersJson.items || []);
      } catch (err: any) {
        setError(err.message);
      }
    };

    fetchData();
  }, [session, branchId]);

  if (!session?.user?.token) return <div>Unauthorized</div>;
  if (error) return <div>Error: {error}</div>;
  if (!branchData) return <div>Loading...</div>;

  const handleDeleteOffer = async (offerIdToDelete: string) => {
    const confirmed = confirm('Delete this offer?');
    if (!confirmed) return;

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/offers/${offerIdToDelete}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${session?.user.token}`,
      },
    });

    if (res.ok) {
      setOffers((prev) => prev.filter(({ offer }) => offer.offerId !== offerIdToDelete));
      alert('Offer deleted');
    } else {
      alert('Failed to delete offer');
    }
  };


  return (
    <div>
      <h1>Branch Details</h1>
      <br />
      <p><b>Name:</b> {branchData.name}</p>
      {branchData.desciption && <p><b>Description:</b> {branchData.desciption}</p>}
      <div className="mb-2">
        <p><b>Address:</b></p>
        <p className="ml-4"><b>Country:</b> {branchData.address.countryName}</p>
        <p className="ml-4"><b>City:</b> {branchData.address.cityName}</p>
        {branchData.address.streetName && <p className="ml-4"><b>Street:</b> {branchData.address.streetName}</p>}
        <p className="ml-4"><b>House number:</b> {branchData.address.houseNumber}</p>
        {branchData.address.apartmentNumber && <p className="ml-4"><b>Apartment number:</b> {branchData.address.apartmentNumber}</p>}
      </div>

      <Link href={`/companies/${id}/${branchId}/edit`} className="text-blue-600 underline">
        Edit Branch
      </Link>
      <br />
      <Link href={`/companies/${id}/${branchId}/publishOffer`} className="text-blue-600">
        Publish offer
      </Link>

      <h2 className="mt-6 mb-2 text-xl font-semibold">Offers in this Branch:</h2>
      
      {offers.length > 0 ? (
        <ul className="list-disc ml-6">
          {offers.map(({ offer, offerTemplate }) => (
            <li key={offer.offerId} className="border p-3 rounded my-2">
              <Link href={`/companies/${id}/${branchId}/offer/${offer.offerId}`}>
                <b>{offerTemplate?.name || 'Untitled Template'}</b>
              </Link>
              <p><b>Status:</b> {STATUS_MAP[offer.statusId] || offer.status}</p>
              <p><b>Start:</b> {new Date(offer.publicationStart).toLocaleDateString()}</p>
              <p><b>End:</b> {new Date(offer.publicationEnd).toLocaleDateString()}</p>
              <p>
                <b>Website:</b>{' '}
                <a href={offer.websiteUrl} target="_blank" rel="noopener noreferrer">
                  {offer.websiteUrl}
                </a>
              </p>

              {offer.statusId !== 1 ? (
                <button
                  onClick={() => handleDeleteOffer(offer.offerId)}
                  className="mt-2 text-red-600 underline"
                >
                  Delete
                </button>
              ) : (
                <p className="text-gray-500 mt-2 italic">Expired (cannot delete)</p>
              )}
            </li>
          ))}

        </ul>
      ) : (
        <p>No offers published for this branch yet.</p>
      )}
    </div>
  );
};

export default BranchDetails;
