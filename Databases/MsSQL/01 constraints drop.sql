-- OUR OWN IMPLEMENTATION OF CONSTRAINTS
--======================================================================================
--======================================================================================
--[Addresses]
--Create
ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT Addresses_Default_AddressId;
--======================================================================================
--[Cities]
--Create
ALTER TABLE [dbo].[Cities] DROP CONSTRAINT Cities_Default_CityId;
DROP SEQUENCE CityId_SEQUENCE;
--======================================================================================
--[Streets]
--Create
ALTER TABLE [dbo].[Streets] DROP CONSTRAINT Streets_Default_StreetId;
DROP SEQUENCE StreetId_SEQUENCE;
--======================================================================================
--[States]
--Create
ALTER TABLE [dbo].[States] DROP CONSTRAINT States_Default_StateId;
DROP SEQUENCE StateId_SEQUENCE;
--======================================================================================
--[Countries]
--Create
ALTER TABLE [dbo].[Countries] DROP CONSTRAINT Countries_Default_CountryId;
DROP SEQUENCE CountryId_SEQUENCE;


--======================================================================================
--======================================================================================
--[Notifications]
--Create
ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT Notifications_Default_NotificationId;
ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT Notifications_Default_Created;
--======================================================================================
--[NChat]
--Create
ALTER TABLE [dbo].[NChat] DROP CONSTRAINT NChat_Default_MessageId;
ALTER TABLE [dbo].[NChat] DROP CONSTRAINT NChat_Default_Created;
--======================================================================================
--[Exs]
--Create
ALTER TABLE [dbo].[Exs] DROP CONSTRAINT Exs_Default_ExceptionId;
ALTER TABLE [dbo].[Exs] DROP CONSTRAINT Exs_Default_Created;
--======================================================================================
--[Faqs]
--Create
ALTER TABLE [dbo].[Faqs] DROP CONSTRAINT Faqs_Default_FaqId;
ALTER TABLE [dbo].[Faqs] DROP CONSTRAINT Faqs_Default_Created;
--======================================================================================
--[NotificationTypes]
--Create
ALTER TABLE [dbo].[NotificationTypes] DROP CONSTRAINT NotificationTypes_Default_NotificationTypeId;
DROP SEQUENCE NotificationTypeId_SEQUENCE;


--======================================================================================
--======================================================================================
--[People]
--Create
ALTER TABLE [dbo].[People] DROP CONSTRAINT People_Default_PersonId;
ALTER TABLE [dbo].[People] DROP CONSTRAINT People_Default_Created;
ALTER TABLE [dbo].[People] DROP CONSTRAINT People_Default_IsTwoFactorAuth;
ALTER TABLE [dbo].[People] DROP CONSTRAINT People_Default_IsStudent;
ALTER TABLE [dbo].[People] DROP CONSTRAINT People_Default_IsAdmin;
--======================================================================================
--[Urls]
--Create
ALTER TABLE [dbo].[Urls] DROP CONSTRAINT Urls_Default_UrlId;
ALTER TABLE [dbo].[Urls] DROP CONSTRAINT Urls_Default_Created;
--======================================================================================
--[PersonSkills]
--Create
ALTER TABLE [dbo].[PersonSkills] DROP CONSTRAINT PersonSkills_Default_PersonSkillId
ALTER TABLE [dbo].[PersonSkills] DROP CONSTRAINT PersonSkills_Default_Created;
--======================================================================================
--[UrlTypes]
--Create
ALTER TABLE [dbo].[UrlTypes] DROP CONSTRAINT UrlTypes_Default_UrlTypeId;
DROP SEQUENCE UrlTypeId_SEQUENCE;


--======================================================================================
--======================================================================================
--[CompanyPeople]
--Create
ALTER TABLE [dbo].[CompanyPeople] DROP CONSTRAINT CompanyPeople_Default_CompanyPersonId;
ALTER TABLE [dbo].[CompanyPeople] DROP CONSTRAINT CompanyPeople_Default_Grant;
--======================================================================================
--[Roles]
--Create
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT Roles_Default_RoleId;
DROP SEQUENCE RoleId_SEQUENCE;


--======================================================================================
--======================================================================================
--[Companies]
--Create
ALTER TABLE [dbo].[Companies] DROP CONSTRAINT Companies_Default_CompanyId;
ALTER TABLE [dbo].[Companies] DROP CONSTRAINT Companies_Default_Created;
--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branches] DROP CONSTRAINT Branches_Default_BranchId;
ALTER TABLE [dbo].[Branches] DROP CONSTRAINT Branches_Default_Created;


--======================================================================================
--======================================================================================
--[SkillTypes]
--Create
ALTER TABLE [dbo].[SkillTypes] DROP CONSTRAINT SkillTypes_Default_SkillTypeId;
DROP SEQUENCE SkillTypeId_SEQUENCE;
--======================================================================================
--[Skills]
--Create
ALTER TABLE [dbo].[Skills] DROP CONSTRAINT Skills_Default_SkillId;
DROP SEQUENCE SkillId_SEQUENCE;
 


--======================================================================================
--======================================================================================
--[OfferTemplates]
--Create
ALTER TABLE [dbo].[OfferTemplates] DROP CONSTRAINT OfferTemplates_Default_OfferTemplateId;
ALTER TABLE [dbo].[OfferTemplates] DROP CONSTRAINT OfferTemplates_Default_Created;
--======================================================================================
--[OfferSkills]
--Create
ALTER TABLE [dbo].[OfferSkills] DROP CONSTRAINT OfferSkills_Default_OfferSkillId;
ALTER TABLE [dbo].[OfferSkills] DROP CONSTRAINT OfferSkills_Default_Created;
ALTER TABLE [dbo].[OfferSkills] DROP CONSTRAINT OfferSkills_Default_IsRequired;
--======================================================================================
--[OfferWorkMode]
--[OfferEmploymentType]
--[EmploymentType]
--[WorkMode]
--[SalaryTerm]
--[Currency]

--[ContractParameterTypes]
--Create
ALTER TABLE [dbo].[ContractParameterTypes] DROP CONSTRAINT ContractParameterTypes_Default_ContractParameterTypeId;
DROP SEQUENCE ContractParameterTypeId_SEQUENCE;
--======================================================================================
--[ContractParameterTypes]
--Create
ALTER TABLE [dbo].[ContractParameters] DROP CONSTRAINT  ContractParameters_Default_ContractParameterTypeId;
DROP SEQUENCE ContractParameterId_SEQUENCE;
--======================================================================================
--[ContractAttributes]
--Create
ALTER TABLE [dbo].[ContractAttributes] DROP CONSTRAINT ContractAttributes_Default_ContractAttributeId;
ALTER TABLE [dbo].[ContractAttributes] DROP CONSTRAINT ContractAttributes_Default_Created;
--======================================================================================
--[ContractConditions]
--Create
ALTER TABLE [dbo].[ContractConditions] DROP CONSTRAINT ContractConditions_Default_ContractAttributeId;
ALTER TABLE [dbo].[ContractConditions] DROP CONSTRAINT ContractConditions_Default_HoursPerTerm;
ALTER TABLE [dbo].[ContractConditions] DROP CONSTRAINT ContractConditions_Default_IsNegotiable;
ALTER TABLE [dbo].[ContractConditions] DROP CONSTRAINT ContractConditions_Default_Created;
--======================================================================================
--[OfferConditions]
--Create
ALTER TABLE [dbo].[OfferConditions] DROP CONSTRAINT OfferConditions_Default_ContractAttributeId;
ALTER TABLE [dbo].[OfferConditions] DROP CONSTRAINT OfferConditions_Default_Created;
--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offers] DROP CONSTRAINT Offers_Default_OfferId;
--======================================================================================
--[OfferConnections]
--Create
ALTER TABLE [dbo].[OfferConnections] DROP CONSTRAINT OfferConnections_Default_OfferConnectionId;
ALTER TABLE [dbo].[OfferConnections] DROP CONSTRAINT OfferConnections_Default_Created; 


--======================================================================================
--======================================================================================
--[HRProcess]
--Create
ALTER TABLE [dbo].[HRProcess] DROP CONSTRAINT HRProcess_Default_ProcessId;
--======================================================================================
--[HRChat]
--Create
ALTER TABLE [dbo].[HRChat] DROP CONSTRAINT HRChat_Default_MessageId;
ALTER TABLE [dbo].[HRChat] DROP CONSTRAINT HRChat_Default_Created;
--======================================================================================
--[ProcessType]
--Create
ALTER TABLE [dbo].[ProcessType] DROP CONSTRAINT ProcessType_Default_ProcessTypeId;
DROP SEQUENCE ProcessTypeId_SEQUENCE;



