'use client';
import React, { useState } from 'react';
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

    // Nowe stany dla błędów walidacji dat
    const [publicationStartError, setPublicationStartError] = useState<string | null>(null);
    const [publicationEndError, setPublicationEndError] = useState<string | null>(null);

    // Funkcja do walidacji dat
    const validateDates = (start: string, end: string) => {
        let isValid = true;
        const now = new Date();
        // Dodaje 1 minutę do obecnego czasu, aby zapewnić, że start jest w przyszłości
        const minStartTime = new Date(now.getTime() + 60 * 1000); // 60 sekund * 1000 ms = 1 minuta

        const startDate = new Date(start);
        const endDate = new Date(end);

        // Walidacja daty rozpoczęcia publikacji
        if (start && startDate <= minStartTime) {
        setPublicationStartError('Publication start must be at least 1 minute in the future.');
        isValid = false;
        } else {
        setPublicationStartError(null);
        }

        // Walidacja daty zakończenia publikacji
        if (end && start && endDate <= startDate) {
        setPublicationEndError('Publication end must be after publication start.');
        isValid = false;
        } else {
        setPublicationEndError(null);
        }
        return isValid;
    };

    const handlePublicationStartChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setForm((prev) => ({ ...prev, publicationStart: value }));
        // Walidujemy od razu po zmianie
        validateDates(value, form.publicationEnd);
    };

    const handlePublicationEndChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setForm((prev) => ({ ...prev, publicationEnd: value }));
        // Walidujemy od razu po zmianie
        validateDates(form.publicationStart, value);
    };

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


    // Obsługa wyboru istniejącego szablonu
    const handleSelectTemplate = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setForm(prev => ({ ...prev, offerTemplateId: e.target.value }));
        // Jeśli wybrano istniejący szablon, ukryj formularz tworzenia nowego
        if (e.target.value !== "") {
            setIncludeNewTemplate(false);
        }
    };

    const handleSelectCondition = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedConditionId(e.target.value);
        if (e.target.value !== "") {
            setIncludeNewCondition(false);
        }
    };



    // Zmodyfikowana funkcja onSubmit, aby chować formularz po stworzeniu
    const handleConditionCreateAndHide = async (formData: ContractConditionFormData) => {
        await onConditionCreate(formData);
        setIncludeNewCondition(false); // Ukryj formularz po stworzeniu
    };

    const handleTemplateCreateAndHide = async () => {
        await onTemplateCreate();
        setIncludeNewTemplate(false); 
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
                                        required
                                        value={selectedConditionId}
                                        onChange={handleSelectCondition}
                                        >
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
                                            onChange={(e) => {
                                                setIncludeNewCondition(e.target.checked);
                                                if (e.target.checked) {
                                                    setSelectedConditionId("");
                                                }
                                                }}
                                            disabled={!!selectedConditionId  && !includeNewCondition}

                                        /> Create New Contract
                                    </label>

                                    {includeNewCondition && (
                                        <ContractConditionForm
                                            parameters={parameters}
                                            onSubmit={handleConditionCreateAndHide}
                                            submitText="Create Condition"
                                        />
                                    )}
                                    {selectedConditionId && !includeNewCondition && (
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
                                        onChange={handleSelectTemplate}
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
                                            onChange={(e) => {
                                                setIncludeNewTemplate(e.target.checked);
                                                if (e.target.checked) {
                                                    setForm(prev => ({ ...prev, offerTemplateId: "" }));
                                                }
                                            }}
                                            disabled={!!form.offerTemplateId  && !includeNewTemplate}
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
                                            onSubmit={handleTemplateCreateAndHide}
                                            submitText="Create Template"
                                        />
                                    )}
                                    {form.offerTemplateId && !includeNewTemplate && (
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
                                        onChange={handlePublicationStartChange}
                                        required
                                        className="global-field-style"
                                    />
                                    {publicationStartError && <p className="text-red-500 text-sm mt-1">{publicationStartError}</p>}
                                </>
                            )}


                            {(isScheduled || isActive || statusId === null) && (
                                <>
                                    <label>Publication End</label>
                                    <input
                                        type="datetime-local"
                                        value={form.publicationEnd}
                                        onChange={handlePublicationEndChange}
                                        required
                                        className="global-field-style"
                                    />
                                    {publicationEndError && <p className="text-red-500 text-sm mt-1">{publicationEndError}</p>}

                                    <label>Employment Length (months)</label>
                                    <input
                                        type="number"
                                        value={form.employmentLength}
                                        onChange={(e) => setForm((prev) => ({ ...prev, employmentLength: Number(e.target.value) }))}
                                        min="0"
                                        required
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