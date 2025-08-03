'use client';

import React, { useEffect, useState, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import SelectItemsPerPage from '@/app/components/SelectItemsPerPage';
import Pagination from '@/app/components/Pagination';
import GeoMap from '@/app/components/GeoMap';
import DeleteBranchButton from '@/app/components/buttons/DeleteBranchButton';
import DeleteOfferButton from '@/app/components/buttons/DeleteOfferButton';
import { InnerSection, OuterContainer } from '@/app/components/layout/PageContainers';

interface Branch {
  name: string;
  description: string | null;
  created: string;
  address: {
    countryName: string;
    stateName: string;
    cityName: string;
    streetName?: string | null;
    houseNumber: string;
    apartmentNumber?: string | null;
    postCode: string;
    lon: number;
    lat: number;
  };
}
interface Company {
  name: string;
}

interface Offer {
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
  contractConditions: ContractConditions[];
}
interface BranchDetailsApi {
  company: Company;
  branch: Branch;
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
  const router = useRouter();

  const [offers, setOffers] = useState<Offer[]>([]);
  const [branchDetails, setBranchDetails] = useState<BranchDetailsApi | null>(null);

  const [error, setError] = useState<string | null>(null);
  const [statusFilter, setStatusFilter] = useState<number | null>(null);
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  // Memoized function to fetch all data for this branch
  const fetchBranchAndOffers = useCallback(async () => {
    if (!session?.user?.token || !branchId) return;

    const headers = {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${session.user.token}`,
    };

    try {
      console.log("Fetching branch details and offers...");
      const [branchRes, offersRes] = await Promise.all([
        fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}`, { headers, cache: 'no-store' }),
        fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}/offers?${statusFilter !== null ? `status=${statusFilter}&` : ''}Page=${page}&ItemsPerPage=${itemsPerPage}`, { headers, cache: 'no-store' }),
      ]);

      if (!branchRes.ok) {
        if (branchRes.status === 404) {
          setError("Branch not found. It might have been deleted.");
          router.replace(`/companies/${id}`);
          return;
        }
        const errorText = await branchRes.text();
        throw new Error(`Failed to fetch branch details: ${errorText}`);
      }
      const branchJson = await branchRes.json();
      setBranchDetails(branchJson.items[0]);

      if (!offersRes.ok) {
        const errorText = await offersRes.text();
        throw new Error(`Failed to fetch offers: ${errorText}`);
      }
      const offersJson = await offersRes.json();
      setOffers(offersJson.items || []);
      setTotalCount(offersJson.totalCount || 0);

      setError(null);

    } catch (err) {
      console.error("Error fetching data in BranchDetails:", err);
      if(err instanceof Error)
      setError(err.message);
    }
  }, [session, branchId, statusFilter, page, itemsPerPage, router, id]);

  useEffect(() => {
    fetchBranchAndOffers();
  }, [fetchBranchAndOffers]);

  const handleOfferDeleted = useCallback(() => {
    fetchBranchAndOffers(); // refresh list after delete
  }, [fetchBranchAndOffers]);

  if (!session?.user?.token) return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  if (error) return <div className="text-center py-4 text-red-600">Error: {error}</div>;
  if (!branchDetails) return <div className="text-center py-4 text-blue-600">Loading branch details...</div>;

  const { branch, company } = branchDetails;

  return (
    <OuterContainer>
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">Branch Details</h1>
      
      <InnerSection>
        <p className="text-xl font-bold mb-2 text-gray-800 dark:text-gray-100">Name: {branch.name}</p>
        <p className="text-lg mb-2 text-gray-700 dark:text-gray-300">
          <Link href={`/companies/${id}`}>
            Company: {company?.name}
          </Link>
        </p>
        {branch.description && <p className="mb-2 text-gray-700 dark:text-gray-300">Description: {branch.description}</p>}
        <div className="mb-2 text-gray-700 dark:text-gray-300">
          <p className="font-semibold">Address:</p>
          <ul className="ml-4 list-disc list-inside">
            <li>Country: {branch.address.countryName}</li>
            <li>City: {branch.address.cityName}</li>
            {branch.address.streetName && <li>Street: {branch.address.streetName}</li>}
            <li>House number: {branch.address.houseNumber}</li>
            {branch.address.apartmentNumber && <li>Apartment number: {branch.address.apartmentNumber}</li>}
            <li>Post Code: {branch.address.postCode}</li>
          </ul>
        </div>
        {branch.address.lat && branch.address.lon && (
          <GeoMap lat={branch.address.lat} lon={branch.address.lon} />
        )}
      <br/>
      <div className="flex flex-wrap gap-4 mb-8">
        <Link href={`/companies/${id}/${branchId}/edit`} className="inline-block bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300">
          Edit Branch
        </Link>
        <DeleteBranchButton
          branchId={branchId}
          companyId={id as string} 
          buttonText="Delete Branch"
          className="inline-block bg-red-600 text-white px-5 py-2 rounded-lg hover:bg-red-700 transition duration-300"
        />
      </div>
      </InnerSection>

      <InnerSection>
      <Link href={`/companies/${id}/${branchId}/publishOffer`} className="inline-block bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300">
        Publish Offer
      </Link>
      <OuterContainer>

      <h2 className="mt-6 mb-4 text-2xl font-bold text-gray-800 dark:text-gray-100">Offers in this Branch:</h2>
   
      <p className="mb-4 text-sm text-gray-600 dark:text-gray-400">
        Showing {offers.length} of {totalCount} offers
      </p>

      <div className="flex flex-wrap items-center gap-4 mb-4">
        <label htmlFor="statusFilter" className="font-semibold text-gray-700 dark:text-gray-300">Filter by Status:</label>
        <select
          id="statusFilter"
          value={statusFilter ?? ''}
          onChange={(e) => {
            const val = e.target.value;
            setStatusFilter(val === '' ? null : Number(val));
            setPage(1);
          }}
          className="border border-gray-300 dark:border-gray-600 rounded-md p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100"
        >
          <option value="">All statuses</option>
          <option value="0">Not Published</option>
          <option value="1">Expired</option>
          <option value="2">Active</option>
          <option value="3">Scheduled</option>
        </select>
        <SelectItemsPerPage
          value={itemsPerPage}
          onChange={(val) => {
            setItemsPerPage(val);
            setPage(1);
          }}
        />
      </div>

      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={offers.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />

      {offers.length > 0 ? (
        <ul className="space-y-4 mt-6">
          {offers.map(({ offer, offerTemplate }) => (
            <li key={offer.offerId} >
            <InnerSection>
              <Link href={`/companies/${id}/${branchId}/offer/${offer.offerId}`} className="text-blue-600 dark:text-blue-400">
                <b className="text-lg font-bold">{offerTemplate?.name || 'Untitled Template'}</b>
              </Link>
              <p className="text-sm text-gray-700 dark:text-gray-300"><b>Status:</b> {STATUS_MAP[offer.statusId] || offer.status}</p>
              <p className="text-sm text-gray-700 dark:text-gray-300"><b>Start:</b> {new Date(offer.publicationStart).toLocaleDateString()}</p>
              <p className="text-sm text-gray-700 dark:text-gray-300"><b>End:</b> {new Date(offer.publicationEnd).toLocaleDateString()}</p>
              <p className="text-sm text-gray-700 dark:text-gray-300">
                <b>Website:</b>{' '}
                <a href={offer.websiteUrl} target="_blank" rel="noopener noreferrer">
                  {offer.websiteUrl}
                </a>
              </p>

              {offer.statusId !== 1 ? (
                <DeleteOfferButton
                  offerId={offer.offerId}
                  onSuccess={handleOfferDeleted} //refresh list
                  buttonText="Delete Offer"
                  confirmationMessage={`Are you sure you want to delete offer "${offerTemplate?.name || offer.offerId}"?`}
                  className="mt-3 bg-red-500 text-white px-3 py-1 rounded-md hover:bg-red-600 transition duration-200 shadow-sm"
                />
              ) : (
                <p className="text-gray-500 dark:text-gray-400 mt-3 italic">Expired (cannot delete)</p>
              )}
              </InnerSection>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-gray-600 dark:text-gray-400 mt-6">No offers published for this branch yet.</p>
      )}
      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={offers.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />
      <br/>
      </OuterContainer>
      <Link href={`/companies/${id}/${branchId}/publishOffer`} className="inline-block bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold">
        Publish Offer
      </Link>
      </InnerSection>
    </OuterContainer>
    
  );
};

export default BranchDetails;
