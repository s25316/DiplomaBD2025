'use client';
import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import { useParams } from 'next/navigation';

import CompanyInfo from '../../components/CompanyInfo';
import BranchesList from '../../components/BranchesList';
import ContractConditionsList from '../../components/ContractConditionsList';
import OfferTemplatesList from '../../components/OfferTemplatesList';
import CreateBranchButton from '@/app/components/buttons/CreateBranchButton';
import CreateOfferTemplateButton from '@/app/components/buttons/CreateOfferTemplateButton';
import CreateContractConditionButton from '@/app/components/buttons/CreateContractConditionButton';
import EditCompanyForm from '@/app/components/forms/EditCompanyForm';

interface OfferTemplate {
  offerTemplateId: string;
  name: string;
}
interface ContractCondition {
  contractConditionId: string;
  hoursPerTerm: number;
  salaryMin: number;
  salaryMax: number;
  isNegotiable: boolean;
  salaryTerm: { name: string };
  currency: { name: string };
  workModes?: { name: string }[];
  employmentTypes?: { name: string }[];
}


const CompanyDetails = () => {
  const { data: session } = useSession();
  const { id } = useParams();

  const [company, setCompany] = useState(null);
  const [branches, setBranches] = useState([]);
  const [templates, setTemplates] = useState<OfferTemplate[]>([]);
  const [conditions, setConditions] = useState<ContractCondition[]>([]);
  const [showEditForm, setShowEditForm] = useState(false);

  useEffect(() => {
    if (!session?.user?.token || !id) return;

    const headers = {
      Authorization: `Bearer ${session.user.token}`,
    };

    const fetchAll = async () => {
      const [c, b, t, cond] = await Promise.all([
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/branches?Page=1&ItemsPerPage=100`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates?Page=1&ItemsPerPage=100`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions`, { headers }),
      ]);

      if (c.ok) setCompany((await c.json()).items[0]);
      if (b.ok) setBranches((await b.json()).items);
      if (t.ok) setTemplates((await t.json()).items.map((item: any) => item.offerTemplate));
      if (cond.ok) {
        const all = (await cond.json()).items.map((item: any) => item.contractCondition);
        setConditions(all.filter((cc: any) => cc.companyId === id));
      }
    };

    fetchAll();
  }, [session, id]);

  return (
    <div>
      <h1>Company Details</h1>
      <CompanyInfo company={company} />

      {company && (
        <>
          <button
            onClick={() => setShowEditForm(prev => !prev)}
            className="bg-blue-500 text-white px-2 py-1 rounded"
          >
            {showEditForm ? 'Cancel Edit' : 'Edit Company'}
          </button>

          {showEditForm && (
            <EditCompanyForm
              company={company}
              companyId={id as string}
              onUpdated={(updated) => {
                setCompany(updated);
                setShowEditForm(false);
              }}
            />
          )}
        </>
      )}
      <br/>
      <br/>

      <BranchesList branches={branches} companyId={id as string} />
      <br />
      

      <ContractConditionsList
        contractConditions={conditions}
        onDelete={(id: string) => setConditions(prev => prev.filter(c => c.contractConditionId !== id))}
      />

      <OfferTemplatesList
        templates={templates}
        onDelete={(id: string) => setTemplates(prev => prev.filter(t => t.offerTemplateId !== id))}
      />

      <CreateBranchButton /><br/>
      <CreateOfferTemplateButton /><br/>
      <CreateContractConditionButton />
    </div>
  );
};

export default CompanyDetails;
