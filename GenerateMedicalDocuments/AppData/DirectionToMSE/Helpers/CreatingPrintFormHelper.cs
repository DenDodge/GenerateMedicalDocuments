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
                // start 11 points.
                "patientCurrentAddressNation", // 11.1 point.
                "patientCurrentAddressPostalCode", // 11.2 point.
                "patientCurrentAddressSubject", // 11.3 point.
                "patientCurrentAddressDistrict", // 11.4 point.
                "patientCurrentAddressLocalityName", // 11.5 point.
                "patientCurrentAddressStreet", // 11.6 point.
                "patientCurrentAddressHouse", // 11.7 point.
                "patientCurrentAddressApartment", // 11.8 point.
                // end 11 points.
                "isBOMZ", // 12 point.
                // start 13 table
                "patientInMedicalOrganization", "addressMedicalOrganization", "OGRNMedicalOrganization", // 13.1 cells.
                "patientInSocialOrganization", "addressSocialOrganization", "OGRNSocialOrganization", // 13.2 cells.
                "patientInCorrectionOrganization", "addressCorrectionOrganization", "OGRNCorrectionOrganization", // 13.3 cells.
                "patientInOtherOrganization", "addressOtherOrganization", "OGRNOtherOrganization", // 13.4 cells.
                "patientInHome", // 13.5 cell.
                // end 13 table.
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
                "otherdegreeDisabilities", // 19.8 point.
                "educationOrg20_1", // 20.1 point.
                "educationOrg20_2", // 20.2 point.
                "educationOrg20_3", // 20.3 point
                // begin 21 points.
                "workplaceMainProfession", // 21.1 point.
                "workplaceQualification", // 21.2 point.
                "workplaceExperience", // 21.3 point.
                "workplaceWorkPprogress", // 21.4 point.
                "workplaceWorkingConditions", // 21.5 point.
                "workplaceWorkplace", // 21.6 point.
                "workplaceWorkAddress", // 21.7 point.
                // end 21 points.
                "startYear", // 22 point.
                "medicalAnamnez", // 23 point.
                "lifeAnamnez", // 24 point.
                // begin table 25
                "startDateRow1", "finishDateRow1", "countDaysRow1", "diagnozRow1", // row 1.
                "startDateRow2", "finishDateRow2", "countDaysRow2", "diagnozRow2", // row 2.
                "startDateRow3", "finishDateRow3", "countDaysRow3", "diagnozRow3", // row 3.
                "startDateRow4", "finishDateRow4", "countDaysRow4", "diagnozRow4", // row 4.
                "startDateRow5", "finishDateRow5", "countDaysRow5", "diagnozRow5", // row 5.
                "startDateRow6", "finishDateRow6", "countDaysRow6", "diagnozRow6", // row 6.
                "startDateRow7", "finishDateRow7", "countDaysRow7", "diagnozRow7", // row 7.
                "startDateRow8", "finishDateRow8", "countDaysRow8", "diagnozRow8", // row 8.
                "startDateRow9", "finishDateRow9", "countDaysRow9", "diagnozRow9", // row 9.
                "startDateRow10", "finishDateRow10", "countDaysRow10", "diagnozRow10", // row 10.
                // end table 25.
                "isELN", // 25.1 point.
                "numberELN", // 25.2 point.
                "IPRANumber", // 26 point.
                "protocolNumber", // 26 point.
                "protocolDate", // 26 point.
                "isresultRestorationFunctions", // 26.1 point.
                "isFull26-1", "isPartial26-1", "isNotResult26-1", // 26.1.1 - 26.1.3 points.
                "isresultCompensationFunction", // 26.2 point.
                "isFull26-2", "isPartial26-2", "isNotResult26-2", // 26.2.2 - 26.2.3 points.
                "growth", "weight", "IMT", // 27.1 - 27.3 points.
                "bodyType", "physiologicalFunctions", "waist", "hips", // 27.4 - 27.6 points.
                "directionState", // 28 point.
                // begin 29 table.
                "dateExamination1", "codeExamination1", "nameExamination1", "resultExamination1", // 29.1 row.
                "dateExamination2", "codeExamination2", "nameExamination2", "resultExamination2", // 29.2 row.
                "dateExamination3", "codeExamination3", "nameExamination3", "resultExamination3", // 29.3 row.
                "dateExamination4", "codeExamination4", "nameExamination4", "resultExamination4", // 29.4 row.
                "dateExamination5", "codeExamination5", "nameExamination5", "resultExamination5", // 29.5 row.
                "dateExamination6", "codeExamination6", "nameExamination6", "resultExamination6", // 29.6 row.
                "dateExamination7", "codeExamination7", "nameExamination7", "resultExamination7", // 29.7 row.
                "dateExamination8", "codeExamination8", "nameExamination8", "resultExamination8", // 29.8 row.
                "dateExamination9", "codeExamination9", "nameExamination9", "resultExamination9", // 29.9 row.
                "dateExamination10", "codeExamination10", "nameExamination10", "resultExamination10", // 29.10 row.
                // end 29 table.
                // begin 30 points.
                "mainDiagnosis",
                "codeMainDiagnosis",
                "complicationMainDiagnosis",
                "otherDiagnosis",
                "codesOtherDiagnosis",
                "complicationOtherDiagnosis",
                // end 30 points.
                "clinicalPrognosis", // 31 point.
                "rehabilitationPotential", // 32 point.
                "rehabilitationPrognosis", // 33 point.
                "medications", // 34.1 point.
                "recommendedMeasuresReconstructiveSurgery", // 35 point.
                "recommendedMeasuresProstheticsAndOrthotics", // 36 point.
                "spaTreatment", // 37 point.
                "otherRecommendatons", // 38 point.
                "createDocumentDate" // 39 point.
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
            this.SetPatientAllDataParameters(documentModel?.RecordTarget?.PatientRole, documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 6 - 16 points.
            this.SetPatienLocationParameters(documentModel?.DocumentBody?.SentSection?.PatientLocationCode?.Code, documentModel?.DocumentBody?.SentSection?.PatientLocation); // 13 points.
            this.SetGuardianAllDataParameters(documentModel?.RecordTarget?.PatientRole?.Guardian); // 17 point.
            this.SetCitizenIsSentToMSEParameters(documentModel?.DocumentBody?.SentSection?.SentParagraphs); // 18 point.
            this.SetAllDisabilityParameters(documentModel?.DocumentBody?.AnamnezSection, documentModel?.DocumentBody?.EducationSection); // 19 - 20 points.
            this.SetWorkplaceSectionParameters(documentModel?.DocumentBody?.WorkplaceSection); // 21 point.
            this.SetAnamnezSectionParameters(documentModel?.DocumentBody?.AnamnezSection); // 22 - 26 points.
            this.SetVitalParametersSectionParameters(documentModel?.DocumentBody?.VitalParametersSection); // 27 points.
            this.SetDirectionStateSectionParameters(documentModel?.DocumentBody?.DirectionStateSection); // 28 point.
            this.SetDiagnosticStudiesSectionParameters(documentModel?.DocumentBody?.DiagnosticStudiesSection); // 29 points.
            this.SetDiagnosisParameters(documentModel?.DocumentBody?.DiagnosisSection); // 30 points.
            this.SetConditionAssessment(documentModel?.DocumentBody?.ConditionAssessmentSection); // 31 - 33 points.
            this.SetRecommendationsParameters(documentModel?.DocumentBody?.RecommendationsSection); // 34 - 38 points.
            this.SetCreateDateParameter(documentModel?.CreateDate);
            
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
                this.parameters["DocumentDate"] = protocolDate.Value.ToString("dd MMMM yyyy");
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
        /// Установить все данные пациента.
        /// </summary>
        /// <param name="patientModel">Модель данных пациента.</param>
        /// <param name="paragraphs">Список парграфов секции "Направление".</param>
        private void SetPatientAllDataParameters(PatientModel patientModel, List<ParagraphModel> paragraphs)
        {
            if (patientModel is not null)
            {
                this.SetPatientNameParameter(patientModel.PatientData?.Name); // 6 point.
                this.SetPatientBirthdateParameter(patientModel.PatientData?.BirthDate); // 7 point.
                this.SetPatientAgeParameter(patientModel.PatientData?.BirthDate); // 7 point.
                this.SetPatienGenderParameter(patientModel.PatientData?.Gender); // 8 point.
                this.SetPatientAddressParameter(patientModel.PermanentAddress, patientModel.ActualAddress); // 11 - 12 points.
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
        /// Установить все данные представителя.
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
        /// Установить ФИО пациента. (6 пункт).
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        private void SetPatientNameParameter(NameModel nameModel)
        {
            this.parameters["PatientFIO"] = MainHelper.GetFIO(nameModel);
        }

        /// <summary>
        /// Установить дату рождения пациента. (7 пункт).
        /// </summary>
        /// <param name="birthdate">Дата рождения.</param>
        private void SetPatientBirthdateParameter(DateTime? birthdate)
        {
            if (birthdate is null)
            {
                return;
            }
            this.parameters["PatientBirthdate"] = birthdate.Value.ToString("dd MMMM yyyy");
        }

        /// <summary>
        /// Установить возраст пациента.
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
        /// Установить пол пациента.
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
        /// Установить гражданства пациента.
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
            }
        }

        /// <summary>
        /// Установить принадлежность пациента к воинской службе.
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
            }
        }

        /// <summary>
        /// Установить место жительства пациента.
        /// </summary>
        /// <param name="permanentAddress">Модель адреса регистрации.</param>
        /// <param name="actualAddress">Модель адреса проживания.</param>
        private void SetPatientAddressParameter(AddressModel permanentAddress, AddressModel actualAddress)
        {
            if (permanentAddress is null && actualAddress is null)
            {
                this.parameters["isBOMZ"] = trueFlag;
                return;
            }

            AddressModel currentAddress;
            if (actualAddress is not null)
            {
                currentAddress = actualAddress;
            }
            else
            {
                currentAddress = permanentAddress;
            }

            if (!String.IsNullOrWhiteSpace(currentAddress.Nation))
            {
                this.parameters["patientCurrentAddressNation"] = currentAddress.Nation;
            }
            
            if (currentAddress.PostalCode is not null)
            {
                this.parameters["patientCurrentAddressPostalCode"] = currentAddress.PostalCode.ToString();
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.SubjectOfRussianFediration))
            {
                this.parameters["patientCurrentAddressSubject"] = currentAddress.SubjectOfRussianFediration;
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.District))
            {
                this.parameters["patientCurrentAddressDistrict"] = currentAddress.District;
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.LocalityName))
            {
                this.parameters["patientCurrentAddressLocalityName"] = currentAddress.LocalityName;
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.Street))
            {
                this.parameters["patientCurrentAddressStreet"] = currentAddress.Street;
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.House))
            {
                this.parameters["patientCurrentAddressHouse"] = currentAddress.House;
            }
            
            if (!String.IsNullOrWhiteSpace(currentAddress.Apartment))
            {
                this.parameters["patientCurrentAddressApartment"] = currentAddress.Apartment;
            }
        }

        /// <summary>
        /// Установить место нахождения пациента.
        /// </summary>
        /// <param name="codeLocation">Код места нахождения.</param>
        /// <param name="organizationLocation">Организация местонахождения.</param>
        private void SetPatienLocationParameters(string codeLocation, OrganizationModel organizationLocation)
        {
            if (String.IsNullOrWhiteSpace(codeLocation) || organizationLocation is null)
            {
                return;
            }

            string organizationType = "";
            if (codeLocation == "1")
            {
                this.parameters["patientInMedicalOrganization"] = trueFlag;
                organizationType = "Medical";
            }
            if (codeLocation == "2")
            {
                this.parameters["patientInSocialOrganization"] = trueFlag;
                organizationType = "Social";
            }
            if (codeLocation == "3")
            {
                this.parameters["patientInCorrectionOrganization"] = trueFlag;
                organizationType = "Correction";
            }
            if (codeLocation == "4")
            {
                this.parameters["patientInOtherOrganization"] = trueFlag;
                organizationType = "Other";
            }
            if (codeLocation == "5")
            {
                this.parameters["patientInHome"] = trueFlag;
                return;
            }

            if (!String.IsNullOrWhiteSpace(organizationLocation.Props?.OGRN))
            {
                this.parameters[$"OGRN{organizationType}Organization"] = organizationLocation.Props.OGRN;
            }

            if (!String.IsNullOrWhiteSpace(organizationLocation.Address?.StreetAddressLine))
            {
                this.parameters[$"address{organizationType}Organization"] = organizationLocation.Address.StreetAddressLine;
            }
        }
        
        /// <summary>
        /// Установить контактные данные пациента.
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
        /// Установить номер СНИЛС и номер полиса ОМС пациента.
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
        /// Установить данные документа удоставеряещего личность пациента.
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

            if (identityDocumentModel.IdentityCardType is not null
                || !String.IsNullOrWhiteSpace(identityDocumentModel.IdentityCardType?.DisplayName))
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
            
            var identityDocumentIssueDate = identityDocumentModel.IssueDate.ToString("dd.MM.yyyy г.");
            
            this.parameters["patientIdentityDocumentName"] = identityDocumentName;
            this.parameters["patientIdentityDocumentSeries"] = identityDocumentSeries;
            this.parameters["patientIdentityDocumentNumber"] = identityDocumentNumber;
            this.parameters["patientIdentityDocumentIssueOrgName"] = identityDocumentIssueOrgName;
            this.parameters["patientIdentityDocumentIssueDate"] = identityDocumentIssueDate;
        }

        #endregion

        #region SetGuardianAllDataParameters

        /// <summary>
        /// Установить ФИО представителя.
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
        /// Установить дату рождения представителя.
        /// </summary>
        /// <param name="birthdate">Дата рождения.</param>
        private void SetGuardianBirthdateParameter(DateTime? birthdate)
        {
            if (birthdate is null)
            {
                return;
            }
            this.parameters["guardianBirthdate"] = birthdate.Value.ToString("dd MMMM yyyy");
        }
        
        /// <summary>
        /// Установить номер СНИЛС представителя.
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
        /// Установить данные документа удоставеряещего личность представителя.
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

            if (identityDocumentModel.IdentityCardType is not null
                || !String.IsNullOrWhiteSpace(identityDocumentModel.IdentityCardType?.DisplayName))
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
            
            var identityDocumentIssueDate = identityDocumentModel.IssueDate.ToString("dd.MM.yyyy г.");
            
            this.parameters["guardianIdentityDocumentName"] = identityDocumentName;
            this.parameters["guardianIdentityDocumentSeries"] = identityDocumentSeries;
            this.parameters["guardianIdentityDocumentNumber"] = identityDocumentNumber;
            this.parameters["guardianIdentityDocumentIssueOrgName"] = identityDocumentIssueOrgName;
            this.parameters["guardianIdentityDocumentIssueDate"] = identityDocumentIssueDate;
        }
        
        /// <summary>
        /// Установить данные документа удоставеряещего полномочия представителя.
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

            if (authorityDocumentModel.IdentityCardType is not null
                || !String.IsNullOrWhiteSpace(authorityDocumentModel.IdentityCardType?.DisplayName))
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
            
            var authorityDocumentIssueDate = authorityDocumentModel.IssueDate.ToString("dd.MM.yyyy г.");
            
            this.parameters["guardianAuthorityDocumentName"] = authorityDocumentName;
            this.parameters["guardianAuthorityDocumentSeries"] = authorityDocumentSeries;
            this.parameters["guardianAuthorityDocumentNumber"] = authorityDocumentNumber;
            this.parameters["guardianAuthorityDocumentIssueOrgName"] = authorityDocumentIssueOrgName;
            this.parameters["guardianAuthorityDocumentIssueDate"] = authorityDocumentIssueDate;
        }

        #endregion

        /// <summary>
        /// Установить поле "Гражданин направляется на медико-социальную экспертизу".
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
            }
        }

        /// <summary>
        /// Установить все параметры секции "Анамнез". (19 - 20 пункты).
        /// </summary>
        /// <param name="anamnezSectionModel">Модель секции "Анамнез".</param>
        /// <param name="educationSectionModel">Модель секции "Образование".</param>
        private void SetAllDisabilityParameters(AnamnezSectionModel anamnezSectionModel, EducationSectionModel educationSectionModel)
        {
            if (anamnezSectionModel is null)
            {
                return;
            }
            
            this.SetDisabilityGroupParameter(anamnezSectionModel.Disability); // 19.1 point.
            this.SetDateDisabilityFinishParameter(anamnezSectionModel.Disability?.DateDisabilityFinish); // 19.2 point.
            this.SetTimeDisabilityParameter(anamnezSectionModel.Disability?.TimeDisability); // 19.3 point.
            this.SetCauseOfDisabilityParameter(anamnezSectionModel.Disability?.CauseOfDisability); // 19.4 point.
            this.SetDegreeDisabilityParameter(anamnezSectionModel.DegreeDisability?.DegreeDisabilities); // 19.5 - 19.8 points.

            if (educationSectionModel is null)
            {
                return;
            }
            
            this.SetEducationOrganizationParameters(educationSectionModel.FillingSection?.Content[0]); // 20 point.
        }

        #region SetAllDisabilityParameters

        /// <summary>
        /// Установить группу инвалидности пациента.
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
        /// Установить дату до которой установлена инвалидность.
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
        /// Установить кол-во дней, которое пациент находится на инвалидности.
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
        /// Установить причину инвалидности.
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
        /// Установить степень утраты профессиональной трудоспособности.
        /// </summary>
        /// <param name="degreeDisabilities">Степень утраты профессиональной трудоспособности.</param>
        private void SetDegreeDisabilityParameter(List<DegreeDisabilityElementModel> degreeDisabilities)
        {
            if (degreeDisabilities is null || degreeDisabilities.Count == 0)
            {
                return;
            }

            // установка на момент направления
            var firstdegreeDisabilityElement = degreeDisabilities[0];
            
            if (firstdegreeDisabilityElement.Percent is not null)
            {
                this.parameters["degreeDisabilityPercent"] = $"{firstdegreeDisabilityElement.Percent} %";
            }
            
            if (!String.IsNullOrWhiteSpace(firstdegreeDisabilityElement.Term))
            {
                this.parameters["degreeDisabilityTerm"] = firstdegreeDisabilityElement.Term;
            }
            
            if (firstdegreeDisabilityElement.DateTo is not null)
            {
                this.parameters["degreeDisabilityDateTo"] = firstdegreeDisabilityElement.DateTo.Value.ToString("dd.MM.yyyy");
            }

            string otherdegreeDisabilities = " ";
            for (int i = 1; i <= degreeDisabilities.Count - 1; i++)
            {
                var degreeDisability = degreeDisabilities[i];
                if (degreeDisability.Percent is not null)
                {
                    otherdegreeDisabilities += $"{degreeDisability.Percent.ToString()} % ";
                }

                if (degreeDisability.DateTo is not null)
                {
                    otherdegreeDisabilities += $"до {degreeDisability.DateTo.Value.ToString("dd.MM.yyyy г. ")}";
                }

                otherdegreeDisabilities += "; ";
            }

            this.parameters["otherdegreeDisabilities"] = otherdegreeDisabilities;
        }

        /// <summary>
        /// Установить сведения о получении образования.
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

        /// <summary>
        /// Установить все параметры секции "Сведения о трудовой деятельности". (21 пункт).
        /// </summary>
        /// <param name="workplaceSectionModel">Модель секции "Трудовая деятельность".</param>
        private void SetWorkplaceSectionParameters(WorkplaceSectionModel workplaceSectionModel)
        {
            if (workplaceSectionModel is null)
            {
                return;
            }

            foreach (var paragraph in workplaceSectionModel.WorkPlaceParagraphs)
            {
                if (paragraph.Caption == ValidationCaptions.WorkplaceMainProfession)
                {
                    this.parameters["workplaceMainProfession"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceQualification)
                {
                    this.parameters["workplaceQualification"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceExperience)
                {
                    this.parameters["workplaceExperience"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceWorkPprogress)
                {
                    this.parameters["workplaceWorkPprogress"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceWorkingonditions)
                {
                    this.parameters["workplaceWorkingConditions"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceWorkplace)
                {
                    this.parameters["workplaceWorkplace"] = paragraph.Content.FirstOrDefault();
                }
                
                if (paragraph.Caption == ValidationCaptions.WorkplaceWorkAddress)
                {
                    this.parameters["workplaceWorkAddress"] = paragraph.Content.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Установить параметры секции "Анамнез".
        /// </summary>
        /// <param name="anamnezSectionModel">Модель секции "Анамнез".</param>
        private void SetAnamnezSectionParameters(AnamnezSectionModel anamnezSectionModel)
        {
            if (anamnezSectionModel is null)
            {
                return;
            }

            if (anamnezSectionModel.StartYear is not null)
            {
                this.parameters["startYear"] = anamnezSectionModel.StartYear.ToString(); // 22 point.
            }

            if (!String.IsNullOrWhiteSpace(anamnezSectionModel.MedicalAnamnez))
            {
                this.parameters["medicalAnamnez"] = anamnezSectionModel.MedicalAnamnez; // 23 point.
            }

            if (!String.IsNullOrWhiteSpace(anamnezSectionModel.LifeAnamnez))
            {
                this.parameters["lifeAnamnez"] = anamnezSectionModel.LifeAnamnez; // 24 point.
            }
            
            this.SetTemporaryDisabilitysParameters(anamnezSectionModel.TemporaryDisabilitys); // 25 table.
            this.SetElectronicСertificateParameters(anamnezSectionModel.CertificateDisabilityNumber); // 25.1 - 25.2 points.
            this.SetIPRAParameters(
                anamnezSectionModel.IPRANumber,
                anamnezSectionModel.ProtocolNumber,
                anamnezSectionModel.ProtocolDate,
                anamnezSectionModel.ResultRestorationFunctions,
                anamnezSectionModel.ResultCompensationFunction); // 26 points.
        }

        #region SetAnamnezSectionParameters

        /// <summary>
        /// Установить параметры таблицы "Частота и длительность временной нетрудоспособности".
        /// </summary>
        /// <param name="temporaryDisabilitys">Список установленных временных нетрудоспособностей.</param>
        private void SetTemporaryDisabilitysParameters(List<TemporaryDisabilityModel> temporaryDisabilitys)
        {
            if (temporaryDisabilitys is null || temporaryDisabilitys.Count == 0)
            {
                return;
            }

            int i = 0;
            foreach (var temporaryDisability in temporaryDisabilitys)
            {
                i++;
                if (temporaryDisability.DateStart is not null)
                {
                    this.parameters[$"startDateRow{i}"] = temporaryDisability.DateStart.Value.ToString("dd.MM.yyyy");
                }
                
                if (temporaryDisability.DateFinish is not null)
                {
                    this.parameters[$"finishDateRow{i}"] = temporaryDisability.DateFinish.Value.ToString("dd.MM.yyyy");
                }
                
                if (!String.IsNullOrWhiteSpace(temporaryDisability.DayCount))
                {
                    this.parameters[$"countDaysRow{i}"] = temporaryDisability.DayCount;
                }
                
                if (!String.IsNullOrWhiteSpace(temporaryDisability.Diagnosis))
                {
                    this.parameters[$"diagnozRow{i}"] = temporaryDisability.Diagnosis;
                }
            }
        }

        /// <summary>
        /// Установить номер электронного листка нетрудоспособности.
        /// </summary>
        /// <param name="electronicCertificateNumber">Номера электронного листка нетрудоспособности</param>
        private void SetElectronicСertificateParameters(string electronicCertificateNumber)
        {
            if (String.IsNullOrWhiteSpace(electronicCertificateNumber))
            {
                return;
            }

            this.parameters["isELN"] = trueFlag;
            this.parameters["numberELN"] = electronicCertificateNumber;
        }

        /// <summary>
        /// Установить результат проведенных мероприятий медицинской реабилитации.
        /// </summary>
        /// <param name="IPRANumber">Номер ИПРА.</param>
        /// <param name="protocolNumber">Номер протокола.</param>
        /// <param name="protocolDate">Дата протокола.</param>
        /// <param name="resultRestorationFunctions">Результат восстановления нарушенных функций.</param>
        /// <param name="resultCompensationFunction">Результат достижения компетенций утраченных либо отсутствующих функций.</param>
        private void SetIPRAParameters(
            string IPRANumber, 
            string protocolNumber, 
            DateTime? protocolDate,
            string resultRestorationFunctions, 
            string resultCompensationFunction)
        {
            if (!String.IsNullOrWhiteSpace(IPRANumber))
            {
                this.parameters[nameof(IPRANumber)] = IPRANumber;
            }

            if (!String.IsNullOrWhiteSpace(protocolNumber))
            {
                this.parameters[nameof(protocolNumber)] = protocolNumber;
            }

            if (protocolDate is not null)
            {
                this.parameters[nameof(protocolDate)] = protocolDate.Value.ToString("dd.MM.yyyy");
            }

            if (!String.IsNullOrWhiteSpace(resultRestorationFunctions))
            {
                this.parameters[$"is{nameof(resultRestorationFunctions)}"] = trueFlag;
                 if (resultRestorationFunctions == ValidationContents.IsFull)
                 {
                     this.parameters["isFull26-1"] = trueFlag;
                 }
                
                 if (resultRestorationFunctions == ValidationContents.IsPartial)
                 {
                     this.parameters["isPartial26-1"] = trueFlag;
                 }
                
                 if (resultRestorationFunctions == ValidationContents.IsNotResult)
                 {
                     this.parameters["isNotResult26-1"] = trueFlag;
                 }
            }
            
            if (!String.IsNullOrWhiteSpace(resultCompensationFunction))
            {
                this.parameters[$"is{nameof(resultCompensationFunction)}"] = trueFlag;
                 if (resultCompensationFunction == ValidationContents.IsFull)
                 {
                     this.parameters["isFull26-2"] = trueFlag;
                 }
                
                 if (resultCompensationFunction == ValidationContents.IsPartial)
                 {
                     this.parameters["isPartial26-2"] = trueFlag;
                 }
                
                 if (resultCompensationFunction == ValidationContents.IsNotResult)
                 {
                     this.parameters["isNotResult26-2"] = trueFlag;
                 }
            }
        }

        #endregion

        /// <summary>
        /// Установить параметры секции "Ватальных параметров".
        /// </summary>
        /// <param name="vitalParametersSectionModel">Модель секции "Витальные параметры".</param>
        private void SetVitalParametersSectionParameters(VitalParametersSectionModel vitalParametersSectionModel)
        {
            if (vitalParametersSectionModel is null)
            {
                return;
            }
            
            if (!String.IsNullOrWhiteSpace(vitalParametersSectionModel.BodyType))
            {
                this.parameters["bodyType"] = vitalParametersSectionModel.BodyType;
            }
            if (vitalParametersSectionModel.VitalParameters is null || vitalParametersSectionModel.VitalParameters.Count == 0)
            {
                return;
            }

            foreach (var vitalParameter in vitalParametersSectionModel.VitalParameters)
            {
                if (vitalParameter.EntryDisplayName == ValidationCaptions.Growth)
                {
                    this.parameters["growth"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
                
                if (vitalParameter.EntryDisplayName == ValidationCaptions.Weight)
                {
                    this.parameters["weight"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
                
                if (vitalParameter.EntryDisplayName == ValidationCaptions.IMT)
                {
                    this.parameters["IMT"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
                
                if (vitalParameter.EntryDisplayName == ValidationCaptions.PhysiologicalFunctions)
                {
                    this.parameters["physiologicalFunctions"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
                
                if (vitalParameter.EntryDisplayName == ValidationCaptions.Waist)
                {
                    this.parameters["waist"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
                
                if (vitalParameter.EntryDisplayName == ValidationCaptions.Hips)
                {
                    this.parameters["hips"] = $"{vitalParameter.Value} {vitalParameter.Unit}";
                }
            }
        }

        /// <summary>
        /// Установить параметры секции "Состояние здоровья при направлении на МСЭ".
        /// </summary>
        /// <param name="directionStateSectionModel">Модель секции "Состояние при направлении".</param>
        private void SetDirectionStateSectionParameters(DirectionStateSectionModel directionStateSectionModel)
        {
            if (directionStateSectionModel is null || 
                String.IsNullOrWhiteSpace(directionStateSectionModel.StateText))
            {
                return;
            }

            this.parameters["directionState"] = directionStateSectionModel.StateText;
        }

        /// <summary>
        /// Уcтановить параметры в таблице "Сведения о медицинских обследованиях". (29 table).
        /// </summary>
        /// <param name="diagnosticStudiesSectionModel">Модель сведений о медицинских обследованиях.</param>
        private void SetDiagnosticStudiesSectionParameters(DiagnosticStudiesSectionModel diagnosticStudiesSectionModel)
        {
            if (diagnosticStudiesSectionModel is null 
                || diagnosticStudiesSectionModel.MedicalExaminations is null 
                || diagnosticStudiesSectionModel.MedicalExaminations.Count == 0)
            {
                return;
            }

            for (int i = 1; i <= diagnosticStudiesSectionModel.MedicalExaminations.Count; i++)
            {
                var medicalExamination = diagnosticStudiesSectionModel.MedicalExaminations[i-1];
                this.parameters[$"dateExamination{i}"] = medicalExamination.Date.ToString("dd.MM.yyyy");
                if (!String.IsNullOrWhiteSpace(medicalExamination.Code))
                {
                    this.parameters[$"codeExamination{i}"] = medicalExamination.Code;
                }

                if (!String.IsNullOrWhiteSpace(medicalExamination.Name))
                {
                    this.parameters[$"nameExamination{i}"] = medicalExamination.Name;
                }

                if (!String.IsNullOrWhiteSpace(medicalExamination.Result))
                {
                    this.parameters[$"resultExamination{i}"] = medicalExamination.Result;
                }
            }
        }

        /// <summary>
        /// Установить диагнозы при направлении на МСЭ. (30 points).
        /// </summary>
        /// <param name="diagnosisSectionModel">Модель диагнозов.</param>
        private void SetDiagnosisParameters(DiagnosisSectionModel diagnosisSectionModel)
        {
            if (diagnosisSectionModel is null
                || diagnosisSectionModel.Diagnosis is null
                || diagnosisSectionModel.Diagnosis.Count == 0)
            {
                return;
            }

            var mainDiagnosis = " ";
            var codeMainDiagnosis = " ";
            var complicationMainDiagnosis = " ";

            var otherDiagnosis = " ";
            var codesOtherDiagnosis = " ";
            var complicationOtherDiagnosis = " ";
            
            foreach (var diagnosis in diagnosisSectionModel.Diagnosis)
            {
                if (diagnosis.Code == "1")
                {
                    mainDiagnosis = diagnosis.Name;
                    codeMainDiagnosis = diagnosis.ID;
                }

                if (diagnosis.Code == "2")
                {
                    complicationMainDiagnosis = $"{diagnosis.Name}({diagnosis.ID})";
                }

                if (diagnosis.Code == "3")
                {
                    if (!String.IsNullOrWhiteSpace(otherDiagnosis))
                    {
                        otherDiagnosis += ", ";
                    }

                    otherDiagnosis += diagnosis.Name;
                    
                    if (!String.IsNullOrWhiteSpace(codesOtherDiagnosis))
                    {
                        codesOtherDiagnosis += ", ";
                    }

                    codesOtherDiagnosis += diagnosis.ID;
                }

                if (diagnosis.Code == "7")
                {
                    if (!String.IsNullOrWhiteSpace(complicationOtherDiagnosis))
                    {
                        complicationOtherDiagnosis += ", ";
                    }

                    complicationOtherDiagnosis += $"{diagnosis.Name}({diagnosis.ID})";
                }
            }

            this.parameters[nameof(mainDiagnosis)] = mainDiagnosis;
            this.parameters[nameof(codeMainDiagnosis)] = codeMainDiagnosis;
            this.parameters[nameof(complicationMainDiagnosis)] = complicationMainDiagnosis;

            this.parameters[nameof(otherDiagnosis)] = otherDiagnosis;
            this.parameters[nameof(codesOtherDiagnosis)] = codesOtherDiagnosis;
            this.parameters[nameof(complicationOtherDiagnosis)] = complicationOtherDiagnosis;
        }

        /// <summary>
        /// Установить прогнозы и потенциалы. (31 - 33 points).
        /// </summary>
        /// <param name="conditionAssessmentSectionModel">Модель секции объективной оценки.</param>
        private void SetConditionAssessment(ConditionAssessmentSectionModel conditionAssessmentSectionModel)
        {
            if (conditionAssessmentSectionModel is null)
            {
                return;
            }

            if (!String.IsNullOrWhiteSpace(conditionAssessmentSectionModel.ClinicalPrognosis.GrateResult))
            {
                this.parameters["clinicalPrognosis"] = conditionAssessmentSectionModel.ClinicalPrognosis.GrateResult;
            }
            
            if (!String.IsNullOrWhiteSpace(conditionAssessmentSectionModel.RehabilitationPotential.GrateResult))
            {
                this.parameters["rehabilitationPotential"] =
                    conditionAssessmentSectionModel.RehabilitationPotential.GrateResult;
            }
            
            if (!String.IsNullOrWhiteSpace(conditionAssessmentSectionModel.RehabilitationPrognosis.GrateResult))
            {
                this.parameters["rehabilitationPrognosis"] =
                    conditionAssessmentSectionModel.RehabilitationPrognosis.GrateResult;
            }
        }

        /// <summary>
        /// Установить параметры рекомендации. (34 - 38 points).
        /// </summary>
        /// <param name="recommendationsSectionModel">Модель секции "Рекомендации".</param>
        private void SetRecommendationsParameters(RecommendationsSectionModel recommendationsSectionModel)
        {
            if (recommendationsSectionModel is null)
            {
                return;
            }

            if (recommendationsSectionModel.Medications is not null
                && recommendationsSectionModel.Medications.Count != 0)
            {
                var medicationsString = " ";
                for (int i = 1; i <= recommendationsSectionModel.Medications.Count; i++)
                {
                    var medication = recommendationsSectionModel.Medications[i - 1];
                    medicationsString += $"{i}. {medication.InternationalName} (" +
                                         $"Форма:{medication.DosageForm}({medication.Dose}) " +
                                         $"Продолжительность приема:{medication.DurationAdmission} по {medication.ReceptionFrequency}" +
                                         "); ";
                }
                // TODO: сделать список и вывести.
                this.parameters["medications"] = medicationsString;
            }
            
            if (!String.IsNullOrWhiteSpace(recommendationsSectionModel.RecommendedMeasuresReconstructiveSurgery))
            {
                this.parameters["recommendedMeasuresReconstructiveSurgery"] =
                    recommendationsSectionModel.RecommendedMeasuresReconstructiveSurgery;
            }
            
            if (!String.IsNullOrWhiteSpace(recommendationsSectionModel.RecommendedMeasuresProstheticsAndOrthotics))
            {
                this.parameters["recommendedMeasuresProstheticsAndOrthotics"] =
                    recommendationsSectionModel.RecommendedMeasuresProstheticsAndOrthotics;
            }
            
            if (!String.IsNullOrWhiteSpace(recommendationsSectionModel.SpaTreatment))
            {
                this.parameters["spaTreatment"] = recommendationsSectionModel.SpaTreatment;
            }
            
            if (!String.IsNullOrWhiteSpace(recommendationsSectionModel.OtherRecommendatons))
            {
                this.parameters["otherRecommendatons"] = recommendationsSectionModel.OtherRecommendatons;
            }
        }

        /// <summary>
        /// Установить дату создания документа. (39 point).
        /// </summary>
        /// <param name="createDocumentDate">Дата создания документа.</param>
        private void SetCreateDateParameter(DateTime? createDocumentDate)
        {
            if (createDocumentDate is null)
            {
                return;
            }

            this.parameters["createDocumentDate"] = createDocumentDate.Value.ToString("dd.MM.yyyy");
        }
        
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