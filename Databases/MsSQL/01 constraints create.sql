-- OUR OWN IMPLEMENTATION OF CONSTRAINTS
--======================================================================================
--======================================================================================
--[Address]
--Create
ALTER TABLE [dbo].[Address]
ADD 
CONSTRAINT Address_Default_AddressId DEFAULT NEWID() FOR [AddressId];
--======================================================================================
--[City]
--Create
CREATE SEQUENCE CityId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[City]
ADD 
CONSTRAINT City_Default_CityId DEFAULT (NEXT VALUE FOR CityId_SEQUENCE) FOR [CityId];
--======================================================================================
--[Street]
--Create
CREATE SEQUENCE StreetId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Street]
ADD 
CONSTRAINT Street_Default_StreetId DEFAULT (NEXT VALUE FOR StreetId_SEQUENCE) FOR [StreetId];
--======================================================================================
--[State]
--Create
CREATE SEQUENCE StateId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[State]
ADD 
CONSTRAINT State_Default_StateId DEFAULT (NEXT VALUE FOR StateId_SEQUENCE) FOR [StateId];
--======================================================================================
--[Country]
--Create
CREATE SEQUENCE CountryId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Country]
ADD 
CONSTRAINT Country_Default_CountryId DEFAULT (NEXT VALUE FOR CountryId_SEQUENCE) FOR [CountryId];


--======================================================================================
--======================================================================================
--[Notification]
--Create
ALTER TABLE [dbo].[Notification]
ADD 
CONSTRAINT Notification_Default_NotificationId DEFAULT NEWID() FOR [NotificationId],
CONSTRAINT Notification_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[NChat]
--Create
ALTER TABLE [dbo].[NChat]
ADD 
CONSTRAINT NChat_Default_MessageId DEFAULT NEWID() FOR [MessageId],
CONSTRAINT NChat_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Ex]
--Create
ALTER TABLE [dbo].[Ex]
ADD 
CONSTRAINT Ex_Default_ExceptionId DEFAULT NEWID() FOR [ExceptionId],
CONSTRAINT Ex_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Faq]
--Create
ALTER TABLE [dbo].[Faq]
ADD 
CONSTRAINT Faq_Default_FaqId DEFAULT NEWID() FOR [FaqId],
CONSTRAINT Faq_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[NotificationType]
--Create
CREATE SEQUENCE FaqId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[NotificationType]
ADD 
CONSTRAINT NotificationType_Default_NotificationTypeId DEFAULT (NEXT VALUE FOR FaqId_SEQUENCE) FOR [NotificationTypeId];


--======================================================================================
--======================================================================================
--[Person]
--Create
ALTER TABLE [dbo].[Person]
ADD 
CONSTRAINT Person_Default_PersonId DEFAULT NEWID() FOR [PersonId],
CONSTRAINT Person_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT Person_Default_IsTwoFactorAuth DEFAULT 0 FOR [IsTwoFactorAuth],
CONSTRAINT Person_Default_IsStudent DEFAULT 0 FOR [IsStudent],
CONSTRAINT Person_Default_IsAdmin DEFAULT 0 FOR [IsAdmin];
--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url]
ADD 
CONSTRAINT Url_Default_UrlId DEFAULT NEWID() FOR [UrlId],
CONSTRAINT Url_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[PersonSkill]
--Create
ALTER TABLE [dbo].[PersonSkill]
ADD 
CONSTRAINT PersonSkill_Default_PersonSkillId DEFAULT NEWID() FOR [PersonSkillId],
CONSTRAINT PersonSkill_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[UrlType]
--Create
CREATE SEQUENCE UrlTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[UrlType]
ADD 
CONSTRAINT UrlType_Default_UrlTypeId DEFAULT (NEXT VALUE FOR UrlTypeId_SEQUENCE) FOR [UrlTypeId];


--======================================================================================
--======================================================================================
--[CompanyPerson]
--Create
ALTER TABLE [dbo].[CompanyPerson]
ADD 
CONSTRAINT CompanyPerson_Default_CompanyPersonId DEFAULT NEWID() FOR [CompanyPersonId],
CONSTRAINT CompanyPerson_Default_Grant DEFAULT GETDATE() FOR [Grant];
--======================================================================================
--[Role]
--Create
CREATE SEQUENCE RoleId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Role]
ADD 
CONSTRAINT Role_Default_RoleId DEFAULT (NEXT VALUE FOR RoleId_SEQUENCE) FOR [RoleId];


--======================================================================================
--======================================================================================
--[Company]
--Create
ALTER TABLE [dbo].[Company]
ADD 
CONSTRAINT Company_Default_CompanyId DEFAULT NEWID() FOR [CompanyId],
CONSTRAINT Company_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branch]
ADD 
CONSTRAINT Branch_Default_BranchId DEFAULT NEWID() FOR [BranchId],
CONSTRAINT Branch_Default_Created DEFAULT GETDATE() FOR [Created];


--======================================================================================
--======================================================================================
--[SkillType]
--Create
CREATE SEQUENCE SkillTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[SkillType]
ADD 
CONSTRAINT SkillType_Default_SkillTypeId DEFAULT (NEXT VALUE FOR SkillTypeId_SEQUENCE) FOR [SkillTypeId];
--======================================================================================
--[Skill]
--Create
CREATE SEQUENCE SkillId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Skill]
ADD 
CONSTRAINT Skill_Default_SkillId DEFAULT (NEXT VALUE FOR SkillId_SEQUENCE) FOR [SkillId];
 

--======================================================================================
--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offer]
ADD 
CONSTRAINT Offer_Default_OfferId DEFAULT NEWID() FOR [OfferId],
CONSTRAINT Offer_Default_PublicationStart DEFAULT GETDATE() FOR [PublicationStart],
CONSTRAINT Offer_Default_IsNegotiated DEFAULT 1 FOR [IsNegotiated];
--======================================================================================
--[OfferTemplate]
--Create
ALTER TABLE [dbo].[OfferTemplate]
ADD 
CONSTRAINT OfferTemplate_Default_OfferTemplateId DEFAULT NEWID() FOR [OfferTemplateId],
CONSTRAINT OfferTemplate_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[OfferSkill]
--Create
ALTER TABLE [dbo].[OfferSkill]
ADD 
CONSTRAINT OfferSkill_Default_OfferSkillId DEFAULT NEWID() FOR [OfferSkillId],
CONSTRAINT OfferSkill_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT OfferSkill_Default_IsRequired DEFAULT 0 FOR [IsRequired];
--======================================================================================
--[OfferWorkMode]
--Create
ALTER TABLE [dbo].[OfferWorkMode]
ADD 
CONSTRAINT OfferWorkMode_Default_OfferSkillId DEFAULT NEWID() FOR [OfferWorkModeId],
CONSTRAINT OfferWorkMode_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[OfferEmploymentType]
--Create
ALTER TABLE [dbo].[OfferEmploymentType]
ADD 
CONSTRAINT OfferEmploymentType_Default_OfferSkillId DEFAULT NEWID() FOR [OfferEmploymentTypeId],
CONSTRAINT OfferEmploymentType_Default_Created DEFAULT GETDATE() FOR [Created];
--======================================================================================
--[EmploymentType]
--Create
CREATE SEQUENCE EmploymentTypeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[EmploymentType]
ADD 
CONSTRAINT EmploymentType_Default_EmploymentTypeId DEFAULT (NEXT VALUE FOR EmploymentTypeId_SEQUENCE) FOR [EmploymentTypeId];
--======================================================================================
--[WorkMode]
--Create
CREATE SEQUENCE WorkModeId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[WorkMode]
ADD 
CONSTRAINT WorkMode_Default_WorkModeId DEFAULT (NEXT VALUE FOR WorkModeId_SEQUENCE) FOR [WorkModeId];
--======================================================================================
--[SalaryTerm]
--Create
CREATE SEQUENCE SalaryTermId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[SalaryTerm]
ADD 
CONSTRAINT SalaryTerm_Default_SalaryTermId DEFAULT (NEXT VALUE FOR SalaryTermId_SEQUENCE) FOR [SalaryTermId];
--======================================================================================
--[Currency]
--Create
CREATE SEQUENCE CurrencyId_SEQUENCE
    START WITH 1 
    INCREMENT BY 1;
ALTER TABLE [dbo].[Currency]
ADD 
CONSTRAINT Currency_Default_CurrencyId DEFAULT (NEXT VALUE FOR CurrencyId_SEQUENCE) FOR [CurrencyId];


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