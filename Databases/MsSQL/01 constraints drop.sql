-- OUR OWN IMPLEMENTATION OF CONSTRAINTS
--======================================================================================
--======================================================================================
--[Address]
--Create
ALTER TABLE [dbo].[Address] DROP CONSTRAINT Address_Default_AddressId;
--======================================================================================
--[City]
--Create
ALTER TABLE [dbo].[City] DROP CONSTRAINT City_Default_CityId;
DROP SEQUENCE CityId_SEQUENCE;
--======================================================================================
--[Street]
--Create
ALTER TABLE [dbo].[Street] DROP CONSTRAINT Street_Default_StreetId;
DROP SEQUENCE StreetId_SEQUENCE;
--======================================================================================
--[State]
--Create
ALTER TABLE [dbo].[State] DROP CONSTRAINT State_Default_StateId;
DROP SEQUENCE StateId_SEQUENCE;
--======================================================================================
--[Country]
--Create
ALTER TABLE [dbo].[Country] DROP CONSTRAINT Country_Default_CountryId;
DROP SEQUENCE CountryId_SEQUENCE;


--======================================================================================
--======================================================================================
--[Notification]
--Create
ALTER TABLE [dbo].[Notification] DROP CONSTRAINT Notification_Default_NotificationId;
ALTER TABLE [dbo].[Notification] DROP CONSTRAINT Notification_Default_Created;
--======================================================================================
--[NChat]
--Create
ALTER TABLE [dbo].[NChat] DROP CONSTRAINT NChat_Default_MessageId;
ALTER TABLE [dbo].[NChat] DROP CONSTRAINT NChat_Default_Created;
--======================================================================================
--[Ex]
--Create
ALTER TABLE [dbo].[Ex] DROP CONSTRAINT Ex_Default_ExceptionId;
ALTER TABLE [dbo].[Ex] DROP CONSTRAINT Ex_Default_Created;
--======================================================================================
--[Faq]
--Create
ALTER TABLE [dbo].[Faq] DROP CONSTRAINT Faq_Default_FaqId;
ALTER TABLE [dbo].[Faq] DROP CONSTRAINT Faq_Default_Created;
--======================================================================================
--[NotificationType]
--Create
ALTER TABLE [dbo].[NotificationType] DROP CONSTRAINT NotificationType_Default_NotificationTypeId;
DROP SEQUENCE FaqId_SEQUENCE;


--======================================================================================
--======================================================================================
--[Person]
--Create
ALTER TABLE [dbo].[Person] DROP CONSTRAINT Person_Default_PersonId;
ALTER TABLE [dbo].[Person] DROP CONSTRAINT Person_Default_Created;
ALTER TABLE [dbo].[Person] DROP CONSTRAINT Person_Default_IsTwoFactorAuth;
ALTER TABLE [dbo].[Person] DROP CONSTRAINT Person_Default_IsStudent;
ALTER TABLE [dbo].[Person] DROP CONSTRAINT Person_Default_IsAdmin;
--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url] DROP CONSTRAINT Url_Default_UrlId;
ALTER TABLE [dbo].[Url] DROP CONSTRAINT Url_Default_Created;
--======================================================================================
--[PersonSkill]
--Create
ALTER TABLE [dbo].[PersonSkill] DROP CONSTRAINT PersonSkill_Default_PersonSkillId
ALTER TABLE [dbo].[PersonSkill] DROP CONSTRAINT PersonSkill_Default_Created;
--======================================================================================
--[UrlType]
--Create
ALTER TABLE [dbo].[UrlType] DROP CONSTRAINT UrlType_Default_UrlTypeId;
DROP SEQUENCE UrlTypeId_SEQUENCE;


--======================================================================================
--======================================================================================
--[CompanyPerson]
--Create
ALTER TABLE [dbo].[CompanyPerson] DROP CONSTRAINT CompanyPerson_Default_CompanyPersonId;
ALTER TABLE [dbo].[CompanyPerson] DROP CONSTRAINT CompanyPerson_Default_Grant;
--======================================================================================
--[Role]
--Create
ALTER TABLE [dbo].[Role] DROP CONSTRAINT Role_Default_RoleId;
DROP SEQUENCE RoleId_SEQUENCE;


--======================================================================================
--======================================================================================
--[Company]
--Create
ALTER TABLE [dbo].[Company] DROP CONSTRAINT Company_Default_CompanyId;
ALTER TABLE [dbo].[Company] DROP CONSTRAINT Company_Default_Created;
--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branch] DROP CONSTRAINT Branch_Default_BranchId;
ALTER TABLE [dbo].[Branch] DROP CONSTRAINT Branch_Default_Created;


--======================================================================================
--======================================================================================
--[SkillType]
--Create
ALTER TABLE [dbo].[SkillType] DROP CONSTRAINT SkillType_Default_SkillTypeId;
DROP SEQUENCE SkillTypeId_SEQUENCE;
--======================================================================================
--[Skill]
--Create
ALTER TABLE [dbo].[Skill] DROP CONSTRAINT Skill_Default_SkillId;
DROP SEQUENCE SkillId_SEQUENCE;
 

--======================================================================================
--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offer] DROP CONSTRAINT Offer_Default_OfferId;
ALTER TABLE [dbo].[Offer] DROP CONSTRAINT Offer_Default_PublicationStart;
ALTER TABLE [dbo].[Offer] DROP CONSTRAINT Offer_Default_IsNegotiated;
--======================================================================================
--[OfferTemplate]
--Create
ALTER TABLE [dbo].[OfferTemplate] DROP CONSTRAINT OfferTemplate_Default_OfferTemplateId;
ALTER TABLE [dbo].[OfferTemplate] DROP CONSTRAINT OfferTemplate_Default_Created;
--======================================================================================
--[OfferSkill]
--Create
ALTER TABLE [dbo].[OfferSkill] DROP CONSTRAINT OfferSkill_Default_OfferSkillId;
ALTER TABLE [dbo].[OfferSkill] DROP CONSTRAINT OfferSkill_Default_Created;
ALTER TABLE [dbo].[OfferSkill] DROP CONSTRAINT OfferSkill_Default_IsRequired;
--======================================================================================
--[OfferWorkMode]
--Create
ALTER TABLE [dbo].[OfferWorkMode] DROP CONSTRAINT OfferWorkMode_Default_OfferSkillId;
ALTER TABLE [dbo].[OfferWorkMode] DROP CONSTRAINT OfferWorkMode_Default_Created;
--======================================================================================
--[OfferEmploymentType]
--Create
ALTER TABLE [dbo].[OfferEmploymentType] DROP CONSTRAINT OfferEmploymentType_Default_OfferSkillId;
ALTER TABLE [dbo].[OfferEmploymentType] DROP CONSTRAINT OfferEmploymentType_Default_Created;
--======================================================================================
--[EmploymentType]
--Create
ALTER TABLE [dbo].[EmploymentType] DROP CONSTRAINT EmploymentType_Default_EmploymentTypeId;
DROP SEQUENCE EmploymentTypeId_SEQUENCE;
--======================================================================================
--[WorkMode]
--Create
ALTER TABLE [dbo].[WorkMode] DROP CONSTRAINT WorkMode_Default_WorkModeId;
DROP SEQUENCE WorkModeId_SEQUENCE;
--======================================================================================
--[SalaryTerm]
--Create
ALTER TABLE [dbo].[SalaryTerm] DROP CONSTRAINT SalaryTerm_Default_SalaryTermId;
DROP SEQUENCE SalaryTermId_SEQUENCE;
--======================================================================================
--[Currency]
--Create
ALTER TABLE [dbo].[Currency] DROP CONSTRAINT Currency_Default_CurrencyId;
DROP SEQUENCE CurrencyId_SEQUENCE;


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



