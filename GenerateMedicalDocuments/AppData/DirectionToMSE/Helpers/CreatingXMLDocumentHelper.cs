﻿using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
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
            clinicalDocumentElement.Add(GenerateRecordTargetElement(documentModel.RecordTarget));

            return clinicalDocumentElement;
        }

        /// <summary>
        /// Создать элементы с начальными данными документа.
        /// Элементы:
        /// - "realmCode";
        /// - "typeId";
        /// - "templateId";
        /// - "id";
        /// - "code";
        /// - "title";
        /// - "effectiveTime";
        /// - "confidentialityCode";
        /// - "languageCode";
        /// - "setId";
        /// - "versionNumber".
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
                GetTypeElementAttributes(
                    codeValue: "34",
                    codeSystemValue: "1.2.643.5.1.13.13.11.1522",
                    codeSystemVersionValue: "4.45",
                    codeSystemNameValue: "Виды медицинской документации",
                    displayNameValue: "Направление на медико-социальную экспертизу"));
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
            staticInitialElements.Add(versionNumberDocument);

            return staticInitialElements;
        }

        /// <summary>
        /// Создать элемент "recordTarget".
        /// [1..1] ДАННЫЕ О ПАЦИЕНТЕ.
        /// </summary>
        /// <param name="recordTargetModel">Модель данных о пациенте.</param>
        /// <returns>Элемент "recordTarget".</returns>
        private static XElement GenerateRecordTargetElement(RecordTargetModel recordTargetModel)
        {
            XElement recordTargetElement = new XElement(xmlnsNamespace + "recordTarget");

            recordTargetElement.Add(GeneratePatientRoleElement(recordTargetModel.PatientRole));

            return recordTargetElement;
        }

        /// <summary>
        /// Создает элемент "patientRole".
        /// [1..1] ПАЦИЕНТ (роль).
        /// </summary>
        /// <param name="patientRoleModel">Модель данных пациента.</param>
        /// <returns>Элемент "patientRole".</returns>
        private static XElement GeneratePatientRoleElement(PatientModel patientRoleModel)
        {
            XElement patientRoleElement = new XElement(xmlnsNamespace + "patientRole");

            XElement idElement = new XElement(xmlnsNamespace + "id", GetIdAttributes(patientRoleModel.ID.Root, patientRoleModel.ID.Extension));
            patientRoleElement.Add(idElement);

            XElement snilsElement = new XElement(xmlnsNamespace + "id", GetIdAttributes("1.2.643.100.3", patientRoleModel.SNILS));
            patientRoleElement.Add(snilsElement);

            patientRoleElement.Add(GeneratePersonalDocumentElement(patientRoleModel.IdentityDocument, "IdentityDoc"));
            patientRoleElement.Add(GenerateInsurancePolicyElement(patientRoleModel.InsurancePolicy));

            if (patientRoleModel.PermanentAddress != null)
            {
                patientRoleElement.Add(GenerateAddrElement(patientRoleModel.PermanentAddress));
            }
            if (patientRoleModel.ActualAddress != null)
            {
                patientRoleElement.Add(GenerateAddrElement(patientRoleModel.ActualAddress));
            }

            patientRoleElement.Add(GenerateTelecomElement(patientRoleModel.ContactPhoneNumber));

            if (patientRoleModel.Contacts != null)
            {
                patientRoleElement.Add(GenerateTelecomElements(patientRoleModel.Contacts));
            }

            XElement patientElement =  GeneratePatientElement(patientRoleModel.PatientData);
            if (patientRoleModel.Guardian != null)
            {
                patientElement.Add(GenerateGuardianElement(patientRoleModel.Guardian));
            }
            patientRoleElement.Add(patientElement);

            patientRoleElement.Add(GenerateProviderOrganizationElement(patientRoleModel.ProviderOrganization));

            return patientRoleElement;
        }

        /// <summary>
        /// Создает элемент по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа</param>
        /// <param name="elementName">Наименование документа.</param>
        /// <returns>Элемент по модели документа.</returns>
        private static XElement GeneratePersonalDocumentElement(DocumentModel documentModel, string elementName)
        {
            XElement identityDocElement = new XElement(identityNamespace + elementName);

            var attributesValue = GetTypeValue(elementName);

            XElement identityCardTypeElement = new XElement(identityNamespace + "IdentityCardType",
                GetTypeElementAttributes(
                    typeValue: attributesValue?.typeValue,
                    codeSystemValue: attributesValue?.codeSystemValue,
                    codeSystemNameValue: attributesValue?.codeSystemNameValue,
                    codeValue: documentModel.IdentityCardType.Code,
                    displayNameValue: documentModel.IdentityCardType.DisplayName,
                    codeSystemVersionValue: documentModel.IdentityCardType.CodeSystemVersion));
            identityDocElement.Add(identityCardTypeElement);

            XElement seriesElement = new XElement(identityNamespace + "Series",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.Series);
            identityDocElement.Add(seriesElement);

            XElement numberElement = new XElement(identityNamespace + "Number",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.Number);
            identityDocElement.Add(numberElement);

            XElement issueOrgNameElement = new XElement(identityNamespace + "IssueOrgName",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.IssueOrgName);
            identityDocElement.Add(issueOrgNameElement);

            if(documentModel.IssueOrgCode != null)
            {
                XElement issueOrgCodeElement = new XElement(identityNamespace + "IssueOrgCode",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.IssueOrgCode);
                identityDocElement.Add(issueOrgCodeElement);
            }

            XElement issueDateElement = new XElement(identityNamespace + "IssueDate",
                new XAttribute(xsiNamespace + "type", "TS"),
                new XAttribute("value", documentModel.IssueDate.ToString("yyyyMMdd")));
            identityDocElement.Add(issueDateElement);

            return identityDocElement;
        }

        /// <summary>
        /// Создает элемент "identity:InsurancePolicy".
        /// [1..1] Полис ОМС.
        /// </summary>
        /// <param name="insurancePolicyModel">Модель полиса ОМС.</param>
        /// <returns>Элемент "identity:InsurancePolicy".</returns>
        private static XElement GenerateInsurancePolicyElement(InsurancePolicyModel insurancePolicyModel)
        {
            XElement insurancePolicyElement = new XElement(identityNamespace + "InsurancePolicy");

            XElement insurancePolicyTypeElement = new XElement(identityNamespace + "InsurancePolicyType",
                GetTypeElementAttributes(
                    typeValue: "CD",
                    codeSystemValue: "1.2.643.5.1.13.13.11.1035",
                    codeSystemNameValue: "Виды полиса обязательного медицинского страхования",
                    codeValue: insurancePolicyModel.InsurancePolicyType.Code,
                    codeSystemVersionValue: insurancePolicyModel.InsurancePolicyType.CodeSystemVersion,
                    displayNameValue: insurancePolicyModel.InsurancePolicyType.DisplayName));
            insurancePolicyElement.Add(insurancePolicyTypeElement);

            if (insurancePolicyModel.Series != null)
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    insurancePolicyModel.Series);
                insurancePolicyElement.Add(seriesElement);
            }

            XElement numberElement = new XElement(identityNamespace + "Number",
                new XAttribute(xsiNamespace + "type", "ST"),
                insurancePolicyModel.Number);
            insurancePolicyElement.Add(numberElement);

            return insurancePolicyElement;
        }

        /// <summary>
        /// Создает элемент "addr".
        /// </summary>
        /// <param name="addressModel">Модель адреса.</param>
        /// <returns>Элемент "addr".</returns>
        private static XElement GenerateAddrElement(AddressModel addressModel)
        {
            XElement addrElement = new XElement(xmlnsNamespace + "addr");

            if (addressModel.Type != null)
            {
                XElement typeElement = new XElement(addressNamespace + "Type",
                GetTypeElementAttributes(
                    typeValue: "CD",
                    codeSystemValue: "1.2.643.5.1.13.13.11.1504",
                    codeSystemNameValue: "Тип адреса пациента",
                    codeValue: addressModel.Type.Code,
                    codeSystemVersionValue: addressModel.Type.CodeSystemVersion,
                    displayNameValue: addressModel.Type.DisplayName));
                addrElement.Add(typeElement);
            }

            XElement streetAddressLine = new XElement(xmlnsNamespace + "streetAddressLine", addressModel.StreetAddressLine);
            addrElement.Add(streetAddressLine);

            XElement stateCodeElement = new XElement(addressNamespace + "stateCode",
                GetTypeElementAttributes(
                    typeValue: "CD",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.206",
                    codeSystemNameValue: "Субъекты Российской Федерации",
                    codeValue: addressModel.StateCode.Code,
                    codeSystemVersionValue: addressModel.StateCode.CodeSystemVersion,
                    displayNameValue: addressModel.StateCode.DisplayName));
            addrElement.Add(stateCodeElement);

            XElement postalCodeElement = new XElement(xmlnsNamespace + "postalCode", addressModel.PostalCode);
            addrElement.Add(postalCodeElement);

            addrElement.Add(GenerateAddressElement(addressModel.AOGUID, addressModel.HOUSEGUID));

            return addrElement;
        }

        /// <summary>
        /// Создает элемент "fias:Address".
        /// [1..1] Кодирование адреса по ФИАС.
        /// </summary>
        /// <param name="AOGUID">[1..1] Глобальный уникальный идентификатор адресного объекта.</param>
        /// <param name="HOUSEGUID">[1..1] Глобальный уникальный идентификатор дома.</param>
        /// <returns>Элемент "fias:Address".</returns>
        private static XElement GenerateAddressElement(Guid AOGUID, Guid HOUSEGUID)
        {
            XElement addressElement = new XElement(fiasNamespace + "Address");

            XElement AOGUIDElement = new XElement(fiasNamespace + "AOGUID",
                AOGUID.ToString());
            addressElement.Add(AOGUIDElement);

            XElement HOUSEGUIDElement = new XElement(fiasNamespace + "HOUSEGUID",
                HOUSEGUID.ToString());
            addressElement.Add(HOUSEGUIDElement);

            return addressElement;
        }

        /// <summary>
        /// Создает элемент "telecom".
        /// [1..1] Контакты пациента (телефон).
        /// </summary>
        /// <param name="telecomModel">Модель контакта.</param>
        /// <param name="isOrgnization">Это контакт органиазции.</param>
        /// <returns>Элемент "telecom".</returns>
        private static XElement GenerateTelecomElement(TelecomModel telecomModel, bool isOrgnization = false)
        {
            XElement telecomElement = new XElement(xmlnsNamespace + "telecom");
            XAttribute useAttribute = null;

            if (telecomModel.Use != null)
            {
                useAttribute = new XAttribute("use", telecomModel.Use);
            }

            XAttribute valueAttribute = new XAttribute("value", GetTelecomValue(telecomModel.Value));
            
            if(isOrgnization)
            {
                telecomElement.Add(valueAttribute);
                if (useAttribute != null)
                {
                    telecomElement.Add(useAttribute);
                }
            }
            else
            {
                if (useAttribute != null)
                {
                    telecomElement.Add(useAttribute);
                }
                telecomElement.Add(valueAttribute);
            }

            return telecomElement;
        }

        /// <summary>
        /// Создает список элементов "telecom".
        /// [0..*] Контакты пациента (мобильный телефон, электронная почта, факс, url).
        /// </summary>
        /// <param name="telecomModels">Список контактов пациентов.</param>
        /// <param name="isOrganization">Это контакты организации.</param>
        /// <returns>Список элементов "telecom".</returns>
        private static List<XElement> GenerateTelecomElements(List<TelecomModel> telecomModels, bool isOrganization = false)
        {
            List<XElement> telecomElements = new List<XElement>();

            foreach(var telecomModel in telecomModels)
            {
                telecomElements.Add(GenerateTelecomElement(telecomModel, isOrganization));
            }

            return telecomElements;
        }

        /// <summary>
        /// Создает элемент "patient".
        /// </summary>
        /// <param name="peopleDataModel">Модель данных о пациенте.</param>
        /// <returns>эдемент "patient".</returns>
        private static XElement GeneratePatientElement(PeopleDataModel peopleDataModel)
        {
            XElement patientElement = new XElement(xmlnsNamespace + "patient");

            patientElement.Add(GenerateNameElement(peopleDataModel.Name));

            XElement administrativeGenderCodeElement = new XElement(xmlnsNamespace + "administrativeGenderCode",
                GetTypeElementAttributes(
                    codeValue: peopleDataModel.Gender.Code,
                    codeSystemVersionValue: peopleDataModel.Gender.CodeSystemVersion,
                    displayNameValue: peopleDataModel.Gender.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1040",
                    codeSystemNameValue: "Пол пациента"));
            patientElement.Add(administrativeGenderCodeElement);

            XElement birthTimeElement = new XElement(xmlnsNamespace + "birthTime",
                new XAttribute("value", peopleDataModel.BirthDate.ToString("yyyyMMdd")));
            patientElement.Add(birthTimeElement);

            return patientElement;
        }

        /// <summary>
        /// Создает элемент "name".
        /// </summary>
        /// <param name="nameModel">Модель имени.</param>
        /// <returns>Элемент "name".</returns>
        private static XElement GenerateNameElement(NameModel nameModel)
        {
            XElement nameElement = new XElement(xmlnsNamespace + "name");

            XElement familyElement = new XElement(xmlnsNamespace + "family", nameModel.Family);
            nameElement.Add(familyElement);

            XElement givenElement = new XElement(xmlnsNamespace + "given", nameModel.Given);
            nameElement.Add(givenElement);

            XElement patronymicElement = new XElement(identityNamespace + "Patronymic",
                new XAttribute(xsiNamespace + "type", "ST"),
                nameModel.Patronymic);
            nameElement.Add(patronymicElement);

            return nameElement;
        }

        /// <summary>
        /// Создает элемент "guardian".
        /// </summary>
        /// <param name="guardianModel">Модель законного представителя.</param>
        /// <returns>Элемент "guardian".</returns>
        private static XElement GenerateGuardianElement(GuardianModel guardianModel)
        {
            XElement guardianElement = new XElement(xmlnsNamespace + "guardian",
                new XAttribute("classCode", "GUARD"));

            XElement snilsElement = new XElement(xmlnsNamespace + "id", GetIdAttributes("1.2.643.100.3", guardianModel.SNILS));
            guardianElement.Add(snilsElement);

            if (guardianModel.IdentityDocument != null)
            {
                guardianElement.Add(GeneratePersonalDocumentElement(guardianModel.IdentityDocument, "IdentityDoc"));
            }

            if (guardianModel.AuthorityDocument != null)
            {
                guardianElement.Add(GeneratePersonalDocumentElement(guardianModel.AuthorityDocument, "AuthorityDoc"));
            }

            if (guardianModel.ActualAddress != null)
            {
                guardianElement.Add(GenerateAddrElement(guardianModel.ActualAddress));
            }

            guardianElement.Add(GenerateTelecomElement(guardianModel.ContactPhoneNumber));

            if (guardianModel.Contacts != null)
            {
                guardianElement.Add(GenerateTelecomElements(guardianModel.Contacts));
            }

            if (guardianModel.Name != null)
            {
                XElement guardianPersonElement = new XElement(xmlnsNamespace + "guardianPerson");
                guardianPersonElement.Add(GenerateNameElement(guardianModel.Name));
                guardianElement.Add(guardianPersonElement);
            }

            return guardianElement;
        }

        /// <summary>
        /// Создает элемент "providerOrganization".
        /// </summary>
        /// <param name="providerOrganizationModel">Модель организации, направляющей на МСЭ.</param>
        /// <returns>Элемент "providerOrganization".</returns>
        private static XElement GenerateProviderOrganizationElement(OrganizationModel providerOrganizationModel)
        {
            XElement providerOrganizationElement = new XElement(xmlnsNamespace + "providerOrganization");

            XElement idElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", providerOrganizationModel.ID));
            providerOrganizationElement.Add(idElement);

            XElement licenseElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", "1.2.643.5.1.13.2.1.1.1504.101"),
                new XAttribute("extension", providerOrganizationModel.License.Number),
                new XAttribute("assigningAuthorityName", providerOrganizationModel.License.AssigningAuthorityName));
            providerOrganizationElement.Add(licenseElement);

            providerOrganizationElement.Add(GeneratePropsElements(providerOrganizationModel.Props));

            XElement nameElement = new XElement(xmlnsNamespace + "name",
                providerOrganizationModel.Name);
            providerOrganizationElement.Add(nameElement);

            providerOrganizationElement.Add(GenerateTelecomElement(providerOrganizationModel.ContactPhoneNumber, true));

            if (providerOrganizationModel.Contacts != null)
            {
                providerOrganizationElement.Add(GenerateTelecomElements(providerOrganizationModel.Contacts, true));
            }

            providerOrganizationElement.Add(GenerateAddrElement(providerOrganizationModel.Address));

            return providerOrganizationElement;
        }

        /// <summary>
        /// Создает элемент "Props".
        /// </summary>
        /// <param name="propsModel">Модель реквизитов организации.</param>
        /// <returns>Элемент "Props".</returns>
        private static XElement GeneratePropsElements(PropsOrganizationModel propsModel)
        {
            XElement propsElements = new XElement(identityNamespace + "Props");

            if(propsModel.OGRN != null)
            {
                propsElements.Add(GenerateChildPropsElement("Ogrn", propsModel.OGRN));
            }
            if(propsModel.OGRNIP != null)
            {
                propsElements.Add(GenerateChildPropsElement("Ogrnip", propsModel.OGRNIP));
            }
            if(propsModel.OKPO != null)
            {
                propsElements.Add(GenerateChildPropsElement("Okpo", propsModel.OKPO));
            }
            if(propsModel.OKATO != null)
            {
                propsElements.Add(GenerateChildPropsElement("Okato", propsModel.OKATO));
            }

            return propsElements;
        }

        /// <summary>
        /// Создает дочерний элемент "Props".
        /// </summary>
        /// <param name="elementName">Наименование элемента.</param>
        /// <param name="elementValue">Значение элемента.</param>
        /// <returns>Дочерний элемент "Props".</returns>
        private static XElement GenerateChildPropsElement(string elementName, string elementValue)
        {
            return new XElement(identityNamespace + elementName,
                new XAttribute(xsiNamespace + "type", "ST"),
                elementValue);
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
        /// <param name="codeValue">Значения атрибута "сode".</param>
        /// <param name="codeSystemValue">Значения атрибута "codeSystem".</param>
        /// <param name="codeSystemVersionValue">Значения атрибута "codeSystemVersion".</param>
        /// <param name="codeSystemNameValue">Значения атрибута "codeSystemName".</param>
        /// <param name="displayNameValue">Значения атрибута "displayName".</param>
        /// <param name="typeValue">Значения атрибута "type".</param>
        /// <returns>Cписок атрибутов элемента "code".</returns>
        private static List<XAttribute> GetTypeElementAttributes(
            string codeValue,
            string codeSystemValue,
            string codeSystemVersionValue,
            string codeSystemNameValue,
            string displayNameValue,
            string typeValue = null)
        {
            List<XAttribute> attributes = new List<XAttribute>();
            if (typeValue != null)
            {
                attributes.Add(new XAttribute(xsiNamespace + "type", typeValue));
            }
            attributes.Add(new XAttribute("code", codeValue));
            attributes.Add(new XAttribute("codeSystem", codeSystemValue));
            attributes.Add(new XAttribute("codeSystemVersion", codeSystemVersionValue));
            attributes.Add(new XAttribute("codeSystemName", codeSystemNameValue));
            attributes.Add(new XAttribute("displayName", displayNameValue));

            return attributes;
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

        #region Helpers methods

        /// <summary>
        /// Получает форматированное значение элемента "telecom".
        /// - tel:
        /// - mailto:
        /// - fax:
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Форматированное значение.</returns>
        private static string GetTelecomValue(string value)
        {
            if (value.Contains("+"))
            {
                return $"tel:{value}";
            }
            if (value.Contains("@"))
            {
                return $"mailto:{value}";
            }
            if (value.Contains("http"))
            {
                return value;
            }
            
            return $"fax:{value}";
        }

        /// <summary>
        /// Получить значения "*Type" элемента.
        /// </summary>
        /// <param name="elementName">Наименование элемента.</param>
        /// <returns>Значения "*Type" элемента.</returns>
        private static (string typeValue, string codeSystemValue, string codeSystemNameValue)? GetTypeValue(string elementName)
        {
            (string typeValue, string codeSystemValue, string codeSystemNameValue) identityDocumentTypeValue = ("CD", "1.2.643.5.1.13.13.99.2.48", "Документы, удостоверяющие личность");
            (string typeValue, string codeSystemValue, string codeSystemNameValue) authorityDocumentTypeValue = ("CD", "1.2.643.5.1.13.13.99.2.313", "Документы, удостоверяющие полномочия законного представителя");

            if (elementName == "IdentityDoc")
            {
                return identityDocumentTypeValue;
            }
            if (elementName == "AuthorityDoc")
            {
                return authorityDocumentTypeValue;
            }

            return null;
        }

        #endregion
    }
}
