SELECT DISTINCT
[Pi].Id AS 'Numara',
[Pi].[Name] AS 'Ad',
[Pi].Surname AS 'Soyad',
[Pi].DateOfBirth AS 'Doğum Tarihi',
[Pi].PlaceOfBirth AS 'Doğum Yeri',
[Pi].Gender AS 'Cinsiyet',
[Pi].Nation AS 'Uyruk',
[Pi].MaritalStatus AS 'Medeni Durum',
[Pi].DriverLicense AS 'Sürücü Ehliyeti',
[Pi].[Address] AS 'Adres',
[Pi].PhoneNumber AS 'Telefon Numarası',
[Pi].Email AS 'E-Posta',
[Pi].Picture AS 'Resim'
FROM [Personal Informations] AS [Pi]
FULL JOIN [Foreign Languages] AS Fl ON (Fl.PersonId = [Pi].Id)
FULL JOIN Competencies AS Co ON (Co.PersonId = [Pi].Id)
WHERE [Pi].[Name] LIKE ''
OR [Pi].Surname LIKE ''
OR [Pi].PlaceOfBirth LIKE ''
OR [Pi].Gender = ''
OR [Pi].Nation LIKE '%b%'
OR [Pi].MaritalStatus = ''
OR [Pi].DriverLicense = ''
OR [Pi].Email LIKE ''
OR [Pi].PhoneNumber LIKE ''
OR Fl.[Language] LIKE ''
OR Co.Competence LIKE ''