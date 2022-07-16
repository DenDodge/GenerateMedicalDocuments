using System;
using System.Collections.Generic;
using System.Linq;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers.MainHelpers;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers
{
    /// <summary>
    /// Помощник создания печатной формы.
    /// </summary>
    public class CreatingPrintFormHelper
    {
        #region Private fields

        /// <summary>
        /// Список параметров для шаблона документа.
        /// key - имя параметра;
        /// value - значение параметра;
        /// </summary>
        private Dictionary<string, string> parameters;

        /// <summary>
        /// Имена параметров.
        /// </summary>
        private List<string> parametersName;

        /// <summary>
        /// Флаг, для установки "Галочки" в поле.
        /// </summary>
        private readonly string trueFlag = "V";
        
        #endregion

        #region Constructors

        public CreatingPrintFormHelper()
        {
            this.InitialParametersNames();
            this.InitialParametersList();
        }

        #endregion

        #region Private members

        #region Initialing

        /// <summary>
        /// Инициализация имен параметров для шаблона документа.
        /// </summary>
        private void InitialParametersNames()
        {
            this.parametersName = new List<string>()
            {
                "OrganizationName",
                "OrganizationAddress",
                "OrganizationOGRN",
                "DocumentNumber",
                "DocumentDate",
                "isToHome",
                "isNeedPaliatMedicalHelp",
                "isPrimaryProsthetics",
                // begin table 5.
                "isEstablishingDisabilityGroup", // cell 5.1.
                "isEstablishingCategoryDisabledChild", // cell 5.2.
                "isEstablishingCauseDisability", // cell 5.3.
                "isEstablishingTimeOfOnsetOfDisability", // cell 5.4.
                "isEstablishmentOfTermOfDisability", // cell 5.5.
                "isDeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent", // cell 5.6.
                "isDeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation", // cell 5.7.
                "isDeterminingNeedForHealthReasonsInConstantOutsideCare", // cell 5.8.
                "isDevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson", // cell 5.9.
                "isDevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident", // cell 5.10.
                // end table 5.
                "PatientFIO",
                "PatientBirthdate",
                "PatientAge",
                "isMan",
                "isWoman",
                "isCitizenOfRussianFederation",
                "isForeignCitizen",
                "isStatelessPerson",
                "isDualCitizenship",
                // begin table 10.
                "isCitizenInMilitary", // cell 10.1.
                "isCitizenEnteringMilitaryRegistration", // cell 10.2.
                "isCitizenNotEegisteredMilitaryButPbligedRegisteredMilitary", // cell 10.3.
                "isCitizenNotEegisteredMilitary", // cell 10.4.
                // end table 10.
                // TODO: need write 11 point parameters.
                "isBOMZ", // 12 point
                // TODO: need write 13 point parameters (table)
                "telephonePatientContacts", // 14.1 point.
                "emailPatientContacts", // 14.2 point.
                "patientSNILSNumber", // 15 point (SNILS)
                "patientInsurancePolicyNumber", // 15 point (insurancePolicy)
                // begin 16 point.
                "patientIdentityDocumentName", // 16.1 point.
                "patientIdentityDocumentSeries", // 16.2 point.
                "patientIdentityDocumentNumber", // 16.2 point.
                "patientIdentityDocumentIssueOrgName", // 16.3 point.
                "patientIdentityDocumentIssueDate", // 16.4 point.
                // end 16 point.
            };
        }
        
        /// <summary>
        /// Инициализация списка параметров для шаблона документа.
        /// </summary>
        private void InitialParametersList()
        {
            this.parameters = new Dictionary<string, string>();
            foreach (var name in this.parametersName)
            {
                this.parameters.Add(name, " ");
            }
        }

        #endregion


        public Dictionary<string, string> GetDataToParametersList(DirectionToMSEDocumentModel documentModel)
        {
            this.SetOrganizationParameters(
                documentModel?.RepresentedCustodianOrganization?.Name,
                documentModel?.RepresentedCustodianOrganization?.Address?.StreetAddressLine,
                documentModel?.RepresentedCustodianOrganization?.Props?.OGRN);
            this.SetProtocolParameters(documentModel?.DocumentBody?.SentSection?.TargetSent?.Protocol?.ProtocolNumber,
                documentModel?.DocumentBody?.SentSection?.TargetSent?.Protocol?.ProtocolDate); // 1 point.
            this.SetIsToHomeParameter(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 2 point.
            this.SetIsNeedPaliatMedicalHelpParameter(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 3 point.
            this.SetIsPrimaryProstheticsParameter(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 4 point.
            this.SetPurposeOfReferralParameters(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 5 point.
            // TODO: в установке адреса не доделано.
            this.SetPatientAllDataParameters(documentModel?.RecordTarget?.PatientRole, documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 6 - 16 points.
            
            return this.parameters;
        }

        #region Set parameterst
        
        /// <summary>
        /// Устанавливает параметры организации.
        /// </summary>
        /// <param name="organizationName">Наименование организации.</param>
        /// <param name="organizationAddress">Адрес организации.</param>
        /// <param name="organizationOGRN">ОГРН организации.</param>
        private void SetOrganizationParameters(
            string organizationName, 
            string organizationAddress,
            string organizationOGRN)
        {
            if (!String.IsNullOrWhiteSpace(organizationName))
            {
                this.parameters["OrganizationName"] = organizationName;
            }

            if (!String.IsNullOrWhiteSpace(organizationAddress))
            {
                this.parameters["OrganizationAddress"] = organizationAddress;
            }

            if (!String.IsNullOrWhiteSpace(organizationOGRN))
            {
                this.parameters["OrganizationOGRN"] = organizationOGRN;
            }
        }

        /// <summary>
        /// Устанавливает параметры протокола врачебной комиссии. (1 пункт).
        /// </summary>
        /// <param name="protocolNumber">Номер протокола.</param>
        /// <param name="protocolDate">Дата протокола.</param>
        private void SetProtocolParameters(string protocolNumber, DateTime? protocolDate)
        {
            if (!String.IsNullOrWhiteSpace(protocolNumber))
            {
                this.parameters["DocumentNumber"] = protocolNumber;
            }

            if (protocolDate is not null)
            {
                this.parameters["DocumentDate"] = protocolDate?.ToString("dd MMMM yyyy");
            }
        }

        /// <summary>
        /// Устанавливает флаг в ячейку "медико-социальную экспертизу необходимо проводить на дому". (2 пункт).
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetIsToHomeParameter(List<ParagraphModel> paragraphs)
        {
            var isToHome = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PersonNotComeToOffice, ValidationContents.MedicalExaminationNeedAtHome);
            if (isToHome)
            {
                this.parameters["isToHome"] = trueFlag;
            }
        }

        /// <summary>
        /// Устанавливает флаг в ячейку "Гражданин нуждается в оказании паллиативной медицинской помощи". (3 пункт).
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetIsNeedPaliatMedicalHelpParameter(List<ParagraphModel> paragraphs)
        {
            var isNeedPaliatMedicalHelp = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PersonNeedsPaliativeHelp, ValidationContents.NeedPalliativeMedicalHelp);
            if (isNeedPaliatMedicalHelp)
            {
                this.parameters["isNeedPaliatMedicalHelp"] = trueFlag;
            }
        }

        /// <summary>
        /// Устанавливает флаг в ячейку "Гражданин нуждающийся в первичном протезировании". (4 пункт).
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetIsPrimaryProstheticsParameter(List<ParagraphModel> paragraphs)
        {
            var isNeedPaliatMedicalHelp = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PersonNeedsPrimaryProsthetics, ValidationContents.NeedPrimaryProsthetics);
            if (isNeedPaliatMedicalHelp)
            {
                this.parameters["isPrimaryProsthetics"] = trueFlag;
            }
        }

        /// <summary>
        /// Устанавливает флаги в таблице "Цель направления".
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetPurposeOfReferralParameters(List<ParagraphModel> paragraphs)
        {
            var isEstablishingDisabilityGroup = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingDisabilityGroup);
            var isEstablishingCategoryDisabledChild = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingCategoryDisabledChild);
            var isEstablishingCauseDisability = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingCauseDisability);
            var isEstablishingTimeOfOnsetOfDisability = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingTimeOfOnsetOfDisability);
            var isEstablishmentOfTermOfDisability = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishmentOfTermOfDisability);
            var isDeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent);
            var isDeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation);
            var isDeterminingNeedForHealthReasonsInConstantOutsideCare = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminingNeedForHealthReasonsInConstantOutsideCare);
            var isDevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson);
            var isDevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident);

            if (isEstablishingDisabilityGroup)
            {
                this.parameters["isEstablishingDisabilityGroup"] = trueFlag;
            }
            if (isEstablishingCategoryDisabledChild)
            {
                this.parameters["isEstablishingCategoryDisabledChild"] = trueFlag;
            }
            if (isEstablishingCauseDisability)
            {
                this.parameters["isEstablishingCauseDisability"] = trueFlag;
            }
            if (isEstablishingTimeOfOnsetOfDisability)
            {
                this.parameters["isEstablishingTimeOfOnsetOfDisability"] = trueFlag;
            }
            if (isEstablishmentOfTermOfDisability)
            {
                this.parameters["isEstablishmentOfTermOfDisability"] = trueFlag;
            }
            if (isDeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent)
            {
                this.parameters["isDeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent"] = trueFlag;
            }
            if (isDeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation)
            {
                this.parameters["isDeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation"] = trueFlag;
            }
            if (isDeterminingNeedForHealthReasonsInConstantOutsideCare)
            {
                this.parameters["isDeterminingNeedForHealthReasonsInConstantOutsideCare"] = trueFlag;
            }
            if (isDevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson)
            {
                this.parameters["isDevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson"] = trueFlag;
            }
            if (isDevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident)
            {
                this.parameters["isDevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident"] = trueFlag;
            }
        }

        /// <summary>
        /// Устанавливает все данные пациента.
        /// </summary>
        /// <param name="patientModel">Модель данных пациента.</param>
        private void SetPatientAllDataParameters(PatientModel patientModel, List<ParagraphModel> paragraphs)
        {
            if (patientModel is not null)
            {
                this.SetPatientNameParameter(patientModel.PatientData?.Name); // 6 point.
                this.SetPatientBirthdateParameter(patientModel.PatientData?.BirthDate); // 7 point.
                this.SetPatientAgeParameter(patientModel.PatientData?.BirthDate); // 7 point.
                this.SetPatienGenderParameter(patientModel.PatientData?.Gender); // 8 point.
                // TODO: сделано только "Лицо без определенного места жительства. В остальном нужно парсить 1 строку - пока не понял как.
                this.SetPatientPermanentAddressParameter(patientModel.PermanentAddress); // 11 - 13 points.
                this.SetPatientContactDataParameters(patientModel.ContactPhoneNumber,
                    patientModel.Contacts); // 14 point.
                this.SetSNILSAndOMSParameters(patientModel.SNILS, patientModel.InsurancePolicy); // 15 point.
                this.SetIdentityDocumentParameters(patientModel.IdentityDocument); // 16 point.
            }

            if (paragraphs is not null && paragraphs.Count != 0)
            {
                this.SetCitizenshipParameter(paragraphs); // 9 point.
                this.SetAttitudeTowardsMilitaryServiceParameter(paragraphs); // 10 point.
            }
        }

        #region SetPatientAllDataParameters

        /// <summary>
        /// Устаналивает ФИО пациента. (6 пункт).
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        private void SetPatientNameParameter(NameModel nameModel)
        {
            this.parameters["PatientFIO"] = this.GetFIO(nameModel);
        }

        /// <summary>
        /// Устанавливает дату рождения пациента. (7 пункт).
        /// </summary>
        /// <param name="birthdate">Дата рождения.</param>
        private void SetPatientBirthdateParameter(DateTime? birthdate)
        {
            if (birthdate is null)
            {
                return;
            }
            this.parameters["PatientBirthdate"] = birthdate?.ToString("dd MMMM yyyy");
        }

        /// <summary>
        /// Устанавливает возраст пациента.
        /// </summary>
        /// <param name="birthdate">Дата рождения.</param>
        private void SetPatientAgeParameter(DateTime? birthdate)
        {
            if (birthdate is null)
            {
                return;
            }

            this.parameters["PatientAge"] = MainHelper.GetPatientAge((DateTime)birthdate);
        }

        /// <summary>
        /// Устанавливает пол пациента.
        /// </summary>
        /// <param name="genderModel">Модель пола пациента.</param>
        private void SetPatienGenderParameter(TypeModel genderModel)
        {
            if (genderModel is null
                || String.IsNullOrWhiteSpace(genderModel.Code))
            {
                return;
            }
            
            if (genderModel.Code == "1")
            {
                this.parameters["isMan"] = "V";
            }
            if (genderModel.Code == "2")
            {
                this.parameters["isWoman"] = "V";
            }
        }

        /// <summary>
        /// Устанавливает гражданства пациента.
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetCitizenshipParameter(List<ParagraphModel> paragraphs)
        {
            var isCitizenOfRussianFederation = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.CitizenOfRussianFederation);
            // TODO: Двойного гражданства в печатной форме нет.
            var isDualCitizenship = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.DualCitizenship);
            var isForeignCitizen = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.ForeignCitizen);
            var isStatelessPerson = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.StatelessPerson);
            if (isCitizenOfRussianFederation)
            {
                this.parameters["isCitizenOfRussianFederation"] = trueFlag;
                return;
            }
            if (isDualCitizenship)
            {
                this.parameters["isDualCitizenship"] = trueFlag;
                return;
            }
            if (isForeignCitizen)
            {
                this.parameters["isForeignCitizen"] = trueFlag;
                return;
            }
            if (isStatelessPerson)
            {
                this.parameters["isStatelessPerson"] = trueFlag;
                return;
            }
        }

        /// <summary>
        /// Устанавливает принадлежность пациента к воинской службе.
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetAttitudeTowardsMilitaryServiceParameter(List<ParagraphModel> paragraphs)
        {
            var isCitizenInMilitary = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenInMilitary);
            var isCitizenEnteringMilitaryRegistration = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenEnteringMilitaryRegistration);
            var isCitizenNotEegisteredMilitaryButPbligedRegisteredMilitary = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenNotEegisteredMilitaryButPbligedRegisteredMilitary);
            var isCitizenNotEegisteredMilitary = this.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenNotEegisteredMilitary);
            if (isCitizenInMilitary)
            {
                this.parameters["isCitizenInMilitary"] = trueFlag;
                return;
            }
            if (isCitizenEnteringMilitaryRegistration)
            {
                this.parameters["isCitizenEnteringMilitaryRegistration"] = trueFlag;
                return;
            }
            if (isCitizenNotEegisteredMilitaryButPbligedRegisteredMilitary)
            {
                this.parameters["isCitizenNotEegisteredMilitaryButPbligedRegisteredMilitary"] = trueFlag;
                return;
            }
            if (isCitizenNotEegisteredMilitary)
            {
                this.parameters["isCitizenNotEegisteredMilitary"] = trueFlag;
                return;
            }
        }

        /// <summary>
        /// Устанавливает место жительства пациента.
        /// </summary>
        /// <param name="permanentAddress">Модель адреса.</param>
        private void SetPatientPermanentAddressParameter(AddressModel permanentAddress)
        {
            if (permanentAddress is null)
            {
                this.parameters["isBOMZ"] = trueFlag;
                return;
            }
        }

        /// <summary>
        /// Устанавливает контактные данные пациента.
        /// </summary>
        /// <param name="contactPhoneNumber">Контактный номер телефона.</param>
        /// <param name="otherContacts">Другие контакты пациента.</param>
        private void SetPatientContactDataParameters(TelecomModel contactPhoneNumber, List<TelecomModel> otherContacts)
        {
            var allContacts = new List<TelecomModel>();
            if (contactPhoneNumber is null 
                && (otherContacts is null || otherContacts.Count == 0))
            {
                return;
            }

            if (contactPhoneNumber is not null)
            {
                allContacts.Add(contactPhoneNumber);
            }

            if (otherContacts is not null && otherContacts.Count != 0)
            {
                allContacts.AddRange(otherContacts);
            }

            var telephoneContacts = " ";
            var emailContacts = " ";
            foreach (var contact in allContacts)
            {
                if (contact.Value.Contains("@"))
                {
                    if (!String.IsNullOrWhiteSpace(emailContacts))
                    {
                        emailContacts += ", ";
                    }
                    emailContacts += contact.Value;
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(telephoneContacts))
                    {
                        telephoneContacts += ", ";
                    }
                    telephoneContacts += contact.Value;
                }
            }
            
            //string telephoneContacts = string.Join(", ", allContacts.Where(c => !c.Value.Contains("@")));
            //string emailContacts = string.Join(", ", allContacts.First(c => c.Value.Contains("@")).Value);

            this.parameters["telephonePatientContacts"] = telephoneContacts;
            this.parameters["emailPatientContacts"] = emailContacts;
        }
        
        /// <summary>
        /// Устанавливает номер СНИЛС и номер полиса ОМС пациента.
        /// </summary>
        /// <param name="SNILSnumber">Номер полиса СНИЛС.</param>
        /// <param name="insurancePolicyModel">Модель полиса ОМС.</param>
        private void SetSNILSAndOMSParameters(string SNILSnumber, InsurancePolicyModel insurancePolicyModel)
        {
            if (String.IsNullOrWhiteSpace(SNILSnumber) && insurancePolicyModel is null)
            {
                return;
            }

            this.parameters["patientSNILSNumber"] = SNILSnumber;

            var insurancePolicySeriesNumber = " ";
            if (!String.IsNullOrWhiteSpace(insurancePolicyModel.Series))
            {
                insurancePolicySeriesNumber += $"{insurancePolicyModel.Series} ";
            }
            if (!String.IsNullOrWhiteSpace(insurancePolicyModel.Number))
            {
                insurancePolicySeriesNumber += insurancePolicyModel.Number;
            }

            this.parameters["patientInsurancePolicyNumber"] = insurancePolicySeriesNumber;
        }

        /// <summary>
        /// Устанавливает данные документа удоставеряещего личность пациента.
        /// </summary>
        /// <param name="identityDocumentModel">Модель документа удоставеряющего личность.</param>
        private void SetIdentityDocumentParameters(DocumentModel identityDocumentModel)
        {
            if (identityDocumentModel is null)
            {
                return;
            }
            
            var identityDocumentName = " ";
            var identityDocumentSeries = " ";
            var identityDocumentNumber = " ";
            var identityDocumentIssueOrgName = " ";
            var identityDocumentIssueDate = " ";

            if (identityDocumentModel.IdentityCardType is not null
                || !String.IsNullOrWhiteSpace(identityDocumentModel?.IdentityCardType?.DisplayName))
            {
                identityDocumentName = identityDocumentModel.IdentityCardType.DisplayName;
            }

            if (!String.IsNullOrWhiteSpace(identityDocumentModel.Series))
            {
                identityDocumentSeries = identityDocumentModel.Series;
            }
            
            if (!String.IsNullOrWhiteSpace(identityDocumentModel.Number))
            {
                identityDocumentNumber = identityDocumentModel.Number;
            }
            
            if (!String.IsNullOrWhiteSpace(identityDocumentModel.IssueOrgName))
            {
                identityDocumentIssueOrgName = identityDocumentModel.IssueOrgName;
            }
            
            identityDocumentIssueDate = identityDocumentModel.IssueDate.ToString("dd.MM.yyyy г.");
            
            this.parameters["patientIdentityDocumentName"] = identityDocumentName;
            this.parameters["patientIdentityDocumentSeries"] = identityDocumentSeries;
            this.parameters["patientIdentityDocumentNumber"] = identityDocumentNumber;
            this.parameters["patientIdentityDocumentIssueOrgName"] = identityDocumentIssueOrgName;
            this.parameters["patientIdentityDocumentIssueDate"] = identityDocumentIssueDate;
        }

        #endregion
        
        #endregion

        #region Helpers methods
        
        /// <summary>
        /// Получить флаг по наименованию и значению параграфа.
        /// </summary>
        /// <param name="paragraphs">Параграфы секции.</param>
        /// <param name="caption">Наименование.</param>
        /// <param name="content">Значение.</param>
        /// <returns>Истина - устанавливаем флаг.</returns>
        private bool GetFlagParameterInParagraphs(
            List<ParagraphModel> paragraphs, 
            string caption, 
            string content)
        {
            var paragraph = paragraphs.FirstOrDefault(p =>
                p.Caption == caption);
            
            return paragraph is null ? false : paragraph.Content.Contains(content);
        }

        /// <summary>
        /// Получить строку "Фамилия Имя Отчество".
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        /// <returns>Строка "Фамилия Имя Отчество".</returns>
        private string GetFIO(NameModel nameModel)
        {
            if (nameModel is null)
            {
                return " ";
            }
            
            string patientFIO = " ";
            if (!String.IsNullOrWhiteSpace(nameModel.Family))
            {
                patientFIO += $"{nameModel.Family} ";
            }

            if (!String.IsNullOrWhiteSpace(nameModel.Given))
            {
                patientFIO += $"{nameModel.Given} ";
            }
            
            if (!String.IsNullOrWhiteSpace(nameModel.Patronymic))
            {
                patientFIO += nameModel.Patronymic;
            }

            return patientFIO;
        }
        
        #endregion
        
        #endregion
    }
}