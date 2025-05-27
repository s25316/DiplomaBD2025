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

  useEffect(() => {
    if (!session?.user?.token || !id) return;

    const headers = {
      Authorization: `Bearer ${session.user.token}`,
    };

    const fetchAll = async () => {
      const [c, b, t, cond] = await Promise.all([
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/branches?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/offerTemplates?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
        fetch(`http://localhost:8080/api/CompanyUser/companies/${id}/contractConditions?Page=${branchPage}&ItemsPerPage=${branchPerPage}`, { headers }),
      ]);

      if (c.ok) setCompany((await c.json()).items[0]);
      if (b.ok){
        const data =await b.json()
        setBranches((data).items);
        setBranchTotal(data.totalCount);
      }
      if (t.ok){
        const data =await t.json();
        setTemplates((data).items.map((item: any) => item.offerTemplate));
        setTemplateTotal (data.totalCount);
      }
      if (cond.ok) {
        const data = await cond.json();
        const all = (data).items.map((item: any) => item.contractCondition);
        setConditions(all.filter((cc: any) => cc.companyId === id));
        setConditionTotal(data.totalCount);
      }
    };

    fetchAll();
  }, [session, id, branchPage,branchPerPage,templatePage,templatePerPage, conditionPage,conditionPerPage]);

  return (
    <div>
      <h1>Company Details</h1>
      <CompanyInfo company={company} />

      {company && (
        <>
          <button
            onClick={() => setShowEditForm(prev => !prev)}
            className="bg-blue-600 text-white px-2 py-1 rounded"
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
          <div className="mt-2"><CreateBranchButton /></div>
          <h2 className="text-lg font-semibold">Branches</h2>
          <p className="mt-2 text-sm text-gray-600">
            Showing {branches.length} of {branchTotal} branches
          </p>
          <SelectItemsPerPage value={branchPerPage} onChange={(val) => { setBranchPerPage(val); setBranchPage(1); }} />
          <BranchesList branches={branches} companyId={id as string} />
          <Pagination
            page={branchPage}
            onPrev={() => setBranchPage(p => Math.max(1, p - 1))}
            onNext={() => setBranchPage(p => p + 1)}
            isNextDisabled={branches.length < branchPerPage ||  branchPerPage * branchPage >= branchTotal}
          />
          
        </div>
      )}
      {activeTab === 'contracts' && (
        <div className="shadow-md rounded p-4">
          <div className="mt-2"><CreateContractConditionButton /></div>
          <h2 className="text-lg font-semibold">Contract Conditions</h2>
          <p className="mt-2 text-sm text-gray-600">
            Showing {conditions.length} of {conditionTotal} conditions
          </p>
          <SelectItemsPerPage value={conditionPerPage} onChange={(val) => { setConditionPerPage(val); setConditionPage(1); }} />
          <ContractConditionsList
            contractConditions={conditions}
            onDelete={(id: string) => setConditions(prev => prev.filter(c => c.contractConditionId !== id))}
          />
          <Pagination
            page={branchPage}
            onPrev={() => setConditionPage(p => Math.max(1, p - 1))}
            onNext={() => setConditionPage(p => p + 1)}
            isNextDisabled={conditions.length < conditionPerPage ||  conditionPerPage * conditionPage >= conditionTotal}
          />
          
        </div>
      )}

      {activeTab === 'templates' && (
        <div className="shadow-md rounded p-4">
          <div className="mt-2"><CreateOfferTemplateButton /></div>
          <h2 className="text-lg font-semibold">Offer Templates</h2>
          <h2 className="mt-2 text-sm text-gray-600">
            Showing {templates.length} of {templateTotal} templates
          </h2>
          <SelectItemsPerPage value={templatePerPage} onChange={(val) => { setTemplatePerPage(val); setTemplatePage(1); }} />
          <OfferTemplatesList
            templates={templates}
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
    </div>
  );
};

export default CompanyDetails;
