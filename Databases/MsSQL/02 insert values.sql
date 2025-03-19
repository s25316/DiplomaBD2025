DELETE FROM [dbo].[NotificationTypes];
DELETE FROM [dbo].[UrlTypes];
DELETE FROM [dbo].[Roles];
DELETE FROM [dbo].[ProcessType];

DELETE FROM [dbo].[ContractParameters];
DELETE FROM [dbo].[ContractParameterTypes];

DELETE FROM [dbo].[SkillConnections];
DELETE FROM [dbo].[Skills];
DELETE FROM [dbo].[SkillTypes];
-- OUR OWN IMPLEMENTATION OF VALUES OF SOME TABLES
--======================================================================================
--======================================================================================
--[NotificationType]
--Create
INSERT INTO [dbo].[NotificationTypes]
(NotificationTypeId, Name, Description)
VALUES
(1, '',''),
(2, '','');


--======================================================================================
--======================================================================================
--[UrlType]
--Create
INSERT INTO [dbo].[UrlTypes]
(UrlTypeId, Name)
VALUES
(1, 'Undefined'),
(2, 'Github Repository');


--======================================================================================
--======================================================================================
--[Role]
--Create
INSERT INTO [dbo].[Roles]
(RoleId, Name, Description)
VALUES
(1, 'Company Owner','The Company Owner has an administrator-level role and can manage all processes.');


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


--======================================================================================
--======================================================================================
--[ContractParameterTypes]
--Create
INSERT INTO [dbo].[ContractParameterTypes]
(ContractParameterTypeId, Name)
VALUES
( 1, 'Work Mode'),
( 2, 'Employment Type'),
( 3, 'Salary Term'),
( 4 , 'Currency');
--======================================================================================
--[ContractParameters]
--Create
INSERT INTO [dbo].[ContractParameters]
(ContractParameterId, Name, ContractParameterTypeId)
VALUES
-- ( 4 , 'Currency');
(1, 'PLN', 4),
(2, 'USD', 4),
(3, 'EUR', 4),
(4, 'GBP', 4),
(5, 'CHF', 4),

-- ( 1, 'Work Mode'),
(1001, 'On-Site', 1), --praca stacjonarna
(1002, 'Remote', 1),
(1003, 'Hybrid', 1),
(1004, 'Flexible', 1),

-- ( 2, 'Employment Type'),
(2001, 'Internship', 2),
(2002, 'Apprenticeship', 2),
(2003, 'Employment Contract', 2),
(2004, 'Fixed-Term Contract', 2),
(2005, 'Indefinite Contract', 2),
(2006, 'Part-Time Contract', 2),
(2007, 'Full-Time Contract', 2),
(2008, 'Commission Contract ', 2), --umowa prowizyjna
(2009, 'Contract of Mandate', 2), --umowa zl300ecenie
(2010, 'Contract for Task/Work Contract', 2),
(2011, 'B2B', 2),
(2012, 'Freelance Agreement', 2),
(2013, 'Temporary Contract', 2),
(2014, 'Casual Work Contract', 2),

-- ( 3, 'Salary Term'),
(3001, 'Per Hour', 3),
(3002, 'Per Day', 3),
(3003, 'Per Week', 3),
(3004, 'Per Bi-Week',3 ),
(3005, 'Per Month', 3),
(3006, 'Per Quarter', 3),
(3007, 'Per Year', 3),
(3008, 'Per Project', 3);


--======================================================================================
--======================================================================================
--[SkillType]
--Create
INSERT INTO [dbo].[SkillTypes]
(SkillTypeId, Name)
VALUES 
(1 , 'Regular language'),
(2 , 'Specialization'),
(3 , 'Programming language'),
(4 , 'Framework'),
(5 , 'Database'),
(7 , 'Technology');
--======================================================================================
--[Skill]
--Create
INSERT INTO [dbo].[Skills]
(SkillId, Name, SkillTypeId)
VALUES

--1 Regular languages
(1, 'Polish [A1]', 1),
(2, 'Polish [A2]', 1),
(3, 'Polish [B1]', 1),
(4, 'Polish [B2]', 1),
(5, 'Polish [C1]', 1),
(6, 'Polish [C2]', 1),

(11, 'English [A1]', 1),
(12, 'English [A2]', 1),
(13, 'English [B1]', 1),
(14, 'English [B2]', 1),
(15, 'English [C1]', 1),
(16, 'English [C2]', 1),

(21, 'Dutch [A1]', 1),
(22, 'Dutch [A2]', 1),
(23, 'Dutch [B1]', 1),
(24, 'Dutch [B2]', 1),
(25, 'Dutch [C1]', 1),
(26, 'Dutch [C2]', 1),

(31, 'French [A1]', 1),
(32, 'French [A2]', 1),
(33, 'French [B1]', 1),
(34, 'French [B2]', 1),
(35, 'French [C1]', 1),
(36, 'French [C2]', 1),

(41, 'Spanish [A1]', 1),
(42, 'Spanish [A2]', 1),
(43, 'Spanish [B1]', 1),
(44, 'Spanish [B2]', 1),
(45, 'Spanish [C1]', 1),
(46, 'Spanish [C2]', 1),

(51, 'Italian [A1]', 1),
(52, 'Italian [A2]', 1),
(53, 'Italian [B1]', 1),
(54, 'Italian [B2]', 1),
(55, 'Italian [C1]', 1),
(56, 'Italian [C2]', 1),

--2 Specialization
(4001, 'Web Development', 2),
(4002, 'Mobile Development', 2),
(4003, 'Desktop Development', 2),
(4004, 'Data Science & Machine Learning', 2),
(4005, 'Game Development', 2),
(4006, 'Testing', 2),
(4007, 'Functional Programming', 2),
(4008, 'Scripting & Automation', 2),
(4009, 'Systems Programming',  2),

--3 Programming language
(1001, 'Python', 3),
(1002, 'JavaScript', 3),
(1003, 'TypeScript', 3),
(1004, 'Java', 3),
(1005, 'C#', 3),
(1006, 'C++', 3),
(1007, 'PHP', 3),
(1008, 'Ruby', 3),
(1009, 'Swift', 3),
(1010, 'Kotlin', 3),
(1011, 'Go (Golang)', 3),
(1012, 'Rust', 3),
(1013, 'R', 3),
(1014, 'Perl', 3),
(1015, 'Scala', 3),
(1016, 'Objective-C', 3),
(1017, 'Shell (Bash, Zsh)', 3),
(1018, 'MATLAB', 3),
(1019, 'Dart', 3),
(1020, 'Elixir', 3),
(1021, 'Haskell', 3),
(1022, 'Lua', 3),
(1023, 'Groovy', 3),
(1024, 'Clojure', 3),
(1025, 'Erlang', 3),
(1026, 'Julia', 3),
(1027, 'Visual Basic .NET', 3),
(1028, 'F#', 3),
(1029, 'COBOL', 3),
(1030, 'Fortran', 3),
(1031, 'Assembly Language', 3),
(1032, 'VBScript', 3),
(1033, 'Tcl', 3),
(1034, 'Crystal', 3),
(1035, 'Nim', 3),
(1036, 'APL', 3),

--4 Frameworki
-- Py
(2001, 'Django', 4),
(2002, 'Flask', 4),
(2003, 'FastAPI', 4),
(2004, 'Pyramid', 4),
(2005, 'Pandas', 4),
(2006, 'NumPy', 4),
(2007, 'SciPy', 4),
(2008, 'Matplotlib', 4),
(2009, 'TensorFlow', 4),
(2010, 'PyTorch', 4),
(2011, 'Scikit-learn', 4),
(2012, 'Keras', 4),
(2013, 'TensorFlow', 4),
(2014, 'Tkinter', 4),
(2015, 'PyQt', 4),
(2016, 'Kivy', 4),
(2017, 'Selenium', 4),
(2018, 'Scrapy', 4),
(2147, 'Pygame', 4),
--JS
(2019, 'React', 4), --TS
(2020, 'Angular', 4),--TS
(2021, 'Vue', 4),--TS
(2022, 'Express', 4),--TS
(2023, 'Next.js', 4),
(2024, 'React Native', 4),
(2025, 'Ionic', 4),
(2026, 'Jest', 4),
(2027, 'Mocha', 4),
(2028, 'Cypress', 4),
(2029, 'Phaser', 4),
(2030, 'Babylon.js', 4),
(2146, 'Node.js', 4),--TS
--Java
(2031, 'Spring', 4),
(2032, 'Hibernate', 4),
(2033, 'JavaServer Faces (JSF)', 4),
(2034, 'Grails', 4), --Groovy
(2035, 'Android SDK', 4),
(2036, 'Apache Cordova', 4),
(2037, 'Java EE', 4),
(2038, 'Jakarta EE', 4),
(2039, 'Apache Hadoop', 4),
--C#
(2041, 'ASP.NET Core', 4),
(2042, 'MVC', 4),
(2043, 'Blazor', 4),
(2044, 'Unity', 4),
(2045, 'WPF', 4), --Visual Basic .NET
(2046, 'Xamarin', 4),
(2047, 'Entity Framework', 4),
--C++
(2048, 'Unreal Engine', 4),
(2049, 'Cocos2d', 4),
(2050, 'Qt', 4),
(2051, 'wxWidgets', 4),
(2052, 'ROS (Robot Operating System)', 4),
(2053, 'OpenCV', 4),
(2054, 'Boost', 4),
(2055, 'Armadillo', 4),
--PHP
(2056, 'Laravel, Symfony', 4),
(2057, 'CodeIgniter', 4),
(2058, 'CakePHP', 4),
(2059, 'WordPress', 4),
(2060, 'Drupal', 4),
(2061, 'Joomla', 4),
--Ruby
(2062, 'Ruby on Rails', 4),
(2063, 'Sinatra', 4),
(2064, 'RSpec', 4),
(2065, 'Capybara', 4),
(2066, 'Chef', 4),
(2067, 'Puppet', 4),
--Swift
(2068, 'iOS SDK', 4),
(2069, 'SwiftUI', 4),
(2070, 'Vapor', 4),
--Kotlin
(2071, 'Android SDK', 4),
(2072, 'Ktor', 4), --Web Development
(2073, 'Spring', 4),
--Go (Golang)
(2074, 'Gin', 4),
(2075, 'Echo', 4),
(2076, 'Revel', 4),
(2077, 'gRPC', 4),
(2078, 'Micro', 4),
--Rust
(2079, 'Rocket', 4),
(2080, 'Actix', 4),
(2081, 'Amethyst', 4),
(2082, 'Bevy', 4),
--SQL Frameworks
(2083, 'NHibernate', 4), --C#
(2084, 'SQLAlchemy ', 4), --Py
(2085, 'Hibernate ', 4),--Java
(2086, 'Dapper ', 4), --C#
--R
(2087, 'Shiny', 4),
(2088, 'ggplot2', 4),
(2089, 'dplyr', 4),
(2090, 'caret', 4),
(2091, 'R Markdown', 4),
(2092, 'Bioconductor', 4),
--Perl
(2093, 'Dancer', 4),
(2094, 'Mojolicious', 4),
(2095, 'Test::More', 4),
--Scala
(2096, 'Play Framework', 4),
(2097, 'Akka', 4),
--Objective-C	
(2098, 'Cocoa Touch (iOS)', 4),
(2099, 'AppKit (macOS)', 4),
--Shell (Bash, Zsh)
(2100, 'Bash scripts', 4),
(2101, 'Zsh scripts', 4),
--MATLAB
(2102, 'MATLAB Toolboxes', 4),
(2103, 'MATLAB Statistics and Machine Learning Toolbox', 4),
--Dart
(2104, 'Flutter', 4), --C, C++, Web Development, Mobile Developmen
--Elixir
(2105, 'Phoenix Framework', 4), --Erlang
(2106, 'Nerves ', 4),
--Haskell
(2107, 'Yesod', 4),
(2108, 'Snap', 4),
(2109, 'GHC', 4),
(2110, 'Haskell Platform', 4),
--Lua
(2111, 'Love2D', 4),
(2112, 'Corona SDK', 4),
(2113, 'LuaJIT', 4),
--Groovy
(2114, 'Spring Boot', 4), -- Java
(2115, 'Spock', 4),
--Clojure
(2116, 'Ring', 4),
(2117, 'Compojure', 4),
(2118, 'Core.async', 4),
--Erlang
(2119, 'Cowboy', 4),
(2120, 'RabbitMQ (AMQP)', 4),
--Julia
(2121, 'JuMP', 4),
(2122, 'DataFrames.jl', 4),
(2123, 'Plots.jl', 4),
(2124, 'JuliaDB', 4),
--Visual Basic .NET
(2125, 'Windows Forms', 4), --C#
(2126, 'ASP.NET Web Forms', 4),
--F#
(2127, 'Giraffe', 4),
(2128, 'Saturn', 4),
(2129, 'FSharp.Data', 4),
--COBOL
(2130, 'CICS', 4),
(2131, 'DB2', 4),
--Fortran
(2132, 'LAPACK', 4),
(2133, 'BLAS', 4),
--Assembly 
(2134, 'NASM', 4),
(2135, 'MASM', 4),
--VBScript
(2136, 'ASP (Active Server Page)', 4),
(2137, 'WSH (Windows Script Host)', 4),
--Tcl
(2138, 'Expect', 4),
(2139, 'Tk (GUI)', 4),
--Crystal
(2140, 'Lucky', 4),
(2141, 'Kemal', 4),
--Nim
(2142, 'Jester', 4),
(2143, 'Nimble', 4),
--APL
(2144, 'Dyalog APL', 4),
(2145, 'APL2', 4),


--7 Technology
(5001, 'Amazon Web Services (AWS)', 7),
(5002, 'Microsoft Azure', 7),
(5003, 'Google Cloud Platform (GCP)', 7),
(5004, 'Docker', 7),
(5005, 'Kubernetes', 7),
(5006, 'Apache Hadoop', 7),
(5007, 'Apache Spark', 7),
(5008, 'TensorFlow', 7),
(5009, 'PyTorch', 7),
(5010, 'Scikit-learn', 7),
(5011, 'Jenkins', 7),
(5012, 'GitLab CI', 7),
(5013, 'Selenium', 7),
(5014, 'Postman', 7),
(5015, 'JUnit', 7),
(5016, 'GraphQL', 7),
(5017, 'RESTful API', 7),
(5018, 'WebAssembly', 7),
(5020, 'Machine Learning', 7),
(5021, 'Elasticsearch', 7),
(5022, 'Redis', 7),
(5023, 'RabbitMQ', 7),
(5024, 'Terraform', 7),
(5025, 'Ansible', 7),
(5026, 'Prometheus', 7),
(5027, 'Grafana', 7),
(5028, 'Nagios', 7),
(5029, 'OpenShift', 7),
(5030, 'Service Mesh (Istio, Linkerd)', 7),
(5031, 'Apache Kafka', 7),
(5032, 'Blockchain', 7);
--======================================================================================
--[SkillConnections]
--Create
INSERT INTO [dbo].[SkillConnections]
(ParentSkillId, ChildSkillId)
VALUES

--Languages and FrameWorks
--Python
(1001,2001),
(1001,2002),
(1001,2003),
(1001,2004),
(1001,2005),
(1001,2006),
(1001,2007),
(1001,2008),
(1001,2009),
(1001,2010),
(1001,2011),
(1001,2012),
(1001,2013),
(1001,2014),
(1001,2015),
(1001,2016),
(1001,2017),
(1001,2018),
(1001,2084),
(1001,2147),
--JavaScript
(1002,2019),
(1002,2020),
(1002,2021),
(1002,2022),
(1002,2023),
(1002,2024),
(1002,2025),
(1002,2026),
(1002,2027),
(1002,2028),
(1002,2029),
(1002,2030),
(1002,2146),
-- TyepScript
(1003,2019),
(1003,2020),
(1003,2021),
(1003,2022),
(1003,2146),
--Java
(1004,2031),
(1004,2032),
(1004,2033),
(1004,2034),
(1004,2035),
(1004,2036),
(1004,2037),
(1004,2038),
(1004,2039),
(1004,2085),
(1004,2114),
--2#
(1005,2041),
(1005,2042),
(1005,2043),
(1005,2044),
(1005,2045),
(1005,2046),
(1005,2047),
(1005,2083),
(1005,2086),
(1005,2125),
--C++
(1006,2048),
(1006,2049),
(1006,2050),
(1006,2051),
(1006,2052),
(1006,2053),
(1006,2054),
(1006,2055),
(1006,2104),
--PHP
(1007,2056),
(1007,2057),
(1007,2058),
(1007,2059),
(1007,2060),
(1007,2061),
--Ruby
(1008,2062),
(1008,2063),
(1008,2064),
(1008,2065),
(1008,2066),
(1008,2067),
--Swift
(1009,2068),
(1009,2069),
(1009,2070),
--Kotlin
(1010,2071),
(1010,2072),
(1010,2073),
--Go (Golang)
(1011,2074),
(1011,2075),
(1011,2076),
(1011,2077),
(1011,2078),
--Rust
(1012,2079),
(1012,2080),
(1012,2081),
(1012,2082),
--R
(1013,2087),
(1013,2088),
(1013,2089),
(1013,2090),
(1013,2091),
(1013,2092),
--Perl
(1014,2093),
(1014,2094),
(1014,2095),
--Scala
(1015,2096),
(1015,2097),
--Objective-C
(1016,2098),
(1016,2099),
--Shell (Bash, Zsh)
(1017,2100),
(1017,2101),
--MATLAB
(1018,2102),
(1018,2103),
--Dart
(1019,2104),
--Elixir
(1020,2105),
(1020,2106),
--Haskell
(1021,2107),
(1021,2108),
(1021,2109),
(1021,2110),
--Lua
(1022,2111),
(1022,2112),
(1022,2113),
--Groovy
(1023,2114),
(1023,2115),
(1023,2034),
--Clojure
(1024,2116),
(1024,2117),
(1024,2118),
--Erlang
(1025,2119),
(1025,2120),
(1025,2105),
--Julia
(1026,2121),
(1026,2122),
(1026,2123),
(1026,2124),
--Visual Basic .NET
(1027,2125),
(1027,2126),
(1027,2045),
--F#
(1028,2127),
(1028,2128),
(1028,2129),
--COBOL
(1029,2130),
(1029,2131),
--Fortran
(1030,2132),
(1030,2133),
--Assembly 
(1031,2134),
(1031,2135),
--VBScript
(1032,2136),
(1032,2137),
--Tcl
(1033,2138),
(1033,2139),
--Crystal
(1034,2140),
(1034,2141),
--Nim
(1035,2142),
(1035,2143),
--APL
(1036,2144),
(1036,2145),

--(101, 'Web Development', '', 9),
--Py
(2001, 4001),
(2002, 4001),
(2003, 4001),
(2004, 4001),
(2018, 4001),
(2017, 4001),
--JS
(2019, 4001),
(2020, 4001),
(2021, 4001),
(2022, 4001),
(2023, 4001),
(2024, 4001),
(2025, 4001),
(2026, 4001),
(2027, 4001),
(2028, 4001),
(2029, 4001),
(2030, 4001),
(2146, 4001),
--Java
(2031, 4001),
(2032, 4001),
(2033, 4001),
(2034, 4001),
(2037, 4001),
(2038, 4001),
(2039, 4001),
--C#
(2041, 4001),
(2042, 4001),
(2043, 4001),
--PHP
(2056, 4001),
(2057, 4001),
(2058, 4001),
(2059, 4001),
(2060, 4001),
(2061, 4001),
--Ruby
(2062, 4001),
(2063, 4001),
--Go (Golang)
(2074, 4001),
(2075, 4001),
(2076, 4001),
(2077, 4001),
--Elixir
(2105, 4001),
(2106, 4001),
--Scala
(2096, 4001),
(2097, 4001),
--Rust
(2079, 4001),
(2080, 4001),

--(104, 'Data Science & Machine Learning', '', 9),
--Py
(2005, 4004),
(2006, 4004),
(2007, 4004),
(2008, 4004),
(2009, 4004),
(2010, 4004),
(2011, 4004),
(2012, 4004),
--R
(2087, 4004),
(2088, 4004),
(2089, 4004),
(2090, 4004),
(2091, 4004),
(2092, 4004),


--(102, 'Mobile Development', '', 9),
(2035, 4002),
(2046, 4002),
(2104, 4002),

--(105, 'Game Development', '', 9),
(2048, 4005),
(2044, 4005),
(2049, 4005),
(2147, 4005),
(2029, 4005),

--(103, 'Desktop Development', '', 9),
(2045, 4003),
(2125, 4003),
(2126, 4003),
(2014, 4003),
(2015, 4003),
(2016, 4003),

--(106, 'Testing', 'Testowanie', 9),
--JS
(2026, 4006),
(2027, 4006),
(2028, 4006),
--Ruby
(2064, 4006),
(2065, 4006),

--(107, 'Functional Programming', '', 9),
--Haskell
(2107, 4007),
(2108, 4007),
(2109, 4007),
(2110, 4007),
--Elixir
(2105, 4007),
--F#
(2127, 4007),
(2128, 4007),
(2129, 4007),

--(109, 'Systems Programming'
--Rust
(2079, 4009),
(2080, 4009),

--(108, 'Scripting & Automation', '', 9),
--Shell (Bash, Zsh)
(2100, 4008),
(2101, 4008),
--Perl
(2093, 4008),
(2094, 4008);