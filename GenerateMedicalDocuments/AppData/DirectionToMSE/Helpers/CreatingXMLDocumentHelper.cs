using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers
{
    /// <summary>
    /// Помощник создания XML документа "Направление на медико-социальную экспертизу".
    /// </summary>
    public static class CreatingXMLDocumentHelper
    {
        #region Namespaces

        private static XNamespace xmlnsNamespace = "urn:hl7-org:v3";
        private static XNamespace xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        private static XNamespace fiasNamespace = "urn:hl7-ru:fias";
        private static XNamespace identityNamespace = "urn:hl7-ru:identity";
        private static XNamespace addressNamespace = "urn:hl7-ru:address";
        private static XNamespace medServiceNamespace = "urn:hl7-ru:medService";

        #endregion

        /// <summary>
        /// Получить XML документ "Направление на медико-социальную экспертизу" по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>XML документ "Направление на медико-социальную экспертизу" по модели документа.</returns>
        public static XDocument GetXMLDocument(DirectionToMSEDocumentModel documentModel)
        {
            XDocument document = new XDocument();

            /// формируем шапку документа.
            document.Add(new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"OrdMedExp.xsl\""));
            document.Add(GenerateClinicalDocumentElement(documentModel));

            return document;
        }

        #region Generate elements

        /// <summary>
        /// Сгенирировать элемент "ClinicalDocument".
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>Элемент "ClinicalDocument".</returns>
        private static XElement GenerateClinicalDocumentElement(DirectionToMSEDocumentModel documentModel)
        {
            XElement clinicalDocumentElement = new XElement(xmlnsNamespace + "ClinicalDocument",
                GetClinicalDocumentElementAttributes());

            clinicalDocumentElement.Add(GenerateInitialElements(
                documentModel.ID.Root,
                documentModel.ID.Extension,
                documentModel.CreateDate,
                documentModel.SetID.Root,
                documentModel.SetID.Extension,
                documentModel.VersionNumber));

            return clinicalDocumentElement;
        }

        /// <summary>
        /// Создать элементы с начальными данными документа.
        /// </summary>
        /// <param name="id_root">Уникальный идентификатор документа (атрибут "root").</param>
        /// <param name="id_extension">Уникальный идентификатор документа (атрибут "extension").</param>
        /// <param name="createDate">Дата создания документа.</param>
        /// <param name="setId_root">Уникальный идентификатор набора версий документа (атрибут "root").</param>
        /// <param name="setId_extension">Уникальный идентификатор набора версий документа (атрибут "extension").</param>
        /// <param name="versionNumber">Номер версии документа.</param>
        /// <returns>Список элементов с начальными данными документа.</returns>
        private static List<XElement> GenerateInitialElements(
            string id_root, 
            string id_extension,
            DateTime createDate,
            string setId_root,
            string setId_extension,
            int versionNumber)
        {
            List<XElement> staticInitialElements = new List<XElement>();

            XElement realmCodeElement = new XElement(xmlnsNamespace + "realmCode",
                new XAttribute("code", "RU"));
            staticInitialElements.Add(realmCodeElement);

            XElement typeIdElement = new XElement(xmlnsNamespace + "typeId",
                new XAttribute("root", "2.16.840.1.113883.1.3"),
                new XAttribute("extension", "POCD_MT000040"));
            staticInitialElements.Add(typeIdElement);

            XElement templateIdElement = new XElement(xmlnsNamespace + "templateId",
                new XAttribute("root", "1.2.643.5.1.13.13.14.34.9.5"));
            staticInitialElements.Add(templateIdElement);

            XElement idElement = new XElement(xmlnsNamespace + "id",
                // TODO: судя по доке - root значение должно генерироваться как "OID_медицинской_организации.100.НомерМИС.НомерЭкзМИС.51".
                // a extension - рандом 7-ми значный.
                GetIdAttributes(id_root, id_extension));
            staticInitialElements.Add(idElement);

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetCodeElementAttributes());
            staticInitialElements.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "Направление на медико-социальную экспертизу");
            staticInitialElements.Add(titleElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime", 
                new XAttribute("value", createDate.ToString("yyyyMMddHHmm+0300")));
            staticInitialElements.Add(effectiveTimeElement);

            XElement confidentialityCodeElement = new XElement(xmlnsNamespace + "confidentialityCode",
                GetConfidentialityCodeElementAttributes());
            staticInitialElements.Add(confidentialityCodeElement);

            XElement languageCodeElement = new XElement(xmlnsNamespace + "languageCode",
                new XAttribute("code", "ru-RU"));
            staticInitialElements.Add(languageCodeElement);

            XElement setIdElement = new XElement(xmlnsNamespace + "setId",
                GetIdAttributes(setId_root, setId_extension));
            staticInitialElements.Add(setIdElement);

            XElement versionNumberDocument = new XElement(xmlnsNamespace + "versionNumber",
                new XAttribute("value", versionNumber));

            return staticInitialElements;
        }

        #endregion

        #region Generate element attributes

        /// <summary>
        /// Получить список атрибутов элемента "ClinicalDocument".
        /// </summary>
        /// <returns>Список автрибутов элемента "ClinicalDocument".</returns>
        private static List<XAttribute> GetClinicalDocumentElementAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("xmlns", "urn:hl7-org:v3"),
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "fias", "urn:hl7-ru:fias"),
                new XAttribute(XNamespace.Xmlns + "identity", "urn:hl7-ru:identity"),
                new XAttribute(XNamespace.Xmlns + "address", "urn:hl7-ru:address"),
                new XAttribute(XNamespace.Xmlns + "medService", "urn:hl7-ru:medService")
            };
        }

        /// <summary>
        /// Получить список атрибутов элемента "code".
        /// </summary>
        /// <returns>Список атрибутов элемента "code".</returns>
        private static List<XAttribute> GetCodeElementAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("code", "34"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1522"),
                new XAttribute("codeSystemVersion", "4.45"),
                new XAttribute("codeSystemName", "Виды медицинской документации"),
                new XAttribute("displayName", "Направление на медико-социальную экспертизу")
            };
        }

        /// <summary>
        /// Получить список атрибутов элемента "confidentialityCode".
        /// </summary>
        /// <returns>Список атрибутов элемента "confidentialityCode".</returns>
        private static List<XAttribute> GetConfidentialityCodeElementAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("code", "N"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.285"),
                new XAttribute("codeSystemVersion", "1.2"),
                new XAttribute("codeSystemName", "Уровень конфиденциальности медицинского документа"),
                new XAttribute("displayName", "Обычный")
            };
        }

        /// <summary>
        /// Получить список атрибутов элемента с уникальным идентификатором.
        /// Атрибуты:
        /// - root
        /// - extension
        /// </summary>
        /// <param name="rootValue">Значение атрибута "root".</param>
        /// <param name="extensionValue">Значение атрибута "extension".</param>
        /// <returns>Список атрибутов элемента с уникальным идентификатором.</returns>
        private static List<XAttribute> GetIdAttributes(string rootValue, string extensionValue)
        {
            return new List<XAttribute>()
            {
                new XAttribute("root", rootValue),
                new XAttribute("extension", extensionValue)
            };
        }

        #endregion
    }
}
