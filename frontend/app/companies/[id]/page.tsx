import CreateBranchButton from '@/app/components/buttons/CreateBranchButton';
import React from 'react';
import CreateOfferTemplateButton from '@/app/components/buttons/CreateOfferTemplateButton';
import { getServerSession } from 'next-auth';
import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import Link from 'next/link';

interface BranchesProfile {
  branch: {
    branchId: string;
    name: string;
  }
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
interface ContractCondition {
  contractConditionId: string;
  hoursPerTerm: number;
  salaryMin: number;
  salaryMax: number;
  salaryTerm: {
    name: string;
  };
  currency: {
    name: string;
  };
  isNegotiable: boolean;
  workModes: { name: string }[];
  employmentTypes: { name: string }[];
}

const CompanyDetails = async ({ params }: { params: { id: string } }) => {
  const session = await getServerSession(authOptions);
  const { id } = await params;

  let companyDetails: CompanyDetails | null = null;
  let branches: BranchesProfile[] = [];
  let offerTemplates: Templates[] = [];
  let contractConditions: ContractCondition[] = [];


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
    const conditionsRes = await fetch(`http://localhost:8080/api/CompanyUser/contractConditions`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.user.token}`,
      },
    });
    
    if (conditionsRes.ok) {
      const data = await conditionsRes.json();
      contractConditions = data.items
        .map((item: any) => item.contractCondition)
        .filter((cond: any) => cond.companyId === id);
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

      <br></br>
      {branches.length > 0 ? (
        <>
          <h2>Branches:</h2>
          <ul>
            {branches.map((value) => (
              <li key={value.branch.branchId}>
                <Link href={`/companies/${id}/${value.branch.branchId}`}><b>Name: {value.branch.name}</b></Link>
              </li>
            ))}
          </ul>
        </>
      ) : (
        <h2>No branches available</h2>
      )}
      <br></br>

      {contractConditions.length > 0 ? (
        <>
          <h2 className="mt-6">Contract Conditions:</h2>
          <ul>
            {contractConditions.map((cond) => (
              <li key={cond.contractConditionId} className="border p-3 rounded my-2">
                <p><b>Hours/Term:</b> {cond.hoursPerTerm}</p>
                <p><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond?.currency?.name} ({cond?.salaryTerm?.name})</p>
                <p><b>Negotiable:</b> {cond.isNegotiable ? "Yes" : "No"}</p>
                <p><b>Work Modes:</b> {cond.workModes.map(w => w.name).join(", ")}</p>
                <p><b>Employment Types:</b> {cond.employmentTypes.map(e => e.name).join(", ")}</p>
              </li>
            ))}
          </ul>
        </>
      ) : (
        <h2 className="mt-6">No contract conditions available</h2>
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
