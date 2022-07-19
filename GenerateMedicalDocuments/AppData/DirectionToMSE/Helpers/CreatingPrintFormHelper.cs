using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                "DocumentNumber", // 1 point.
                "DocumentDate", // 1 point.
                "isToHome", // 2 point.
                "isNeedPaliatMedicalHelp", // 3 point.
                "isPrimaryProsthetics", // 4 point.
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
                "PatientFIO", // 6 point.
                "PatientBirthdate", // 7 point.
                "PatientAge", // 7 point.
                "isMan", // 8.1 point.
                "isWoman", // 8.2 point.
                "isCitizenOfRussianFederation", // 9.1 point.
                "isForeignCitizen", // 9.2 point.
                "isStatelessPerson", // 9.3 point.
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
                // begin 17 point.
                "guardianFIO",
                "guardianBirthdate", // don't use.
                // begin 17.2 point
                "guardianAuthorityDocumentName", // 17.2.1 point.
                "guardianAuthorityDocumentSeries", // 17.2.2 point.
                "guardianAuthorityDocumentNumber", // 17.2.2 point.
                "guardianAuthorityDocumentIssueOrgName", // 17.2.3 point.
                "guardianAuthorityDocumentIssueDate", // 17.2.3 point.
                // end 17.2 point.
                // begin 17.3 point.
                "guardianIdentityDocumentName", // 17.3.1 point.
                "guardianIdentityDocumentSeries", // 17.3.2 point.
                "guardianIdentityDocumentNumber", // 17.3.2 point.
                "guardianIdentityDocumentIssueOrgName", // 17.3.3 point.
                "guardianIdentityDocumentIssueDate", // 17.3.4 point.
                // end 17.3 point.
                // begin 17.4 point.
                "telephoneGuardianContacts", // 17.4.1 point.
                "emailGuardianContacts", // 17.4.2 point.
                // end 17.4 point.
                "guardianSNILSNumber", // 17.5 point.
                // end 17 point.
                "isPrimarySent", // 18.1 point.
                "isRepeatedSent", // 18.2 point.
                "isFirstGroup", // 19.1.1 point.
                "isSecondGroup", // 19.1.2 point.
                "isThirdGroup", // 19.1.3 point.
                "isFourthGroup", // 19.1.4 point.
                "dateDisabilityFinish", // 19.2 point.
                "timeDisabilityOneYear", // 19.3.1 point.
                "timeDisabilityTwoYear", // 19.3.2 point.
                "timeDisabilityThreeYear", // 19.3.3 point.
                "timeDisabilityFourYear", // 19.3.4 point.
                "is19_4_1", "is19_4_2", "is19_4_3", "is19_4_4", // 19.4.1 - 19.4.4 points.
                "is19_4_5", "is19_4_6", "is19_4_7", "is19_4_8", // 19.4.5 - 19.4.8 points.
                "is19_4_9", "is19_4_10", "is19_4_11", "is19_4_12", // 19.4.9 - 19.4.12 points.
                "is19_4_13", "is19_4_14", "is19_4_15", "is19_4_16", // 19.4.13 - 19.4.16 points.
                "is19_4_17", // 19.4.17 point.
                "degreeDisabilityPercent", // 19.5 point.
                "degreeDisabilityTerm", // 19.6 point.
                "degreeDisabilityDateTo", // 19.7 point.
                "educationOrg20_1",
                "educationOrg20_2",
                "educationOrg20_3",
                
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


        /// <summary>
        /// Получить параметры по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>Список параметров по модели документа.</returns>
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
            this.SetGuardianAllDataParameters(documentModel?.RecordTarget?.PatientRole?.Guardian); // 17 point.
            this.SetCitizenIsSentToMSEParameters(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 18 point.
            this.SetAllDisabilityParameters(documentModel?.DocumentBody?.AnamnezSection, documentModel?.DocumentBody?.EducationSection); // 19 point.
            
            return this.parameters;
        }

        #region Set parameterst

        #region First parameters

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
            var isToHome = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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
            var isNeedPaliatMedicalHelp = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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
            var isNeedPaliatMedicalHelp = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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
            var isEstablishingDisabilityGroup = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingDisabilityGroup);
            var isEstablishingCategoryDisabledChild = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingCategoryDisabledChild);
            var isEstablishingCauseDisability = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingCauseDisability);
            var isEstablishingTimeOfOnsetOfDisability = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishingTimeOfOnsetOfDisability);
            var isEstablishmentOfTermOfDisability = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.EstablishmentOfTermOfDisability);
            var isDeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent);
            var isDeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation);
            var isDeterminingNeedForHealthReasonsInConstantOutsideCare = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DeterminingNeedForHealthReasonsInConstantOutsideCare);
            var isDevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.PurposeOfReferral, ValidationContents.DevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson);
            var isDevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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

        #endregion

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
                this.SetPatientContactDataParameters(patientModel.ContactPhoneNumber, patientModel.Contacts); // 14 point.
                this.SetPatientSNILSAndOMSParameters(patientModel.SNILS, patientModel.InsurancePolicy); // 15 point.
                this.SetPatientIdentityDocumentParameters(patientModel.IdentityDocument); // 16 point.
            }

            if (paragraphs is not null && paragraphs.Count != 0)
            {
                this.SetPatientCitizenshipParameter(paragraphs); // 9 point.
                this.SetPatientAttitudeTowardsMilitaryServiceParameter(paragraphs); // 10 point.
            }
        }

        /// <summary>
        /// Устанавливает все данные представителя.
        /// </summary>
        /// <param name="guardianModel">Модель данных представителя.</param>
        private void SetGuardianAllDataParameters(GuardianModel guardianModel)
        {
            if (guardianModel is not null)
            {
                this.SetGuardianNameParameter(guardianModel.Name); // 17.1 point.
                // TODO: у представителя не указывается дата рождения.
                //this.SetGuardianBirthdateParameter(guardianModel.BirthDate); // 17.1.1 point.
                this.SetGuardianAuthorityDocumentParameters(guardianModel.AuthorityDocument); // 17.2 point.
                this.SetGuardianIdentityDocumentParameters(guardianModel.IdentityDocument); // 17.3 point.
                this.SetGuardianContactDataParameters(guardianModel.ContactPhoneNumber, guardianModel.Contacts); // 17.4 point.
                this.SetGuardianSNILSParameters(guardianModel.SNILS); // 17.5 point.
            }
        }
        
        #region SetPatientAllDataParameters

        /// <summary>
        /// Устаналивает ФИО пациента. (6 пункт).
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        private void SetPatientNameParameter(NameModel nameModel)
        {
            this.parameters["PatientFIO"] = MainHelper.GetFIO(nameModel);
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
        private void SetPatientCitizenshipParameter(List<ParagraphModel> paragraphs)
        {
            var isCitizenOfRussianFederation = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.CitizenOfRussianFederation);
            // TODO: Двойного гражданства в печатной форме нет.
            var isDualCitizenship = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.DualCitizenship);
            var isForeignCitizen = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.Citizenship, ValidationContents.ForeignCitizen);
            var isStatelessPerson = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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
        private void SetPatientAttitudeTowardsMilitaryServiceParameter(List<ParagraphModel> paragraphs)
        {
            var isCitizenInMilitary = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenInMilitary);
            var isCitizenEnteringMilitaryRegistration = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenEnteringMilitaryRegistration);
            var isCitizenNotEegisteredMilitaryButPbligedRegisteredMilitary = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.AttitudeTowardsMilitaryService, ValidationContents.CitizenNotEegisteredMilitaryButPbligedRegisteredMilitary);
            var isCitizenNotEegisteredMilitary = MainHelper.GetFlagParameterInParagraphs(paragraphs,
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
        private void SetPatientSNILSAndOMSParameters(string SNILSnumber, InsurancePolicyModel insurancePolicyModel)
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
        private void SetPatientIdentityDocumentParameters(DocumentModel identityDocumentModel)
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

        #region SetGuardianAllDataParameters

        /// <summary>
        /// Устаналивает ФИО представителя.
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        private void SetGuardianNameParameter(NameModel nameModel)
        {
            this.parameters["guardianFIO"] = MainHelper.GetFIO(nameModel);
        }

        /// <summary>
        /// Устанавливает контактные данные представителя.
        /// </summary>
        /// <param name="contactPhoneNumber">Контактный номер телефона.</param>
        /// <param name="otherContacts">Другие контакты пациента.</param>
        private void SetGuardianContactDataParameters(TelecomModel contactPhoneNumber, List<TelecomModel> otherContacts)
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

            this.parameters["telephoneGuardianContacts"] = telephoneContacts;
            this.parameters["emailGuardianContacts"] = emailContacts;
        }
        
        /// <summary>
        /// Устанавливает дату рождения представителя.
        /// </summary>
        /// <param name="birthdate">Дата рождения.</param>
        private void SetGuardianBirthdateParameter(DateTime? birthdate)
        {
            if (birthdate is null)
            {
                return;
            }
            this.parameters["guardianBirthdate"] = birthdate?.ToString("dd MMMM yyyy");
        }
        
        /// <summary>
        /// Устанавливает номер СНИЛС представителя.
        /// </summary>
        /// <param name="SNILSnumber">Номер полиса СНИЛС.</param>
        private void SetGuardianSNILSParameters(string SNILSnumber)
        {
            if (String.IsNullOrWhiteSpace(SNILSnumber))
            {
                return;
            }

            this.parameters["guardianSNILSNumber"] = SNILSnumber;
        }

        /// <summary>
        /// Устанавливает данные документа удоставеряещего личность представителя.
        /// </summary>
        /// <param name="identityDocumentModel">Модель документа удоставеряющего личность.</param>
        private void SetGuardianIdentityDocumentParameters(DocumentModel identityDocumentModel)
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
            
            this.parameters["guardianIdentityDocumentName"] = identityDocumentName;
            this.parameters["guardianIdentityDocumentSeries"] = identityDocumentSeries;
            this.parameters["guardianIdentityDocumentNumber"] = identityDocumentNumber;
            this.parameters["guardianIdentityDocumentIssueOrgName"] = identityDocumentIssueOrgName;
            this.parameters["guardianIdentityDocumentIssueDate"] = identityDocumentIssueDate;
        }
        
        /// <summary>
        /// Устанавливает данные документа удоставеряещего полномочия представителя.
        /// </summary>
        /// <param name="authorityDocumentModel">Модель документа удоставеряющего полномочия.</param>
        private void SetGuardianAuthorityDocumentParameters(DocumentModel authorityDocumentModel)
        {
            if (authorityDocumentModel is null)
            {
                return;
            }
            
            var authorityDocumentName = " ";
            var authorityDocumentSeries = " ";
            var authorityDocumentNumber = " ";
            var authorityDocumentIssueOrgName = " ";
            var authorityDocumentIssueDate = " ";

            if (authorityDocumentModel.IdentityCardType is not null
                || !String.IsNullOrWhiteSpace(authorityDocumentModel?.IdentityCardType?.DisplayName))
            {
                authorityDocumentName = authorityDocumentModel.IdentityCardType.DisplayName;
            }

            if (!String.IsNullOrWhiteSpace(authorityDocumentModel.Series))
            {
                authorityDocumentSeries = authorityDocumentModel.Series;
            }
            
            if (!String.IsNullOrWhiteSpace(authorityDocumentModel.Number))
            {
                authorityDocumentNumber = authorityDocumentModel.Number;
            }
            
            if (!String.IsNullOrWhiteSpace(authorityDocumentModel.IssueOrgName))
            {
                authorityDocumentIssueOrgName = authorityDocumentModel.IssueOrgName;
            }
            
            authorityDocumentIssueDate = authorityDocumentModel.IssueDate.ToString("dd.MM.yyyy г.");
            
            this.parameters["guardianAuthorityDocumentName"] = authorityDocumentName;
            this.parameters["guardianAuthorityDocumentSeries"] = authorityDocumentSeries;
            this.parameters["guardianAuthorityDocumentNumber"] = authorityDocumentNumber;
            this.parameters["guardianAuthorityDocumentIssueOrgName"] = authorityDocumentIssueOrgName;
            this.parameters["guardianAuthorityDocumentIssueDate"] = authorityDocumentIssueDate;
        }

        #endregion

        /// <summary>
        /// Устанавливает поле "Гражданин направляется на медико-социальную экспертизу".
        /// </summary>
        /// <param name="paragraphs">Параграфы секции "Направление".</param>
        private void SetCitizenIsSentToMSEParameters(List<ParagraphModel> paragraphs)
        {
            var isPrimary = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.CitizenIsSentToMSE, ValidationContents.Primary);
            var isRepeated = MainHelper.GetFlagParameterInParagraphs(paragraphs,
                ValidationCaptions.CitizenIsSentToMSE, ValidationContents.Repeated);
            
            if (isPrimary)
            {
                this.parameters["isPrimarySent"] = trueFlag;
                return;
            }
            if (isRepeated)
            {
                this.parameters["isRepeatedSent"] = trueFlag;
                return;
            }
        }

        /// <summary>
        /// Установка всех параметров секции "Анамнез" (19 пункт).
        /// </summary>
        /// <param name="anamnezSectionModel">Модель секции "Анамнез".</param>
        private void SetAllDisabilityParameters(AnamnezSectionModel anamnezSectionModel, EducationSectionModel educationSectionModel)
        {
            if (anamnezSectionModel is null)
            {
                return;
            }
            
            this.SetDisabilityGroupParameter(anamnezSectionModel?.Disability); // 19.1 point.
            this.SetDateDisabilityFinishParameter(anamnezSectionModel?.Disability?.DateDisabilityFinish); // 19.2 point.
            this.SetTimeDisabilityParameter(anamnezSectionModel?.Disability?.TimeDisability); // 19.3 point.
            this.SetCauseOfDisabilityParameter(anamnezSectionModel?.Disability?.CauseOfDisability); // 19.4 point.
            this.SetDegreeDisabilityParameter(anamnezSectionModel?.DegreeDisability?.DegreeDisabilities); // 19.5 point.

            if (educationSectionModel is null)
            {
                return;
            }
            
            this.SetEducationOrganizationParameters(educationSectionModel?.FillingSection?.Content[0]); // 20 point.
        }

        #region SetAllDisabilityParameters

        /// <summary>
        /// Устанавливает группу инвалидности пациента.
        /// </summary>
        /// <param name="disabilityModel">Модель инвалидности пациента.</param>
        private void SetDisabilityGroupParameter(DisabilityModel disabilityModel)
        {
            if (disabilityModel is null || disabilityModel.Group is null)
            {
                return;
            }

            switch (disabilityModel.Group)
            {
                case 1:
                    this.parameters["isFirstGroup"] = trueFlag;
                    return;
                case 2:
                    this.parameters["isSecondGroup"] = trueFlag;
                    return;
                case 3:
                    this.parameters["isThirdGroup"] = trueFlag;
                    return;
                case 4:
                    this.parameters["isFourthGroup"] = trueFlag;
                    return;
            }
        }

        /// <summary>
        /// Устаавливает дату до которой установлена инвалидность.
        /// </summary>
        /// <param name="dateDisabilityFinish">Дата до которой установлена инвалидность.</param>
        private void SetDateDisabilityFinishParameter(DateTime? dateDisabilityFinish)
        {
            if (dateDisabilityFinish is null)
            {
                return;
            }

            this.parameters["dateDisabilityFinish"] = dateDisabilityFinish.Value.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Устанавливает кол-во дней, которое пациент находится на инвалидности.
        /// </summary>
        /// <param name="timeDisability">Кол-во дней, которое пациент находится на инвалидности.</param>
        private void SetTimeDisabilityParameter(string timeDisability)
        {
            if (String.IsNullOrWhiteSpace(timeDisability))
            {
                return;
            }

            if (timeDisability.ToLower().Contains("один"))
            {
                this.parameters["timeDisabilityOneYear"] = trueFlag;
                return;
            }
            if (timeDisability.ToLower().Contains("два"))
            {
                this.parameters["timeDisabilityTwoYear"] = trueFlag;
                return;
            }
            if (timeDisability.ToLower().Contains("три"))
            {
                this.parameters["timeDisabilityThreeYear"] = trueFlag;
                return;
            }

            this.parameters["timeDisabilityFourYear"] = trueFlag;
        }

        /// <summary>
        /// Устанавливает причину инвалидности.
        /// </summary>
        /// <param name="causeOfDisability">Причина инвалидности.</param>
        private void SetCauseOfDisabilityParameter(string causeOfDisability)
        {
            if (causeOfDisability.Contains(ValidationContents.is19_4_1))
            {
                this.parameters["is19_4_1"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_2))
            {
                this.parameters["is19_4_2"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_3))
            {
                this.parameters["is19_4_3"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_4))
            {
                this.parameters["is19_4_4"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_5))
            {
                this.parameters["is19_4_5"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_6))
            {
                this.parameters["is19_4_6"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_7))
            {
                this.parameters["is19_4_7"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_8))
            {
                this.parameters["is19_4_8"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_9))
            {
                this.parameters["is19_4_9"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_10))
            {
                this.parameters["is19_4_10"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_11))
            {
                this.parameters["is19_4_11"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_12))
            {
                this.parameters["is19_4_12"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_13))
            {
                this.parameters["is19_4_13"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_14))
            {
                this.parameters["is19_4_14"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_15))
            {
                this.parameters["is19_4_15"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_16))
            {
                this.parameters["is19_4_16"] = trueFlag;
            }
            if (causeOfDisability.Contains(ValidationContents.is19_4_17))
            {
                this.parameters["is19_4_17"] = trueFlag;
            }
        }

        /// <summary>
        /// Устанавливает степень утраты профессиональной трудоспособности.
        /// </summary>
        /// <param name="degreeDisabilities">Степень утраты профессиональной трудоспособности.</param>
        private void SetDegreeDisabilityParameter(List<DegreeDisabilityElementModel> degreeDisabilities)
        {
            if (degreeDisabilities is null || degreeDisabilities.Count == 0)
            {
                return;
            }

            DegreeDisabilityElementModel maxDisabilityElementModel = degreeDisabilities.First();
            foreach (var degreeDisability in degreeDisabilities)
            {
                if (maxDisabilityElementModel is not null &&
                    maxDisabilityElementModel.Percent < degreeDisability.Percent)
                {
                    maxDisabilityElementModel = degreeDisability;
                }
            }

            if (maxDisabilityElementModel is null)
            {
                return;
            }

            if (maxDisabilityElementModel.Percent is not null)
            {
                this.parameters["degreeDisabilityPercent"] = $"{maxDisabilityElementModel.Percent} %";
            }

            if (!String.IsNullOrWhiteSpace(maxDisabilityElementModel.Term))
            {
                this.parameters["degreeDisabilityTerm"] = maxDisabilityElementModel.Term;
            }

            if (maxDisabilityElementModel.DateTo is not null)
            {
                this.parameters["degreeDisabilityDateTo"] = maxDisabilityElementModel.DateTo.Value.ToString("dd.MM.yyyy");
            }
        }

        /// <summary>
        /// Устанавливает сведения о получении образования.
        /// </summary>
        /// <param name="educationString">Строка сведений об образовании.</param>
        private void SetEducationOrganizationParameters(string educationString)
        {
            var educationParameters = GetEducationData(educationString);
            this.parameters["educationOrg20_1"] = $"{educationParameters.organizationName} {educationParameters.organizationAddress}";
            this.parameters["educationOrg20_2"] = educationParameters.curse;
            this.parameters["educationOrg20_3"] = educationParameters.profession;
        }
        
        #endregion

        #endregion

        #region Helpers methods

        /// <summary>
        /// Получить параметры секции образования.
        /// Парсинг данных из строки.
        /// </summary>
        /// <param name="educationString">Строка сведений об образовании.</param>
        /// <returns>Параметры секции образования.</returns>
        private static (string organizationName, string organizationAddress, string curse, string profession) GetEducationData(string educationString)
        {
            if (String.IsNullOrWhiteSpace(educationString))
            {
                return (" ", " ", " ", " ");
            }
            
            string _educationTemplate = @"Организация: (.*?), адрес: (.*?), курс: (.*?), профессия: (.*?)\.";
            var regexp = new Regex(_educationTemplate);
            string organizationName = " ";
            string organizationAddress = " ";
            string curse = " ";
            string profession = " ";
            if (regexp.IsMatch(educationString))
            {
                var match = regexp.Match(educationString);
                organizationName = match.Groups[1].Value;
                organizationAddress = match.Groups[2].Value;
                curse = match.Groups[3].Value;
                profession = match.Groups[4].Value;
            }

            return (organizationName, organizationAddress, curse, profession);
        }

        #endregion
        
        #endregion
    }
}