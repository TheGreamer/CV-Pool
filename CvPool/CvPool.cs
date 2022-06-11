using CvPool.Classes;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;

namespace CvPool
{
    public partial class CvPool : Form
    {
        #region Fields
        public static string imagesFolder;
        public static string textFolder;
        public static GroupBox[] groupBoxes;

        private int id, personId, companyId;
        private string personImage, personImagePath, companyImage, companyImagePath, companyName;
        private bool pictureState, companyPictureState;
        private readonly string[] tableNames;
        private readonly Control[] operationControls, leftPanelControls, companyControls, leftPanelOperationControls;
        private readonly ComboBox[] comboBoxes;
        private readonly ListView[] listViews;
        #endregion

        #region Constructor Method
        public CvPool()
        {
            InitializeComponent();

            panelCvFindLeft.Width = Width / 4;
            panelCvFindRight.Width = (int)(Width * 1.4); // (int)(Width * 1.4)
            panelCompanyFindLeft.Width = Width / 4;
            panelCompanyFindRight.Width = (int)(Width * 1.4);

            personImage = string.Empty;
            companyImage = string.Empty;
            companyName = string.Empty;
            pictureState = false;
            companyPictureState = false;
            imagesFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.GetDirectories("Images")[0].FullName;
            textFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.GetDirectories("Text Files")[0].FullName;

            tableNames = new string[] { "Educations", "[Work Experiences]", "Courses", "Certificates", "Competencies", "[Foreign Languages]", "Projects", "[References]", "Hobbies", "[Personal Informations]" };
            operationControls = new Control[] { textBoxOperationCompetence, textBoxOperationEmail, textBoxOperationForeignLanguage, textBoxOperationName, textBoxOperationNation, textBoxOperationPhone, textBoxOperationPlaceOfBirth, textBoxOperationSurname, comboBoxOperationDriverLicense, comboBoxOperationGender, comboBoxOperationMaritalStatus };
            leftPanelControls = new Control[] { buttonGetCvsByFilters, buttonGetAllCvs, buttonClearFilters, buttonClearTable, buttonShowDetails, comboBoxDetails };
            companyControls = new Control[] { textBoxSearchCompany, textBoxSearchCompanyTitle, textBoxSearchCompanyIndustry, textBoxSearchCompanyEmail, textBoxSearchCompanyPhone };
            leftPanelOperationControls = new Control[] { buttonGetCompaniesByFilters, buttonGetAllCompanies, buttonClearCompanyFilters, buttonClearCompanyTable, buttonUpdateSelectedCompany, buttonDeleteCompany };
            groupBoxes = new GroupBox[] { groupBoxCertificate, groupBoxCompetence, groupBoxCourse, groupBoxEducationInfo, groupBoxForeignLanguageInfo, groupBoxHobby, groupBoxPersonInfo, groupBoxProject, groupBoxReference, groupBoxWorkExperience, groupBoxCvOperations, groupBoxUpdatePerson, groupBoxUpdateEducation, groupBoxUpdateWorkExperience, groupBoxUpdateCourse, groupBoxUpdateCertificate, groupBoxUpdateCompetence, groupBoxUpdateLanguage, groupBoxUpdateProject, groupBoxUpdateReference, groupBoxUpdateHobby, groupBoxCompany, groupBoxUpdateCompany, groupBoxCompanyOperations, groupBoxPanel, groupBoxCvDetails };
            comboBoxes = new ComboBox[] { comboBoxCertificatePerson, comboBoxCompetencePerson, comboBoxCoursePerson, comboBoxEducationPerson, comboBoxForeignLanguagePerson, comboBoxHobbyPerson, comboBoxProjectPerson, comboBoxReferencePerson, comboBoxWorkExperiencePerson };
            listViews = new ListView[] { listViewEducations, listViewWorkExperiences, listViewCourses, listViewCertificates, listViewCompetencies, listViewForeignLanguages, listViewProjects, listViewReferences, listViewHobbies };

            Utilities.ToggleControlsVisibleState(new Component[] { titleCv, titleCompany, titlePanel, optionCancel }, false);
            Utilities.GroupBoxProcess(processType: VisibilityProcess.SetDocks);

            for (int i = 0; i < comboBoxes.Length; i++)
            {
                DbManager.GetData(comboBoxes[i], tableNames[^1], null, null, null, null, null, null, false, false, null, "Name", "Id");
            }

            DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, false);
            DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, null, null, false, false);
        }
        #endregion

        #region Menu Options
        private void PictureCloseClick(object sender, EventArgs e) => Application.Exit();

        private void PictureMinimizeClick(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        private void OptionAddPersonInfoClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxPersonInfo);

        private void OptionAddEducationInfoClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxEducationInfo);

        private void OptionAddWorkExperienceClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxWorkExperience);

        private void OptionAddForeignLanguageClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxForeignLanguageInfo);

        private void OptionAddCourseClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxCourse);

        private void OptionAddCompetenceClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxCompetence);

        private void OptionAddCertificateClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxCertificate);

        private void OptionAddProjectClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxProject);

        private void OptionAddReferenceClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxReference);

        private void OptionAddHobbyClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxHobby);

        private void OptionCvOperationsClick(object sender, EventArgs e)
        {
            Utilities.GroupBoxProcess(groupBoxCvOperations);
            Utilities.ClearSelectionFromDGV(dataGridViewCvData);
        }

        private void OptionAddCompanyClick(object sender, EventArgs e) => Utilities.GroupBoxProcess(groupBoxCompany);

        private void OptionCompanyOperationsClick(object sender, EventArgs e)
        {
            Utilities.GroupBoxProcess(groupBoxCompanyOperations);
            Utilities.ClearSelectionFromDGV(dataGridViewCompanyData);
        }

        private void OptionPanelClick(object sender, EventArgs e)
        {
            Utilities.GroupBoxProcess(groupBoxPanel);

            DbManager.GetData(comboBoxCompetencies, tableNames[4], new string[] { "Competence" }, null, null, null, null, null, true, false, null, "Competence");
            DbManager.GetData(comboBoxForeignLanguages, tableNames[5], new string[] { "Language" }, null, null, null, null, null, true, false, null, "Language");

            labelAgeLessThan18_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DATEDIFF(YEAR, DateOfBirth, GETDATE())" }, new object[] { 18 }, new ArithmeticOperator[] { ArithmeticOperator.LessThan }).ToString();
            labelAgeBetween18And30_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DATEDIFF(YEAR, DateOfBirth, GETDATE())", "DATEDIFF(YEAR, DateOfBirth, GETDATE())" }, new object[] { 18, 30 }, new ArithmeticOperator[] { ArithmeticOperator.BiggerThanOrEqual, ArithmeticOperator.LessThan }).ToString();
            labelAgeBetween30And50_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DATEDIFF(YEAR, DateOfBirth, GETDATE())", "DATEDIFF(YEAR, DateOfBirth, GETDATE())" }, new object[] { 30, 50 }, new ArithmeticOperator[] { ArithmeticOperator.BiggerThanOrEqual, ArithmeticOperator.LessThan }).ToString();
            labelAgeBiggerThan50_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DATEDIFF(YEAR, DateOfBirth, GETDATE())" }, new object[] { 50 }, new ArithmeticOperator[] { ArithmeticOperator.BiggerThanOrEqual }).ToString();

            int maleCount = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "Gender" }, new object[] { "'Bay'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal });
            int femaleCount = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "Gender" }, new object[] { "'Bayan'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal });
            labelMaleCount_.Text = maleCount.ToString();
            labelFemaleCount_.Text = femaleCount.ToString();
            labelMaleFemaleRate_.Text = maleCount == 0 || femaleCount == 0 ? "-" : (maleCount / femaleCount).ToString();

            labelLocalNationCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "Nation" }, new object[] { "'TC'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal }).ToString();
            labelNonLocalNationCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "Nation" }, new object[] { "'TC'" }, new ArithmeticOperator[] { ArithmeticOperator.NotEqual }).ToString();

            int marriedCount = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "MaritalStatus" }, new object[] { "'Evli'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal });
            int nonMarriedCount = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "MaritalStatus" }, new object[] { "'Bekar'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal });
            labelMarriedCount_.Text = marriedCount.ToString();
            labelNonMarriedCount_.Text = nonMarriedCount.ToString();
            labelMarriedNonMarriedRate_.Text = marriedCount == 0 || nonMarriedCount == 0 ? "-" : (marriedCount / nonMarriedCount).ToString();
            labelDivorcedCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "MaritalStatus" }, new object[] { "'Boşanmış'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal }).ToString();

            labelCarLicenseCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DriverLicense", "DriverLicense" }, new object[] { "'B'", "'BE'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal, ArithmeticOperator.Equal }, false, LogicOperator.Or).ToString();
            labelBikeLicenseCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DriverLicense", "DriverLicense", "DriverLicense", "DriverLicense", "DriverLicense" }, new object[] { "'M'", "'A1'", "'A2'", "'A'", "'B1'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal }, false, LogicOperator.Or).ToString();
            labelTruckLicenseCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DriverLicense", "DriverLicense", "DriverLicense", "DriverLicense" }, new object[] { "'C1'", "'C1E'", "'C'", "'CE'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal }, false, LogicOperator.Or).ToString();
            labelBusLicenseCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DriverLicense", "DriverLicense", "DriverLicense", "DriverLicense" }, new object[] { "'D1'", "'D1E'", "'D'", "'DE'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal, ArithmeticOperator.Equal }, false, LogicOperator.Or).ToString();
            labelTractorLicenseCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, new string[] { "DriverLicense" }, new object[] { "'F'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal }).ToString();

            labelTotalCvCount_.Text = DbManager.GetDataCount(tableNames[^1], string.Empty, null, null, null).ToString();
            labelTotalCompanyCount_.Text = DbManager.GetDataCount("Companies", string.Empty, null, null, null).ToString();

            labelCourseCount_.Text = DbManager.GetDataCount(tableNames[2], "COUNT(DISTINCT PersonId)", null, null, null).ToString();
            labelCertificateCount_.Text = DbManager.GetDataCount(tableNames[3], "COUNT(DISTINCT PersonId)", null, null, null).ToString();
            labelProjectCount_.Text = DbManager.GetDataCount(tableNames[6], "COUNT(DISTINCT PersonId)", null, null, null).ToString();
            labelWorkExperienceCount_.Text = DbManager.GetDataCount(tableNames[1], "COUNT(DISTINCT PersonId)", null, null, null).ToString();
        }

        private void OptionCancelClick(object sender, EventArgs e)
        {
            switch (comboBoxDetails.SelectedIndex)
            {
                case < 1:
                    Utilities.GroupBoxProcess(processType: VisibilityProcess.HideAll);
                    break;
                default:
                    DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, false);
                    Utilities.ClearValues(operationControls);
                    Utilities.ClearValues(leftPanelControls);
                    Utilities.ClearValues(companyControls);
                    Utilities.ClearValues(leftPanelOperationControls);
                    Utilities.ToggleControlsEnabledState(operationControls, true);
                    Utilities.ToggleControlsEnabledState(leftPanelControls, true);
                    Utilities.ToggleControlsEnabledState(companyControls, true);
                    Utilities.ToggleControlsEnabledState(leftPanelOperationControls, true);
                    Utilities.ClearSelectionFromDGV(dataGridViewCvData);
                    Utilities.ClearSelectionFromDGV(dataGridViewCompanyData);
                    break;
            }
        }
        #endregion

        #region CV Addition Events
        private void ButtonSelectPersonPictureClick(object sender, EventArgs e)
        {
            personImage = string.Empty;

            switch (pictureDialog.ShowDialog())
            {
                case DialogResult.OK:
                    personImage = pictureDialog.FileName;
                    picturePerson.BackgroundImage = Image.FromFile(personImage);
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Resim seçimini iptal ettiniz. Resim seçimi yapmalısınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void ButtonAddPersonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPersonName.Text) || string.IsNullOrWhiteSpace(textBoxPersonSurname.Text) || string.IsNullOrWhiteSpace(textBoxPersonPlaceOfBirth.Text) || string.IsNullOrWhiteSpace(comboBoxPersonGender.Text) || string.IsNullOrWhiteSpace(textBoxPersonNation.Text) || string.IsNullOrWhiteSpace(comboBoxPersonMaritalStatus.Text) || string.IsNullOrWhiteSpace(comboBoxPersonDriverLicense.Text) || string.IsNullOrWhiteSpace(richTextBoxPersonAddress.Text) || string.IsNullOrWhiteSpace(textBoxPersonPhone.Text) || string.IsNullOrWhiteSpace(textBoxPersonEmail.Text) || string.IsNullOrEmpty(personImage) || string.IsNullOrWhiteSpace(datePersonDateOfBirth.Value.ToString()))
                MessageBox.Show("Kişisel bilgiler ve iletişim bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[^1], "Name, Surname, DateOfBirth, PlaceOfBirth, Gender, Nation, MaritalStatus, DriverLicense, Address, PhoneNumber, Email, Picture", $"'{textBoxPersonName.Text}', '{textBoxPersonSurname.Text}', '{datePersonDateOfBirth.Value:MM/dd/yyyy}', '{textBoxPersonPlaceOfBirth.Text}', '{comboBoxPersonGender.Text}', '{textBoxPersonNation.Text}', '{comboBoxPersonMaritalStatus.Text}', '{comboBoxPersonDriverLicense.Text}', '{richTextBoxPersonAddress.Text}', '{textBoxPersonPhone.Text}', '{textBoxPersonEmail.Text}', '{Path.GetFileName(personImage)}'", "Kişi bilgisi eklendi.");
                Utilities.SaveImage(personImage, imagesFolder);
                Utilities.ClearValues(new object[] { textBoxPersonName, textBoxPersonSurname, textBoxPersonPlaceOfBirth, comboBoxPersonGender, textBoxPersonNation, comboBoxPersonMaritalStatus, comboBoxPersonDriverLicense, richTextBoxPersonAddress, textBoxPersonPhone, textBoxPersonEmail, personImage, datePersonDateOfBirth, picturePerson });

                for (int i = 0; i < comboBoxes.Length; i++)
                {
                    DbManager.GetData(comboBoxes[i], tableNames[^1], null, null, null, null, null, null, false, false, null, "Name", "Id");
                }

                DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, false);
            }
        }

        private void ButtonAddEducationClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxEducationPerson.Text) || string.IsNullOrWhiteSpace(dateEducationStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateEducationEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxEducationDiplomaName.Text) || string.IsNullOrWhiteSpace(textBoxEducationSchoolName.Text) || string.IsNullOrWhiteSpace(textBoxEducationSectionName.Text) || string.IsNullOrWhiteSpace(richTextBoxEducationInfo.Text))
                MessageBox.Show("Eğitim bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[0], "PersonId, StartDate, EndDate, DiplomaName, SchoolName, SectionName, Information", $"{comboBoxEducationPerson.SelectedValue}, '{dateEducationStart.Value:MM/dd/yyyy}', '{dateEducationEnd.Value:MM/dd/yyyy}', '{textBoxEducationDiplomaName.Text}', '{textBoxEducationSchoolName.Text}', '{textBoxEducationSectionName.Text}', '{richTextBoxEducationInfo.Text}'", $"{comboBoxEducationPerson.Text} adlı kişiye eğitim bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxEducationPerson, dateEducationStart, dateEducationEnd, textBoxEducationDiplomaName, textBoxEducationSchoolName, textBoxEducationSectionName, richTextBoxEducationInfo });
            }
        }

        private void ButtonAddWorkExperienceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxWorkExperiencePerson.Text) || string.IsNullOrWhiteSpace(dateWorkExperienceStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateWorkExperienceEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxWorkExperienceRole.Text) || string.IsNullOrWhiteSpace(textBoxWorkExperienceCompanyName.Text) || string.IsNullOrWhiteSpace(textBoxWorkExperienceCompanyLocation.Text) || string.IsNullOrWhiteSpace(richTextBoxWorkExperienceDescription.Text))
                MessageBox.Show("İş deneyimi bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[1], "PersonId, StartDate, EndDate, Role, CompanyName, CompanyLocation, Information", $"{comboBoxWorkExperiencePerson.SelectedValue}, '{dateWorkExperienceStart.Value:MM/dd/yyyy}', '{dateWorkExperienceEnd.Value:MM/dd/yyyy}', '{textBoxWorkExperienceRole.Text}', '{textBoxWorkExperienceCompanyName.Text}', '{textBoxWorkExperienceCompanyLocation.Text}', '{richTextBoxWorkExperienceDescription.Text}'", $"{comboBoxWorkExperiencePerson.Text} adlı kişiye iş deneyimi bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxWorkExperiencePerson, dateWorkExperienceStart, dateWorkExperienceEnd, textBoxWorkExperienceRole, textBoxWorkExperienceCompanyName, textBoxWorkExperienceCompanyLocation, richTextBoxWorkExperienceDescription });
            }
        }

        private void ButtonAddCourseClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxCoursePerson.Text) || string.IsNullOrWhiteSpace(dateCourseStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateCourseEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxCourseName.Text) || string.IsNullOrWhiteSpace(textBoxCompanyName.Text)|| string.IsNullOrWhiteSpace(richTextBoxCourseDescription.Text))
                MessageBox.Show("Kurs/Seminer bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[2], "PersonId, StartDate, EndDate, CourseName, CourseCompany, Information", $"{comboBoxCoursePerson.SelectedValue}, '{dateCourseStart.Value:MM/dd/yyyy}', '{dateCourseEnd.Value:MM/dd/yyyy}', '{textBoxCourseName.Text}', '{textBoxCompanyName.Text}', '{richTextBoxCourseDescription.Text}'", $"{comboBoxCoursePerson.Text} adlı kişiye kurs/seminer bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxCoursePerson, dateCourseStart, dateCourseEnd, textBoxCourseName, textBoxCompanyName, richTextBoxCourseDescription });
            }
        }

        private void ButtonAddCertificateClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxCertificatePerson.Text) || string.IsNullOrWhiteSpace(dateCertificateStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateCertificateEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxCertificateCompanyName.Text) || string.IsNullOrWhiteSpace(textBoxCertificateNo.Text) || string.IsNullOrWhiteSpace(numericUpDownCertificateDuration.Value.ToString()) || string.IsNullOrWhiteSpace(richTextBoxCertificateDescription.Text))
                MessageBox.Show("Sertifika bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[3], "PersonId, StartDate, EndDate, CompanyName, CertificateNo, Duration, Information", $"{comboBoxCertificatePerson.SelectedValue}, '{dateCertificateStart.Value:MM/dd/yyyy}', '{dateCertificateEnd.Value:MM/dd/yyyy}', '{textBoxCertificateCompanyName.Text}', '{textBoxCertificateNo.Text}', {Convert.ToInt32(numericUpDownCertificateDuration.Value)}, '{richTextBoxCertificateDescription.Text}'", $"{comboBoxCertificatePerson.Text} adlı kişiye sertifika bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxCertificatePerson, dateCertificateStart, dateCertificateEnd, textBoxCertificateCompanyName, textBoxCertificateNo, numericUpDownCertificateDuration, richTextBoxCertificateDescription });
            }
        }

        private void ButtonAddCompetenceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxCompetencePerson.Text) || string.IsNullOrWhiteSpace(textBoxCompetence.Text) || string.IsNullOrWhiteSpace(comboBoxCompetenceRate.Text))
                MessageBox.Show("Yetkinlik bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[4], "PersonId, Competence, Rate", $"{comboBoxCompetencePerson.SelectedValue}, '{textBoxCompetence.Text}', {Convert.ToInt32(comboBoxCompetenceRate.Text)}", $"{comboBoxCompetencePerson.Text} adlı kişiye {textBoxCompetence.Text} yetkinliği eklendi.");
                Utilities.ClearValues(new object[] { comboBoxCompetencePerson, textBoxCompetence, comboBoxCompetenceRate });
            }
        }

        private void ButtonAddForeignLanguageClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxForeignLanguagePerson.Text) || string.IsNullOrWhiteSpace(textBoxForeignLanguage.Text) || string.IsNullOrWhiteSpace(comboBoxForeignLanguageRate.Text))
                MessageBox.Show("Yabancı dil bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[5], "PersonId, Language, Rate", $"{comboBoxForeignLanguagePerson.SelectedValue}, '{textBoxForeignLanguage.Text}', '{comboBoxForeignLanguageRate.Text}'", $"{comboBoxForeignLanguagePerson.Text} adlı kişiye {textBoxForeignLanguage.Text} yabancı dili eklendi.");
                Utilities.ClearValues(new object[] { comboBoxForeignLanguagePerson, textBoxForeignLanguage, comboBoxForeignLanguageRate });
            }
        }

        private void ButtonAddProjectClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxProjectPerson.Text) || string.IsNullOrWhiteSpace(textBoxProjectName.Text) || string.IsNullOrWhiteSpace(dateProject.Value.ToString()) || string.IsNullOrWhiteSpace(richTextBoxProjectDescription.Text))
                MessageBox.Show("Proje bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[6], "PersonId, Name, Date, Information", $"{comboBoxProjectPerson.SelectedValue}, '{textBoxProjectName.Text}', '{dateProject.Value:MM/dd/yyyy}', '{richTextBoxProjectDescription.Text}'", $"{comboBoxProjectPerson.Text} adlı kişiye proje bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxProjectPerson, textBoxProjectName, dateProject, richTextBoxProjectDescription });
            }
        }

        private void ButtonAddReferenceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxReferencePerson.Text) || string.IsNullOrWhiteSpace(textBoxReferenceName.Text) || string.IsNullOrWhiteSpace(textBoxReferenceSurname.Text) || string.IsNullOrWhiteSpace(textBoxReferenceRole.Text) || string.IsNullOrWhiteSpace(textBoxReferencePhoneNumber.Text) || string.IsNullOrWhiteSpace(textBoxReferenceEmail.Text))
                MessageBox.Show("Referans bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[7], "PersonId, Name, Surname, Role, PhoneNumber, Email, Information", $"{comboBoxReferencePerson.SelectedValue}, '{textBoxReferenceName.Text}', '{textBoxReferenceSurname.Text}', '{textBoxReferenceRole.Text}', '{textBoxReferencePhoneNumber.Text}', '{textBoxReferenceEmail.Text}', '{textBoxReferenceName.Text}'", $"{comboBoxReferencePerson.Text} adlı kişiye referans bilgisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxReferencePerson, textBoxReferenceName, textBoxReferenceSurname, textBoxReferenceRole, textBoxReferencePhoneNumber, textBoxReferenceEmail });
            }
        }

        private void ButtonAddHobbyClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxHobbyPerson.Text) || string.IsNullOrWhiteSpace(textBoxHobby.Text))
                MessageBox.Show("Hobi bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData(tableNames[8], "PersonId, Hobby", $"{comboBoxHobbyPerson.SelectedValue}, '{textBoxHobby.Text}'", $"{comboBoxHobbyPerson.Text} adlı kişiye {textBoxHobby.Text} hobisi eklendi.");
                Utilities.ClearValues(new object[] { comboBoxHobbyPerson, textBoxHobby });
            }
        }
        #endregion

        #region CV Update Events
        private void ButtonUpdatePersonPictureClick(object sender, EventArgs e)
        {
            personImage = string.Empty;

            switch (pictureDialog.ShowDialog())
            {
                case DialogResult.OK:
                    personImage = pictureDialog.FileName;
                    pictureUpdatePerson.BackgroundImage = Image.FromFile(personImage);
                    pictureState = true;
                    break;
            }
        }

        private void ButtonUpdatePersonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdatePersonName.Text) || string.IsNullOrWhiteSpace(textBoxUpdatePersonSurname.Text) || string.IsNullOrWhiteSpace(textBoxUpdatePersonPlaceOfBirth.Text) || string.IsNullOrWhiteSpace(comboBoxUpdatePersonGender.Text) || string.IsNullOrWhiteSpace(textBoxUpdatePersonNation.Text) || string.IsNullOrWhiteSpace(comboBoxUpdatePersonMaritalStatus.Text) || string.IsNullOrWhiteSpace(comboBoxUpdatePersonDriverLicense.Text) || string.IsNullOrWhiteSpace(richTextBoxUpdatePersonAddress.Text) || string.IsNullOrWhiteSpace(textBoxUpdatePersonPhone.Text) || string.IsNullOrWhiteSpace(textBoxUpdatePersonEmail.Text) || string.IsNullOrWhiteSpace(dateUpdatePersonDateOfBirth.Value.ToString()))
                MessageBox.Show("Kişisel bilgiler ve iletişim bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[^1], new string[] { "Name", "Surname", "DateOfBirth", "PlaceOfBirth", "Gender", "Nation", "MaritalStatus", "DriverLicense", "Address", "PhoneNumber", "Email", "Picture" }, new object[] { $"'{textBoxUpdatePersonName.Text}'", $"'{textBoxUpdatePersonSurname.Text}'", $"'{dateUpdatePersonDateOfBirth.Value:MM/dd/yyyy}'", $"'{textBoxUpdatePersonPlaceOfBirth.Text}'", $"'{comboBoxUpdatePersonGender.Text}'", $"'{textBoxUpdatePersonNation.Text}'", $"'{comboBoxUpdatePersonMaritalStatus.Text}'", $"'{comboBoxUpdatePersonDriverLicense.Text}'", $"'{richTextBoxUpdatePersonAddress.Text}'", $"'{textBoxUpdatePersonPhone.Text}'", $"'{textBoxUpdatePersonEmail.Text}'", $"'{(personImage == string.Empty ? Path.GetFileName(personImagePath) : Path.GetFileName(personImage))}'" }, new string[] { "Id" }, new object[] { personId }, "Kişi bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, false);

                if (pictureState)
                {
                    Utilities.SaveImage(personImage, imagesFolder, false);
                    pictureState = false;
                }
            }
        }

        private void ButtonUpdateEducationClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateUpdateEducationStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateUpdateEducationEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxUpdateEducationDiplomaName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateEducationSchoolName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateEducationSectionName.Text) || string.IsNullOrWhiteSpace(richTextBoxUpdateEducationDescription.Text))
                MessageBox.Show("Eğitim bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[0], new string[] { "StartDate", "EndDate", "DiplomaName", "SchoolName", "SectionName", "Information" }, new object[] { $"'{dateUpdateEducationStart.Value:MM/dd/yyyy}'", $"'{dateUpdateEducationEnd.Value:MM/dd/yyyy}'", $"'{textBoxUpdateEducationDiplomaName.Text}'", $"'{textBoxUpdateEducationSchoolName.Text}'", $"'{textBoxUpdateEducationSectionName.Text}'", $"'{richTextBoxUpdateEducationDescription.Text}'" }, new string[] { "PersonId", "Id" }, new object[] { personId, id }, "Eğitim bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[0], new string[] { "Id AS 'Numara'", "DiplomaName AS 'Eğitim'", "SchoolName AS 'Okul'", "SectionName AS 'Bölüm'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateWorkExperienceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateUpdateWorkExperienceStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateUpdateWorkExperienceEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxUpdateWorkExperienceRole.Text) || string.IsNullOrWhiteSpace(textBoxUpdateWorkExperienceCompanyName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateWorkExperienceCompanyLocation.Text) || string.IsNullOrWhiteSpace(richTextBoxUpdateWorkExperienceDescription.Text))
                MessageBox.Show("İş deneyimi bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[1], new string[] { "StartDate", "EndDate", "Role", "CompanyName", "CompanyLocation", "Information" }, new object[] { $"'{dateUpdateWorkExperienceStart.Value:MM/dd/yyyy}'", $"'{dateUpdateWorkExperienceEnd.Value:MM/dd/yyyy}'", $"'{textBoxUpdateWorkExperienceRole.Text}'", $"'{textBoxUpdateWorkExperienceCompanyName.Text}'", $"'{textBoxUpdateWorkExperienceCompanyLocation.Text}'", $"'{richTextBoxUpdateWorkExperienceDescription.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "İş deneyimi bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[1], new string[] { "Id AS 'Numara'", "Role AS 'Pozisyon'", "CompanyName AS 'Şirket'", "CompanyLocation AS 'Konum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateCourseClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateUpdateCourseStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateUpdateCourseEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxUpdateCourseName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCourseCompany.Text) || string.IsNullOrWhiteSpace(richTextBoxUpdateCourseDescription.Text))
                MessageBox.Show("Kurs - Seminer bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[2], new string[] { "StartDate", "EndDate", "CourseName", "CourseCompany", "Information" }, new object[] { $"'{dateUpdateCourseStart.Value:MM/dd/yyyy}'", $"'{dateUpdateCourseEnd.Value:MM/dd/yyyy}'", $"'{textBoxUpdateCourseName.Text}'", $"'{textBoxUpdateCourseCompany.Text}'", $"'{richTextBoxUpdateCourseDescription.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Kurs - Seminer bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[2], new string[] { "Id AS 'Numara'", "CourseName AS 'Kurs - Seminer'", "CourseCompany AS 'Kurum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateCertificateClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateUpdateCertificateStart.Value.ToString()) || string.IsNullOrWhiteSpace(dateUpdateCertificateEnd.Value.ToString()) || string.IsNullOrWhiteSpace(textBoxUpdateCertificateCompanyName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCertificateNo.Text) || string.IsNullOrWhiteSpace(numericUpDownUpdateCertificateDuration.Value.ToString()) || string.IsNullOrWhiteSpace(richTextBoxUpdateCertificateDescription.Text))
                MessageBox.Show("Sertifika bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[3], new string[] { "StartDate", "EndDate", "CompanyName", "CertificateNo", "Duration", "Information" }, new object[] { $"'{dateUpdateCertificateStart.Value:MM/dd/yyyy}'", $"'{dateUpdateCertificateEnd.Value:MM/dd/yyyy}'", $"'{textBoxUpdateCertificateCompanyName.Text}'", $"'{textBoxUpdateCertificateNo.Text}'", $"'{numericUpDownUpdateCertificateDuration.Value}'", $"'{richTextBoxUpdateCertificateDescription.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Sertifika bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[3], new string[] { "Id AS 'Numara'", "CompanyName AS 'Sertifika Kurumu - Adı'", "CertificateNo AS 'Sertifika Numarası'", "Duration AS 'Süre'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateCompetenceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateCompetence.Text) || string.IsNullOrWhiteSpace(comboBoxUpdateCompetenceRate.Text))
                MessageBox.Show("Yetkinlik bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[4], new string[] { "Competence", "Rate" }, new object[] { $"'{textBoxUpdateCompetence.Text}'", $"{Convert.ToInt32(comboBoxUpdateCompetenceRate.Text)}" }, new string[] { "PersonId", "Id" }, new object[] { personId, id }, "Yetkinlik bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[4], new string[] { "Id AS 'Numara'", "Competence AS 'Yetkinlik'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateForeignLanguageClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateForeignLanguage.Text) || string.IsNullOrWhiteSpace(comboBoxUpdateLanguageRate.Text))
                MessageBox.Show("Yabancı dil bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[5], new string[] { "Language", "Rate" }, new object[] { $"'{textBoxUpdateForeignLanguage.Text}'", $"'{comboBoxUpdateLanguageRate.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Yabancı dil bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[5], new string[] { "Id AS 'Numara'", "Language AS 'Yabancı Dil'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateProjectClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateProjectName.Text) || string.IsNullOrWhiteSpace(dateUpdateProject.Value.ToString()) || string.IsNullOrWhiteSpace(richTextBoxUpdateProjectDescription.Text))
                MessageBox.Show("Proje bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[6], new string[] { "Name", "Date", "Information" }, new object[] { $"'{textBoxUpdateProjectName.Text}'", $"'{dateUpdateProject.Value:MM/dd/yyyy}'", $"'{richTextBoxUpdateProjectDescription.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Proje bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[6], new string[] { "Id AS 'Numara'", "Name AS 'Proje'", "Date AS 'Tarih'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateReferenceClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateReferenceName.Text) || string.IsNullOrWhiteSpace(textBoxUpdateReferenceSurname.Text) || string.IsNullOrWhiteSpace(textBoxUpdateReferenceRole.Text) || string.IsNullOrWhiteSpace(textBoxUpdateReferencePhone.Text) || string.IsNullOrWhiteSpace(textBoxUpdateReferenceEmail.Text))
                MessageBox.Show("Referans bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[7], new string[] { "Name", "Surname", "Role", "PhoneNumber", "Email" }, new object[] { $"'{textBoxUpdateReferenceName.Text}'", $"'{textBoxUpdateReferenceSurname.Text}'", $"'{textBoxUpdateReferenceRole.Text}'", $"'{textBoxUpdateReferencePhone.Text}'", $"'{textBoxUpdateReferenceEmail.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Referans bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[7], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "Role AS 'Pozisyon'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }

        private void ButtonUpdateHobbyClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateHobby.Text))
                MessageBox.Show("Hobi bilgisi boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData(tableNames[8], new string[] { "Hobby" }, new object[] { $"'{textBoxUpdateHobby.Text}'" }, new string[] { "PersonId", "Id" }, new object[] {personId, id }, "Hobi bilgisi güncellendi", groupBoxCvOperations);
                DbManager.GetData(dataGridViewCvData, tableNames[8], new string[] { "Id AS 'Numara'", "Hobby AS 'Hobi'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            }
        }
        #endregion

        #region CV Operation Events
        private void ButtonGetAllCvsClick(object sender, EventArgs e) => DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, true);

        private void ButtonGetCvsByFiltersClick(object sender, EventArgs e)
        {
            DbManager.GetData(dataGridViewCvData, $"{tableNames[^1]} AS [Pi]", new string[] { "[Pi].Id AS 'Numara'", "[Pi].[Name] AS 'Ad'", "[Pi].Surname AS 'Soyad'", "[Pi].DateOfBirth AS 'Doğum Tarihi'", "[Pi].PlaceOfBirth AS 'Doğum Yeri'", "[Pi].Gender AS 'Cinsiyet'", "[Pi].Nation AS 'Uyruk'", "[Pi].MaritalStatus AS 'Medeni Durum'", "[Pi].DriverLicense AS 'Sürücü Ehliyeti'", "[Pi].[Address] AS 'Adres'", "[Pi].PhoneNumber AS 'Telefon Numarası'", "[Pi].Email AS 'E-Posta'", "[Pi].Picture AS 'Resim'" }, new string[] { $"{tableNames[5]} AS Fl", $"{tableNames[4]} AS Co" }, new string[] { "Fl.PersonId", "Co.PersonId" }, "[Pi].Id)", new string[] { "[Pi].[Name]", "[Pi].Surname", "[Pi].PlaceOfBirth", "[Pi].Gender", "[Pi].Nation", "[Pi].MaritalStatus", "[Pi].DriverLicense", "[Pi].Email", "[Pi].PhoneNumber", "Fl.Language", "Co.Competence" }, new object[] { $"'{(string.IsNullOrWhiteSpace(textBoxOperationName.Text) ? "" : $"%{textBoxOperationName.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationSurname.Text) ? "" : $"%{textBoxOperationSurname.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationPlaceOfBirth.Text) ? "" : $"%{textBoxOperationPlaceOfBirth.Text}%")}'", $"'{(comboBoxOperationGender.Text == string.Empty ? string.Empty : comboBoxOperationGender.Text)}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationNation.Text) ? "" : $"%{textBoxOperationNation.Text}%")}'", $"'{(comboBoxOperationMaritalStatus.Text == string.Empty ? string.Empty : comboBoxOperationMaritalStatus.Text)}'", $"'{(comboBoxOperationDriverLicense.Text == string.Empty ? string.Empty : comboBoxOperationDriverLicense.Text)}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationEmail.Text) ? "" : $"%{textBoxOperationEmail.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationPhone.Text) ? "" : $"%{textBoxOperationPhone.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationForeignLanguage.Text) ? "" : $"%{textBoxOperationForeignLanguage.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxOperationCompetence.Text) ? "" : $"%{textBoxOperationCompetence.Text}%")}'" }, true, true, JoinType.Full);
            Utilities.SetFilterColors(dataGridViewCvData, operationControls);
        }

        private void ButtonClearFiltersClick(object sender, EventArgs e) => Utilities.ClearValues(operationControls);

        private void ButtonClearTableClick(object sender, EventArgs e)
        {
            switch (dataGridViewCvData.Rows.Count)
            {
                case > 0:
                    MessageBox.Show("Tablo temizlendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Utilities.ClearValues(new Control[] { dataGridViewCvData });
                    break;
                default:
                    MessageBox.Show("Tabloda temizlenecek veri bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void ButtonUpdateCvClick(object sender, EventArgs e)
        {
            if (dataGridViewCvData.SelectedCells.Count != 0)
            {
                switch (comboBoxDetails.SelectedIndex)
                {
                    case 1: DbManager.GetSelectedRowData(groupBoxUpdateEducation, new Control[] { dateUpdateEducationStart, dateUpdateEducationEnd, textBoxUpdateEducationDiplomaName, textBoxUpdateEducationSchoolName, textBoxUpdateEducationSectionName, richTextBoxUpdateEducationDescription }, tableNames[0], "Id", id, "StartDate", "EndDate", "DiplomaName", "SchoolName", "SectionName", "Information"); break;
                    case 2: DbManager.GetSelectedRowData(groupBoxUpdateWorkExperience, new Control[] { dateUpdateWorkExperienceStart, dateUpdateWorkExperienceEnd, textBoxUpdateWorkExperienceRole, textBoxUpdateWorkExperienceCompanyName, textBoxUpdateWorkExperienceCompanyLocation, richTextBoxUpdateWorkExperienceDescription }, tableNames[1], "Id", id, "StartDate", "EndDate", "Role", "CompanyName", "CompanyLocation", "Information"); break;
                    case 3: DbManager.GetSelectedRowData(groupBoxUpdateCourse, new Control[] { dateUpdateCourseStart, dateUpdateCourseEnd, textBoxUpdateCourseName, textBoxUpdateCourseCompany, richTextBoxUpdateCourseDescription }, tableNames[2], "Id", id, "StartDate", "EndDate", "CourseName", "CourseCompany", "Information"); break;
                    case 4: DbManager.GetSelectedRowData(groupBoxUpdateCertificate, new Control[] { dateUpdateCertificateStart, dateUpdateCertificateEnd, textBoxUpdateCertificateCompanyName, textBoxUpdateCertificateNo, numericUpDownUpdateCertificateDuration, richTextBoxUpdateCertificateDescription }, tableNames[3], "Id", id, "StartDate", "EndDate", "CourseCompany", "CertificateNo", "Duration", "Information"); break;
                    case 5: DbManager.GetSelectedRowData(groupBoxUpdateCompetence, new Control[] { textBoxUpdateCompetence, comboBoxUpdateCompetenceRate }, tableNames[4], "Id", id, "Competence", "Rate"); break;
                    case 6: DbManager.GetSelectedRowData(groupBoxUpdateLanguage, new Control[] { textBoxUpdateForeignLanguage, comboBoxUpdateLanguageRate }, tableNames[5], "Id", id, "Language", "Rate"); break;
                    case 7: DbManager.GetSelectedRowData(groupBoxUpdateProject, new Control[] { textBoxUpdateProjectName, dateUpdateProject, richTextBoxUpdateProjectDescription }, tableNames[6], "Id", id, "Name", "Date", "Information"); break;
                    case 8: DbManager.GetSelectedRowData(groupBoxUpdateReference, new Control[] { textBoxUpdateReferenceName, textBoxUpdateReferenceSurname, textBoxUpdateReferenceRole, textBoxUpdateReferencePhone, textBoxUpdateReferenceEmail }, tableNames[7], "Id", id, "Name", "Surname", "Role", "PhoneNumber", "Email"); break;
                    case 9: DbManager.GetSelectedRowData(groupBoxUpdateHobby, new Control[] { textBoxUpdateHobby }, tableNames[8], "Id", id, "Hobby"); break;
                    default: DbManager.GetSelectedRowData(groupBoxUpdatePerson, new Control[] { textBoxUpdatePersonName, textBoxUpdatePersonSurname, dateUpdatePersonDateOfBirth, textBoxUpdatePersonPlaceOfBirth, comboBoxUpdatePersonGender, textBoxUpdatePersonNation, comboBoxUpdatePersonMaritalStatus, comboBoxUpdatePersonDriverLicense, textBoxUpdatePersonEmail, textBoxUpdatePersonPhone, richTextBoxUpdatePersonAddress, pictureUpdatePerson }, tableNames[9], "Id", personId, "Name", "Surname", "DateOfBirth", "PlaceOfBirth", "Gender", "Nation", "MaritalStatus", "DriverLicense", "Email", "PhoneNumber", "Address", "Picture"); break;
                }
            }
            else
            {
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonDeleteCvClick(object sender, EventArgs e)
        {
            if (dataGridViewCvData.SelectedCells.Count != 0)
            {
                if (MessageBox.Show($"{(comboBoxDetails.SelectedIndex >= 1 && comboBoxDetails.SelectedIndex <= 9 ? "Seçilen kaydı silmek istediğinize emin misiniz?" : "Seçili kayda ait tüm bilgileri silmek istediğinize emin misiniz?")}", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    switch (comboBoxDetails.SelectedIndex)
                    {
                        case 1:
                            DbManager.DeleteData(tableNames[0], "PersonId", personId, "Id", id, "Eğitim bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[0], new string[] { "Id AS 'Numara'", "DiplomaName AS 'Eğitim'", "SchoolName AS 'Okul'", "SectionName AS 'Bölüm'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 2:
                            DbManager.DeleteData(tableNames[1], "PersonId", personId, "Id", id, "İş deneyimi bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[1], new string[] { "Id AS 'Numara'", "Role AS 'Pozisyon'", "CompanyName AS 'Şirket'", "CompanyLocation AS 'Konum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 3:
                            DbManager.DeleteData(tableNames[2], "PersonId", personId, "Id", id, "Kurs - Seminer bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[2], new string[] { "Id AS 'Numara'", "CourseName AS 'Kurs - Seminer'", "CourseCompany AS 'Kurum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 4:
                            DbManager.DeleteData(tableNames[3], "PersonId", personId, "Id", id, "Sertifika bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[3], new string[] { "Id AS 'Numara'", "CompanyName AS 'Sertifika Kurumu - Adı'", "CertificateNo AS 'Sertifika Numarası'", "Duration AS 'Süre'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 5:
                            DbManager.DeleteData(tableNames[4], "PersonId", personId, "Id", id, "Yetkinlik bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[4], new string[] { "Id AS 'Numara'", "Competence AS 'Yetkinlik'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 6:
                            DbManager.DeleteData(tableNames[5], "PersonId", personId, "Id", id, "Yabancı dil bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[5], new string[] { "Id AS 'Numara'", "Language AS 'Yabancı Dil'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 7:
                            DbManager.DeleteData(tableNames[6], "PersonId", personId, "Id", id, "Proje bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[6], new string[] { "Id AS 'Numara'", "Name AS 'Proje'", "Date AS 'Tarih'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 8:
                            DbManager.DeleteData(tableNames[7], "PersonId", personId, "Id", id, "Referans bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[7], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "Role AS 'Pozisyon'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        case 9:
                            DbManager.DeleteData(tableNames[8], "PersonId", personId, "Id", id, "Hobi bilgisi silindi.");
                            DbManager.GetData(dataGridViewCvData, tableNames[8], new string[] { "Id AS 'Numara'", "Hobby AS 'Hobi'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
                            break;
                        default:
                            for (int i = 0; i < tableNames.Length; i++)
                            {
                                DbManager.DeleteData(tableNames[i], $"{(tableNames[i] == "[Personal Informations]" ? "Id" : "PersonId")}", personId);
                            }

                            foreach (FileInfo file in new DirectoryInfo(imagesFolder).GetFiles(personImagePath).Where(file => file.Exists))
                            {
                                file.Delete();
                            }

                            DbManager.GetData(dataGridViewCvData, tableNames[^1], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "DateOfBirth AS 'Doğum Tarihi'", "PlaceOfBirth AS 'Doğum Yeri'", "Gender AS 'Cinsiyet'", "Nation AS 'Uyruk'", "MaritalStatus AS 'Medeni Durum'", "DriverLicense AS 'Sürücü Ehliyeti'", "Address AS 'Adres'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'", "Picture AS 'Resim'" }, null, null, null, null, null, false, false);
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonShowDetailsClick(object sender, EventArgs e)
        {
            if (dataGridViewCvData.SelectedCells.Count != 0)
            {
                switch (comboBoxDetails.SelectedIndex)
                {
                    case 1: DbManager.GetData(dataGridViewCvData, tableNames[0], new string[] { "Id AS 'Numara'", "DiplomaName AS 'Eğitim'", "SchoolName AS 'Okul'", "SectionName AS 'Bölüm'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 2: DbManager.GetData(dataGridViewCvData, tableNames[1], new string[] { "Id AS 'Numara'", "Role AS 'Pozisyon'", "CompanyName AS 'Şirket'", "CompanyLocation AS 'Konum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 3: DbManager.GetData(dataGridViewCvData, tableNames[2], new string[] { "Id AS 'Numara'", "CourseName AS 'Kurs - Seminer'", "CourseCompany AS 'Kurum'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 4: DbManager.GetData(dataGridViewCvData, tableNames[3], new string[] { "Id AS 'Numara'", "CompanyName AS 'Sertifika Kurumu - Adı'", "CertificateNo AS 'Sertifika Numarası'", "Duration AS 'Süre'", "StartDate AS 'Başlangıç'", "EndDate AS 'Bitiş'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 5: DbManager.GetData(dataGridViewCvData, tableNames[4], new string[] { "Id AS 'Numara'", "Competence AS 'Yetkinlik'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 6: DbManager.GetData(dataGridViewCvData, tableNames[5], new string[] { "Id AS 'Numara'", "Language AS 'Yabancı Dil'", "Rate AS 'Seviye'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 7: DbManager.GetData(dataGridViewCvData, tableNames[6], new string[] { "Id AS 'Numara'", "Name AS 'Proje'", "Date AS 'Tarih'", "Information AS 'Açıklama'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 8: DbManager.GetData(dataGridViewCvData, tableNames[7], new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Surname AS 'Soyad'", "Role AS 'Pozisyon'", "PhoneNumber AS 'Telefon Numarası'", "Email AS 'E-Posta'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    case 9: DbManager.GetData(dataGridViewCvData, tableNames[8], new string[] { "Id AS 'Numara'", "Hobby AS 'Hobi'" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, true); break;
                    default: MessageBox.Show("Bir detay türü seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); break;
                }

                if (comboBoxDetails.SelectedIndex >= 1)
                {
                    Utilities.ToggleControlsEnabledState(operationControls, false);
                    Utilities.ToggleControlsEnabledState(leftPanelControls, false);
                }
            }
            else
            {
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DataGridViewCvDataCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCvData.SelectedCells.Count != 0)
            {
                switch (comboBoxDetails.SelectedIndex)
                {
                    case >= 1 and <= 9:
                        id = Convert.ToInt32(dataGridViewCvData.CurrentRow.Cells["Numara"].Value);
                        break;
                    default:
                        personId = Convert.ToInt32(dataGridViewCvData.CurrentRow.Cells["Numara"].Value);
                        personImagePath = dataGridViewCvData.CurrentRow.Cells["Resim"].Value.ToString();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DataGridViewCvDataCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Utilities.ClearValues(listViews);
            DbManager.GetSelectedRowData(groupBoxCvDetails, new Control[] { groupBoxCvDetails, groupBoxCvDetails, labelDateOfBirth_, labelPlaceOfBirth_, labelGender_, labelNation_, labelMaritalStatus_, labelDriverLicense_, labelAddress_, labelEmail_, labelPhoneNumber_, pictureCv }, tableNames[^1], "Id", personId, "Name", "Surname", "DateOfBirth", "PlaceOfBirth", "Gender", "Nation", "MaritalStatus", "DriverLicense", "Address", "Email", "PhoneNumber", "Picture");

            DbManager.GetData(listViews[0], tableNames[0], new string[] { "DiplomaName", "SchoolName", "SectionName", "StartDate", "EndDate", "Information" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[1], tableNames[1], new string[] { "Role", "CompanyName", "CompanyLocation", "StartDate", "EndDate", "Information" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[2], tableNames[2], new string[] { "CourseName", "CourseCompany", "StartDate", "EndDate", "Information" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[3], tableNames[3], new string[] { "CompanyName", "CertificateNo", "Duration", "StartDate", "EndDate", "Information" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[4], tableNames[4], new string[] { "Competence", "Rate" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[5], tableNames[5], new string[] { "Language", "Rate" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[6], tableNames[6], new string[] { "Name", "Date", "Information" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[7], tableNames[7], new string[] { "Name", "Surname", "Role", "PhoneNumber", "Email" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);
            DbManager.GetData(listViews[8], tableNames[8], new string[] { "Hobby" }, null, null, null, new string[] { "PersonId" }, new object[] { personId }, false, false);

            string htmlMailPath = $"{textFolder}\\HtmlMail.txt";
            string oldText = "CV'niz Görüntülendi";
            string newText = $"CV'niz {companyName} firması tarafından görüntülendi.";

            Utilities.ReplaceInFile(htmlMailPath, oldText, newText);
            Utilities.SendMail(labelEmail_.Text, "Görüntülenme", File.ReadAllText(htmlMailPath));
            Utilities.ReplaceInFile(htmlMailPath, newText, oldText);
        }

        private void DataGridViewCvDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) => Utilities.ClearSelectionFromDGV(dataGridViewCvData);
        #endregion

        #region Company Events
        private void ButtonLoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(menuTextCompanyNumber.Text))
                MessageBox.Show("Firma numarası boş bırakılamaz.", "Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrWhiteSpace(menuTextCompanyPassword.Text))
                MessageBox.Show("Firma şifresi boş bırakılamaz.", "Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (DbManager.GetDataCount("Companies", string.Empty, new string[] { "Number", "Password" }, new object[] { $"'{menuTextCompanyNumber.Text}'", $"'{menuTextCompanyPassword.Text}'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal, ArithmeticOperator.Equal }) > 0)
                {
                    companyName = DbManager.GetData("Companies", new string[] { "Name" }, new string[] { "Number", "Password" }, new object[] { $"'{menuTextCompanyNumber.Text}'", $"'{menuTextCompanyPassword.Text}'" });
                    Utilities.ToggleControlsVisibleState(new Component[] { titleCv, titleCompany, titlePanel, optionCancel }, true, titleCompanyLogin, new List<object> { "Giriş yapıldı.", "Giriş", MessageBoxButtons.OK, MessageBoxIcon.Information });
                }
                else
                    MessageBox.Show("Girilen bilgiler uyuşmuyor.", "Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonSelectCompanyLogoClick(object sender, EventArgs e)
        {
            companyImage = string.Empty;

            switch (pictureDialog.ShowDialog())
            {
                case DialogResult.OK:
                    companyImage = pictureDialog.FileName;
                    pictureCompanyLogo.BackgroundImage = Image.FromFile(companyImage);
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Resim seçimini iptal ettiniz. Resim seçimi yapmalısınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void ButtonAddCompanyClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCompany.Text) || string.IsNullOrWhiteSpace(textBoxCompanyTitle.Text) || string.IsNullOrWhiteSpace(textBoxCompanyIndustry.Text) || string.IsNullOrWhiteSpace(textBoxCompanyEmail.Text) || string.IsNullOrWhiteSpace(textBoxCompanyPhoneNumber.Text) || string.IsNullOrEmpty(companyImage) || string.IsNullOrWhiteSpace(richTextBoxCompanyDescription.Text))
                MessageBox.Show("Firma bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.AddData("Companies", "Name, Title, Industry, Email, PhoneNumber, Logo, Information", $"'{textBoxCompany.Text}', '{textBoxCompanyTitle.Text}', '{textBoxCompanyIndustry.Text}', '{textBoxCompanyEmail.Text}', '{textBoxCompanyPhoneNumber.Text}', '{Path.GetFileName(companyImage)}', '{richTextBoxCompanyDescription.Text}'", $"{textBoxCompany.Text} adlı {textBoxCompanyIndustry.Text} firması eklendi.");
                Utilities.SaveImage(companyImage, imagesFolder);
                Utilities.ClearValues(new object[] { textBoxCompany, textBoxCompanyTitle, textBoxCompanyIndustry, textBoxCompanyEmail, textBoxCompanyPhoneNumber, companyImage, richTextBoxCompanyDescription });
                DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, null, null, false, false);
            }
        }

        private void ButtonUpdateCompanyLogoClick(object sender, EventArgs e)
        {
            companyImage = string.Empty;

            switch (pictureDialog.ShowDialog())
            {
                case DialogResult.OK:
                    companyImage = pictureDialog.FileName;
                    pictureUpdateCompanyLogo.BackgroundImage = Image.FromFile(companyImage);
                    companyPictureState = true;
                    break;
            }
        }

        private void ButtonUpdateCompanyClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUpdateCompany.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCompanyTitle.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCompanyIndustry.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCompanyEmail.Text) || string.IsNullOrWhiteSpace(textBoxUpdateCompanyPhone.Text) || string.IsNullOrWhiteSpace(richTextBoxUpdateCompanyDescription.Text))
                MessageBox.Show("Kişisel bilgiler ve iletişim bilgileri boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DbManager.UpdateData("Companies", new string[] { "Name", "Title", "Industry", "Email", "PhoneNumber", "Logo", "Information" }, new object[] { $"'{textBoxUpdateCompany.Text}'", $"'{textBoxUpdateCompanyTitle.Text}'", $"'{textBoxUpdateCompanyIndustry.Text}'", $"'{textBoxUpdateCompanyEmail.Text}'", $"'{textBoxUpdateCompanyPhone.Text}'", $"'{(companyImage == string.Empty ? Path.GetFileName(companyImagePath) : Path.GetFileName(companyImage))}'", $"'{richTextBoxUpdateCompanyDescription.Text}'" }, new string[] { "Id" }, new object[] { companyId }, "Firma bilgisi güncellendi.", groupBoxCompanyOperations);
                DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, null, null, false, false);

                if (companyPictureState)
                {
                    Utilities.SaveImage(companyImage, imagesFolder, false);
                    companyPictureState = false;
                }
            }
        }

        private void ButtonGetAllCompaniesClick(object sender, EventArgs e)
        {
            DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, null, null, false, true);
            Utilities.ClearSelectionFromDGV(dataGridViewCompanyData);
        }

        private void ButtonGetCompaniesByFiltersClick(object sender, EventArgs e)
        {
            DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, new string[] { "Name", "Title", "Industry", "Email", "PhoneNumber" }, new object[] { $"'{(string.IsNullOrWhiteSpace(textBoxSearchCompany.Text) ? "" : $"%{textBoxSearchCompany.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxSearchCompanyTitle.Text) ? "" : $"%{textBoxSearchCompanyTitle.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxSearchCompanyIndustry.Text) ? "" : $"%{textBoxSearchCompanyIndustry.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxSearchCompanyEmail.Text) ? "" : $"%{textBoxSearchCompanyEmail.Text}%")}'", $"'{(string.IsNullOrWhiteSpace(textBoxSearchCompanyPhone.Text) ? "" : $"%{textBoxSearchCompanyPhone.Text}%")}'" }, false, true);
            Utilities.SetFilterColors(dataGridViewCompanyData, companyControls);
        }

        private void ButtonClearCompanyFiltersClick(object sender, EventArgs e) => Utilities.ClearValues(companyControls);

        private void ButtonClearCompanyTableClick(object sender, EventArgs e)
        {
            switch (dataGridViewCompanyData.Rows.Count)
            {
                case > 0:
                    MessageBox.Show("Tablo temizlendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Utilities.ClearValues(new Control[] { dataGridViewCompanyData });
                    break;
                default:
                    MessageBox.Show("Tabloda temizlenecek veri bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void ButtonUpdateSelectedCompanyClick(object sender, EventArgs e)
        {
            if (dataGridViewCompanyData.SelectedCells.Count != 0)
                DbManager.GetSelectedRowData(groupBoxUpdateCompany, new Control[] { textBoxUpdateCompany, textBoxUpdateCompanyTitle, textBoxUpdateCompanyIndustry, textBoxUpdateCompanyEmail, textBoxUpdateCompanyPhone, pictureUpdateCompanyLogo, richTextBoxUpdateCompanyDescription }, "Companies", "Id", companyId, "Name", "Title", "Industry", "Email", "PhoneNumber", "Logo", "Information");
            else
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ButtonDeleteCompanyClick(object sender, EventArgs e)
        {
            if (dataGridViewCompanyData.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Seçilen kaydı silmek istediğinize emin misiniz?", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DbManager.DeleteData("Companies", "Id", companyId, message: "Firma bilgisi silindi.");

                    foreach (FileInfo file in new DirectoryInfo(imagesFolder).GetFiles(companyImagePath).Where(file => file.Exists))
                    {
                        file.Delete();
                    }

                    DbManager.GetData(dataGridViewCompanyData, "Companies", new string[] { "Id AS 'Numara'", "Name AS 'Ad'", "Title AS 'Başlık'", "Industry AS 'Sektör'", "Email AS 'E-Posta'", "PhoneNumber AS 'Telefon Numarası'", "Information AS 'Açıklama'", "Logo" }, null, null, null, null, null, false, false);
                }
            }
            else
            {
                MessageBox.Show("Bir kayıt seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DataGridViewCompanyDataCellClick(object sender, DataGridViewCellEventArgs e)
        {
            companyId = Convert.ToInt32(dataGridViewCompanyData.CurrentRow.Cells["Numara"].Value);
            companyImagePath = dataGridViewCompanyData.CurrentRow.Cells["Logo"].Value.ToString();
        }

        private void DataGridViewCompanyDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) => Utilities.ClearSelectionFromDGV(dataGridViewCvData);
        #endregion
        
        #region Panel ComboBox Events
        private void ComboBoxCompetencesSelectedIndexChanged(object sender, EventArgs e) => labelCompetenceCount_.Text = DbManager.GetDataCount(tableNames[4], string.Empty, new string[] { "Competence" }, new object[] { $"'{comboBoxCompetencies.Text}'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal }).ToString();

        private void ComboBoxForeignLanguagesSelectedIndexChanged(object sender, EventArgs e) => labelForeignLanguageCount_.Text = DbManager.GetDataCount(tableNames[5], string.Empty, new string[] { "Language" }, new object[] { $"'{comboBoxForeignLanguages.Text}'" }, new ArithmeticOperator[] { ArithmeticOperator.Equal }).ToString();
        #endregion

        #region Extra Events
        private void MenuTextCompanyNumberClick(object sender, EventArgs e)
        {
            if (menuTextCompanyNumber.Text == "Numara")
                menuTextCompanyNumber.Clear();
        }

        private void MenuTextCompanyPasswordClick(object sender, EventArgs e)
        {
            if (menuTextCompanyPassword.Text == "Şifre")
                menuTextCompanyPassword.Clear();
        }
        #endregion
    }
}