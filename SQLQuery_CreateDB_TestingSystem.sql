USE model;
GO

CREATE DATABASE TestingSystem;
GO

USE TestingSystem;
GO

CREATE TABLE Category	--categories
(
	"Id" SMALLINT NOT NULL PRIMARY KEY IDENTITY,
	"Name" NVARCHAR(30) NOT NULL,
	"Active" BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Test	--tests
(
	"Id" SMALLINT NOT NULL PRIMARY KEY IDENTITY,
	"Name" NVARCHAR(60) NOT NULL,
	"CategoryId" SMALLINT NOT NULL
		FOREIGN KEY
		REFERENCES dbo.Category(Id),
	"Active" BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Question --issues
(
	"Id" INT NOT NULL PRIMARY KEY IDENTITY,
	"QuestionText" NVARCHAR(500) NOT NULL,
	"TestId" SMALLINT NOT NULL
		FOREIGN KEY
		REFERENCES dbo.Test(Id),
	"Active" BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Answer	--answers
(
	"Id" INT NOT NULL PRIMARY KEY IDENTITY,
	"ResponseText" NVARCHAR(300) NOT NULL,
	"QuestionId" INT NOT NULL
		FOREIGN KEY
		REFERENCES dbo.Question(Id),
	"CorrectAnswer" BIT NOT NULL DEFAULT 0
);
GO


-- Добавление тестовых первичных данных.

SET IDENTITY_INSERT [dbo].[Category] ON
INSERT INTO [dbo].[Category] ([Id], [Name], [Active]) VALUES (13, N'C#', 1)
INSERT INTO [dbo].[Category] ([Id], [Name], [Active]) VALUES (15, N'С', 0)
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Test] ON
INSERT INTO [dbo].[Test] ([Id], [Name], [CategoryId], [Active]) VALUES (12, N'Тест2', 13, 1)
INSERT INTO [dbo].[Test] ([Id], [Name], [CategoryId], [Active]) VALUES (13, N'Легкий', 13, 1)
SET IDENTITY_INSERT [dbo].[Test] OFF
SET IDENTITY_INSERT [dbo].[Question] ON
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (9, N'Вопрос 1', 12, 1)
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (10, N'Вопрос 2', 12, 1)
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (11, N'Какой тип переменной используется в коде: int a = 5', 13, 1)
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (12, N'Что делает оператор «%»', 13, 1)
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (13, N'Что сделает программа выполнив следующий код: Console.WriteLine(«Hello, World!»)', 13, 1)
INSERT INTO [dbo].[Question] ([Id], [QuestionText], [TestId], [Active]) VALUES (14, N'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum д', 12, 1)
SET IDENTITY_INSERT [dbo].[Question] OFF
SET IDENTITY_INSERT [dbo].[Answer] ON
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (17, N'ответ 1', 9, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (18, N'ответ 2', 9, 1)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (20, N'ответ 1', 10, 1)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (21, N'ответ 2', 10, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (22, N'"Знаковое 32-бит целое"', 11, 1)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (23, N'"Знаковое 64-бит целое"', 11, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (24, N'"Знаковое 8-бит целое"', 11, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (25, N'"1 байт*"', 11, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (26, N'"Ни чего из выше перечисленного"', 12, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (27, N'"Возвращает остаток от деления"', 12, 1)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (28, N'"Возвращает процент от суммы"', 12, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (29, N'"Возвращает тригонометрическую функцию"', 12, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (30, N'"Вырежет слово Hello, World! из всего текста"', 13, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (31, N'"Удалит все значения с Hello, World!"', 13, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (32, N'"Напишет Hello, World!"', 13, 1)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (33, N'"Напишет на новой строчке Hello, World!"', 13, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (34, N'Давно выяснено, что при оценке дизайна и композиции читаемый текст мешает сосредоточиться. Lorem Ipsum используют потому, что тот обеспечивает более или менее стандартное заполнение шаблона, а также реальное распределение букв и пробелов в абзацах, которое не получается при простой дубликации "Здесь', 14, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (35, N'разу показывает, как много веб-страниц всё ещё дожидаются своего настоящего рождения. За прошедшие годы текст Lorem Ipsum получил много версий. Некоторые версии появились по ошибке, некоторые - намеренно (например, юмористические варианты).', 14, 0)
INSERT INTO [dbo].[Answer] ([Id], [ResponseText], [QuestionId], [CorrectAnswer]) VALUES (36, N'ариантов Lorem Ipsum, но большинство из них имеет не всегда приемлемые модификации, например, юмористические вставки или слова, которые даже отдалённо не напоминают латынь. Если вам нужен Lorem Ipsum для серьёзного проекта, вы наверняка не хотите какой-нибудь шутки, скрытой в середине абзаца. Также ', 14, 1)
SET IDENTITY_INSERT [dbo].[Answer] OFF