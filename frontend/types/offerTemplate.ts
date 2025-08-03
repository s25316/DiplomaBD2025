import { SkillWithRequired } from "@/app/components/forms/OfferForm";

export interface OfferTemplate{
    offerTemplateId: string;
    name: string;
    description: string
    skills: SkillWithRequired[]
}