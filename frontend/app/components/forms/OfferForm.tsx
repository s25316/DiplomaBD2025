'use client';
import React from 'react';
import ContractConditionForm, { ContractConditionFormData, ContractParameter } from '@/app/components/forms/ContractConditionForm';
import OfferTemplateForm from '@/app/components/forms/OfferTemplateForm';
import { InnerSection } from '../layout/PageContainers';
import { OfferFormData } from '@/app/companies/[id]/[branchId]/offer/[offerId]/edit/page';
import { OfferTemplate } from '@/types/offerTemplate';

export interface SkillWithRequired {
    skillId: number;
    name: string,
    skill: {
        name: string;
        skillType: { name: string };
    };
    isRequired: boolean;
    skillType: {
        name: string
    }
}

interface OfferFormProps {
    form: OfferFormData;
    setForm: (updater: (prev: OfferFormData) => OfferFormData) => void;
    templates: OfferTemplate[];
    parameters: ContractParameter[];
    skills: SkillWithRequired[];
    existingConditions: ContractConditions[];
    selectedConditionId: string;
    setSelectedConditionId: (id: string) => void;
    includeNewCondition: boolean;
    setIncludeNewCondition: (val: boolean) => void;
    onConditionCreate: (formData: ContractConditionFormData) => Promise<void>;
    includeNewTemplate: boolean;
    setIncludeNewTemplate: (val: boolean) => void;
    newTemplateForm: {
        name: string,
        description: string,
        skills: SkillWithRequired[],
    };
    setNewTemplateForm: (updater: (prev: {
        name: string,
        description: string,
        skills: SkillWithRequired[],
    }) => {
        name: string,
        description: string,
        skills: SkillWithRequired[],
    }) => void;
    onTemplateCreate: () => void;
    statusId: number | null;
}

const OfferForm = ({
    form, setForm,
    templates, parameters, skills, existingConditions,
    selectedConditionId, setSelectedConditionId,
    includeNewCondition, setIncludeNewCondition,
    onConditionCreate,
    includeNewTemplate, setIncludeNewTemplate,
    newTemplateForm, setNewTemplateForm,
    onTemplateCreate,
    statusId,
}: OfferFormProps) => {
    const isScheduled = statusId === 3;
    const isActive = statusId === 2;
    const isExpired = statusId === 1;
    const handleTemplateChange = (field: 'name' | 'description', value: string) => {
        setNewTemplateForm(prev => ({ ...prev, [field]: value }));
    };
    const handleSkillToggle = (skillId: number, checked: boolean) => {
        setNewTemplateForm(prev => ({
            ...prev, skills: checked ? [...prev.skills,
            { ...skills.find(s => s.skillId === skillId)!, isRequired: true }] : prev.skills.filter((s: SkillWithRequired) => s.skillId !== skillId),
        }));
    };

    const handleSkillRequiredToggle = (skillId: number, isRequired: boolean) => {
        setNewTemplateForm(prev => ({
            ...prev, skills: prev.skills.map((s: SkillWithRequired) =>
                s.skillId === skillId ? { ...s, isRequired } : s),
        }));
    };

    return (
        <div className="flex flex-col gap-4 p-4">
            <>
                {isExpired && (
                    <div className="text-red-600 font-semibold mb-4">
                        Editing is not allowed. This offer is expired.
                    </div>
                )}

                {isExpired ? null : (
                    <>
                        {(isScheduled || statusId === null) && (
                            <>
                                {(statusId != null) && (<p>Status: {statusId}</p>)}
                                <InnerSection className="flex flex-col gap-4 p-4">
                                    <h2 className="text-2xl font-bold mb-4">Contract Condition</h2>
                                    <select
                                        className="global-field-style"
                                        value={selectedConditionId}
                                        onChange={(e) => setSelectedConditionId(e.target.value)}>
                                        <option value="">-- None --</option>
                                        {existingConditions.map((c: ContractConditions) => (
                                            <option key={c.contractConditionId} value={c.contractConditionId}>
                                                [{c.hoursPerTerm}h] {c.salaryMin}-{c.salaryMax} {c.currency?.name}
                                            </option>
                                        ))}
                                    </select>

                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={includeNewCondition}
                                            onChange={(e) => setIncludeNewCondition(e.target.checked)}
                                            disabled={!!selectedConditionId}

                                        /> Create New Condition
                                    </label>

                                    {includeNewCondition && (
                                        <ContractConditionForm
                                            parameters={parameters}
                                            onSubmit={onConditionCreate}
                                            submitText="Create Condition"
                                        />
                                    )}
                                    {selectedConditionId && (
                                        <div className="text-2xl font-bold mb-4">
                                            <h4>Selected Contract Details</h4>
                                            {(() => {
                                                const cond = existingConditions.find(c => c.contractConditionId === selectedConditionId);
                                                if (!cond) return <p className="text-gray-500">Loading...</p>;
                                                return (
                                                    <ul className="text-sm list-disc list-inside text-gray-700 dark:text-gray-400">
                                                        <li><b>Hours/Term:</b> {cond.hoursPerTerm}</li>
                                                        <li><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name}</li>
                                                        <li><b>Negotiable:</b> {cond.isNegotiable ? 'Yes' : 'No'}</li>
                                                        <li><b>Salary Term:</b> {cond.salaryTerm?.name}</li>
                                                        <li><b>Work Modes:</b> {cond.workModes?.map((w: { name: string }) => w.name).join(', ')}</li>
                                                        <li><b>Employment Types:</b> {cond.employmentTypes?.map((e: { name : string}) => e.name).join(', ')}</li>
                                                    </ul>
                                                );
                                            })()}
                                        </div>
                                    )}
                                </InnerSection>

                                <InnerSection className="flex flex-col gap-4 p-4">
                                    <h2 className="text-2xl font-bold mb-2">Offer Template</h2>

                                    <select
                                        className="global-field-style"
                                        required
                                        value={form.offerTemplateId}
                                        onChange={(e) => setForm(prev => ({ ...prev, offerTemplateId: e.target.value }))}
                                    >
                                        <option value="">-- Select Template --</option>
                                        {templates.map((tpl) => (
                                            <option key={tpl.offerTemplateId} value={tpl.offerTemplateId}>
                                                {tpl.name}
                                            </option>
                                        ))}
                                    </select>

                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={includeNewTemplate}
                                            onChange={(e) => setIncludeNewTemplate(e.target.checked)}
                                            disabled={!!form.offerTemplateId}
                                        /> Create New Offer Template
                                    </label>

                                    {includeNewTemplate && (
                                        <OfferTemplateForm
                                            name={newTemplateForm.name}
                                            description={newTemplateForm.description}
                                            skills={skills}
                                            selectedSkills={newTemplateForm.skills}
                                            onChange={handleTemplateChange}
                                            onSkillToggle={handleSkillToggle}
                                            onSkillRequiredToggle={handleSkillRequiredToggle}
                                            onSubmit={onTemplateCreate}
                                            submitText="Create Template"
                                        />
                                    )}
                                    {form.offerTemplateId && (
                                        <div className="text-2xl font-bold mb-4">
                                            <h4>Selected Offer Template</h4>
                                            {(() => {
                                                const tpl = templates.find(t => t.offerTemplateId === form.offerTemplateId);
                                                if (!tpl && templates.length === 0) return <p className="text-gray-500">Loading templates...</p>;
                                                if (!tpl) return <p className="text-red-500">Template not found</p>;

                                                return (
                                                    <ul className="text-sm list-disc list-inside text-gray-700 dark:text-gray-400">
                                                        <li><b>Name:</b> {tpl.name}</li>
                                                        <li><b>Description:</b> {tpl.description}</li>
                                                        <li><b>Skills:</b>
                                                            <ul className="list-disc pl-6">
                                                                {(tpl.skills as SkillWithRequired[]).map((s, idx) => (
                                                                    <li key={idx}>
                                                                        {s.skill.name} ({s.skill.skillType.name}) {s.isRequired ? '(required)' : ''}
                                                                    </li>
                                                                ))}

                                                            </ul>
                                                        </li>
                                                    </ul>
                                                );
                                            })()}
                                        </div>
                                    )}
                                </InnerSection>
                            </>
                        )}
                        <InnerSection className="flex flex-col gap-4 p-4">
                            {(isScheduled || statusId === null) && (
                                <>
                                    <label>Publication Start</label>
                                    <input
                                        type="datetime-local"
                                        value={form.publicationStart}
                                        onChange={(e) => setForm((prev) => ({ ...prev, publicationStart: e.target.value }))}
                                        required
                                        className="global-field-style"
                                    />
                                </>
                            )}


                            {(isScheduled || isActive || statusId === null) && (
                                <>
                                    <label>Publication End</label>
                                    <input
                                        type="datetime-local"
                                        value={form.publicationEnd}
                                        onChange={(e) => setForm((prev) => ({ ...prev, publicationEnd: e.target.value }))}
                                        required
                                        className="global-field-style"
                                    />

                                    <label>Employment Length (months)</label>
                                    <input
                                        type="number"
                                        value={form.employmentLength}
                                        onChange={(e) => setForm((prev) => ({ ...prev, employmentLength: Number(e.target.value) }))}
                                        min="0"
                                        className="global-field-style"
                                    />

                                    <label>Website URL</label>
                                    <input
                                        type="url"
                                        value={form.websiteUrl}
                                        onChange={(e) => setForm((prev) => ({ ...prev, websiteUrl: e.target.value }))}
                                        required
                                        className="global-field-style"
                                    />
                                </>
                            )}
                        </InnerSection>
                    </>
                )}
            </>
        </div>
    );
};

export default OfferForm;