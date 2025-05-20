'use client';
import React from 'react';
import ContractConditionForm, { ContractConditionFormData, ContractParameter } from '@/app/components/forms/ContractConditionForm';
import OfferTemplateForm from '@/app/components/forms/OfferTemplateForm';

interface OfferFormProps {
    form: any;
    setForm: (updater: (prev: any) => any) => void;
    templates: any[];
    parameters: ContractParameter[];
    skills: any[];
    existingConditions: any[];
    selectedConditionId: string;
    setSelectedConditionId: (id: string) => void;
    includeNewCondition: boolean;
    setIncludeNewCondition: (val: boolean) => void;
    onConditionCreate: (formData: ContractConditionFormData) => void;
    includeNewTemplate: boolean;
    setIncludeNewTemplate: (val: boolean) => void;
    newTemplateForm: any;
    setNewTemplateForm: (updater: (prev: any) => any) => void;
    onTemplateCreate: () => void;
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
}: OfferFormProps) => {
    const handleTemplateChange = (field: 'name' | 'description', value: string) => {
        setNewTemplateForm(prev => ({ ...prev, [field]: value }));
    };
    const handleSkillToggle = (skillId: number, checked: boolean) => {
        setNewTemplateForm(prev => ({
            ...prev, skills: checked ? [...prev.skills,
            { skillId, isRequired: true }] : prev.skills.filter((s: any) => s.skillId !== skillId),
        }));
    };

    const handleSkillRequiredToggle = (skillId: number, isRequired: boolean) => {
        setNewTemplateForm(prev => ({
            ...prev, skills: prev.skills.map((s: any) => 
                s.skillId === skillId ? { ...s, isRequired } : s ),
        }));
    };

    return (
        <>
        <label className="font-semibold">Contract Condition</label>
        <select value={selectedConditionId} onChange={(e) => setSelectedConditionId(e.target.value)}>
            <option value="">-- None --</option>
            {existingConditions.map((c: any) => (
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
            <div className="font-semibold mb-2">
                <h4>Selected Contract Details</h4>
                {(() => {
                const cond = existingConditions.find(c => c.contractConditionId === selectedConditionId);
                if (!cond) return <p className="text-gray-500">Loading...</p>;
                return (
                    <ul className="text-sm list-disc list-inside text-gray-700">
                    <li><b>Hours/Term:</b> {cond.hoursPerTerm}</li>
                    <li><b>Salary:</b> {cond.salaryMin} – {cond.salaryMax} {cond.currency?.name}</li>
                    <li><b>Negotiable:</b> {cond.isNegotiable ? 'Yes' : 'No'}</li>
                    <li><b>Salary Term:</b> {cond.salaryTerm?.name}</li>
                    <li><b>Work Modes:</b> {cond.workModes?.map((w: any) => w.name).join(', ')}</li>
                    <li><b>Employment Types:</b> {cond.employmentTypes?.map((e: any) => e.name).join(', ')}</li>
                    </ul>
                );
                })()}
            </div>
            )}
        <hr className="my-4" />

        <label className="font-semibold">Offer Template</label>
        <select
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
        <div className="font-semibold mb-2">
            
            <h4>Selected Offer Template</h4>
            {(() => {
            const tpl = templates.find(t => t.offerTemplateId === form.offerTemplateId);
            if (!tpl && templates.length === 0) return <p className="text-gray-500">Loading templates...</p>;
            if (!tpl) return <p className="text-red-500">Template not found</p>;

            return (
                <ul className="text-sm list-disc list-inside text-gray-700">
                <li><b>Name:</b> {tpl.name}</li>
                <li><b>Description:</b> {tpl.description}</li>
                <li><b>Skills:</b>
                    <ul className="list-disc pl-6">
                    {tpl.skills.map((s, idx) => (
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
        <label>Publication Start</label>
        <input
            type="datetime-local"
            value={form.publicationStart}
            onChange={(e) => setForm((prev) => ({ ...prev, publicationStart: e.target.value }))}
            required
        />

        <label>Publication End</label>
        <input
            type="datetime-local"
            value={form.publicationEnd}
            onChange={(e) => setForm((prev) => ({ ...prev, publicationEnd: e.target.value }))}
            required
        />

        <label>Employment Length (months)</label>
        <input
            type="number"
            value={form.employmentLength}
            onChange={(e) => setForm((prev) => ({ ...prev, employmentLength: Number(e.target.value) }))}
        />

        <label>Website URL</label>
        <input
            type="url"
            value={form.websiteUrl}
            onChange={(e) => setForm((prev) => ({ ...prev, websiteUrl: e.target.value }))}
            required
        />
    </>
  );
};

export default OfferForm;
