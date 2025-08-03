'use client';

import React, { useEffect, useState, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import ReturnButton from '@/app/components/buttons/ReturnButton';

interface ProcessType {
  processTypeId: number;
  name: string;
}

const AVAILABLE_PROCESS_TYPES: ProcessType[] = [
  { processTypeId: 11, name: 'Passed' },
  { processTypeId: 12, name: 'Rejected' },
];

interface RecruitmentData {
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
    description: string | null;
    phoneNum: string | null;
    contactEmail: string;
    age: number;
    isStudent: boolean;
    created: string;
    skills: {
      skillId: number;
      name: string;
      skillType: {
        skillTypeId: number;
        name: string
      }
    }[];
    urls: {
      urlId: string;
      url: string
    }[];
    address: Address;
  };
  company: {
    companyId: string;
    name: string;
    description: string | null;
    websiteUrl: string | null;
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
    description: string | null;
  };
  offer: {
    offerId: string;
    publicationStart: string;
    publicationEnd: string;
    employmentLength: number;
    websiteUrl: string;
    statusId: number;
    status: string;
  };
  contractConditions: ContractConditions[];
}

interface UpdateRecruitmentPayload {
  accepted: boolean;
}

interface SendMessagePayload {
  message: string;
}

const RecruitmentDetailsPage = () => {
  const { processId } = useParams() as { processId: string };
  const { data: session, status } = useSession();
  const router = useRouter();

  const [recruitmentDetails, setRecruitmentDetails] = useState<RecruitmentData | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [newMessage, setNewMessage] = useState<string>('');
  const [newProcessType, setNewProcessType] = useState<number | ''>('');

  // Account type
  const [isIndividual, setIsIndividual] = useState(true);
  useEffect(() => {
    if(session)
      fetch('http://localhost:8080/api/User', {
        headers: { Authorization: `Bearer ${session.user.token}` }
      })
        .then(res => res.json())
        .then(res => setIsIndividual(res.personPerspective.isIndividual))
    }, [session])

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
        if (e instanceof Error)
          console.error(e.message)
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  const fetchRecruitmentDetails = useCallback(async () => {
    if (status !== 'authenticated' || !session?.user?.token) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);

    let apiUrl = 'http://localhost:8080/api/'
    if (!isIndividual) {
      apiUrl += `CompanyUser/recruitments/${processId}`
    }
    else {
      apiUrl += `User/recruitments/${processId}`
    }

    try {
      const res = await fetch(
        apiUrl,
        {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          cache: 'no-store',
        }
      );

      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(`Failed to fetch recruitment details: ${errorText}`);
      }

      const data = await res.json();
      const recruitmentData = data.items[0];
      setRecruitmentDetails(recruitmentData);
      setNewProcessType(recruitmentData.recruitment.processTypeId);

    } catch (err) {
      console.error("Error fetching recruitment details:", err);
      if (err instanceof Error) {
        setError(err.message);
        showCustomAlert(`Error loading recruitment details: ${err.message}`, true);
      }
    } finally {
      setLoading(false);
    }
  }, [session, status, processId, isIndividual]);

  useEffect(() => {
    fetchRecruitmentDetails();
  }, [fetchRecruitmentDetails]);

  const handleDownloadFile = async () => {
    if (status !== 'authenticated' || !session?.user?.token) {
      showCustomAlert("Authentication required to download file.", true);
      return;
    }

    let apiUrl = 'http://localhost:8080/api/'
    if (!isIndividual) {
      apiUrl += `CompanyUser/recruitments/${processId}/file`
    }
    else {
      apiUrl += `User/recruitments/${processId}/file`
    }

    try {
      const res = await fetch(
        apiUrl,
        {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
          },
        }
      );

      if (!res.ok) {
        const errorBody = await res.text();
        console.error(`HTTP Error: ${res.status} ${res.statusText}`, errorBody);
        throw new Error(`Failed to download file: ${res.status} ${res.statusText}. Details: ${errorBody.substring(0, 200)}...`);
      }

      const contentDisposition = res.headers.get('Content-Disposition');
      let filename = 'downloaded_file';

      console.log('Raw Content-Disposition from browser:', contentDisposition);

      if (contentDisposition) {
        const filenameStarMatch = /filename\*=(?:utf-8''|['"]?)([^;"]+)/i.exec(contentDisposition);
        if (filenameStarMatch && filenameStarMatch[1]) {
          try {
            filename = decodeURIComponent(filenameStarMatch[1].replace(/^["']|["']$/g, ''));
            console.log("Filename from filename* (decoded):", filename);
          } catch (e) {
            console.warn("Failed to decode filename*:", e);
            filename = filenameStarMatch[1].replace(/^["']|["']$/g, '');
            console.log("Filename from filename* (raw, fallback):", filename);
          }
        } else {
          const filenameMatch = /filename=["']?([^"';]+)["']?/.exec(contentDisposition);
          if (filenameMatch && filenameMatch[1]) {
            filename = filenameMatch[1].replace(/%20/g, ' ');
            console.log("Filename from filename:", filename);
          }
        }
      }

      console.log("Final filename to save:", filename);
      const blob = await res.blob();

      // użycie wbudowanych API przeglądarki
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = filename; // Ustawia nazwę pliku do pobrania
      document.body.appendChild(a); // Dodaj element do DOM (wymagane dla Firefox)
      a.click(); // Symuluj kliknięcie linku
      document.body.removeChild(a); // Usuń element z DOM
      window.URL.revokeObjectURL(url); // Zwolnij zasoby URL

      showCustomAlert("File downloaded successfully!");

    } catch (err) {
      console.error("Error downloading file:", err);
      if (err instanceof Error)
        showCustomAlert(`Error downloading file: ${err.message}`, true);
    }
  };

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (status !== 'authenticated' || !session?.user?.token) {
      showCustomAlert("Authentication required to send message.", true);
      return;
    }
    if (!newMessage.trim()) {
      showCustomAlert("Message cannot be empty.", true);
      return;
    }

    let apiUrl = 'http://localhost:8080/api/'
    if (!isIndividual) {
      apiUrl += `CompanyUser/recruitments/${processId}/messages`
    }
    else {
      apiUrl += `User/recruitments/${processId}/messages`
    }

    try {
      const payload: SendMessagePayload = { message: newMessage };
      const res = await fetch(
        apiUrl,
        {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(payload),
        }
      );

      if (res.ok) {
        showCustomAlert("Message sent successfully!");
        setNewMessage('');
        router.push(`/recruitments/${processId}/messages`);
      } else {
        const errorText = await res.text();
        console.error("Failed to send message:", errorText);
        showCustomAlert(`Failed to send message: ${errorText}`, true);
      }
    } catch (err) {
      console.error("Error sending message:", err);
      if (err instanceof Error)
        showCustomAlert(`An unexpected error occurred while sending message: ${err.message}`, true);
    }
  };

  const handleChangeProcessType = async (e: React.FormEvent) => {
    e.preventDefault();
    if (status !== 'authenticated' || !session?.user?.token) {
      showCustomAlert("Authentication required to change process type.", true);
      return;
    }

    if (newProcessType === '' || newProcessType === null) {
      showCustomAlert("Please select a valid process type.", true);
      return;
    }

    const currentProcessTypeId = recruitmentDetails?.recruitment.processTypeId;
    const currentProcessTypeName = recruitmentDetails?.recruitment.processType.name;

    if (currentProcessTypeId === 11 || currentProcessTypeId === 12) {
      showCustomAlert(`Failed to update process type: Unable to change, actual: ${currentProcessTypeName}.`, true);
      return;
    }

    let payload: UpdateRecruitmentPayload;
    let isSupportedAction = false;

    if (newProcessType === 11) { // Passed
      payload = { accepted: true };
      isSupportedAction = true;
    } else if (newProcessType === 12) { // Rejected
      payload = { accepted: false };
      isSupportedAction = true;
    } else {
      const selectedTypeName = AVAILABLE_PROCESS_TYPES.find(t => t.processTypeId === newProcessType)?.name || newProcessType;
      showCustomAlert(`Changing to "${selectedTypeName}" status is not supported by this API endpoint. This endpoint is for 'Passed' or 'Rejected' actions only.`, true);
      return;
    }

    if (!isSupportedAction) {
      return;
    }

    try {
      const res = await fetch(
        `http://localhost:8080/api/CompanyUser/companies/recruitments/${processId}`,
        {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(payload),
        }
      );

      if (res.ok) {
        showCustomAlert("Process type updated successfully!");
        fetchRecruitmentDetails();
      } else {
        const errorText = await res.text();
        console.error("Failed to update process type:", errorText);
        showCustomAlert(`Failed to update process type: ${errorText}`, true);
      }
    } catch (err) {
      console.error("Error updating process type:", err);
      if (err instanceof Error)
        showCustomAlert(`An unexpected error occurred while updating process type: ${err.message}`, true);
    }
  };


  if (status === 'loading' || loading) {
    return <div className="text-center py-4 text-blue-600">Loading recruitment details...</div>;
  }

  if (status === 'unauthenticated') {
    return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  }

  if (error) {
    return <div className="text-center py-4 text-red-600">Error: {error}</div>;
  }

  if (!recruitmentDetails) {
    return <div className="text-center py-4 text-gray-600">Recruitment not found.</div>;
  }

  const { recruitment, person, company, branch, offerTemplate, offer } = recruitmentDetails;

  const isFinalStatus = recruitment.processTypeId === 11 || recruitment.processTypeId === 12;

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white dark:bg-gray-900 rounded-lg shadow-xl mt-8 font-inter text-gray-900 dark:text-gray-100">
      <ReturnButton />
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">Recruitment Details</h1>

      <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Recruitment: {recruitment.processId}</h2>
        <p><strong>Process Type:</strong> {recruitment.processType.name}</p>
        {recruitment.message && <p><strong>Message:</strong> {recruitment.message}</p>}
        <p><strong>Created:</strong> {new Date(recruitment.created).toLocaleDateString()} {new Date(recruitment.created).toLocaleTimeString()}</p>
      </div>

      {!isIndividual && <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Candidate Details</h2>
        <p><strong>Name:</strong> {person.name} {person.surname}</p>
        <p><strong>Email:</strong> {person.contactEmail}</p>
        {person.phoneNum && <p><strong>Phone:</strong> {person.phoneNum}</p>}
        {person.description && <p><strong>Description:</strong> {person.description}</p>}
        <p><strong>Age:</strong> {person.age}</p>
        <p><strong>Student:</strong> {person.isStudent ? 'Yes' : 'No'}</p>
        <p><strong>Created:</strong> {new Date(person.created).toLocaleDateString()}</p>
        {person.skills.length > 0 && (
          <div className="mt-2">
            <p className="font-semibold">Skills:</p>
            <ul className="list-disc ml-6">
              {person.skills.map((s, idx) => (
                <li key={idx}>{s.name} ({s.skillType.name})</li>
              ))}
            </ul>
          </div>
        )}
        {person.urls.length > 0 && (
          <div className="mt-2">
            <p className="font-semibold">URLs:</p>
            <ul className="list-disc ml-6">
              {person.urls.map((u, idx) => (
                <li key={idx}><a href={u.url} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:underline">{u.url}</a></li>
              ))}
            </ul>
          </div>
        )}
      </div>}

      <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Offer Details</h2>
        <p><strong>Offer Template:</strong> <Link href={`/companies/${company.companyId}/offer-templates/${offerTemplate.offerTemplateId}`} className="text-blue-600 hover:underline">{offerTemplate.name}</Link></p>
        <p><strong>Company:</strong> <Link href={`/companies/${company.companyId}`} className="text-blue-600 hover:underline">{company.name}</Link></p>
        <p><strong>Branch:</strong> <Link href={`/companies/${company.companyId}/${branch.branchId}`} className="text-blue-600 hover:underline">{branch.name}</Link> ({branch.address.cityName}, {branch.address.streetName || 'N/A'})</p>
        <p><strong>Publication Start:</strong> {new Date(offer.publicationStart).toLocaleDateString()}</p>
        <p><strong>Publication End:</strong> {new Date(offer.publicationEnd).toLocaleDateString()}</p>
        <p><strong>Employment Length:</strong> {offer.employmentLength} months</p>
        <p><strong>Website:</strong> <a href={offer.websiteUrl} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:underline">{offer.websiteUrl}</a></p>
        <p><strong>Status:</strong> {offer.status}</p>
        {recruitmentDetails.contractConditions.length > 0 && (
          <div className="mt-2">
            <p className="font-semibold">Contract Conditions:</p>
            <ul className="list-disc ml-6">
              {recruitmentDetails.contractConditions.map((cc, idx) => (
                <li key={idx}>
                  {cc.hoursPerTerm} hours/term, {cc.salaryMin}-{cc.salaryMax} {cc.currency?.name} {cc.salaryTerm?.name} ({cc.isNegotiable ? 'Negotiable' : 'Fixed'})
                  {cc.workModes?.length > 0 && ` - Work Modes: ${cc.workModes.map((wm: { name : string }) => wm.name).join(', ')}`}
                  {cc.employmentTypes?.length > 0 && ` - Employment Types: ${cc.employmentTypes.map((et: { name : string }) => et.name).join(', ')}`}
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>

      <div className="flex flex-wrap gap-4 mb-8">
        <button
          onClick={handleDownloadFile}
          className="bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold"
        >
          Download File
        </button>
        <Link href={`/recruitments/${processId}/messages`} className="bg-purple-600 text-white px-5 py-2 rounded-lg hover:bg-purple-700 transition duration-300 ease-in-out shadow-md font-semibold">
          View Messages
        </Link>
      </div>

      {/* Sekcja zmiany typu procesu */}
      {!isIndividual && <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Change Process Type</h2>
        {isFinalStatus && (
          <p className="text-red-600 dark:text-red-400 mb-4 font-semibold">
            Cannot change process type. Recruitment is already {recruitment.processType.name}.
          </p>
        )}
        <form onSubmit={handleChangeProcessType} className="flex flex-col gap-4">
          <label htmlFor="processTypeSelect" className="font-semibold text-gray-700 dark:text-gray-300">Select New Process Type:</label>
          <select
            id="processTypeSelect"
            value={newProcessType ?? ''}
            onChange={(e) => setNewProcessType(Number(e.target.value))}
            className="border border-gray-300 dark:border-gray-600 rounded-md p-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isFinalStatus}
          >
            <option value="">-- Select --</option>
            {AVAILABLE_PROCESS_TYPES.map(type => (
              <option key={type.processTypeId} value={type.processTypeId}>
                {type.name}
              </option>
            ))}
          </select>
          <button
            type="submit"
            className="bg-indigo-600 text-white px-5 py-2 rounded-lg hover:bg-indigo-700 transition duration-300 ease-in-out shadow-md font-semibold self-start disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isFinalStatus}
          >
            Update Process Type
          </button>
        </form>
      </div>}

      {/* Sekcja wysyłania wiadomości */}
      <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Send Message to {!isIndividual ? "Candidate" : "Company"}</h2>
        <form onSubmit={handleSendMessage} className="flex flex-col gap-4">
          <label htmlFor="messageTextarea" className="font-semibold text-gray-700 dark:text-gray-300">Message:</label>
          <textarea
            id="messageTextarea"
            value={newMessage}
            onChange={(e) => setNewMessage(e.target.value)}
            rows={5}
            placeholder="Type your message here..."
            className="border border-gray-300 dark:border-gray-600 rounded-md p-2 w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500"
          ></textarea>
          <button
            type="submit"
            className="bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold self-start"
          >
            Send Message
          </button>
        </form>
      </div>
    </div>
  );
};

export default RecruitmentDetailsPage;