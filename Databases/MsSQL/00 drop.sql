-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-02-22 20:16:16.808

-- foreign keys
ALTER TABLE Address DROP CONSTRAINT Address_City;

ALTER TABLE Address DROP CONSTRAINT Address_Street;

ALTER TABLE Branch DROP CONSTRAINT Branch_Address;

ALTER TABLE Branch DROP CONSTRAINT Branch_Company;

ALTER TABLE City DROP CONSTRAINT City_State;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Company;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Person;

ALTER TABLE CompanyPerson DROP CONSTRAINT CompanyPerson_Role;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_HRProcess;

ALTER TABLE HRChat DROP CONSTRAINT HRChat_ProcessType;

ALTER TABLE HRProcess DROP CONSTRAINT HRProcess_Offer;

ALTER TABLE HRProcess DROP CONSTRAINT HRProcess_Person;

ALTER TABLE NChat DROP CONSTRAINT NChat_Notification;

ALTER TABLE Notification DROP CONSTRAINT Notification_NotificationType;

ALTER TABLE Notification DROP CONSTRAINT Notification_Person;

ALTER TABLE OfferEmploymentType DROP CONSTRAINT OfferEmploymentType_EmploymentType;

ALTER TABLE OfferEmploymentType DROP CONSTRAINT OfferEmploymentType_Offer;

ALTER TABLE OfferSkill DROP CONSTRAINT OfferSkill_OfferTemplate;

ALTER TABLE OfferSkill DROP CONSTRAINT OfferSkill_Skill;

ALTER TABLE OfferWorkMode DROP CONSTRAINT OfferWorkMode_Offer;

ALTER TABLE OfferWorkMode DROP CONSTRAINT OfferWorkMode_WorkMode;

ALTER TABLE Offer DROP CONSTRAINT Offer_Branch;

ALTER TABLE Offer DROP CONSTRAINT Offer_Company;

ALTER TABLE Offer DROP CONSTRAINT Offer_Currency;

ALTER TABLE Offer DROP CONSTRAINT Offer_OfferTemplate;

ALTER TABLE Offer DROP CONSTRAINT Offer_SalaryTerm;

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

DROP TABLE CompanyPerson;

DROP TABLE Country;

DROP TABLE Currency;

DROP TABLE EmploymentType;

DROP TABLE Ex;

DROP TABLE Faq;

DROP TABLE HRChat;

DROP TABLE HRProcess;

DROP TABLE NChat;

DROP TABLE Notification;

DROP TABLE NotificationType;

DROP TABLE Offer;

DROP TABLE OfferEmploymentType;

DROP TABLE OfferSkill;

DROP TABLE OfferTemplate;

DROP TABLE OfferWorkMode;

DROP TABLE Person;

DROP TABLE PersonSkill;

DROP TABLE ProcessType;

DROP TABLE Role;

DROP TABLE SalaryTerm;

DROP TABLE Skill;

DROP TABLE SkillConnections;

DROP TABLE SkillType;

DROP TABLE State;

DROP TABLE Street;

DROP TABLE Url;

DROP TABLE UrlType;

DROP TABLE WorkMode;

-- End of file.

