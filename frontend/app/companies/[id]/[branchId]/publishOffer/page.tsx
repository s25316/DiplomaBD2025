'use client';
import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import { useParams, useRouter } from 'next/navigation';
import OfferForm, { SkillWithRequired } from '@/app/components/forms/OfferForm';
import {  ContractConditionFormData } from '@/app/components/forms/ContractConditionForm';
import { OuterContainer } from '@/app/components/layout/PageContainers';
import { OfferTemplate } from '@/types/offerTemplate';
import CancelButton from '@/app/components/buttons/CancelButton';

const PublishOffer = () => {
  const { id, branchId } = useParams();
  const router = useRouter();
  const { data: session } = useSession();
  const [templates, setTemplates] = useState([]);
  const [parameters, setParameters] = useState([]);
  const [skills, setSkills] = useState([]);
  const [existingConditions, setExistingConditions] = useState([]);

  const [form, setForm] = useState({
    offerTemplateId: '',
    publicationStart: '',
    publicationEnd: '',
    employmentLength: 0,
    websiteUrl: 'https://',
    conditionIds: [] as string[],
  });

  const [selectedConditionId, setSelectedConditionId] = useState('');
  const [includeNewCondition, setIncludeNewCondition] = useState(false);
  const [includeNewTemplate, setIncludeNewTemplate] = useState(false);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const [newTemplateForm, setNewTemplateForm] = useState({
    name: '', description: '', skills: [] as SkillWithRequired[],
  });

  useEffect(() => {
    if (!session?.user?.token || !id) return;
    const headers = { Authorization: `Bearer ${session.user.token}` };

    Promise.all([
      fetch(`${backUrl}/api/CompanyUser/companies/${id}/offerTemplates?Page=1&ItemsPerPage=100`, { headers }),
      fetch(`${backUrl}/api/Dictionaries/contractParameters`, { headers }),
      fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions`, { headers }),
      fetch(`${backUrl}/api/Dictionaries/skills`, { headers }),
    ]).then(async ([tplRes, paramRes, condRes, skillsRes]) => {
      const [tplData, paramData, condData, skillData] = await Promise.all([
        tplRes.json(), paramRes.json(), condRes.json(), skillsRes.json(),
      ]);
      setTemplates(tplData.items.map((i: { offerTemplate : OfferTemplate}) => i.offerTemplate));
      setParameters(paramData);
      setExistingConditions(condData.items.map((i: { contractCondition : ContractConditions}) => i.contractCondition));
      setSkills(skillData);
    });
  }, [session, id]);

  const handleConditionCreate = async (formData: ContractConditionFormData) => {
    const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify([formData]),
    });

    if (!res.ok) return alert("Failed to create contract condition");

    const all = await refreshConditions();
    const match = all.find((c : ContractConditions) =>
      c.salaryMin === formData.salaryMin &&
      c.salaryMax === formData.salaryMax &&
      c.hoursPerTerm === formData.hoursPerTerm
    );

    if (match?.contractConditionId) {
      setSelectedConditionId(match.contractConditionId);
      alert("Contract condition created and selected.");
    } else {
      alert("Condition added, but could not auto-select.");
    }

    setIncludeNewCondition(false);
  };

  const handleTemplateCreate = async () => {
  const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/offerTemplates`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session?.user.token}`,
    },
    body: JSON.stringify([newTemplateForm]),
  });

  if (!res.ok) return alert("Failed to create offer template");

  const allTemplates = await refreshTemplates();

  const newest = allTemplates.find((t: OfferTemplate) =>
    t.name === newTemplateForm.name &&
    t.description === newTemplateForm.description
  );

  if (newest?.offerTemplateId) {
    setForm(prev => ({ ...prev, offerTemplateId: newest.offerTemplateId }));
    alert("Template created and selected.");
  } else {
    alert("Template created, but could not auto-select.");
  }

  setIncludeNewTemplate(false);
};


  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    const payload = [{
      ...form,
      branchId,
      conditionIds: selectedConditionId ? [selectedConditionId] : [],
    }];

    const res = await fetch(`${backUrl}/api/CompanyUser/companies/offers`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      alert('Offer published!');
      router.replace(`/companies/${id}/${branchId}`);
    } else {
      alert('Failed to publish offer.');
    }
  };
  const refreshTemplates = async () => {
  const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/offerTemplates?Page=1&ItemsPerPage=100`, {
    headers: { Authorization: `Bearer ${session?.user.token}` },
  });
  const data = await res.json();
  const all = data.items.map((i: { offerTemplate : OfferTemplate}) => i.offerTemplate).filter((tpl : OfferTemplate) => !!tpl.offerTemplateId);
  setTemplates(all);
  return all;
};

const refreshConditions = async () => {
  const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions?Page=1&ItemsPerPage=100`, {
    headers: { Authorization: `Bearer ${session?.user.token}` },
  });
  const data = await res.json();
  const all = data.items.map((i: { contractCondition : ContractConditions}) => i.contractCondition).filter((c: ContractConditions) => c.companyId === id);
  setExistingConditions(all);
  return all;
};


  return (
    <OuterContainer>
      <h1 className="text-2xl font-bold mb-4 text-center">Publish Offer</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <OfferForm
          form={form}
          setForm={setForm}
          templates={templates}
          parameters={parameters}
          skills={skills}
          existingConditions={existingConditions}
          selectedConditionId={selectedConditionId}
          setSelectedConditionId={setSelectedConditionId}
          includeNewCondition={includeNewCondition}
          setIncludeNewCondition={setIncludeNewCondition}
          onConditionCreate={handleConditionCreate}
          includeNewTemplate={includeNewTemplate}
          setIncludeNewTemplate={setIncludeNewTemplate}
          newTemplateForm={newTemplateForm}
          setNewTemplateForm={setNewTemplateForm}
          onTemplateCreate={handleTemplateCreate}
          statusId= {null}
        />
        <button type="submit" className="bg-blue-600 text-white p-2 rounded mt-4">Publish Offer</button>
      </form>
      <br/>
      <CancelButton/>
    </OuterContainer>
  );
};

export default PublishOffer;


