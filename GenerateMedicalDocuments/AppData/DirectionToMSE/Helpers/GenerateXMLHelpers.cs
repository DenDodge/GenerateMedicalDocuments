using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers
{
    /// <summary>
    /// Статический класс помощник создания XML документа "Направление на медико-социальную экспертизу".
    /// </summary>
    public static class GenerateXMLHelpers
    {
        #region Namespaces

        private static XNamespace xmlnsNamespace = "urn:hl7-org:v3";
        private static XNamespace xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        private static XNamespace fiasNamespace = "urn:hl7-ru:fias";
        private static XNamespace identityNamespace = "urn:hl7-ru:identity";
        private static XNamespace addressNamespace = "urn:hl7-ru:address";
        private static XNamespace medServiceNamespace = "urn:hl7-ru:medService";

        #endregion

        public static void GetXMLDocument(XDocument document, DirectionToMSEDocument documentModel)
        {
            document.Add(new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"OrdMedExp.xsl\""));

            XElement clinicalDocumentElement = new XElement(xmlnsNamespace + "ClinicalDocument", GenerateXMLHelpers.GetClinicalDocumentElementAttributes());
            clinicalDocumentElement.Add(GenerateXMLHelpers.GetClinicalDocumentElementBeginningElements(documentModel.CreationDate));
            clinicalDocumentElement.Add(GenerateXMLHelpers.GetRecordTargetElement(documentModel));

            document.Add(clinicalDocumentElement);
        }

        public static void SaveDocument(XDocument document, string documentPatch)
        {
            document.Save(documentPatch);
        }

        #region Private Methods

        /// <summary>
        /// Получить начальные элементы секции "ClinicalDocument".
        /// </summary>
        /// <param name="creationDateDocument">Дата создания документа.</param>
        /// <returns>Начальные элементы "ClinicalDocument".</returns>
        private static List<XElement> GetClinicalDocumentElementBeginningElements(DateTime creationDateDocument)
        {
            List<XElement> elements = new List<XElement>();

            XElement realmCodeElement = new XElement("realmCode", new XAttribute("code", "RU"));
            elements.Add(realmCodeElement);
            
            XElement typeId = new XElement(
                "typeId",
                new XAttribute("root", "2.16.840.1.113883.1.3"),
                new XAttribute("extension", "POCD_MT000040"));
            elements.Add(typeId);
            
            XElement templateId = new XElement("templateId", new XAttribute("root", "1.2.643.5.1.13.13.14.34.9.5"));
            elements.Add(templateId);
            
            XElement id = new XElement(
                "id",
                new XAttribute("root", "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.51"),
                //TODO: не нужноли значение передавать или генерировать?
                new XAttribute("extension", "7854321"));
            elements.Add(id);
            
            XElement code = new XElement(
                "code",
                new XAttribute("code", "34"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1522"),
                new XAttribute("codeSystemVersion", "4.45"),
                new XAttribute("codeSystemName", "Виды медицинской документации"),
                new XAttribute("displayName", "Направление на медико-социальную экспертизу"));
            elements.Add(code);

            XElement title = new XElement("title", "Направление на медико-социальную экспертизу");
            elements.Add(title);

            XElement effectiveTime = new XElement("effectiveTime", new XAttribute("value", GenerateXMLHelpers.DateTimeToString(creationDateDocument, "yyyymmddHHMM+0300")));
            elements.Add(effectiveTime);

            XElement confidentialityCode = new XElement(
                "confidentialityCode",
                new XAttribute("code", "N"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.285"),
                new XAttribute("codeSystemVersion", "1.2"),
                new XAttribute("codeSystemName", "Уровень конфиденциальности медицинского документа"),
                new XAttribute("displayName", "Обычный"));
            elements.Add(confidentialityCode);

            XElement languageCode = new XElement("languageCode", new XAttribute("code", "ru-RU"));
            elements.Add(languageCode);

            XElement setId = new XElement(
                "setId",
                new XAttribute("root", "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.50"),
                //TODO: не нужноли значение передавать или генерировать?
                new XAttribute("extension", "7854321"));
            elements.Add(setId);

            XElement versionNumber = new XElement("versionNumber", new XAttribute("value", "2"));
            elements.Add(versionNumber);

            return elements;
        }

        /// <summary>
        /// Получение секции "recordTarget".
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>Секция "recordTarget", заполненная по модели документа.</returns>
        private static XElement GetRecordTargetElement(DirectionToMSEDocument documentModel)
        {
            XElement recordTargetElement = new XElement("recordTarget");

            recordTargetElement.Add(GetPatientRoleElement(documentModel.Patient));

            return recordTargetElement;
        }

        /// <summary>
        /// Получении секции "patientRole".
        /// </summary>
        /// <param name="patientModel">Модель пациента.</param>
        /// <returns>Секция "patientRole" по модели пациента.</returns>
        private static XElement GetPatientRoleElement(Patient patientModel)
        {
            XElement patientRoleElement = new XElement("patientRole");

            XElement id = new XElement("id", new XAttribute("extension", patientModel.Id));
            patientRoleElement.Add(id);

            XElement snils = new XElement(
                "id", 
                new XAttribute("root", "1.2.643.100.3"),
                new XAttribute("extension", patientModel.SNILS));
            patientRoleElement.Add(snils);

            patientRoleElement.Add(GenerateXMLHelpers.GetIdentityDocElement(patientModel.IdentityDocument));
            patientRoleElement.Add(GenerateXMLHelpers.GetInsurancePolicyElement(patientModel.InsurancePolicy));

            return patientRoleElement;
        }

        /// <summary>
        /// Получить секцияю "IdentityDocElement".
        /// </summary>
        /// <param name="identityDocumentModel">Модель документа удоставеряющего личность.</param>
        /// <returns>Секция "IdentityDocElement", заполненная по модели документа удоставеряющего личность.</returns>
        private static XElement GetIdentityDocElement(IdentityDocument identityDocumentModel)
        {   
            XElement patientRoleElement = new XElement(identityNamespace + "IdentityDoc");

            XElement identityCardType = new XElement(identityNamespace + "IdentityCardType",
                new XAttribute(xsiNamespace + "type", identityDocumentModel.IdentityCardType.Type),
                new XAttribute("code", identityDocumentModel.IdentityCardType.Code),
                new XAttribute("codeSystem", identityDocumentModel.IdentityCardType.CodeSystem),
                new XAttribute("codeSystemVersion", identityDocumentModel.IdentityCardType.CodeSystemName),
                new XAttribute("codeSystemName", identityDocumentModel.IdentityCardType.CodeSystemName),
                new XAttribute("displayName", identityDocumentModel.IdentityCardType.DisplayName));
            patientRoleElement.Add(identityCardType);

            XElement series = new XElement(identityNamespace + "Series",
                new XAttribute(xsiNamespace + "type", "ST"),
                identityDocumentModel.Series);
            patientRoleElement.Add(series);

            XElement number = new XElement(identityNamespace + "Number",
                new XAttribute(xsiNamespace + "type", "ST"),
                identityDocumentModel.Number);
            patientRoleElement.Add(number);

            XElement issueOrgName = new XElement(identityNamespace + "IssueOrgName",
                new XAttribute(xsiNamespace + "type", "ST"),
                identityDocumentModel.IssueOrgName);
            patientRoleElement.Add(issueOrgName);

            XElement issueOrgCode = new XElement(identityNamespace + "IssueOrgCode",
                new XAttribute(xsiNamespace + "type", "ST"),
                identityDocumentModel.IssueOrgCode);
            patientRoleElement.Add(issueOrgCode);

            XElement issueDate = new XElement(identityNamespace + "IssueDate",
                new XAttribute(xsiNamespace + "type", "TS"),
                new XAttribute("value", GenerateXMLHelpers.DateTimeToString(identityDocumentModel.IssueDate, "yyyymmdd")));
            patientRoleElement.Add(issueDate);

            return patientRoleElement;
        }

        /// <summary>
        /// Получить секция "InsurancePolicy".
        /// </summary>
        /// <param name="insurancePolicyModel">Модель полиса ОМС.</param>
        /// <returns>Секция "InsurancePolicy" по модели полиса ОМС.</returns>
        private static XElement GetInsurancePolicyElement(InsurancePolicy insurancePolicyModel)
        {
            XElement insurancePolicyElement = new XElement(identityNamespace + "IdentityDoc");

            XElement insurancePolicyType = new XElement(identityNamespace + "InsurancePolicyType",
                new XAttribute(xsiNamespace + "type", insurancePolicyModel.InsurancePolicyType.Type),
                new XAttribute("code", insurancePolicyModel.InsurancePolicyType.Code),
                new XAttribute("codeSystem", insurancePolicyModel.InsurancePolicyType.CodeSystem),
                new XAttribute("codeSystemVersion", insurancePolicyModel.InsurancePolicyType.CodeSystemName),
                new XAttribute("codeSystemName", insurancePolicyModel.InsurancePolicyType.CodeSystemName),
                new XAttribute("displayName", insurancePolicyModel.InsurancePolicyType.DisplayName));
            insurancePolicyElement.Add(insurancePolicyType);

            if(insurancePolicyModel.Series != null)
            {
                XElement series = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    insurancePolicyModel.Series);
                insurancePolicyElement.Add(series);
            }

            XElement number = new XElement(identityNamespace + "Number",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    insurancePolicyModel.Number);
            insurancePolicyElement.Add(number);

            return insurancePolicyElement;
        }


        /// <summary>
        /// Получить атрибуты для элемента "ClinicalDocument".
        /// </summary>
        /// <returns>Атрибуты для элемента "ClinicalDocument".</returns>
        private static List<XAttribute> GetClinicalDocumentElementAttributes()
        {
            List<XAttribute> clinicalDocumentElementAttributes = new List<XAttribute>();

            clinicalDocumentElementAttributes.Add(new XAttribute("xmlns", "urn:hl7-org:v3"));
            clinicalDocumentElementAttributes.Add(new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));
            clinicalDocumentElementAttributes.Add(new XAttribute(XNamespace.Xmlns + "fias", "urn:hl7-ru:fias"));
            clinicalDocumentElementAttributes.Add(new XAttribute(XNamespace.Xmlns + "identity", "urn:hl7-ru:identity"));
            clinicalDocumentElementAttributes.Add(new XAttribute(XNamespace.Xmlns + "address", "urn:hl7-ru:address"));
            clinicalDocumentElementAttributes.Add(new XAttribute(XNamespace.Xmlns + "medService", "urn:hl7-ru:medService"));

            return clinicalDocumentElementAttributes;
        }

        /// <summary>
        /// Преобразовать DateTime в строку форматом "yyyymmddHHMM+0300".
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns>Форматированная строка даты.</returns>
        private static string DateTimeToString(DateTime date, string templates)
        {
            return date.ToString(templates);
        }
        #endregion
    }
}
