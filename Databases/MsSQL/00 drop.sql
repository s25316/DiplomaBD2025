-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-03-15 02:02:15.639

-- foreign keys
ALTER TABLE Addresses DROP CONSTRAINT Addresses_Cities;

ALTER TABLE Addresses DROP CONSTRAINT Addresses_Streets;

ALTER TABLE Branches DROP CONSTRAINT Branches_Addresses;

ALTER TABLE Branches DROP CONSTRAINT Branches_Companies;

ALTER TABLE Cities DROP CONSTRAINT Cities_States;

ALTER TABLE CompanyPeople DROP CONSTRAINT CompanyPeople_Companies;

ALTER TABLE CompanyPeople DROP CONSTRAINT CompanyPeople_People;

ALTER TABLE CompanyPeople DROP CONSTRAINT CompanyPeople_Roles;

ALTER TABLE ContractAttributes DROP CONSTRAINT ContractAttributes_ContractConditions;

ALTER TABLE ContractAttributes DROP CONSTRAINT ContractAttributes_ContractParameters;

ALTER TABLE ContractConditions DROP CONSTRAINT ContractConditions_Companies;

ALTER TABLE ContractParameters DROP CONSTRAINT ContractParameters_ContractParameterTypes;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_HRProcess;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_ProcessType;

ALTER TABLE Notifications DROP CONSTRAINT Notifications_NotificationTypes;

ALTER TABLE Notifications DROP CONSTRAINT Notifications_People;

ALTER TABLE OfferConditions DROP CONSTRAINT OfferConditions_ContractConditions;

ALTER TABLE OfferConditions DROP CONSTRAINT OfferConditions_Offers;

ALTER TABLE OfferConnections DROP CONSTRAINT OfferConnections_OfferTemplates;

ALTER TABLE OfferConnections DROP CONSTRAINT OfferConnections_Offers;

ALTER TABLE OfferSkills DROP CONSTRAINT OfferSkills_OfferTemplates;

ALTER TABLE OfferSkills DROP CONSTRAINT OfferSkills_Skills;

ALTER TABLE OfferTemplates DROP CONSTRAINT OfferTemplates_Companies;

ALTER TABLE Offers DROP CONSTRAINT Offers_Branches;

ALTER TABLE People DROP CONSTRAINT People_Addresses;

ALTER TABLE PersonSkills DROP CONSTRAINT PersonSkills_People;

ALTER TABLE PersonSkills DROP CONSTRAINT PersonSkills_Skills;

ALTER TABLE SkillConnections DROP CONSTRAINT SkillConnections_Skills1;

ALTER TABLE SkillConnections DROP CONSTRAINT SkillConnections_Skills2;

ALTER TABLE Skills DROP CONSTRAINT Skills_SkillTypes;

ALTER TABLE States DROP CONSTRAINT States_Countries;

ALTER TABLE Urls DROP CONSTRAINT Urls_People;

ALTER TABLE Urls DROP CONSTRAINT Urls_UrlTypes;

-- tables
DROP TABLE Addresses;

DROP TABLE Branches;

DROP TABLE Cities;

DROP TABLE Companies;

DROP TABLE CompanyPeople;

DROP TABLE ContractAttributes;

DROP TABLE ContractConditions;

DROP TABLE ContractParameterTypes;

DROP TABLE ContractParameters;

DROP TABLE Countries;

DROP TABLE Exs;

DROP TABLE Faqs;

DROP TABLE HRChat;

DROP TABLE HrProcess;

DROP TABLE NChat;

DROP TABLE NotificationTypes;

DROP TABLE Notifications;

DROP TABLE OfferConditions;

DROP TABLE OfferConnections;

DROP TABLE OfferSkills;

DROP TABLE OfferTemplates;

DROP TABLE Offers;

DROP TABLE People;

DROP TABLE PersonSkills;

DROP TABLE ProcessType;

DROP TABLE Roles;

DROP TABLE SkillConnections;

DROP TABLE SkillTypes;

DROP TABLE Skills;

DROP TABLE States;

DROP TABLE Streets;

DROP TABLE UrlTypes;

DROP TABLE Urls;

-- End of file.

