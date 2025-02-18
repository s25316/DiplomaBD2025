-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-02-17 23:05:37.373

-- foreign keys
ALTER TABLE Address DROP CONSTRAINT Address_City;

ALTER TABLE Address DROP CONSTRAINT Address_Street;

ALTER TABLE Branch DROP CONSTRAINT Branch_Address;

ALTER TABLE Branch DROP CONSTRAINT Branch_Company;

ALTER TABLE City DROP CONSTRAINT City_State;

ALTER TABLE CompanyOfferContract DROP CONSTRAINT CompanyOfferContract_CompanyOffer;

ALTER TABLE CompanyOfferContract DROP CONSTRAINT CompanyOfferContract_ContractType;

ALTER TABLE CompanyOfferWork DROP CONSTRAINT CompanyOfferWork_CompanyOffer;

ALTER TABLE CompanyOfferWork DROP CONSTRAINT CompanyOfferWork_WorkType;

ALTER TABLE CompanyOffer DROP CONSTRAINT CompanyOffer_Branch;

ALTER TABLE CompanyOffer DROP CONSTRAINT CompanyOffer_Company;

ALTER TABLE CompanyOffer DROP CONSTRAINT CompanyOffer_Offer;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Company;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Person;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Role;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_HRProcess;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_ProcessType;

ALTER TABLE HRProcess DROP CONSTRAINT HRProcess_CompanyOffer;

ALTER TABLE HRProcess DROP CONSTRAINT HRProcess_Person;

ALTER TABLE NChat DROP CONSTRAINT NChat_Notification;

ALTER TABLE Notification DROP CONSTRAINT Notification_NotificationType;

ALTER TABLE Notification DROP CONSTRAINT Notification_Person;

ALTER TABLE OfferSkill DROP CONSTRAINT OfferSkill_Offer;

ALTER TABLE OfferSkill DROP CONSTRAINT OfferSkill_Skill;

ALTER TABLE PersonSkill DROP CONSTRAINT PersonSkill_Person;

ALTER TABLE PersonSkill DROP CONSTRAINT PersonSkill_Skill;

ALTER TABLE Person DROP CONSTRAINT Person_Address;

ALTER TABLE SkillConnections DROP CONSTRAINT SkillConnections_Skill1;

ALTER TABLE SkillConnections DROP CONSTRAINT SkillConnections_Skill2;

ALTER TABLE Skill DROP CONSTRAINT Skill_SkillType;

ALTER TABLE State DROP CONSTRAINT State_Country;

ALTER TABLE Url DROP CONSTRAINT Url_Person;

ALTER TABLE Url DROP CONSTRAINT Url_UrlType;

-- tables
DROP TABLE Address;

DROP TABLE Branch;

DROP TABLE City;

DROP TABLE Company;

DROP TABLE CompanyOffer;

DROP TABLE CompanyOfferContract;

DROP TABLE CompanyOfferWork;

DROP TABLE CompanyPerson;

DROP TABLE ContractType;

DROP TABLE Country;

DROP TABLE Ex;

DROP TABLE Faq;

DROP TABLE HRChat;

DROP TABLE HRProcess;

DROP TABLE NChat;

DROP TABLE Notification;

DROP TABLE NotificationType;

DROP TABLE Offer;

DROP TABLE OfferSkill;

DROP TABLE Person;

DROP TABLE PersonSkill;

DROP TABLE ProcessType;

DROP TABLE Role;

DROP TABLE Skill;

DROP TABLE SkillConnections;

DROP TABLE SkillType;

DROP TABLE State;

DROP TABLE Street;

DROP TABLE Url;

DROP TABLE UrlType;

DROP TABLE WorkType;

-- End of file.

