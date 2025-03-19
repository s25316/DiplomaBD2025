import CreateBranchButton from '@/app/components/buttons/CreateBranchButton';
import React from 'react';
import CreateOfferTemplateButton from '@/app/components/buttons/CreateOfferTemplateButton';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import Link from 'next/link';

interface BranchesProfile {
  branchId: string;
  name: string;
}

interface Templates {
  offerTemplateId: string;
  name: string;
}

interface CompanyDetails {
  companyId: string;
  name: string;
  description: string;
  regon: string;
  nip: string;
  krs: string;
  websiteUrl?: string;
  created: string;
}

const CompanyDetails = async ({ params }: { params: { id: string } }) => {
  const session = await getServerSession(authOptions);
  const { id } = params;

  let companyDetails: CompanyDetails | null = null;
  let branches: BranchesProfile[] = [];
  let offerTemplates: Templates[] = [];

  if (session?.user.token) {
    // Pobranie danych firmy
    const companyRes = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`,
      },
    });

    if (companyRes.ok) {
      const data = await companyRes.json();
      companyDetails = data.items[0]; // Pobieramy pierwszy obiekt z `items`
    }

    // Pobranie oddziałów
    const branchesRes = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/branches`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`,
      },
    });

    if (branchesRes.ok) {
      let tmp = await branchesRes.json();
      branches = tmp.items;
    }

    // Pobranie szablonów ofert
    const templatesRes = await fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`,
      },
    });

    if (templatesRes.ok) {
      const data = await templatesRes.json();
      offerTemplates = data.items.map((item: any) => ({
        offerTemplateId: item.offerTemplate.offerTemplateId,
        name: item.offerTemplate.name,
      }));
    }
  }

  return (
    <div>
      <h1>Company Details</h1>
      {companyDetails ? (
        <>
          <p><b>Name:</b> {companyDetails.name}</p>
          <p><b>Description:</b> {companyDetails.description}</p>
          <p><b>REGON:</b> {companyDetails.regon}</p>
          <p><b>NIP:</b> {companyDetails.nip}</p>
          <p><b>KRS:</b> {companyDetails.krs}</p>
          <p><b>Created:</b> {new Date(companyDetails.created).toLocaleDateString()}</p>
          {companyDetails.websiteUrl && (
            <p>
              <b>Website:</b>{" "}
              <a href={companyDetails.websiteUrl.match(/https?:\/\/[^\s]+/g)?.[0]} target="_blank" rel="noopener noreferrer">
                {companyDetails.websiteUrl.match(/https?:\/\/[^\s]+/g)?.[0]}
              </a>
            </p>
          )}
        </>
      ) : (
        <p>Loading company details...</p>
      )}

      {branches.length > 0 ? (
        <>
          <h2>Branches:</h2>
          <ul>
            {branches.map((value) => (
              <li key={value.branchId}>
                <Link href={`/companies/${id}/${value.branchId}`}><b>Name: {value.name}</b></Link>
              </li>
            ))}
          </ul>
        </>
      ) : (
        <h2>No branches available</h2>
      )}

      {offerTemplates.length > 0 ? (
        <>
          <h2>Offer Templates:</h2>
          <ul>
            {offerTemplates.map((value) => (
              <li key={value.offerTemplateId}>
                <Link href={`/companies/${id}/templates/${value.offerTemplateId}`}><b>Name: {value.name}</b></Link>
              </li>
            ))}
          </ul>
        </>
      ) : (
        <h2>No templates available</h2>
      )}

      <br />
      <CreateBranchButton />
      <br />
      <CreateOfferTemplateButton />
    </div>
  );
};

export default CompanyDetails;
