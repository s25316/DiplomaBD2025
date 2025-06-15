-- OUR OWN IMPLEMENTATION OF CONSTRAINTS
--======================================================================================
--======================================================================================
--[Addresses]
--Create
ALTER TABLE [dbo].[Addresses]
ADD 
CONSTRAINT Addresses_Default_AddressId DEFAULT NEWID() FOR [AddressId];
--======================================================================================
--[Cities]
--Create
CREATE SEQUENCE CityId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Cities] 
ADD 
CONSTRAINT Cities_Default_CityId DEFAULT (NEXT VALUE FOR CityId_SEQUENCE) FOR [CityId];
--======================================================================================
--[Streets]
--Create
CREATE SEQUENCE StreetId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Streets]
ADD 
CONSTRAINT Streets_Default_StreetId DEFAULT (NEXT VALUE FOR StreetId_SEQUENCE) FOR [StreetId];
--======================================================================================
--[States]
--Create
CREATE SEQUENCE StateId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[States]
ADD 
CONSTRAINT States_Default_StateId DEFAULT (NEXT VALUE FOR StateId_SEQUENCE) FOR [StateId];
--======================================================================================
--[Countries]
--Create
CREATE SEQUENCE CountryId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Countries]
ADD 
CONSTRAINT Countries_Default_CountryId DEFAULT (NEXT VALUE FOR CountryId_SEQUENCE) FOR [CountryId];


--======================================================================================
--======================================================================================
--[Notification]
--Create
ALTER TABLE [dbo].[Notifications]
ADD 
CONSTRAINT Notifications_Default_NotificationId DEFAULT NEWID() FOR [NotificationId],
CONSTRAINT Notifications_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[NChat] [rebuild]
--Create
ALTER TABLE [dbo].[NChat]
ADD 
CONSTRAINT NChat_Default_MessageId DEFAULT NEWID() FOR [MessageId],
CONSTRAINT NChat_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Exs]
--Create
ALTER TABLE [dbo].[Exs]
ADD 
CONSTRAINT Exs_Default_ExceptionId DEFAULT NEWID() FOR [ExceptionId],
CONSTRAINT Exs_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Faqs]
--Create
ALTER TABLE [dbo].[Faqs]
ADD 
CONSTRAINT Faqs_Default_FaqId DEFAULT NEWID() FOR [FaqId],
CONSTRAINT Faqs_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[NotificationTypes]
--Create
CREATE SEQUENCE NotificationTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[NotificationTypes]
ADD 
CONSTRAINT NotificationTypes_Default_NotificationTypeId DEFAULT (NEXT VALUE FOR NotificationTypeId_SEQUENCE) FOR [NotificationTypeId];


--======================================================================================
--======================================================================================
--[People]
--Create
ALTER TABLE [dbo].[People]
ADD 
CONSTRAINT People_Default_PersonId DEFAULT NEWID() FOR [PersonId],
CONSTRAINT People_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT People_Default_IsTwoFactorAuth DEFAULT 0 FOR [IsTwoFactorAuth],
CONSTRAINT People_Default_IsStudent DEFAULT 0 FOR [IsStudent],
CONSTRAINT People_Default_IsAdmin DEFAULT 0 FOR [IsAdmin],
CONSTRAINT People_Default_IsIndividual DEFAULT 0 FOR [IsIndividual];
--======================================================================================
--[Urls]
--Create
ALTER TABLE [dbo].[Urls]
ADD 
CONSTRAINT Urls_Default_UrlId DEFAULT NEWID() FOR [UrlId],
CONSTRAINT Urls_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[PersonSkills]
--Create
ALTER TABLE [dbo].[PersonSkills]
ADD 
CONSTRAINT PersonSkills_Default_PersonSkillId DEFAULT NEWID() FOR [PersonSkillId],
CONSTRAINT PersonSkills_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[UrlTypes]
--Create
CREATE SEQUENCE UrlTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[UrlTypes]
ADD 
CONSTRAINT UrlTypes_Default_UrlTypeId DEFAULT (NEXT VALUE FOR UrlTypeId_SEQUENCE) FOR [UrlTypeId];


--======================================================================================
--======================================================================================
--[CompanyPeople]
--Create
ALTER TABLE [dbo].[CompanyPeople]
ADD 
CONSTRAINT CompanyPeople_Default_CompanyPersonId DEFAULT NEWID() FOR [CompanyPersonId],
CONSTRAINT CompanyPeople_Default_Grant DEFAULT GETDATE() FOR [Grant];
--======================================================================================
--[Roles]
--Create
CREATE SEQUENCE RoleId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Roles]
ADD 
CONSTRAINT Roles_Default_RoleId DEFAULT (NEXT VALUE FOR RoleId_SEQUENCE) FOR [RoleId];


--======================================================================================
--======================================================================================
--[Companies]
--Create
ALTER TABLE [dbo].[Companies]
ADD 
CONSTRAINT Companies_Default_CompanyId DEFAULT NEWID() FOR [CompanyId],
CONSTRAINT Companies_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Branches]
--Create
ALTER TABLE [dbo].[Branches]
ADD 
CONSTRAINT Branches_Default_BranchId DEFAULT NEWID() FOR [BranchId],
CONSTRAINT Branches_Default_Created DEFAULT GETDATE() FOR [Created];


--======================================================================================
--======================================================================================
--[SkillTypes]
--Create
CREATE SEQUENCE SkillTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[SkillTypes]
ADD 
CONSTRAINT SkillTypes_Default_SkillTypeId DEFAULT (NEXT VALUE FOR SkillTypeId_SEQUENCE) FOR [SkillTypeId];
--======================================================================================
--[Skills]
--Create
CREATE SEQUENCE SkillId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Skills]
ADD 
CONSTRAINT Skills_Default_SkillId DEFAULT (NEXT VALUE FOR SkillId_SEQUENCE) FOR [SkillId];
 

--======================================================================================
--======================================================================================
--[OfferTemplates]
--Create
ALTER TABLE [dbo].[OfferTemplates]
ADD 
CONSTRAINT OfferTemplates_Default_OfferTemplateId DEFAULT NEWID() FOR [OfferTemplateId],
CONSTRAINT OfferTemplates_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[OfferSkills]
--Create
ALTER TABLE [dbo].[OfferSkills]
ADD 
CONSTRAINT OfferSkills_Default_OfferSkillId DEFAULT NEWID() FOR [OfferSkillId],
CONSTRAINT OfferSkills_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT OfferSkills_Default_IsRequired DEFAULT 0 FOR [IsRequired];
--======================================================================================
--[OfferWorkMode]
--[OfferEmploymentType]
--[EmploymentType]
--[WorkMode]
--[SalaryTerm]
--[Currency]

--[ContractParameterTypes]
--Create
CREATE SEQUENCE ContractParameterTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[ContractParameterTypes]
ADD 
CONSTRAINT ContractParameterTypes_Default_ContractParameterTypeId DEFAULT (NEXT VALUE FOR ContractParameterTypeId_SEQUENCE) FOR [ContractParameterTypeId];
--======================================================================================
--[ContractParameterTypes]
--Create
CREATE SEQUENCE ContractParameterId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[ContractParameters]
ADD 
CONSTRAINT ContractParameters_Default_ContractParameterTypeId DEFAULT (NEXT VALUE FOR ContractParameterId_SEQUENCE) FOR [ContractParameterId];
--======================================================================================
--[ContractAttributes]
--Create
ALTER TABLE [dbo].[ContractAttributes]
ADD 
CONSTRAINT ContractAttributes_Default_ContractAttributeId DEFAULT NEWID() FOR [ContractAttributeId],
CONSTRAINT ContractAttributes_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[ContractConditions]
--Create
ALTER TABLE [dbo].[ContractConditions]
ADD 
CONSTRAINT ContractConditions_Default_ContractAttributeId DEFAULT NEWID() FOR [ContractConditionId],
CONSTRAINT ContractConditions_Default_HoursPerTerm DEFAULT 1 FOR [HoursPerTerm],
CONSTRAINT ContractConditions_Default_IsNegotiable DEFAULT 1 FOR [IsNegotiable],
CONSTRAINT ContractConditions_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[OfferConditions]
--Create
ALTER TABLE [dbo].[OfferConditions]
ADD 
CONSTRAINT OfferConditions_Default_ContractAttributeId DEFAULT NEWID() FOR [OfferConditionId],
CONSTRAINT OfferConditions_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offers]
ADD 
CONSTRAINT Offers_Default_OfferId DEFAULT NEWID() FOR [OfferId];
--======================================================================================
--[OfferConnections]
--Create
ALTER TABLE [dbo].[OfferConnections]
ADD 
CONSTRAINT OfferConnections_Default_OfferConnectionId DEFAULT NEWID() FOR [OfferConnectionId],
CONSTRAINT OfferConnections_Default_Created DEFAULT GETDATE() FOR [Created];
 

--======================================================================================
--======================================================================================
--[HRProcess]
--Create
ALTER TABLE [dbo].[HRProcess]
ADD 
CONSTRAINT HRProcess_Default_ProcessId DEFAULT NEWID() FOR [ProcessId];
--======================================================================================
--[HRChat]
--Create
ALTER TABLE [dbo].[HRChat]
ADD 
CONSTRAINT HRChat_Default_MessageId DEFAULT NEWID() FOR [MessageId],
CONSTRAINT HRChat_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[ProcessType]
--Create
CREATE SEQUENCE ProcessTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[ProcessType]
ADD 
CONSTRAINT ProcessType_Default_ProcessTypeId DEFAULT (NEXT VALUE FOR ProcessTypeId_SEQUENCE) FOR [ProcessTypeId];