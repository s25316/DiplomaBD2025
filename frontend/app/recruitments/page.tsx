'use client';

import React, { useEffect, useState, useMemo } from 'react';
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import SelectItemsPerPage from '@/app/components/SelectItemsPerPage';
import Pagination from '@/app/components/Pagination';

interface ProcessType {
  processTypeId: number;
  name: string;
}

interface Recruitment {
  recruitment: {
    processId: string;
    offerId: string;
    personId: string;
    processTypeId: number;
    message: string | null;
    created: string;
    processType: ProcessType;
  };
  person: {
    name: string;
    surname: string;
    contactEmail: string;
    phoneNum: string | null;
  };
  company: {
    companyId: string;
    name: string;
  };
  branch: {
    branchId: string;
    name: string;
    address: {
      cityName: string;
      streetName: string | null;
    };
  };
  offerTemplate: {
    offerTemplateId: string;
    name: string;
  };
  offer: {
    offerId: string;
    publicationStart: string;
    publicationEnd: string;
    status: string;
  };
}

interface CompanyOption {
  companyId: string;
  name: string;
}

interface BranchOption {
  branchId: string;
  name: string;
  address: {
    cityName: string;
  };
}

interface OfferOption {
  offerId: string;
  name: string;
}

const RecruitmentsPage = () => {
  const { data: session, status } = useSession();
  const [allRecruitments, setAllRecruitments] = useState<Recruitment[]>([]);
  const [filteredRecruitments, setFilteredRecruitments] = useState<Recruitment[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // State for selected filter values
  const [selectedCompanyId, setSelectedCompanyId] = useState<string>('');
  const [selectedBranchId, setSelectedBranchId] = useState<string>('');
  const [selectedOfferId, setSelectedOfferId] = useState<string>('');
  const [searchText, setSearchText] = useState<string>('');
  const [selectedProcessType, setSelectedProcessType] = useState<number | ''>('');

  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [isIndividual, setIsIndividual] = useState<boolean | null>(null);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  // Main data fetching logic, now combined for a cleaner flow
  useEffect(() => {
    const fetchData = async () => {
      if (status !== 'authenticated' || !session?.user?.token) {
        setLoading(false);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        // Fetch user data to determine account type (Individual or Company)
        const userRes = await fetch(`${backUrl}/api/User`, {
          headers: { Authorization: `Bearer ${session.user.token}` }
        });

        if (!userRes.ok) {
          throw new Error("Failed to fetch user data.");
        }
        const userData = await userRes.json();
        const isIndividualUser = userData.personPerspective.isIndividual;
        setIsIndividual(isIndividualUser);

        // Based on the user type, fetch the correct recruitments data
        const apiUrl = isIndividualUser
          ? `${backUrl}/api/User/recruitments`
          : `${backUrl}/api/CompanyUser/recruitments`;

        const res = await fetch(apiUrl, {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          cache: 'no-store',
        });

        if (!res.ok) {
          const errorText = await res.text();
          throw new Error(`Failed to fetch recruitments: ${errorText}`);
        }

        const data = await res.json();
        const recruitments = data.items || [];
        setAllRecruitments(recruitments);
        // Set total count based on all fetched items for client-side pagination
        setTotalCount(recruitments.length);

      } catch (err) {
        console.error("Error fetching data:", err);
        if (err instanceof Error) {
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [session, status]);

  // Extract filter options from allRecruitments
  const companiesOptions = useMemo(() => {
    const uniqueCompanies = new Map<string, CompanyOption>();
    allRecruitments.forEach(r => {
      if (!uniqueCompanies.has(r.company.companyId)) {
        uniqueCompanies.set(r.company.companyId, {
          companyId: r.company.companyId,
          name: r.company.name
        });
      }
    });
    return Array.from(uniqueCompanies.values());
  }, [allRecruitments]);

  const branchesOptions = useMemo(() => {
    const uniqueBranches = new Map<string, BranchOption>();
    const filteredByCompany = selectedCompanyId
      ? allRecruitments.filter(r => r.company.companyId === selectedCompanyId)
      : allRecruitments;

    filteredByCompany.forEach(r => {
      if (!uniqueBranches.has(r.branch.branchId)) {
        uniqueBranches.set(r.branch.branchId, {
          branchId: r.branch.branchId,
          name: r.branch.name,
          address: { cityName: r.branch.address.cityName }
        });
      }
    });
    return Array.from(uniqueBranches.values());
  }, [allRecruitments, selectedCompanyId]);

  const offersOptions = useMemo(() => {
    const uniqueOffers = new Map<string, OfferOption>();
    const filteredByBranch = selectedBranchId
      ? allRecruitments.filter(r => r.branch.branchId === selectedBranchId)
      : allRecruitments;

    filteredByBranch.forEach(r => {
      if (!uniqueOffers.has(r.offer.offerId)) {
        uniqueOffers.set(r.offer.offerId, {
          offerId: r.offer.offerId,
          name: r.offerTemplate?.name || 'Unknown Offer'
        });
      }
    });
    return Array.from(uniqueOffers.values());
  }, [allRecruitments, selectedBranchId]);

  // Client-side filtering and pagination logic
  useEffect(() => {
    let filtered = allRecruitments;

    if (selectedCompanyId) {
      filtered = filtered.filter(r => r.company.companyId === selectedCompanyId);
    }

    if (selectedBranchId) {
      filtered = filtered.filter(r => r.branch.branchId === selectedBranchId);
    }

    if (selectedOfferId) {
      filtered = filtered.filter(r => r.offer.offerId === selectedOfferId);
    }

    if (searchText) {
      const lowerSearchText = searchText.toLowerCase();
      filtered = filtered.filter(r =>
        r.person.name.toLowerCase().includes(lowerSearchText) ||
        r.person.surname.toLowerCase().includes(lowerSearchText) ||
        r.person.contactEmail.toLowerCase().includes(lowerSearchText)
      );
    }

    if (selectedProcessType !== '') {
      filtered = filtered.filter(r => r.recruitment.processType.processTypeId === selectedProcessType);
    }

    setTotalCount(filtered.length);
    // Apply pagination to the filtered results
    const paginatedRecruitments = filtered.slice((page - 1) * itemsPerPage, page * itemsPerPage);
    setFilteredRecruitments(paginatedRecruitments);
  }, [allRecruitments, selectedCompanyId, selectedBranchId, selectedOfferId, searchText, selectedProcessType, page, itemsPerPage]);

  // Handlers for filter changes - only update state, no API calls
  const handleCompanyChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedCompanyId(e.target.value);
    setSelectedBranchId('');
    setSelectedOfferId('');
    setPage(1);
  };

  const handleBranchChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedBranchId(e.target.value);
    setSelectedOfferId('');
    setPage(1);
  };

  const handleOfferChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedOfferId(e.target.value);
    setPage(1);
  };

  const handleSearchTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchText(e.target.value);
    setPage(1);
  };

  const handleProcessTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedProcessType(e.target.value === '' ? '' : Number(e.target.value));
    setPage(1);
  };

  if (status === 'loading' || loading || isIndividual === null) {
    return <div className="text-center py-4 text-blue-600">Loading recruitments...</div>;
  }

  if (status === 'unauthenticated') {
    return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  }

  if (error) {
    return <div className="text-center py-4 text-red-600">Error: {error}</div>;
  }

  return (
    <div className="max-w-6xl mx-auto p-6 bg-white dark:bg-gray-900 rounded-lg shadow-xl mt-8 font-inter text-gray-900 dark:text-gray-100">
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">All Recruitments</h1>

      {/* Filter Section - only visible for company users */}
      {!isIndividual && (
        <div className="bg-gray-100 dark:bg-gray-800 p-4 rounded-lg mb-6 shadow-inner grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {/* Company Filter */}
          <div>
            <label htmlFor="companyFilter" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Filter by Company:</label>
            <select
              id="companyFilter"
              value={selectedCompanyId}
              onChange={handleCompanyChange}
              className="block w-full border border-gray-300 dark:border-gray-600 rounded-md shadow-sm p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="">All Companies</option>
              {companiesOptions.map((company) => (
                <option key={company.companyId} value={company.companyId}>
                  {company.name}
                </option>
              ))}
            </select>
          </div>

          {/* Branch Filter */}
          <div>
            <label htmlFor="branchFilter" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Filter by Branch:</label>
            <select
              id="branchFilter"
              value={selectedBranchId}
              onChange={handleBranchChange}
              disabled={!selectedCompanyId || branchesOptions.length === 0}
              className="block w-full border border-gray-300 dark:border-gray-600 rounded-md shadow-sm p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <option value="">All Branches</option>
              {branchesOptions.map((branch) => (
                <option key={branch.branchId} value={branch.branchId}>
                  {branch.name} ({branch.address.cityName})
                </option>
              ))}
            </select>
          </div>

          {/* Offer Filter */}
          <div>
            <label htmlFor="offerFilter" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Filter by Offer:</label>
            <select
              id="offerFilter"
              value={selectedOfferId}
              onChange={handleOfferChange}
              disabled={!selectedBranchId || offersOptions.length === 0}
              className="block w-full border border-gray-300 dark:border-gray-600 rounded-md shadow-sm p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <option value="">All Offers</option>
              {offersOptions.map((offer) => (
                <option key={offer.offerId} value={offer.offerId}>
                  {offer.name}
                </option>
              ))}
            </select>
          </div>

          {/* Search Text Filter */}
          <div>
            <label htmlFor="searchText" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Search Text:</label>
            <input
              type="text"
              id="searchText"
              value={searchText}
              onChange={handleSearchTextChange}
              placeholder="Search by name, email, etc."
              className="block w-full border border-gray-300 dark:border-gray-600 rounded-md shadow-sm p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500"
            />
          </div>

          {/* Process Type Filter */}
          <div>
            <label htmlFor="processTypeFilter" className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Process Type:</label>
            <select
              id="processTypeFilter"
              value={selectedProcessType}
              onChange={handleProcessTypeChange}
              className="block w-full border border-gray-300 dark:border-gray-600 rounded-md shadow-sm p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="">All Types</option>
              <option value={1}>Recruit</option>
              <option value={2}>Interview</option>
              <option value={11}>Accepted</option>
              <option value={12}>Rejected</option>
            </select>
          </div>
        </div>
      )}

      <p className="mb-4 text-sm text-gray-600 dark:text-gray-400">
        Showing {filteredRecruitments.length} of {totalCount} recruitments
      </p>

      <SelectItemsPerPage
        value={itemsPerPage}
        onChange={(val) => {
          setItemsPerPage(val);
          setPage(1);
        }}
      />

      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={filteredRecruitments.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />

      {filteredRecruitments.length > 0 ? (
        <ul className="space-y-4 mt-6">
          {filteredRecruitments.map(({ recruitment, person, company, branch, offerTemplate, offer }) => (
            <li key={recruitment.processId} className="border p-4 rounded-lg shadow-sm font-inter bg-white dark:bg-gray-800 border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100">
              <h2 className="text-xl font-bold mb-2">Recruitment ID: {recruitment.processId.substring(0, 8)}</h2>
              <p><strong>Offer:</strong> <Link href={`/companies/${company.companyId}/${branch.branchId}/offer/${offer.offerId}`} className="text-blue-600 hover:underline">{offerTemplate.name}</Link></p>
              <p><strong>Company:</strong> <Link href={`/companies/${company.companyId}`} className="text-blue-600 hover:underline">{company.name}</Link></p>
              <p><strong>Branch:</strong> <Link href={`/companies/${company.companyId}/${branch.branchId}`} className="text-blue-600 hover:underline">{branch.name}</Link></p>
              <p><strong>Candidate:</strong> {person.name} {person.surname} ({person.contactEmail})</p>
              <p><strong>Process Type:</strong> {recruitment.processType.name}</p>
              {recruitment.message && <p><strong>Message:</strong> {recruitment.message}</p>}
              <p><strong>Created:</strong> {new Date(recruitment.created).toLocaleDateString()} {new Date(recruitment.created).toLocaleTimeString()}</p>
              <p><strong>Offer Status:</strong> {offer.status}</p>
              <p><strong>Offer Publication:</strong> {new Date(offer.publicationStart).toLocaleDateString()} - {new Date(offer.publicationEnd).toLocaleDateString()}</p>

              <div className="mt-4 flex gap-2">
                <Link href={`/recruitments/${recruitment.processId}`} className="bg-blue-500 text-white px-3 py-1 rounded-md hover:bg-blue-600 transition duration-200 shadow-sm">
                  View Details
                </Link>
                <Link href={`/recruitments/${recruitment.processId}/messages`} className="bg-purple-500 text-white px-3 py-1 rounded-md hover:bg-purple-600 transition duration-200 shadow-sm">
                  Messages
                </Link>
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-gray-600 dark:text-gray-400 mt-6">No recruitments found matching your criteria.</p>
      )}

      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={filteredRecruitments.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />
    </div>
  );
};

export default RecruitmentsPage;
