'use client';

import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import { InnerSection, OuterContainer } from '@/app/components/layout/PageContainers';
import GeoMap from '@/app/components/GeoMap';

interface Company {
  name: string;
}

interface Branch {
  name: string;
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
    skillId: number;
    name: string;
    skillType: {
      skillTypeId: number 
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

  const [isOwnOffer, setIsOwnOffer] = useState(false);
  const [isIndividual, setIsIndividual] = useState(false);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  useEffect(() => {
    if (!offerId) return;

    const fetchOfferDetails = async () => {
      try {
        let fetchedData: OfferDetailsData | null = null;
        let isOwnerOfThisOffer = false; // Flaga dla własności tej konkretnej oferty

        // 1. Zawsze próbuj pobrać dane użytkownika, jeśli istnieje sesja, aby ustalić isIndividual
        if (session?.user?.token) {
          try {
            const userRes = await fetch(`${backUrl}/api/User`, {
              method: "GET",
              headers: { 'Authorization': `Bearer ${session.user.token}`, },
              cache: 'no-store'
            });
            if (userRes.ok) {
              const userJson = await userRes.json();
              setIsIndividual(userJson.personPerspective.isIndividual);
            }
          } catch (userErr) {
            console.warn("Nie udało się pobrać szczegółów użytkownika dla sprawdzenia isIndividual:", userErr);
          }

          // 2. Spróbuj pobrać ofertę jako CompanyUser, aby ustalić, czy jesteśmy jej właścicielem
          try {
            const companyUserRes = await fetch(`${backUrl}/api/CompanyUser/offers/${offerId}`, {
              headers: { 'Authorization': `Bearer ${session.user.token}`, },
              cache: 'no-store', // Ważne, aby pobierać najnowsze dane
            });

            if (companyUserRes.ok) {
              const companyUserJson = await companyUserRes.json();
              if (companyUserJson.items?.[0]) {
                fetchedData = companyUserJson.items[0];
                isOwnerOfThisOffer = true;
              }
            } else {
              console.log(`Pobieranie oferty ${offerId} jako CompanyUser nie powiodło się (status: ${companyUserRes.status}), próbuję jako GuestQueries.`);
            }
          } catch (e) {
            console.warn(`Błąd sieci podczas pobierania oferty ${offerId} jako CompanyUser, próbuję jako GuestQueries:`, e);
          }
        }

        // 3. Jeśli nie znaleziono danych przez CompanyUser (lub brak tokena), spróbuj jako GuestQueries
        if (!fetchedData) {
          try {
            const guestRes = await fetch(`${backUrl}/api/GuestQueries/offers/${offerId}`, { cache: 'no-store' });
            if (guestRes.ok) {
              const guestJson = await guestRes.json();
              if (guestJson.items?.[0]) {
                fetchedData = guestJson.items[0];
                isOwnerOfThisOffer = false; // Potwierdzone, że nie jest właścicielem, jeśli pobrano przez Guest
              }
            } else {
              console.log(`Pobieranie oferty ${offerId} jako GuestQueries nie powiodło się (status: ${guestRes.status}).`);
            }
          } catch (e) {
            console.warn(`Błąd sieci podczas pobierania oferty ${offerId} jako GuestQueries:`, e);
          }
        }

        // 4. Ustaw stan na podstawie ostatecznie pobranych danych
        if (fetchedData) {
          setOfferDetails(fetchedData);
          setIsOwnOffer(isOwnerOfThisOffer);
          setError(null); // Wyczyść wszelkie poprzednie błędy
        } else {
          // Jeśli nie znaleziono danych z żadnego źródła, zgłoś błąd
          throw new Error('Brak danych oferty lub oferta jest niedostępna dla tego ID.');
        }
      } catch (err) {
        if (err instanceof Error) {
          setError(err.message);
          console.error("Błąd podczas pobierania szczegółów oferty:", err);
        } else {
          setError("Wystąpił nieznany błąd podczas pobierania szczegółów oferty.");
          console.error("Wystąpił nieznany błąd:", err);
        }
      }
    };

    fetchOfferDetails();
  }, [session, offerId]); // Zależności: session i offerId


  // if (!session?.user?.token) return <div>Unauthorized</div>;
  if (error) return <div>Error: {error}</div>;
  if (!offerDetails) return <div className="text-center p-4 text-gray-600">Loading offer details...</div>;
  
  const handleDelete = async () => {
    if (!session?.user?.token) {
      alert("Autorization needed!");
      return;
    }
    if (!window.confirm('Are you sure you want to delete this offer?')) {
      return;
    }

    setIsDeleting(true);
    setError(null);

    try {
      const res = await fetch(`${backUrl}/api/CompanyUser/companies/offers/${offerId}`, {
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

    } catch (err) {
      if(err instanceof Error)
      setError(err.message);
    } finally {
      setIsDeleting(false);
    }
  };

  // if (!session?.user?.token) return <div className="text-center p-4">Unauthorized</div>;
  if (error) return <div className="text-center p-4 text-red-600">Error: {error}</div>;
  if (!offerDetails) return <div className="text-center p-4">Loading...</div>;

  const { company, branch, offer, offerTemplate, contractConditions } = offerDetails;

  // Logika grupowania i sortowania umiejętności
  const groupedSkills = offerTemplate.skills.reduce((acc, skillInfo) => {
    const typeName = skillInfo.skill.skillType.name;
    if (!acc[typeName]) {
      acc[typeName] = [];
    }
    acc[typeName].push(skillInfo);
    return acc;
  }, {} as Record<string, SkillInfo[]>);

  // Posortowanie typów umiejętności alfabetycznie
  const sortedSkillTypes = Object.keys(groupedSkills).sort((a, b) => a.localeCompare(b));
  // Posortowanie umiejętności w każdej grupie alfabetycznie
  const sortedGroupedSkills: Record<string, SkillInfo[]> = {};
  sortedSkillTypes.forEach(type => {
    sortedGroupedSkills[type] = groupedSkills[type].sort((a, b) => a.skill.name.localeCompare(b.skill.name));
  });

  return (
    <OuterContainer>
      <h1 className="text-2xl font-bold mb-4 text-center">Offer Details</h1>
      <InnerSection>

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

        <div className="mb-2 text-gray-700 dark:text-gray-300">
          <p className="font-semibold">Address:</p>
          <p>
            {branch.address.countryName} ul. {branch?.address?.streetName} {branch.address.houseNumber}
            {branch.address.apartmentNumber ? `/${branch.address.apartmentNumber}` : ''} {branch.address.cityName}
          </p>
          <p>Post Code: {branch.address.postCode}</p>

        </div>
        {branch.address.lat && branch.address.lon && (
          <GeoMap lat={branch.address.lat} lon={branch.address.lon} />
        )}

      </InnerSection>

      <div>
        <h2 className="text-2xl font-bold mb-4">Template Details</h2>
        <InnerSection>
          {Object.keys(sortedGroupedSkills).length > 0 ? (
            <div className="space-y-4">
              {sortedSkillTypes.map(skillType => (
                <div key={skillType} className="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg border border-gray-200 dark:border-gray-600">
                  <h4 className="font-semibold text-lg mb-2 text-gray-900 dark:text-gray-100 flex items-center gap-2">
                    {skillType}:
                  </h4>
                  <ul className="list-disc list-inside text-gray-700 dark:text-gray-300 ml-4 space-y-1">
                    {sortedGroupedSkills[skillType].map((s) => (
                      <li key={s.skill.skillId}>
                        {s.skill.name} {s.isRequired && <span className="font-bold text-blue-600 dark:text-blue-400 text-xs">(required)</span>}
                      </li>
                    ))}
                  </ul>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-500 italic">No skills specified for this offer template.</p>
          )}
        </InnerSection>
      </div>

      <div>
        <h2 className="text-2xl font-bold mb-4">Contract Conditions</h2>
        {contractConditions?.length > 0 ? (
          <div className="space-y-4">
            {contractConditions.map((cond, index) => (
              <div key={index} >
                <InnerSection>
                  <p><span className="font-semibold">Salary:</span> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name} ({cond.salaryTerm?.name})</p>
                  <p><span className="font-semibold">Hours/Term:</span> {cond.hoursPerTerm}</p>
                  <p><span className="font-semibold">Negotiable:</span> {cond.isNegotiable ? 'Yes' : 'No'}</p>
                  <p><span className="font-semibold">Work Modes:</span> {cond.workModes?.map((w) => w.name).join(', ')}</p>
                  <p><span className="font-semibold">Employment Types:</span> {cond.employmentTypes?.map((e) => e.name).join(', ')}</p>
                </InnerSection>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 italic">No contract conditions specified.</p>
        )}
      </div>

      {isOwnOffer && offer.statusId !== 1 && (
        <div className="mt-8 flex items-center gap-4">
          <Link
            href={`/companies/${id}/${branchId}/offer/${offerId}/edit`}
            className="inline-block bg-blue-600  p-2 rounded-md hover:bg-blue-700 transition-colors"
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

      {isIndividual && !isOwnOffer && (
        <div className="mt-8 flex items-center gap-4">
          {session?.user.token ? <Link
          href={`/companies/${id}/${branchId}/offer/${offerId}/recruite`}
          className="inline-block bg-green-600  p-2 rounded-md hover:bg-green-700 transition-colors">
            Apply
          </Link> : "Please sign in to apply"}
        </div>
      )}

    </OuterContainer>
  );
};

export default OfferDetails;