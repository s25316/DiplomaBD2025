-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-03-15 02:02:15.639

-- tables
-- Table: Addresses
CREATE TABLE Addresses (
    AddressId uniqueidentifier  NOT NULL,
    CityId int  NOT NULL,
    StreetId int  NULL,
    HouseNumber nvarchar(25)  NOT NULL,
    ApartmentNumber nvarchar(25)  NULL,
    PostCode nvarchar(25)  NOT NULL,
    Lon real  NOT NULL,
    Lat real  NOT NULL,
    Point geography  NOT NULL,
    CONSTRAINT Addresses_pk PRIMARY KEY  (AddressId)
);

-- Table: Branches
CREATE TABLE Branches (
    BranchId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    AddressId uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Branches_pk PRIMARY KEY  (BranchId)
);

-- Table: Cities
CREATE TABLE Cities (
    CityId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    StateId int  NOT NULL,
    CONSTRAINT Cities_pk PRIMARY KEY  (CityId)
);

-- Table: Companies
CREATE TABLE Companies (
    CompanyId uniqueidentifier  NOT NULL,
    Logo nvarchar(100)  NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Regon nvarchar(25)  NULL,
    Nip nvarchar(25)  NULL,
    Krs nvarchar(25)  NULL,
    WebsiteUrl nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    Blocked datetime  NULL,
    CONSTRAINT Companies_pk PRIMARY KEY  (CompanyId)
);

-- Table: CompanyPeople
CREATE TABLE CompanyPeople (
    CompanyPersonId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    RoleId int  NOT NULL,
    "Grant" datetime  NOT NULL,
    "Deny" datetime  NULL,
    CONSTRAINT CompanyPeople_pk PRIMARY KEY  (CompanyPersonId)
);

-- Table: ContractAttributes
CREATE TABLE ContractAttributes (
    ContractAttributeId uniqueidentifier  NOT NULL,
    ContractParameterId int  NOT NULL,
    ContractConditionId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT ContractAttributes_pk PRIMARY KEY  (ContractAttributeId)
);

-- Table: ContractConditions
CREATE TABLE ContractConditions (
    ContractConditionId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    SalaryMin money  NULL,
    SalaryMax money  NULL,
    HoursPerTerm int  NOT NULL,
    IsNegotiable bit  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT ContractConditions_pk PRIMARY KEY  (ContractConditionId)
);

-- Table: ContractParameterTypes
CREATE TABLE ContractParameterTypes (
    ContractParameterTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT ContractParameterTypes_pk PRIMARY KEY  (ContractParameterTypeId)
);

-- Table: ContractParameters
CREATE TABLE ContractParameters (
    ContractParameterId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    ContractParameterTypeId int  NOT NULL,
    CONSTRAINT ContractParameters_pk PRIMARY KEY  (ContractParameterId)
);

-- Table: Countries
CREATE TABLE Countries (
    CountryId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Countries_pk PRIMARY KEY  (CountryId)
);

-- Table: Exs
CREATE TABLE Exs (
    ExceptionId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Handled datetime  NULL,
    ExceptionType nvarchar(100)  NOT NULL,
    Method nvarchar(100)  NULL,
    StackTrace nvarchar(800)  NOT NULL,
    Message nvarchar(max)  NOT NULL,
    Other nvarchar(max)  NULL,
    Request int  NULL,
    CONSTRAINT Exs_pk PRIMARY KEY  (ExceptionId)
);

-- Table: Faqs
CREATE TABLE Faqs (
    FaqId uniqueidentifier  NOT NULL,
    Question nvarchar(800)  NOT NULL,
    Answer nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Faqs_pk PRIMARY KEY  (FaqId)
);

-- Table: HRChat
CREATE TABLE HRChat (
    MessageId uniqueidentifier  NOT NULL,
    ProcessId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    "Read" datetime  NULL,
    IsPersonSend bit  NOT NULL,
    Message nvarchar(800)  NULL,
    MongoUrl nvarchar(100)  NULL,
    ProcessTypeId int  NOT NULL,
    CONSTRAINT HRChat_pk PRIMARY KEY  (MessageId)
);

-- Table: HrProcess
CREATE TABLE HrProcess (
    ProcessId uniqueidentifier  NOT NULL,
    CONSTRAINT HrProcess_pk PRIMARY KEY  (ProcessId)
);

-- Table: NChat
CREATE TABLE NChat (
    MessageId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    "Read" datetime  NULL,
    IsAdminSend bit  NOT NULL,
    Message nvarchar(800)  NULL,
    MongoUrl nvarchar(100)  NULL,
    CONSTRAINT NChat_pk PRIMARY KEY  (MessageId)
);

-- Table: NotificationTypes
CREATE TABLE NotificationTypes (
    NotificationTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(100)  NOT NULL,
    CONSTRAINT NotificationTypes_pk PRIMARY KEY  (NotificationTypeId)
);

-- Table: Notifications
CREATE TABLE Notifications (
    NotificationId uniqueidentifier  NOT NULL,
    NotificationTypeId int  NOT NULL,
    PersonId uniqueidentifier  NULL,
    IsAdminSend bit  NOT NULL,
    Email nvarchar(100)  NULL,
    Message nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    "Read" datetime  NULL,
    Completed datetime  NULL,
    ObjectId uniqueidentifier  NULL,
    CONSTRAINT Notifications_pk PRIMARY KEY  (NotificationId)
);

-- Table: OfferConditions
CREATE TABLE OfferConditions (
    OfferConditionId uniqueidentifier  NOT NULL,
    ContractConditionId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT OfferConditions_pk PRIMARY KEY  (OfferConditionId)
);

-- Table: OfferConnections
CREATE TABLE OfferConnections (
    OfferConnectionId uniqueidentifier  NOT NULL,
    OfferTemplateId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT OfferConnections_pk PRIMARY KEY  (OfferConnectionId)
);

-- Table: OfferSkills
CREATE TABLE OfferSkills (
    OfferSkillId uniqueidentifier  NOT NULL,
    OfferTemplateId uniqueidentifier  NOT NULL,
    SkillId int  NOT NULL,
    IsRequired bit  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT OfferSkills_pk PRIMARY KEY  (OfferSkillId)
);

-- Table: OfferTemplates
CREATE TABLE OfferTemplates (
    OfferTemplateId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT OfferTemplates_pk PRIMARY KEY  (OfferTemplateId)
);

-- Table: Offers
CREATE TABLE Offers (
    OfferId uniqueidentifier  NOT NULL,
    BranchId uniqueidentifier  NULL,
    PublicationStart datetime  NOT NULL,
    PublicationEnd datetime  NULL,
    EmploymentLength real  NULL,
    WebsiteUrl nvarchar(800)  NULL,
    CONSTRAINT Offers_pk PRIMARY KEY  (OfferId)
);

-- Table: People
CREATE TABLE People (
    PersonId uniqueidentifier  NOT NULL,
    AddressId uniqueidentifier  NULL,
    Login nvarchar(100)  NULL,
    Logo nvarchar(100)  NULL,
    Name nvarchar(100)  NULL,
    Surname nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    PhoneNum nvarchar(100)  NULL,
    ContactEmail nvarchar(100)  NULL,
    BirthDate date  NULL,
    IsTwoFactorAuth bit  NOT NULL,
    IsStudent bit  NOT NULL,
    IsAdmin bit  NOT NULL,
    Password nvarchar(max)  NOT NULL,
    Salt nvarchar(max)  NOT NULL,
    Created datetime  NOT NULL,
    Blocked datetime  NULL,
    Removed datetime  NULL,
    CONSTRAINT People_pk PRIMARY KEY  (PersonId)
);

-- Table: PersonSkills
CREATE TABLE PersonSkills (
    PersonSkillId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    SkillId int  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT PersonSkills_pk PRIMARY KEY  (PersonSkillId)
);

-- Table: ProcessType
CREATE TABLE ProcessType (
    ProcessTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT ProcessType_pk PRIMARY KEY  (ProcessTypeId)
);

-- Table: Roles
CREATE TABLE Roles (
    RoleId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    CONSTRAINT Roles_pk PRIMARY KEY  (RoleId)
);

-- Table: SkillConnections
CREATE TABLE SkillConnections (
    ParentSkillId int  NOT NULL,
    ChildSkillId int  NOT NULL,
    CONSTRAINT SkillConnections_pk PRIMARY KEY  (ParentSkillId,ChildSkillId)
);

-- Table: SkillTypes
CREATE TABLE SkillTypes (
    SkillTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT SkillTypes_pk PRIMARY KEY  (SkillTypeId)
);

-- Table: Skills
CREATE TABLE Skills (
    SkillId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    SkillTypeId int  NOT NULL,
    CONSTRAINT Skills_pk PRIMARY KEY  (SkillId)
);

-- Table: States
CREATE TABLE States (
    StateId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CountryId int  NOT NULL,
    CONSTRAINT States_pk PRIMARY KEY  (StateId)
);

-- Table: Streets
CREATE TABLE Streets (
    StreetId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Streets_pk PRIMARY KEY  (StreetId)
);

-- Table: UrlTypes
CREATE TABLE UrlTypes (
    UrlTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT UrlTypes_pk PRIMARY KEY  (UrlTypeId)
);

-- Table: Urls
CREATE TABLE Urls (
    UrlId uniqueidentifier  NOT NULL,
    UrlTypeId int  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    Value nvarchar(800)  NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Urls_pk PRIMARY KEY  (UrlId)
);

-- foreign keys
-- Reference: Addresses_Cities (table: Addresses)
ALTER TABLE Addresses ADD CONSTRAINT Addresses_Cities
    FOREIGN KEY (CityId)
    REFERENCES Cities (CityId);

-- Reference: Addresses_Streets (table: Addresses)
ALTER TABLE Addresses ADD CONSTRAINT Addresses_Streets
    FOREIGN KEY (StreetId)
    REFERENCES Streets (StreetId);

-- Reference: Branches_Addresses (table: Branches)
ALTER TABLE Branches ADD CONSTRAINT Branches_Addresses
    FOREIGN KEY (AddressId)
    REFERENCES Addresses (AddressId);

-- Reference: Branches_Companies (table: Branches)
ALTER TABLE Branches ADD CONSTRAINT Branches_Companies
    FOREIGN KEY (CompanyId)
    REFERENCES Companies (CompanyId);

-- Reference: Cities_States (table: Cities)
ALTER TABLE Cities ADD CONSTRAINT Cities_States
    FOREIGN KEY (StateId)
    REFERENCES States (StateId);

-- Reference: CompanyPeople_Companies (table: CompanyPeople)
ALTER TABLE CompanyPeople ADD CONSTRAINT CompanyPeople_Companies
    FOREIGN KEY (CompanyId)
    REFERENCES Companies (CompanyId);

-- Reference: CompanyPeople_People (table: CompanyPeople)
ALTER TABLE CompanyPeople ADD CONSTRAINT CompanyPeople_People
    FOREIGN KEY (PersonId)
    REFERENCES People (PersonId);

-- Reference: CompanyPeople_Roles (table: CompanyPeople)
ALTER TABLE CompanyPeople ADD CONSTRAINT CompanyPeople_Roles
    FOREIGN KEY (RoleId)
    REFERENCES Roles (RoleId);

-- Reference: ContractAttributes_ContractConditions (table: ContractAttributes)
ALTER TABLE ContractAttributes ADD CONSTRAINT ContractAttributes_ContractConditions
    FOREIGN KEY (ContractConditionId)
    REFERENCES ContractConditions (ContractConditionId);

-- Reference: ContractAttributes_ContractParameters (table: ContractAttributes)
ALTER TABLE ContractAttributes ADD CONSTRAINT ContractAttributes_ContractParameters
    FOREIGN KEY (ContractParameterId)
    REFERENCES ContractParameters (ContractParameterId);

-- Reference: ContractConditions_Companies (table: ContractConditions)
ALTER TABLE ContractConditions ADD CONSTRAINT ContractConditions_Companies
    FOREIGN KEY (CompanyId)
    REFERENCES Companies (CompanyId);

-- Reference: ContractParameters_ContractParameterTypes (table: ContractParameters)
ALTER TABLE ContractParameters ADD CONSTRAINT ContractParameters_ContractParameterTypes
    FOREIGN KEY (ContractParameterTypeId)
    REFERENCES ContractParameterTypes (ContractParameterTypeId);

-- Reference: HRChat_HRProcess (table: HRChat)
ALTER TABLE HRChat ADD CONSTRAINT HRChat_HRProcess
    FOREIGN KEY (ProcessId)
    REFERENCES HrProcess (ProcessId);

-- Reference: HRChat_ProcessType (table: HRChat)
ALTER TABLE HRChat ADD CONSTRAINT HRChat_ProcessType
    FOREIGN KEY (ProcessTypeId)
    REFERENCES ProcessType (ProcessTypeId);

-- Reference: Notifications_NotificationTypes (table: Notifications)
ALTER TABLE Notifications ADD CONSTRAINT Notifications_NotificationTypes
    FOREIGN KEY (NotificationTypeId)
    REFERENCES NotificationTypes (NotificationTypeId);

-- Reference: Notifications_People (table: Notifications)
ALTER TABLE Notifications ADD CONSTRAINT Notifications_People
    FOREIGN KEY (PersonId)
    REFERENCES People (PersonId);

-- Reference: OfferConditions_ContractConditions (table: OfferConditions)
ALTER TABLE OfferConditions ADD CONSTRAINT OfferConditions_ContractConditions
    FOREIGN KEY (ContractConditionId)
    REFERENCES ContractConditions (ContractConditionId);

-- Reference: OfferConditions_Offers (table: OfferConditions)
ALTER TABLE OfferConditions ADD CONSTRAINT OfferConditions_Offers
    FOREIGN KEY (OfferId)
    REFERENCES Offers (OfferId);

-- Reference: OfferConnections_OfferTemplates (table: OfferConnections)
ALTER TABLE OfferConnections ADD CONSTRAINT OfferConnections_OfferTemplates
    FOREIGN KEY (OfferTemplateId)
    REFERENCES OfferTemplates (OfferTemplateId);

-- Reference: OfferConnections_Offers (table: OfferConnections)
ALTER TABLE OfferConnections ADD CONSTRAINT OfferConnections_Offers
    FOREIGN KEY (OfferId)
    REFERENCES Offers (OfferId);

-- Reference: OfferSkills_OfferTemplates (table: OfferSkills)
ALTER TABLE OfferSkills ADD CONSTRAINT OfferSkills_OfferTemplates
    FOREIGN KEY (OfferTemplateId)
    REFERENCES OfferTemplates (OfferTemplateId);

-- Reference: OfferSkills_Skills (table: OfferSkills)
ALTER TABLE OfferSkills ADD CONSTRAINT OfferSkills_Skills
    FOREIGN KEY (SkillId)
    REFERENCES Skills (SkillId);

-- Reference: OfferTemplates_Companies (table: OfferTemplates)
ALTER TABLE OfferTemplates ADD CONSTRAINT OfferTemplates_Companies
    FOREIGN KEY (CompanyId)
    REFERENCES Companies (CompanyId);

-- Reference: Offers_Branches (table: Offers)
ALTER TABLE Offers ADD CONSTRAINT Offers_Branches
    FOREIGN KEY (BranchId)
    REFERENCES Branches (BranchId);

-- Reference: People_Addresses (table: People)
ALTER TABLE People ADD CONSTRAINT People_Addresses
    FOREIGN KEY (AddressId)
    REFERENCES Addresses (AddressId);

-- Reference: PersonSkills_People (table: PersonSkills)
ALTER TABLE PersonSkills ADD CONSTRAINT PersonSkills_People
    FOREIGN KEY (PersonId)
    REFERENCES People (PersonId);

-- Reference: PersonSkills_Skills (table: PersonSkills)
ALTER TABLE PersonSkills ADD CONSTRAINT PersonSkills_Skills
    FOREIGN KEY (SkillId)
    REFERENCES Skills (SkillId);

-- Reference: SkillConnections_Skills1 (table: SkillConnections)
ALTER TABLE SkillConnections ADD CONSTRAINT SkillConnections_Skills1
    FOREIGN KEY (ChildSkillId)
    REFERENCES Skills (SkillId);

-- Reference: SkillConnections_Skills2 (table: SkillConnections)
ALTER TABLE SkillConnections ADD CONSTRAINT SkillConnections_Skills2
    FOREIGN KEY (ParentSkillId)
    REFERENCES Skills (SkillId);

-- Reference: Skills_SkillTypes (table: Skills)
ALTER TABLE Skills ADD CONSTRAINT Skills_SkillTypes
    FOREIGN KEY (SkillTypeId)
    REFERENCES SkillTypes (SkillTypeId);

-- Reference: States_Countries (table: States)
ALTER TABLE States ADD CONSTRAINT States_Countries
    FOREIGN KEY (CountryId)
    REFERENCES Countries (CountryId);

-- Reference: Urls_People (table: Urls)
ALTER TABLE Urls ADD CONSTRAINT Urls_People
    FOREIGN KEY (PersonId)
    REFERENCES People (PersonId);

-- Reference: Urls_UrlTypes (table: Urls)
ALTER TABLE Urls ADD CONSTRAINT Urls_UrlTypes
    FOREIGN KEY (UrlTypeId)
    REFERENCES UrlTypes (UrlTypeId);

-- End of file.

