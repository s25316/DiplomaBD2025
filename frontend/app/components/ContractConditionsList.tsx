'use client'

import React from 'react';
import { useRouter, useParams } from 'next/navigation';
import { useSession } from 'next-auth/react';
import { InnerSection } from './layout/PageContainers';

interface ContractCondition {
    contractConditionId: string;
    hoursPerTerm: number;
    salaryMin: number;
    salaryMax: number;
    isNegotiable: boolean;
    salaryTerm: { name: string } | null;
    currency: { name: string } | null;
    workModes?: { name: string }[];
    employmentTypes?: { name: string }[];
  }
  
  const ContractConditionsList = ({
    contractConditions,
    onDelete,
    isOwner,
  }: {
    contractConditions: ContractCondition[];
    onDelete: (id: string) => void;
    isOwner: boolean;
  }) => {
  const { data: session } = useSession();
  const router = useRouter();
  const { id } = useParams();

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  if (!contractConditions.length) return <h2>No contract conditions available</h2>;

  return (
    <>
      <ul>
        {contractConditions.map((cond) => (
          <li key={cond.contractConditionId} >
            <InnerSection className="my-2 max-w-md">
            <p><b>Hours/Term:</b> {cond.hoursPerTerm}</p>
            <p><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name ?? "Unkown currency"} ({cond.salaryTerm?.name ?? "Unkown salary term"})</p>
            <p><b>Negotiable:</b> {cond.isNegotiable ? "Yes" : "No"}</p>
            <p><b>Work Modes:</b> {cond.workModes?.map(w => w.name).join(", ") ?? "N/A"}</p>
            <p><b>Employment Types:</b> {cond.employmentTypes?.map(e => e.name).join(", ") ?? "N/A"}</p>

            {isOwner && <div className="mt-2 flex gap-2">
              <button
                onClick={() => router.push(`/companies/${id}/contractCondition/${cond.contractConditionId}/edit`)}
                className="bg-blue-500 text-white px-2 py-1 rounded"
              >Edit</button>
              <button
              onClick={async () => {
                const confirmDelete = confirm("Are you sure you want to delete this contract condition?");
                if (!confirmDelete || !session?.user.token) return;

                const res = await fetch(`${backUrl}/api/CompanyUser/companies/contractConditions/${cond.contractConditionId}`, {
                  method: "DELETE",
                  headers: {
                    Authorization: `Bearer ${session.user.token}`,
                  },
                });

                if (res.ok) {
                  onDelete(cond.contractConditionId);
                } else {
                  alert("Failed to delete.");
                }
              }}
                className="bg-red-500 text-white px-2 py-1 rounded"
              >Delete</button>
            </div>}
            </InnerSection>
          </li>
        ))}
      </ul>
    </>
  );
};

export default ContractConditionsList;
