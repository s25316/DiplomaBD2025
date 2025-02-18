-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-02-17 23:05:37.373

-- tables
-- Table: Address
CREATE TABLE Address (
    AddressId uniqueidentifier  NOT NULL,
    CityId int  NOT NULL,
    StreetId int  NULL,
    HouseNumber nvarchar(25)  NOT NULL,
    ApartmentNumber nvarchar(25)  NOT NULL,
    PostCode nvarchar(25)  NOT NULL,
    Lon real  NOT NULL,
    Lat real  NOT NULL,
    Point geography  NOT NULL,
    CONSTRAINT Address_pk PRIMARY KEY  (AddressId)
);

-- Table: Branch
CREATE TABLE Branch (
    BranchId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    AddressId uniqueidentifier  NOT NULL,
    CONSTRAINT Branch_pk PRIMARY KEY  (BranchId)
);

-- Table: City
CREATE TABLE City (
    CityId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    StateId int  NOT NULL,
    CONSTRAINT City_pk PRIMARY KEY  (CityId)
);

-- Table: Company
CREATE TABLE Company (
    CompanyId uniqueidentifier  NOT NULL,
    Logo nvarchar(100)  NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Regon nvarchar(25)  NULL,
    Nip nvarchar(25)  NULL,
    Krs nvarchar(25)  NULL,
    Url nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    Blocked datetime  NOT NULL,
    CONSTRAINT Company_pk PRIMARY KEY  (CompanyId)
);

-- Table: CompanyOffer
CREATE TABLE CompanyOffer (
    CompanyOfferId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    BranchId uniqueidentifier  NULL,
    PublishStart datetime  NOT NULL,
    PublishEnd datetime  NULL,
    WorkStart date  NULL,
    WorkEnd date  NULL,
    MinSalary money  NOT NULL,
    MaxSalary money  NOT NULL,
    IsNegotiated bit  NOT NULL,
    Url nvarchar(800)  NULL,
    CONSTRAINT CompanyOffer_pk PRIMARY KEY  (CompanyOfferId)
);

-- Table: CompanyOfferContract
CREATE TABLE CompanyOfferContract (
    ContractId int  NOT NULL,
    ContractTypeId int  NOT NULL,
    CompanyOfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT CompanyOfferContract_pk PRIMARY KEY  (ContractId)
);

-- Table: CompanyOfferWork
CREATE TABLE CompanyOfferWork (
    WorkId int  NOT NULL,
    WorkTypeId int  NOT NULL,
    CompanyOfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT CompanyOfferWork_pk PRIMARY KEY  (WorkId)
);

-- Table: CompanyPerson
CREATE TABLE CompanyPerson (
    CompanyPersonId uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    RoleId int  NOT NULL,
    "Grant" datetime  NOT NULL,
    "Deny" datetime  NULL,
    CONSTRAINT CompanyPerson_pk PRIMARY KEY  (CompanyPersonId)
);

-- Table: ContractType
CREATE TABLE ContractType (
    ContractTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT ContractType_pk PRIMARY KEY  (ContractTypeId)
);

-- Table: Country
CREATE TABLE Country (
    CountryId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY  (CountryId)
);

-- Table: Ex
CREATE TABLE Ex (
    ExceptionId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    ExceptionType nvarchar(100)  NOT NULL,
    Method nvarchar(100)  NULL,
    StackTrace nvarchar(800)  NOT NULL,
    Message  nvarchar(max)  NOT NULL,
    Other nvarchar(max)  NULL,
    Request int  NULL,
    CONSTRAINT Ex_pk PRIMARY KEY  (ExceptionId)
);

-- Table: Faq
CREATE TABLE Faq (
    FaqId int  NOT NULL,
    Question nvarchar(800)  NOT NULL,
    Answer nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Faq_pk PRIMARY KEY  (FaqId)
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

-- Table: HRProcess
CREATE TABLE HRProcess (
    ProcessId uniqueidentifier  NOT NULL,
    CompanyOfferId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    CONSTRAINT HRProcess_pk PRIMARY KEY  (ProcessId)
);

-- Table: NChat
CREATE TABLE NChat (
    MessageId uniqueidentifier  NOT NULL,
    NotificationId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    "Read" datetime  NULL,
    IsAdminSend bit  NOT NULL,
    Message nvarchar(800)  NULL,
    MongoUrl nvarchar(100)  NULL,
    CONSTRAINT NChat_pk PRIMARY KEY  (MessageId)
);

-- Table: Notification
CREATE TABLE Notification (
    NotificationId uniqueidentifier  NOT NULL,
    IsAdminSend bit  NOT NULL,
    PersonId uniqueidentifier  NULL,
    Email nvarchar(100)  NULL,
    Message nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    "Read" datetime  NULL,
    Completed datetime  NULL,
    NotificationTypeId int  NOT NULL,
    ObjectId uniqueidentifier  NULL,
    CONSTRAINT Notification_pk PRIMARY KEY  (NotificationId)
);

-- Table: NotificationType
CREATE TABLE NotificationType (
    NotificationTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(100)  NOT NULL,
    CONSTRAINT NotificationType_pk PRIMARY KEY  (NotificationTypeId)
);

-- Table: Offer
CREATE TABLE Offer (
    OfferId uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Offer_pk PRIMARY KEY  (OfferId)
);

-- Table: OfferSkill
CREATE TABLE OfferSkill (
    OfferId uniqueidentifier  NOT NULL,
    SkillId int  NOT NULL,
    IsRequired bit  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT OfferSkill_pk PRIMARY KEY  (OfferId,SkillId)
);

-- Table: Person
CREATE TABLE Person (
    PersonId uniqueidentifier  NOT NULL,
    Login nvarchar(100)  NULL,
    Logo nvarchar(100)  NULL,
    Name nvarchar(100)  NULL,
    Surname nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    PhoneNum nvarchar(100)  NULL,
    ContactEmail nvarchar(100)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    Blocked datetime  NULL,
    BirthDate date  NULL,
    IsTwoFactorAuth bit  NOT NULL,
    IsStudent bit  NOT NULL,
    IsAdmin bit  NOT NULL,
    AddressId uniqueidentifier  NULL,
    CONSTRAINT Person_pk PRIMARY KEY  (PersonId)
);

-- Table: PersonSkill
CREATE TABLE PersonSkill (
    PersonSkillId int  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    Skilld int  NOT NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT PersonSkill_pk PRIMARY KEY  (PersonSkillId)
);

-- Table: ProcessType
CREATE TABLE ProcessType (
    ProcessTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT ProcessType_pk PRIMARY KEY  (ProcessTypeId)
);

-- Table: Role
CREATE TABLE Role (
    RoleId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    CONSTRAINT Role_pk PRIMARY KEY  (RoleId)
);

-- Table: Skill
CREATE TABLE Skill (
    Skilld int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    SkillTypeId int  NOT NULL,
    CONSTRAINT Skill_pk PRIMARY KEY  (Skilld)
);

-- Table: SkillConnections
CREATE TABLE SkillConnections (
    Parentld int  NOT NULL,
    Childld int  NOT NULL,
    CONSTRAINT SkillConnections_pk PRIMARY KEY  (Parentld,Childld)
);

-- Table: SkillType
CREATE TABLE SkillType (
    SkillTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    CONSTRAINT SkillType_pk PRIMARY KEY  (SkillTypeId)
);

-- Table: State
CREATE TABLE State (
    StateId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CountryId int  NOT NULL,
    CONSTRAINT State_pk PRIMARY KEY  (StateId)
);

-- Table: Street
CREATE TABLE Street (
    StreetId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Street_pk PRIMARY KEY  (StreetId)
);

-- Table: Url
CREATE TABLE Url (
    UrlId int  NOT NULL,
    UrlTypeId int  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    Value nvarchar(800)  NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(800)  NULL,
    Created datetime  NOT NULL,
    Removed datetime  NULL,
    CONSTRAINT Url_pk PRIMARY KEY  (UrlId)
);

-- Table: UrlType
CREATE TABLE UrlType (
    UrlTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(800)  NOT NULL,
    CONSTRAINT UrlType_pk PRIMARY KEY  (UrlTypeId)
);

-- Table: WorkType
CREATE TABLE WorkType (
    WorkTypeId int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT WorkType_pk PRIMARY KEY  (WorkTypeId)
);

-- foreign keys
-- Reference: Address_City (table: Address)
ALTER TABLE Address ADD CONSTRAINT Address_City
    FOREIGN KEY (CityId)
    REFERENCES City (CityId);

-- Reference: Address_Street (table: Address)
ALTER TABLE Address ADD CONSTRAINT Address_Street
    FOREIGN KEY (StreetId)
    REFERENCES Street (StreetId);

-- Reference: Branch_Address (table: Branch)
ALTER TABLE Branch ADD CONSTRAINT Branch_Address
    FOREIGN KEY (AddressId)
    REFERENCES Address (AddressId);

-- Reference: Branch_Company (table: Branch)
ALTER TABLE Branch ADD CONSTRAINT Branch_Company
    FOREIGN KEY (CompanyId)
    REFERENCES Company (CompanyId);

-- Reference: City_State (table: City)
ALTER TABLE City ADD CONSTRAINT City_State
    FOREIGN KEY (StateId)
    REFERENCES State (StateId);

-- Reference: CompanyOfferContract_CompanyOffer (table: CompanyOfferContract)
ALTER TABLE CompanyOfferContract ADD CONSTRAINT CompanyOfferContract_CompanyOffer
    FOREIGN KEY (CompanyOfferId)
    REFERENCES CompanyOffer (CompanyOfferId);

-- Reference: CompanyOfferContract_ContractType (table: CompanyOfferContract)
ALTER TABLE CompanyOfferContract ADD CONSTRAINT CompanyOfferContract_ContractType
    FOREIGN KEY (ContractTypeId)
    REFERENCES ContractType (ContractTypeId);

-- Reference: CompanyOfferWork_CompanyOffer (table: CompanyOfferWork)
ALTER TABLE CompanyOfferWork ADD CONSTRAINT CompanyOfferWork_CompanyOffer
    FOREIGN KEY (CompanyOfferId)
    REFERENCES CompanyOffer (CompanyOfferId);

-- Reference: CompanyOfferWork_WorkType (table: CompanyOfferWork)
ALTER TABLE CompanyOfferWork ADD CONSTRAINT CompanyOfferWork_WorkType
    FOREIGN KEY (WorkTypeId)
    REFERENCES WorkType (WorkTypeId);

-- Reference: CompanyOffer_Branch (table: CompanyOffer)
ALTER TABLE CompanyOffer ADD CONSTRAINT CompanyOffer_Branch
    FOREIGN KEY (BranchId)
    REFERENCES Branch (BranchId);

-- Reference: CompanyOffer_Company (table: CompanyOffer)
ALTER TABLE CompanyOffer ADD CONSTRAINT CompanyOffer_Company
    FOREIGN KEY (CompanyId)
    REFERENCES Company (CompanyId);

-- Reference: CompanyOffer_Offer (table: CompanyOffer)
ALTER TABLE CompanyOffer ADD CONSTRAINT CompanyOffer_Offer
    FOREIGN KEY (OfferId)
    REFERENCES Offer (OfferId);

-- Reference: CompanyPerson_Company (table: CompanyPerson)
ALTER TABLE CompanyPerson ADD CONSTRAINT CompanyPerson_Company
    FOREIGN KEY (CompanyId)
    REFERENCES Company (CompanyId);

-- Reference: CompanyPerson_Person (table: CompanyPerson)
ALTER TABLE CompanyPerson ADD CONSTRAINT CompanyPerson_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (PersonId);

-- Reference: CompanyPerson_Role (table: CompanyPerson)
ALTER TABLE CompanyPerson ADD CONSTRAINT CompanyPerson_Role
    FOREIGN KEY (RoleId)
    REFERENCES Role (RoleId);

-- Reference: HRChat_HRProcess (table: HRChat)
ALTER TABLE HRChat ADD CONSTRAINT HRChat_HRProcess
    FOREIGN KEY (ProcessId)
    REFERENCES HRProcess (ProcessId);

-- Reference: HRChat_ProcessType (table: HRChat)
ALTER TABLE HRChat ADD CONSTRAINT HRChat_ProcessType
    FOREIGN KEY (ProcessTypeId)
    REFERENCES ProcessType (ProcessTypeId);

-- Reference: HRProcess_CompanyOffer (table: HRProcess)
ALTER TABLE HRProcess ADD CONSTRAINT HRProcess_CompanyOffer
    FOREIGN KEY (CompanyOfferId)
    REFERENCES CompanyOffer (CompanyOfferId);

-- Reference: HRProcess_Person (table: HRProcess)
ALTER TABLE HRProcess ADD CONSTRAINT HRProcess_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (PersonId);

-- Reference: NChat_Notification (table: NChat)
ALTER TABLE NChat ADD CONSTRAINT NChat_Notification
    FOREIGN KEY (NotificationId)
    REFERENCES Notification (NotificationId);

-- Reference: Notification_NotificationType (table: Notification)
ALTER TABLE Notification ADD CONSTRAINT Notification_NotificationType
    FOREIGN KEY (NotificationTypeId)
    REFERENCES NotificationType (NotificationTypeId);

-- Reference: Notification_Person (table: Notification)
ALTER TABLE Notification ADD CONSTRAINT Notification_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (PersonId);

-- Reference: OfferSkill_Offer (table: OfferSkill)
ALTER TABLE OfferSkill ADD CONSTRAINT OfferSkill_Offer
    FOREIGN KEY (OfferId)
    REFERENCES Offer (OfferId);

-- Reference: OfferSkill_Skill (table: OfferSkill)
ALTER TABLE OfferSkill ADD CONSTRAINT OfferSkill_Skill
    FOREIGN KEY (SkillId)
    REFERENCES Skill (Skilld);

-- Reference: PersonSkill_Person (table: PersonSkill)
ALTER TABLE PersonSkill ADD CONSTRAINT PersonSkill_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (PersonId);

-- Reference: PersonSkill_Skill (table: PersonSkill)
ALTER TABLE PersonSkill ADD CONSTRAINT PersonSkill_Skill
    FOREIGN KEY (Skilld)
    REFERENCES Skill (Skilld);

-- Reference: Person_Address (table: Person)
ALTER TABLE Person ADD CONSTRAINT Person_Address
    FOREIGN KEY (AddressId)
    REFERENCES Address (AddressId);

-- Reference: SkillConnections_Skill1 (table: SkillConnections)
ALTER TABLE SkillConnections ADD CONSTRAINT SkillConnections_Skill1
    FOREIGN KEY (Parentld)
    REFERENCES Skill (Skilld);

-- Reference: SkillConnections_Skill2 (table: SkillConnections)
ALTER TABLE SkillConnections ADD CONSTRAINT SkillConnections_Skill2
    FOREIGN KEY (Childld)
    REFERENCES Skill (Skilld);

-- Reference: Skill_SkillType (table: Skill)
ALTER TABLE Skill ADD CONSTRAINT Skill_SkillType
    FOREIGN KEY (SkillTypeId)
    REFERENCES SkillType (SkillTypeId);

-- Reference: State_Country (table: State)
ALTER TABLE State ADD CONSTRAINT State_Country
    FOREIGN KEY (CountryId)
    REFERENCES Country (CountryId);

-- Reference: Url_Person (table: Url)
ALTER TABLE Url ADD CONSTRAINT Url_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (PersonId);

-- Reference: Url_UrlType (table: Url)
ALTER TABLE Url ADD CONSTRAINT Url_UrlType
    FOREIGN KEY (UrlTypeId)
    REFERENCES UrlType (UrlTypeId);

-- End of file.

