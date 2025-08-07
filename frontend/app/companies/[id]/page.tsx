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
import SelectItemsPerPage from '@/app/components/SelectItemsPerPage';
import Pagination from '@/app/components/Pagination';
import { InnerSection, OuterContainer } from '@/app/components/layout/PageContainers';

interface OfferTemplate {
  offerTemplateId: string;
  name: string;
}
interface Branch {
  branchId: string;
  name: string;
  address: {
    cityName: string;
    streetName: string | null;
  }
}
// interface Company{
//   name: string;
//   description: string;
//   regon: string;
//   nip: string;
//   krs: string;
//   created: string;
//   websiteUrl: string;
// }
interface ContractCondition {
  companyId: string;
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

  const [company, setCompany] = useState<Company | null>(null);
  const [branches, setBranches] = useState<Branch[]>([]);
  const [templates, setTemplates] = useState<OfferTemplate[]>([]);
  const [conditions, setConditions] = useState<ContractCondition[]>([]);
  const [showEditForm, setShowEditForm] = useState(false);

  const [branchPage, setBranchPage] = useState(1);
  const [branchPerPage, setBranchPerPage] = useState(10);
  const [branchTotal, setBranchTotal] = useState(0);

  const [templatePage, setTemplatePage] = useState(1);
  const [templatePerPage, setTemplatePerPage] = useState(10);
  const [templateTotal, setTemplateTotal] = useState(0);

  const [conditionPage, setConditionPage] = useState(1);
  const [conditionPerPage, setConditionPerPage] = useState(10);
  const [conditionTotal, setConditionTotal] = useState(0);

  const [activeTab, setActiveTab] = useState<'branches' | 'contracts' | 'templates'>('branches');

  const [isOwner, setIsOwner] = useState(Boolean(session))

  useEffect(() => {
    if (!id) return;

    let headers = {}
    let apiUrl = ''

    if (session?.user.token && isOwner) {
      headers = {
        Authorization: `Bearer ${session.user.token}`,
      };
      apiUrl = 'http://localhost:8080/api/CompanyUser/'
    }else{
      apiUrl = 'http://localhost:8080/api/GuestQueries/'
    }

    const fetchAll = async () => {
      const [c, b, t, cond] = await Promise.all([
        fetch(`${apiUrl}companies/${id}`, { headers }),
        fetch(`${apiUrl}companies/${id}/branches?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
        fetch(`${apiUrl}companies/${id}/offerTemplates?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
        fetch(`${apiUrl}companies/${id}/contractConditions?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
      ]);

      if (!c.ok) {
        setIsOwner(false);
        return;
      }

      if (c.ok) setCompany((await c.json()).items[0]);
      if (b.ok) {
        const data = await b.json()
        setBranches((data).items.map((item: { branch: Branch }) => item.branch));
        setBranchTotal(data.totalCount);
      }
      if (t.ok) {
        const data = await t.json();
        setTemplates((data).items.map((item: { offerTemplate: OfferTemplate }) => item.offerTemplate));
        setTemplateTotal(data.totalCount);
      }
      if (cond.ok) {
        const data = await cond.json();
        const all = (data).items.map((item: { contractCondition: ContractCondition }) => item.contractCondition);
        setConditions(all.filter((cc: ContractCondition) => cc.companyId === id));
        setConditionTotal(data.totalCount);
      }
    };


    fetchAll();
  }, [session, id, branchPage, branchPerPage, templatePage, templatePerPage, conditionPage, conditionPerPage, isOwner]);

  return (
    <OuterContainer>
      <h1 className="text-2xl font-bold">Company Details</h1>
      <InnerSection>
        <CompanyInfo company={company} />

        {company && (
          <>
            {isOwner && <button
              onClick={() => setShowEditForm(prev => !prev)}
              className="bg-blue-600 text-white px-2 py-1 rounded"
            >
              {showEditForm ? 'Cancel Edit' : 'Edit Company'}
            </button>}

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
      </InnerSection>
      <br />

      <div className="flex space-x-4 border-b pb-2 mb-4">
        <button
          onClick={() => setActiveTab('branches')}
          className={`px-4 py-2 rounded-t ${activeTab === 'branches' ? 'bg-blue-600 text-white' : 'bg-blue-400'}`}
        >
          Branches
        </button>
        <button
          onClick={() => setActiveTab('contracts')}
          className={`px-4 py-2 rounded-t ${activeTab === 'contracts' ? 'bg-blue-600 text-white' : 'bg-blue-400'}`}
        >
          Contract Conditions
        </button>
        <button
          onClick={() => setActiveTab('templates')}
          className={`px-4 py-2 rounded-t ${activeTab === 'templates' ? 'bg-blue-600 text-white' : 'bg-blue-400'}`}
        >
          Offer Templates
        </button>
      </div>

      {activeTab === 'branches' && (
        <div className="shadow-md rounded p-4">
          {isOwner && <div className="mt-2"><CreateBranchButton /></div>}
          <p className="mt-2 text-sm text-gray-600">
            Showing {branches.length} of {branchTotal} branches
          </p>
          <SelectItemsPerPage value={branchPerPage} onChange={(val) => { setBranchPerPage(val); setBranchPage(1); }} />
          <BranchesList
            branches={branches} companyId={id as string} isOwner={isOwner}
            onDelete={(id: string) => setBranches(prev => prev.filter(b => b.branchId !== id))}
          />
          <Pagination
            page={branchPage}
            onPrev={() => setBranchPage(p => Math.max(1, p - 1))}
            onNext={() => setBranchPage(p => p + 1)}
            isNextDisabled={branches.length < branchPerPage || branchPerPage * branchPage >= branchTotal}
          />

        </div>
      )}
      {activeTab === 'contracts' && (
        <div className="shadow-md rounded p-4">
          {isOwner && <div className="mt-2"><CreateContractConditionButton /></div>}
          <p className="mt-2 text-sm text-gray-600">
            Showing {conditions.length} of {conditionTotal} conditions
          </p>
          <SelectItemsPerPage value={conditionPerPage} onChange={(val) => { setConditionPerPage(val); setConditionPage(1); }} />
          <ContractConditionsList
            contractConditions={conditions} isOwner={isOwner}
            onDelete={(id: string) => setConditions(prev => prev.filter(c => c.contractConditionId !== id))}
          />
          <Pagination
            page={branchPage}
            onPrev={() => setConditionPage(p => Math.max(1, p - 1))}
            onNext={() => setConditionPage(p => p + 1)}
            isNextDisabled={conditions.length < conditionPerPage || conditionPerPage * conditionPage >= conditionTotal}
          />

        </div>
      )}

      {activeTab === 'templates' && (
        <div className="shadow-md rounded p-4">
          {isOwner && <div className="mt-2"><CreateOfferTemplateButton /></div>}
          <h2 className="mt-2 text-sm text-gray-600">
            Showing {templates.length} of {templateTotal} templates
          </h2>
          <SelectItemsPerPage value={templatePerPage} onChange={(val) => { setTemplatePerPage(val); setTemplatePage(1); }} />
          <OfferTemplatesList
            templates={templates} isOwner={isOwner}
            onDelete={(id: string) => setTemplates(prev => prev.filter(t => t.offerTemplateId !== id))}
          />
          <Pagination
            page={templatePage}
            onPrev={() => setTemplatePage(p => Math.max(1, p - 1))}
            onNext={() => setTemplatePage(p => p + 1)}
            isNextDisabled={templates.length < templatePerPage || templatePerPage * templatePage >= templateTotal}
          />
        </div>
      )}
    </OuterContainer>
  );
};

export default CompanyDetails;
