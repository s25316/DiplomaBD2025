'use client';

import React, { useEffect, useState, useCallback } from 'react';
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
  offerTemplate: {
    name: string;
  };
}

const RecruitmentsPage = () => {
  const { data: session, status } = useSession();
  const [recruitments, setRecruitments] = useState<Recruitment[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // State for filter options
  const [companiesOptions, setCompaniesOptions] = useState<CompanyOption[]>([]);
  const [branchesOptions, setBranchesOptions] = useState<BranchOption[]>([]);
  const [offersOptions, setOffersOptions] = useState<OfferOption[]>([]);
  
  // State for selected filter values
  const [selectedCompanyId, setSelectedCompanyId] = useState<string>('');
  const [selectedBranchId, setSelectedBranchId] = useState<string>('');
  const [selectedOfferId, setSelectedOfferId] = useState<string>('');
  const [searchText, setSearchText] = useState<string>('');
  const [selectedProcessType, setSelectedProcessType] = useState<number | ''>('');

  // Pagination states
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  // Custom alert function
  const showCustomAlert = (message: string, isError: boolean = false) => {
    let alertMessage = message;
    if (isError) {
      try {
        const errorJson = JSON.parse(message);
        if (errorJson.errors) {
          alertMessage = "Validation Errors:\n" + Object.entries(errorJson.errors).map(([key, value]) => `${key}: ${(value as string[]).join(", ")}`).join("\n");
        } else if (errorJson.title) {
          alertMessage = errorJson.title + (errorJson.detail ? `\n${errorJson.detail}` : "");
        } else {
          alertMessage = message;
        }
      } catch (e) {
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  // Fetch companies on component mount
  useEffect(() => {
    const fetchCompanies = async () => {
      if (status !== 'authenticated' || !session?.user?.token) return;
      try {
        const res = await fetch('http://localhost:8080/api/CompanyUser/companies', {
          headers: { Authorization: `Bearer ${session.user.token}` },
          cache: 'no-store',
        });
        if (res.ok) {
          const data = await res.json();
          // Mapowanie, aby upewnić się, że CompanyOption ma odpowiednie pola
          setCompaniesOptions(data.items.map((c: any) => ({ companyId: c.companyId, name: c.name })) || []);
        } else {
          console.error('Failed to fetch companies:', await res.text());
        }
      } catch (err) {
        console.error('Error fetching companies:', err);
      }
    };
    fetchCompanies();
  }, [session, status]);

  // Fetch branches when selectedCompanyId changes
  useEffect(() => {
    const fetchBranches = async () => {
      if (status !== 'authenticated' || !session?.user?.token || !selectedCompanyId) {
        setBranchesOptions([]);
        setSelectedBranchId(''); // Reset branch when company changes
        setSelectedOfferId(''); // Reset offer when company changes
        return;
      }
      try {
        const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${selectedCompanyId}/branches`, {
          headers: { Authorization: `Bearer ${session.user.token}` },
          cache: 'no-store',
        });
        if (res.ok) {
          const data = await res.json();
          // Mapowanie, aby upewnić się, że BranchOption ma odpowiednie pola
          setBranchesOptions(data.items.map((item: any) => ({
            branchId: item.branch.branchId,
            name: item.branch.name,
            address: { cityName: item.branch.address.cityName }
          })) || []);
        } else {
          console.error('Failed to fetch branches:', await res.text());
        }
      } catch (err) {
        console.error('Error fetching branches:', err);
      }
    };
    fetchBranches();
  }, [session, status, selectedCompanyId]);

  // Fetch offers when selectedBranchId changes
  useEffect(() => {
    const fetchOffers = async () => {
      if (status !== 'authenticated' || !session?.user?.token || !selectedBranchId) {
        setOffersOptions([]);
        setSelectedOfferId(''); // Reset offer when branch changes
        return;
      }
      try {
        const res = await fetch(`http://localhost:8080/api/CompanyUser/branches/${selectedBranchId}/offers?status=2`, {
          headers: { Authorization: `Bearer ${session.user.token}` },
          cache: 'no-store',
        });
        if (res.ok) {
          const data = await res.json();
          // Mapowanie, aby upewnić się, że OfferOption ma odpowiednie pola
          setOffersOptions(data.items.map((item: any) => ({
            offerId: item.offer.offerId,
            offerTemplate: { name: item.offerTemplate.name }
          })) || []);
        } else {
          console.error('Failed to fetch offers:', await res.text());
        }
      } catch (err) {
        console.error('Error fetching offers:', err);
      }
    };
    fetchOffers();
  }, [session, status, selectedBranchId]);


  // --- Main recruitment data fetching ---
  const fetchRecruitments = useCallback(async () => {
    if (status !== 'authenticated' || !session?.user?.token) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);

    try {
      let apiUrl = 'http://localhost:8080/api/CompanyUser/recruitments';
      const queryParams = new URLSearchParams();

      if (selectedOfferId) {
        apiUrl = `http://localhost:8080/api/CompanyUser/offers/${selectedOfferId}/recruitments`;
      } else if (selectedBranchId) {
        apiUrl = `http://localhost:8080/api/CompanyUser/branches/${selectedBranchId}/recruitments`;
    //   } else if (selectedCompanyId) {
    //     apiUrl = `http://localhost:8080/api/CompanyUser/companies/${selectedCompanyId}/recruitments`; //nie działa
      }

      // Add common query parameters
      queryParams.append('Page', page.toString());
      queryParams.append('ItemsPerPage', itemsPerPage.toString());

      if (searchText) queryParams.append('searchText', searchText);
      if (selectedProcessType !== '') queryParams.append('processType', selectedProcessType.toString());

      const fullUrl = `${apiUrl}?${queryParams.toString()}`;
      console.log("Fetching recruitments from:", fullUrl);

      const res = await fetch(fullUrl, {
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
      setRecruitments(data.items || []);
      setTotalCount(data.totalCount || 0);

    } catch (err: any) {
      console.error("Error fetching recruitments:", err);
      setError(err.message);
      showCustomAlert(`Error loading recruitments: ${err.message}`, true);
    } finally {
      setLoading(false);
    }
  }, [session, status, page, itemsPerPage, selectedCompanyId, selectedBranchId, selectedOfferId, searchText, selectedProcessType]);

  useEffect(() => {
    fetchRecruitments();
  }, [fetchRecruitments]);

  // Handlers for filter changes - reset lower level filters and page
  const handleCompanyChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedCompanyId(e.target.value);
    setSelectedBranchId(''); // Reset branch when company changes
    setSelectedOfferId(''); // Reset offer when company changes
    setPage(1); // Reset page on filter change
  };

  const handleBranchChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedBranchId(e.target.value);
    setSelectedOfferId(''); // Reset offer when branch changes
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


  if (status === 'loading' || loading) {
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

      {/* Filter Section */}
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
                {offer.offerTemplate?.name || `Offer ID: ${offer.offerId.substring(0, 8)}...`}
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
            {/* Add more process types as defined by your API */}
          </select>
        </div>
      </div>

      <p className="mb-4 text-sm text-gray-600 dark:text-gray-400">
        Showing {recruitments.length} of {totalCount} recruitments
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
        isNextDisabled={recruitments.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />

      {recruitments.length > 0 ? (
        <ul className="space-y-4 mt-6">
          {recruitments.map(({ recruitment, person, company, branch, offerTemplate, offer }) => (
            <li key={recruitment.processId} className="border p-4 rounded-lg shadow-sm font-inter
                                                bg-white dark:bg-gray-800 border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100">
              <h2 className="text-xl font-bold mb-2">Recruitment ID: {recruitment.processId.substring(0, 8)}...</h2>
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
        isNextDisabled={recruitments.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />
    </div>
  );
};

export default RecruitmentsPage;