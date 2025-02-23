-- OUR OWN IMPLEMENTATION OF VALUES OF SOME TABLES
--======================================================================================
--======================================================================================
--[NotificationType]
--Create
INSERT INTO [dbo].[NotificationType]
(NotificationTypeId, Name, Description)
VALUES
(1, '',''),
(2, '','');


--======================================================================================
--======================================================================================
--[UrlType]
--Create
INSERT INTO [dbo].[UrlType]
(UrlTypeId, Name, Description)
VALUES
(1, 'Undefined',''),
(2, 'Github Repository','');


--======================================================================================
--======================================================================================
--[Role]
--Create
INSERT INTO [dbo].[Role]
(RoleId, Name, Description)
VALUES
(1, 'Company Owner','The Company Owner has an administrator-level role and can manage all processes.');


--======================================================================================
--======================================================================================
--[WorkMode]
--Create
INSERT INTO [dbo].[WorkMode]
(WorkModeId, Name)
VALUES
(1, 'On-Site'), --praca stacjonarna
(2, 'Remote'),
(3, 'Hybrid'),
(4, 'Flexible');


--======================================================================================
--======================================================================================
--[EmploymentType]
--Create
INSERT INTO [dbo].[EmploymentType]
(EmploymentTypeId, Name)
VALUES
(1, 'Internship '),
(2, 'Apprenticeship '),
(3, 'Employment Contract'),
(4, 'Fixed-Term Contract'),
(5, 'Indefinite Contract'),
(6, 'Part-Time Contract'),
(7, 'Full-Time Contract'),
(8, 'Commission Contract '), --umowa prowizyjna
(9, 'Contract of Mandate'), --umowa zlecenie
(10, 'Contract for Task/Work Contract'),
(11, 'B2B'),
(12, 'Freelance Agreement'),
(13, 'Temporary Contract'),
(14, 'Casual Work Contract');
--======================================================================================
--[SalaryTerm]
--Create
INSERT INTO [dbo].[SalaryTerm]
(SalaryTermId, Name)
VALUES
(1, 'Per Hour'),
(2, 'Per Day'),
(3, 'Per Week'),
(4, 'Per Bi-Week'),
(5, 'Per Month'),
(6, 'Per Quarter'),
(7, 'Per Year'),
(8, 'Per Project'),
(9, 'Per Task'),
(10, 'Per Milestone'),
(11, 'Per Feature');
--======================================================================================
--[Currency]
--Create
INSERT INTO [dbo].[Currency]
(CurrencyId, Name)
VALUES
(1, 'PLN'),
(2, 'USD'),
(3, 'EUR'),
(4, 'GBP'),
(5, 'CHF');


--======================================================================================
--======================================================================================
--[ProcessType]
--Create
INSERT INTO [dbo].[ProcessType]
(ProcessTypeId, Name)
VALUES
(1, 'Message'),

(2, 'Accept'),
(3, 'Rejected'),
(5, 'Passed'),

(4, 'Hired'),

(11, 'Application Submitted'),
(12, 'Technical Assessment'),
(13, 'Technical Interview'),
(14, 'Soft Skills Interview'),
(15, 'Job Offer'),
(16, 'CV Screening'),
(17, 'HR Screening');
