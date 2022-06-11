TRUNCATE TABLE [Competencies]

INSERT INTO [dbo].[Personal Informations] ([Name], [Surname], [DateOfBirth], [PlaceOfBirth], [Gender], [Nation], [MaritalStatus], [DriverLicense], [Address], [PhoneNumber], [Email], [Picture]) VALUES ('Tennyson', 'Hoffman', '1992-06-05', 'Los Angeles', 'Bay', 'ABD', 'Bekar', 'B1', 'LA, Verthein Sokağı. - 7/15', '6693391845', 'tennysonhoffman5502629_outlook.com', 'TennysonHoffman.jpg')

INSERT INTO [dbo].[Competencies] ([PersonId], [Competence], [Rate]) VALUES (1, 'C#', 4)
INSERT INTO [dbo].[Competencies] ([PersonId], [Competence], [Rate]) VALUES (1, 'Java', 3)
INSERT INTO [dbo].[Competencies] ([PersonId], [Competence], [Rate]) VALUES (1, 'HTML5', 3)

UPDATE Competencies SET Competence = 'Javaz', Rate = 4 WHERE PersonId = 8 AND Id = 3

SELECT COUNT(*) FROM [Personal Informations] WHERE DATEDIFF(YEAR, DateOfBirth, GETDATE()) >= 18

SELECT COUNT(*) FROM [Personal Informations] WHERE DATEDIFF(YEAR, DateOfBirth, GETDATE()) >= 18 AND DATEDIFF(YEAR, DateOfBirth, GETDATE()) <= 30

SELECT COUNT(*) FROM [Personal Informations] WHERE Gender = 'Bay'

SELECT COUNT(*) FROM [Personal Informations] WHERE DriverLicense = 'B' OR DriverLicense = 'BE'

SELECT COUNT(*) FROM [Personal Informations]

SELECT COUNT(DISTINCT PersonId) FROM Competencies

SELECT DISTINCT Competence FROM Competencies