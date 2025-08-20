'use client';

import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import OfferForm, { SkillWithRequired } from '@/app/components/forms/OfferForm';
import { OuterContainer } from '@/app/components/layout/PageContainers';
import CancelButton from '@/app/components/buttons/CancelButton';
import { ContractConditionFormData } from '@/app/components/forms/ContractConditionForm';
import { OfferTemplate } from '@/types/offerTemplate';

export interface OfferFormData {
  offerTemplateId: string;
  publicationStart: string;
  publicationEnd: string;
  employmentLength: number;
  websiteUrl: string;
  conditionIds: string[];
}

const EditOfferPage = () => {
  const { offerId, id, branchId } = useParams();
  const { data: session } = useSession();
  const router = useRouter();
  const [offerStatusId, setOfferStatusId] = useState<number | null>(null);

  const [form, setForm] = useState<OfferFormData>({
    offerTemplateId: '',
    publicationStart: '',
    publicationEnd: '',
    employmentLength: 0,
    websiteUrl: '',
    conditionIds: [],
  });

  const [templates, setTemplates] = useState([]);
  const [parameters, setParameters] = useState([]);
  const [skills, setSkills] = useState([]);
  const [existingConditions, setExistingConditions] = useState([]);

  const [selectedConditionId, setSelectedConditionId] = useState('');
  const [includeNewCondition, setIncludeNewCondition] = useState(false);
  const [includeNewTemplate, setIncludeNewTemplate] = useState(false);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const [newTemplateForm, setNewTemplateForm] = useState({
    name: '',
    description: '',
    skills: [] as SkillWithRequired[],
  });

  useEffect(() => {
    if (!session?.user?.token || !id || !offerId) return;
    const headers = { Authorization: `Bearer ${session.user.token}` };

    const fetchAll = async () => {
        const [offerRes, tplRes, paramRes, condRes, skillsRes] = await Promise.all([
            fetch(`${backUrl}/api/CompanyUser/offers/${offerId}`, { headers }),
            fetch(`${backUrl}/api/CompanyUser/companies/${id}/offerTemplates?Page=1&ItemsPerPage=100`, { headers }),
            fetch(`${backUrl}/api/Dictionaries/contractParameters`, { headers }),
            fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions?Page=1&ItemsPerPage=100`, { headers }),
            fetch(`${backUrl}/api/Dictionaries/skills`, { headers }),
        ]);

        if (!offerRes.ok) {
        console.error("Failed to fetch offer details:", await offerRes.text());
        console.error("OfferId: ", offerId)
        return;
        }

        const offerData = await offerRes.json();
        const offer = offerData.items?.[0]?.offer;


        if (!offer) {
        console.error("Offer not found or malformed");
        return;
        }

        setForm({
        offerTemplateId: offerData.items?.[0]?.offerTemplate.offerTemplateId,
        publicationStart: offer.publicationStart,
        publicationEnd: offer.publicationEnd,
        employmentLength: offer.employmentLength,
        websiteUrl: offer.websiteUrl,
        conditionIds: offerData.items?.[0]?.contractConditions?.map((c: ContractConditions) => c.contractConditionId) || [],
        });

        setSelectedConditionId(offerData.items?.[0]?.contractConditions?.[0]?.contractConditionId || '');


        setTemplates((await tplRes.json()).items.map((i: { offerTemplate : OfferTemplate}) => i.offerTemplate));
        setParameters(await paramRes.json());
        setExistingConditions((await condRes.json()).items.map((i: { contractCondition : ContractConditions}) => i.contractCondition));
        setSkills(await skillsRes.json());
        setOfferStatusId(offerData.items?.[0]?.offer.statusId);
    };

    fetchAll();
  }, [session, id, offerId]);

  const handleConditionCreate = async (formData: ContractConditionFormData) => {
    const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/contractConditions`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify([formData]),
    });

    const result = await res.json();
    const newId = result[0]?.item?.contractConditionId;
    if (newId) setSelectedConditionId(newId);
    setIncludeNewCondition(false);
  };

  const handleTemplateCreate = async () => {
    const res = await fetch(`${backUrl}/api/CompanyUser/companies/${id}/offerTemplates`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify([newTemplateForm]),
    });

    const result = await res.json();
    const newId = result[0]?.item?.offerTemplateId;
    if (newId) setForm((prev) => ({ ...prev, offerTemplateId: newId }));
    setIncludeNewTemplate(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      ...form,
      branchId,
      conditionIds: selectedConditionId ? [selectedConditionId] : [],
    };

    const res = await fetch(`${backUrl}/api/CompanyUser/companies/offers/${offerId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      alert('Offer updated!');
      router.replace(`/companies/${id}/${branchId}/offer/${offerId}`);
    } else {
      alert('Failed to update offer.');
    }
  };

  return (
    <OuterContainer maxWidth="max-w-4xl">
      <h1 className="text-2xl font-bold mb-4 text-center">Edit Offer</h1>
      <div>
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
          statusId={offerStatusId}
        />
        <button type="submit" className="inline-block mt-6 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
          Update Offer
        </button>
        <CancelButton/>
      </form>
    </div>
    </OuterContainer>
  );
};

export default EditOfferPage;
