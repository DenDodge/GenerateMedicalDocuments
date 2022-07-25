using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Static elements

        private static readonly XElement NewLineElement = new XElement(xmlnsNamespace + "br");

        #endregion

        /// <summary>
        /// Получить XML документ "Направление на медико-социальную экспертизу" по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>XML документ "Направление на медико-социальную экспертизу" по модели документа.</returns>
        public static XDocument GetXMLDocument(DirectionToMSEDocumentModel documentModel)
        {
            if (documentModel == null)
            {
                return null;
            }
            
            XDocument document = new XDocument();

            // формируем шапку документа.
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
            if (documentModel == null)
            {
                return null;
            }
            
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
            clinicalDocumentElement.Add(GenerateAuthorElement(documentModel.Author));
            clinicalDocumentElement.Add(GenerateCustodianElement(documentModel.RepresentedCustodianOrganization));
            clinicalDocumentElement.Add(GenerateInformationRecipientElement());
            clinicalDocumentElement.Add(GenerateLegalAuthenticatorElement(documentModel.LegalAuthenticator));
            clinicalDocumentElement.Add(GenerateParticipantElement(documentModel.Participant));
            clinicalDocumentElement.Add(GenerateDocumentationOfElement(documentModel.ServiceEvent));
            clinicalDocumentElement.Add(GenerateBodyDocumentElement(documentModel.DocumentBody));

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
            if (recordTargetModel == null)
            {
                return null;
            }
            
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
            if (patientRoleModel == null)
            {
                return null;
            }
            
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

            patientRoleElement.Add(GenerateOrganizationElement(patientRoleModel.ProviderOrganization, "providerOrganization"));

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
            if (documentModel == null)
            {
                return null;
            }
            
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

            if (documentModel.Series is not null)
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    documentModel.Series);
                identityDocElement.Add(seriesElement);
            }
            else
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    new XAttribute("nullFlavor", "NA"));
                identityDocElement.Add(seriesElement);
            }

            XElement numberElement = new XElement(identityNamespace + "Number",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.Number);
            identityDocElement.Add(numberElement);

            if (documentModel.IssueOrgName is not null)
            {
                XElement issueOrgNameElement = new XElement(identityNamespace + "IssueOrgName",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    documentModel.IssueOrgName);
                identityDocElement.Add(issueOrgNameElement);
            }
            else
            {
                XElement issueOrgNameElement = new XElement(identityNamespace + "IssueOrgName",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    new XAttribute("nullFlavor", "NA"));
                identityDocElement.Add(issueOrgNameElement);
            }

            if(documentModel.IssueOrgCode is not null)
            {
                XElement issueOrgCodeElement = new XElement(identityNamespace + "IssueOrgCode",
                new XAttribute(xsiNamespace + "type", "ST"),
                documentModel.IssueOrgCode);
                identityDocElement.Add(issueOrgCodeElement);
            }
            else
            {
                XElement issueOrgCodeElement = new XElement(identityNamespace + "IssueOrgCode",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    new XAttribute("nullFlavor", "NA"));
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
            if (insurancePolicyModel == null)
            {
                return null;
            }
            
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

            if (insurancePolicyModel.Series is not null)
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    insurancePolicyModel.Series);
                insurancePolicyElement.Add(seriesElement);
            }
            // else
            // {
            //     XElement seriesElement = new XElement(identityNamespace + "Series",
            //         new XAttribute(xsiNamespace + "type", "ST"),
            //         new XAttribute("nullFlavor", "NA"));
            //     insurancePolicyElement.Add(seriesElement);
            // }

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
            
            if (addressModel is null)
            {
                XAttribute nullFlavorAddressAttribute = new XAttribute("nullFlavor", "NI");
                addrElement.Add(nullFlavorAddressAttribute);
                return addrElement;
            }

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

            XElement postalCodeElement;
            if (addressModel.PostalCode is not null)
            {
                postalCodeElement = new XElement(xmlnsNamespace + "postalCode", addressModel.PostalCode);
            }
            else
            {
                postalCodeElement = new XElement(xmlnsNamespace + "postalCode", addressModel.PostalCode);
                XAttribute nullFlavorPostalCodeAttribute = new XAttribute("nullFlavor", "NI");
                postalCodeElement.Add(nullFlavorPostalCodeAttribute);
            }
            
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
        private static XElement GenerateAddressElement(Guid AOGUID, Guid? HOUSEGUID)
        {
            XElement addressElement = new XElement(fiasNamespace + "Address");

            XElement AOGUIDElement = new XElement(fiasNamespace + "AOGUID",
                AOGUID.ToString());
            addressElement.Add(AOGUIDElement);

            XElement HOUSEGUIDElement;
            if (HOUSEGUID is not null)
            {
                HOUSEGUIDElement = new XElement(fiasNamespace + "HOUSEGUID",
                    HOUSEGUID.ToString());
            }
            else
            {
                HOUSEGUIDElement = new XElement(fiasNamespace + "HOUSEGUID");
                XAttribute nullFlavorHOUSEGUIDEAttribute = new XAttribute("nullFlavor", "NI");
                HOUSEGUIDElement.Add(nullFlavorHOUSEGUIDEAttribute);
            }
            
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
            if (telecomModel is null)
            {
                XElement nullFlavorTelecomElement = new XElement(xmlnsNamespace + "telecom");
                XAttribute nullFlavorTelecomAttribute = new XAttribute("nullFlavor", "NI");
                nullFlavorTelecomElement.Add(nullFlavorTelecomAttribute);
                return nullFlavorTelecomElement;
            }
            
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

            if (telecomModels == null)
            {
                return null;
            }
            
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
            if (peopleDataModel == null)
            {
                return null;
            }
            
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
            if (nameModel == null)
            {
                return null;
            }
            
            XElement nameElement = new XElement(xmlnsNamespace + "name");

            XElement familyElement = new XElement(xmlnsNamespace + "family", nameModel.Family);
            nameElement.Add(familyElement);

            XElement givenElement = new XElement(xmlnsNamespace + "given", nameModel.Given);
            nameElement.Add(givenElement);

            XElement patronymicElement = new XElement(identityNamespace + "Patronymic",
                new XAttribute(xsiNamespace + "type", "ST"));
            if (nameModel.Patronymic is not null)
            {
                patronymicElement.Add(nameModel.Patronymic);
            }
            else
            {
                XAttribute nullFlavorPatronymicAttribute = new XAttribute("nullFlavor", "NI");
                patronymicElement.Add(nullFlavorPatronymicAttribute);
            }
            
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
            if (guardianModel == null)
            {
                return null;
            }
            
            XElement guardianElement = new XElement(xmlnsNamespace + "guardian",
                new XAttribute("classCode", "GUARD"));

            if (guardianModel.SNILS is not null)
            {
                XElement snilsElement = new XElement(xmlnsNamespace + "id", GetIdAttributes("1.2.643.100.3", guardianModel.SNILS));
                guardianElement.Add(snilsElement);
            }

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
        /// Создает элемент организации.
        /// </summary>
        /// <param name="organizationModel">Модель организации, направляющей на МСЭ.</param>
        /// <param name="elementName">Наименование элемента.</param>
        /// <param name="classCodeAttributValue">Значение атрибута "classCode".</param>
        /// <param name="rootValue">Значение параметра root в ID.</param>
        /// <returns>Элемент организации.</returns>
        private static XElement GenerateOrganizationElement(
            OrganizationModel organizationModel, 
            string elementName, 
            string classCodeAttributValue = null, 
            string rootValue = null)
        {
            if (organizationModel == null)
            {
                return null;
            }

            XElement organizationElement = new XElement(xmlnsNamespace + elementName);
            if (classCodeAttributValue != null)
            {
                XAttribute classCodeAttribute = new XAttribute("classCode", classCodeAttributValue);
                organizationElement.Add(classCodeAttribute);
            }

            if (organizationModel.ID != null)
            {
                if (rootValue == null)
                {
                    XElement idElement = new XElement(xmlnsNamespace + "id",
                        new XAttribute("root", organizationModel.ID));
                    organizationElement.Add(idElement);
                }
                else
                {
                    XElement idElement = new XElement(xmlnsNamespace + "id",
                        new XAttribute("root", rootValue),
                        new XAttribute("extension", organizationModel.ID));
                    organizationElement.Add(idElement);
                }
            }
            else
            {
                XElement idElement = new XElement(xmlnsNamespace + "id",
                    new XAttribute("nullFlavor", "NI"));
                organizationElement.Add(idElement);
            }

            if (organizationModel.License != null)
            {
                XElement licenseElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", "1.2.643.5.1.13.2.1.1.1504.101"),
                new XAttribute("extension", organizationModel.License.Number),
                new XAttribute("assigningAuthorityName", organizationModel.License.AssigningAuthorityName));
                organizationElement.Add(licenseElement);
            }
            
            if (organizationModel.Props != null)
            {
                organizationElement.Add(GeneratePropsElements(organizationModel.Props));
            }
            //else
            //{
            //    XElement propsElement = new XElement(identityNamespace + "Props",
            //        new XAttribute("nullFlavor", "NI"));//,
            //        //new XAttribute(xsiNamespace + "type", "ST"));
            //    organizationElement.Add(propsElement);
            //}

            if (organizationModel.Name != null)
            {
                XElement nameElement = new XElement(xmlnsNamespace + "name", organizationModel.Name);
                organizationElement.Add(nameElement);
            }

            organizationElement.Add(GenerateTelecomElement(organizationModel.ContactPhoneNumber, true));

            if (organizationModel.Contacts != null)
            {
                organizationElement.Add(GenerateTelecomElements(organizationModel.Contacts, true));
            }

            organizationElement.Add(GenerateAddrElement(organizationModel.Address));

            return organizationElement;
        }

        /// <summary>
        /// Создает элемент "Props".
        /// </summary>
        /// <param name="propsModel">Модель реквизитов организации.</param>
        /// <returns>Элемент "Props".</returns>
        private static XElement GeneratePropsElements(PropsOrganizationModel propsModel)
        {
            if (propsModel == null)
            {
                return null;
            }
            
            XElement propsElements = new XElement(identityNamespace + "Props");

            if(propsModel.OGRN != null)
            {
                propsElements.Add(GenerateChildPropsElement("Ogrn", propsModel.OGRN));
            }
            else
            {
                propsElements.Add(new XElement(identityNamespace + "Ogrn",
                    new XAttribute(xsiNamespace + "nullFlavor", "NI"),
                    new XAttribute(xsiNamespace + "type", "ST")));
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

        /// <summary>
        /// Создает элемент "author".
        /// </summary>
        /// <param name="authorDateModel">Модель автора документа.</param>
        /// <returns>Элемент "author".</returns>
        private static XElement GenerateAuthorElement(AuthorDataModel authorDateModel)
        {
            if (authorDateModel == null)
            {
                return null;
            }
            
            XElement authorElement = new XElement(xmlnsNamespace + "author");

            XElement timeElement = new XElement(xmlnsNamespace + "time",
                new XAttribute("value", authorDateModel.SignatureDate.ToString("yyyyMMddHHmm+0300")));
            authorElement.Add(timeElement);

            authorElement.Add(GenerateAssignedElement(authorDateModel.Author, "assignedAuthor"));

            return authorElement;
        }

        /// <summary>
        /// Создает элемент "assigned*".
        /// </summary>
        /// <param name="authorModel">Модель роли.</param>
        /// <param name="elementName">Имя элемента.</param>
        /// <returns>Элемент "assigned*".</returns>
        private static XElement GenerateAssignedElement(AuthorModel authorModel, string elementName)
        {
            if (authorModel == null)
            {
                return null;
            }
            
            XElement assignedElement = new XElement(xmlnsNamespace + elementName);

            XElement idElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", authorModel.ID.Root),
                new XAttribute("extension", authorModel.ID.Extension));
            assignedElement.Add(idElement);

            XElement snilsElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", "1.2.643.100.3"),
                new XAttribute("extension", authorModel.SNILS));
            assignedElement.Add(snilsElement);

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeSystemValue: "1.2.643.5.1.13.13.11.1002",
                    codeSystemNameValue: "Должности медицинских и фармацевтических работников",
                    codeValue: authorModel.Position.Code,
                    codeSystemVersionValue: authorModel.Position.CodeSystemVersion,
                    displayNameValue: authorModel.Position.DisplayName));
            assignedElement.Add(codeElement);

            if (authorModel.ActualAddress != null)
            {
                assignedElement.Add(GenerateAddrElement(authorModel.ActualAddress));
            }

            if (authorModel.Address != null)
            {
                assignedElement.Add(GenerateAddrElement(authorModel.Address));
            }

            assignedElement.Add(GenerateTelecomElement(authorModel.ContactPhoneNumber));

            if (authorModel.Contacts != null)
            {
                assignedElement.Add(GenerateTelecomElements(authorModel.Contacts));
            }

            XElement assignedPersonElement = new XElement(xmlnsNamespace + "assignedPerson");
            assignedPersonElement.Add(GenerateNameElement(authorModel.Name));
            assignedElement.Add(assignedPersonElement);

            assignedElement.Add(GenerateOrganizationElement(authorModel.RepresentedOrganization, "representedOrganization", "ORG"));

            return assignedElement;
        }

        /// <summary>
        /// Создает элемент "custodian" с дочерними элементами.
        /// </summary>
        /// <param name="representedCustodianOrganizationModel">Модель данных об организации-владельце документа.</param>
        /// <returns>Элемент "custodian" с дочерними элементами.</returns>
        private static XElement GenerateCustodianElement(OrganizationModel representedCustodianOrganizationModel)
        {
            if (representedCustodianOrganizationModel == null)
            {
                return null;
            }
            
            XElement custodianElement = new XElement(xmlnsNamespace + "custodian");

            XElement assignedCustodianElement = new XElement(xmlnsNamespace + "assignedCustodian");

            assignedCustodianElement.Add(GenerateOrganizationElement(representedCustodianOrganizationModel, "representedCustodianOrganization", "ORG"));
            custodianElement.Add(assignedCustodianElement);

            return custodianElement;
        }

        /// <summary>
        /// Создает элемент "informationRecipient".
        /// </summary>
        /// <returns>Элемент "informationRecipient".</returns>
        private static XElement GenerateInformationRecipientElement()
        {
            XElement informationRecipientElement = new XElement(xmlnsNamespace + "informationRecipient");

            XElement intendedRecipientElement = new XElement(xmlnsNamespace + "intendedRecipient");

            XElement receivedOrganizationElement = new XElement(xmlnsNamespace + "receivedOrganization");

            XElement idElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", "1.2.643.5.1.13"));
            receivedOrganizationElement.Add(idElement);

            XElement nameElement = new XElement(xmlnsNamespace + "name", "Министерство здравоохранения Российской Федерации");
            receivedOrganizationElement.Add(nameElement);

            intendedRecipientElement.Add(receivedOrganizationElement);
            informationRecipientElement.Add(intendedRecipientElement);

            return informationRecipientElement;
        }

        /// <summary>
        /// Создает элемент "legalAuthenticator".
        /// </summary>
        /// <param name="legalAuthenticatorModel">Модель лица, придавшего юридическую силу документу.</param>
        /// <returns>Элемент "legalAuthenticator".</returns>
        private static XElement GenerateLegalAuthenticatorElement(LegalAuthenticatorModel legalAuthenticatorModel)
        {
            if (legalAuthenticatorModel == null)
            {
                return null;
            }
            
            XElement authorElement = new XElement(xmlnsNamespace + "legalAuthenticator");

            XElement timeElement = new XElement(xmlnsNamespace + "time",
                new XAttribute("value", legalAuthenticatorModel.SignatureDate.ToString("yyyyMMddHHmm+0300")));
            authorElement.Add(timeElement);

            XElement signatureCodeElement = new XElement(xmlnsNamespace + "signatureCode",
                new XAttribute("code", "S"));
            authorElement.Add(signatureCodeElement);

            authorElement.Add(GenerateAssignedElement(legalAuthenticatorModel.AssignedEntity, "assignedEntity"));

            return authorElement;
        }

        /// <summary>
        /// Создает элемент "participant".
        /// </summary>
        /// <param name="participantModel">Модель сведений об источнике оплаты.</param>
        /// <returns>Элемент "participant".</returns>
        private static XElement GenerateParticipantElement(ParticipantModel participantModel)
        {
            if (participantModel == null)
            {
                return null;
            }
            
            XElement participantElement = new XElement(xmlnsNamespace + "participant",
                new XAttribute("typeCode", "IND"));

            XElement associatedEntityElement = new XElement(xmlnsNamespace + "associatedEntity",
                new XAttribute("classCode", "GUAR"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: participantModel.Code.Code,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1039",
                    codeSystemVersionValue: participantModel.Code.CodeSystemVersion,
                    codeSystemNameValue: "Источники оплаты медицинской помощи",
                    displayNameValue: participantModel.Code.DisplayName));
            associatedEntityElement.Add(codeElement);

            associatedEntityElement.Add(GenerateBasisDocumentElement(participantModel.DocInfo));
            associatedEntityElement.Add(GenerateOrganizationElement(participantModel.ScopingOrganization, "scopingOrganization", rootValue: "1.2.643.5.1.13.13.99.2.183"));

            participantElement.Add(associatedEntityElement);

            return participantElement;
        }

        /// <summary>
        /// Создать элемент "DocInfo".
        /// </summary>
        /// <param name="basisDocumentModel">Модель документа-основания.</param>
        /// <returns>Элемент "DocInfo".</returns>
        private static XElement GenerateBasisDocumentElement(BasisDocumentModel basisDocumentModel)
        {
            if (basisDocumentModel == null)
            {
                XElement nullFlavorDocInfoElement = new XElement(identityNamespace + "DocInfo");
                XAttribute nullFlavorDocInfoAttribute = new XAttribute("nullFlavor", "INV");
                nullFlavorDocInfoElement.Add(nullFlavorDocInfoAttribute);
                return nullFlavorDocInfoElement;
            }
            
            XElement docInfoElement = new XElement(identityNamespace + "DocInfo");

            XElement identityDocTypeElement = new XElement(identityNamespace + "IdentityDocType",
                GetTypeElementAttributes(
                    typeValue: "CD",
                    codeValue: basisDocumentModel.IdentityDocType.Code,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.724",
                    codeSystemVersionValue: basisDocumentModel.IdentityDocType.CodeSystemVersion,
                    codeSystemNameValue: "Типы документов оснований",
                    displayNameValue: basisDocumentModel.IdentityDocType.DisplayName));
            docInfoElement.Add(identityDocTypeElement);

            if (basisDocumentModel.InsurancePolicyType is not null)
            {
                XElement insurancePolicyTypeElement = new XElement(identityNamespace + "InsurancePolicyType",
                    GetTypeElementAttributes(
                        typeValue: "CD",
                        codeValue: basisDocumentModel.InsurancePolicyType.Code,
                        codeSystemValue: "1.2.643.5.1.13.13.11.1035",
                        codeSystemVersionValue: basisDocumentModel.InsurancePolicyType.CodeSystemVersion,
                        codeSystemNameValue: "Виды полиса обязательного медицинского страхования",
                        displayNameValue: basisDocumentModel.InsurancePolicyType.DisplayName));
                docInfoElement.Add(insurancePolicyTypeElement);
            }
            else
            {
                XElement insurancePolicyTypeElement = new XElement(identityNamespace + "InsurancePolicyType");
                XAttribute nullFlavorInsurancePolicyTypeAttribute = new XAttribute("nullFlavor", "NA");
                insurancePolicyTypeElement.Add(nullFlavorInsurancePolicyTypeAttribute);
                docInfoElement.Add(insurancePolicyTypeElement);
            }

            if (basisDocumentModel.Series is not null)
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    basisDocumentModel.Series);
                docInfoElement.Add(seriesElement);
            }
            else
            {
                XElement seriesElement = new XElement(identityNamespace + "Series",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    new XAttribute("nullFlavor", "NA"));
                docInfoElement.Add(seriesElement);
            }

            if (basisDocumentModel.Number is not null)
            {
                XElement numberElement = new XElement(identityNamespace + "Number",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    basisDocumentModel.Number);
                docInfoElement.Add(numberElement);
            }

            if (basisDocumentModel.INN is not null)
            {
                XElement INNElement = new XElement(identityNamespace + "INN",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    basisDocumentModel.INN);
                docInfoElement.Add(INNElement);
            }
            else
            {
                XElement INNElement = new XElement(identityNamespace + "INN",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    new XAttribute("nullFlavor", "NA"));
                docInfoElement.Add(INNElement);
            }
            
            docInfoElement.Add(GenerateEffectiveTimeElement(basisDocumentModel.StartDateDocument, basisDocumentModel.FinishDateDocument, identityNamespace));

            return docInfoElement;
        }

        /// <summary>
        /// Создает элемент "effectiveTime".
        /// </summary>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="finishDate">Дата окончания.</param>
        /// <param name="isUseTime">Используется время.</param>
        /// <returns>Элемент "effectiveTime".</returns>
        private static XElement GenerateEffectiveTimeElement(
            DateTime startDate, 
            DateTime? finishDate, 
            XNamespace namespaceValue, 
            bool isUseTime = false,
            bool isDocumentationOf = false)
        {
            string startDateString = null;
            string finishDateString = null;
            if (isUseTime)
            {
                startDateString = startDate.ToString("yyyyMMddHHmm+0300");
                if (finishDate != null)
                {
                    finishDateString = finishDate.Value.ToString("yyyyMMddHHmm+0300");
                }
            }
            else
            {
                startDateString = startDate.ToString("yyyyMMdd");
                if (finishDate != null)
                {
                    finishDateString = finishDate.Value.ToString("yyyyMMdd");
                }
            }

            XElement effectiveTimeElement = new XElement(namespaceValue + "effectiveTime");

            XElement lowElement = new XElement(namespaceValue + "low");
            
            if (!isUseTime)
            {
                XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "TS");
                lowElement.Add(typeAttribute);
            }
            XAttribute lowValueAttribute = new XAttribute("value", startDateString);
            lowElement.Add(lowValueAttribute);

            effectiveTimeElement.Add(lowElement);

            if (finishDateString is not null)
            {
                XElement highElement = new XElement(namespaceValue + "high");
                
                if (!isUseTime)
                {
                    XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "TS");
                    highElement.Add(typeAttribute);
                }
                
                XAttribute highValueAttribute = new XAttribute("value", finishDateString);
                highElement.Add(highValueAttribute);
                
                effectiveTimeElement.Add(highElement);
            }
            else
            {
                if (!isDocumentationOf)
                {
                    XElement highElement = new XElement(namespaceValue + "high");
                    XAttribute nullFlavorAttribute = new XAttribute("nullFlavor", "NAV");
                    highElement.Add(nullFlavorAttribute);
                    effectiveTimeElement.Add(highElement);
                }
            }

            return effectiveTimeElement;
        }

        /// <summary>
        /// Создает элемент "documentationOf".
        /// </summary>
        /// <param name="serviceEventModel">Модель сведений о документируемом событии.</param>
        /// <returns></returns>
        private static XElement GenerateDocumentationOfElement(ServiceEventModel serviceEventModel)
        {
            if (serviceEventModel == null)
            {
                return null;
            }

            XElement documentationOfElement = new XElement(xmlnsNamespace + "documentationOf");
            XElement serviceEventElement = new XElement(xmlnsNamespace + "serviceEvent");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: serviceEventModel.Code.Code,
                    codeSystemVersionValue: serviceEventModel.Code.CodeSystemVersion,
                    displayNameValue: serviceEventModel.Code.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.726",
                    codeSystemNameValue: "Типы документированных событий"));
            serviceEventElement.Add(codeElement);

            serviceEventElement.Add(GenerateEffectiveTimeElement(
                serviceEventModel.StartServiceDate,
                serviceEventModel.FinishServiceDate, 
                xmlnsNamespace, 
                true, 
                true));

            if (serviceEventModel.ServiceForm != null)
            {
                XElement serviceFormElement = new XElement(medServiceNamespace + "serviceForm",
                GetTypeElementAttributes(
                    codeValue: serviceEventModel.ServiceForm.Code,
                    codeSystemVersionValue: serviceEventModel.ServiceForm.CodeSystemVersion,
                    displayNameValue: serviceEventModel.ServiceForm.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1551",
                    codeSystemNameValue: "Формы оказания медицинской помощи"));
                serviceEventElement.Add(serviceFormElement);
            }

            if (serviceEventModel.ServiceType != null)
            {
                XElement serviceTypeElement = new XElement(medServiceNamespace + "serviceType",
                    GetTypeElementAttributes(
                        codeValue: serviceEventModel.ServiceType.Code,
                        codeSystemVersionValue: serviceEventModel.ServiceType.CodeSystemVersion,
                        displayNameValue: serviceEventModel.ServiceType.DisplayName,
                        codeSystemValue: "1.2.643.5.1.13.13.11.1034",
                        codeSystemNameValue: "Виды медицинской помощи"));
                serviceEventElement.Add(serviceTypeElement);
            }

            if (serviceEventModel.ServiceCond != null)
            {
                XElement serviceCondElement = new XElement(medServiceNamespace + "serviceCond",
                    GetTypeElementAttributes(
                        codeValue: serviceEventModel.ServiceCond.Code,
                        codeSystemVersionValue: serviceEventModel.ServiceCond.CodeSystemVersion,
                        displayNameValue: serviceEventModel.ServiceCond.DisplayName,
                        codeSystemValue: "1.2.643.5.1.13.13.99.2.322",
                        codeSystemNameValue: "Условия оказания медицинской помощи"));
                serviceEventElement.Add(serviceCondElement);
            }

            serviceEventElement.Add(GeneratePerformerElement(serviceEventModel.Performer, "PPRF"));

            if (serviceEventModel.OtherPerformers != null)
            {
                foreach(var performer in serviceEventModel.OtherPerformers)
                {
                    serviceEventElement.Add(GeneratePerformerElement(performer, "SPRF"));
                }
            }

            documentationOfElement.Add(serviceEventElement);
            return documentationOfElement;
        }

        /// <summary>
        /// Создает элемент "performer".
        /// </summary>
        /// <param name="performerModel">Модель члена врачебной комиссии.</param>
        /// <param name="typeCodeValue">Тип кода.</param>
        /// <returns>Элемент "performer".</returns>
        private static XElement GeneratePerformerElement(PerformerModel performerModel, string typeCodeValue)
        {
            if (performerModel == null)
            {
                return null;
            }
            
            XElement performerElement = new XElement(xmlnsNamespace + "performer",
                new XAttribute("typeCode", typeCodeValue));

            performerElement.Add(GenerateAssignedElement(performerModel, "assignedEntity"));

            return performerElement;
        }

        /// <summary>
        /// Создает элемент "component" с наполнением (тело документа).
        /// </summary>
        /// <param name="documentBodyModel">Модель тела докумнта.</param>
        /// <returns>Элемент "component" с наполнением (тело документа).</returns>
        private static XElement GenerateBodyDocumentElement(DocumentBodyModel documentBodyModel)
        {
            if (documentBodyModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement structuredBodyElement = new XElement(xmlnsNamespace + "structuredBody");

            structuredBodyElement.Add(GenerateSentSectionElement(documentBodyModel.SentSection));
            structuredBodyElement.Add(GenerateWorkLocationSectionElement(documentBodyModel.WorkplaceSection));
            structuredBodyElement.Add(GenerateEducationSectionElement(documentBodyModel.EducationSection));
            structuredBodyElement.Add(GenerateAnamnezSectionElement(documentBodyModel.AnamnezSection));
            structuredBodyElement.Add(GenerateVitalParametersSectionElement(documentBodyModel.VitalParametersSection));
            structuredBodyElement.Add(GenerateDirectionStateSectionElement(documentBodyModel.DirectionStateSection));
            structuredBodyElement.Add(GenerateDiagnosticStudiesSectionElement(documentBodyModel.DiagnosticStudiesSection));
            structuredBodyElement.Add(GenerateDiagnosisSection(documentBodyModel.DiagnosisSection));
            structuredBodyElement.Add(GenerateConditionAssessmentSectionElement(documentBodyModel.ConditionAssessmentSection));
            structuredBodyElement.Add(GenerateRecommendationSectionElement(documentBodyModel.RecommendationsSection));
            structuredBodyElement.Add(GenerateOutsideSpecialMedicalCareSection(documentBodyModel.OutsideSpecialMedicalCareSection));
            structuredBodyElement.Add(GenerateAttachmentDocumentsSectionElement(documentBodyModel.AttachmentDocumentsSection));

            componentElement.Add(structuredBodyElement);
            return componentElement;
        }


        #region Vital parameters

        /// <summary>
        /// Создает секцияю "Витальные параметры".
        /// </summary>
        /// <param name="vitalParametersSectionModel">Модель секции "Витальные параметры".</param>
        /// <returns>Cекция "Витальные параметры".</returns>
        private static XElement GenerateVitalParametersSectionElement(VitalParametersSectionModel vitalParametersSectionModel)
        {
            if (vitalParametersSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            var codeValue = GetCodeValue("mainVitalParameters");
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue,
                    displayNameValue: codeValue.displayNameValue));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "АНТРОПОМЕТРИЧЕСКИЕ ДАННЫЕ И ФИЗИОЛОГИЧЕСКИЕ ПАРАМЕТРЫ");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateVitalParametersSectionTableElement(vitalParametersSectionModel));
            sectionElement.Add(GenerateVitalParametersSectionCodingElements(vitalParametersSectionModel));

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает таблицу наполнения секции "Витальные параметры".
        /// </summary>
        /// <param name="vitalParametersSectionModel">Модель "Витальные параметры".</param>
        /// <returns>Таблица наполнения секции "Витальные параметры".</returns>
        private static XElement GenerateVitalParametersSectionTableElement(VitalParametersSectionModel vitalParametersSectionModel)
        {
            if (vitalParametersSectionModel == null)
            {
                return null;
            }
            
            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement tableElement = new XElement(xmlnsNamespace + "table",
                new XAttribute("width", "100%"));

            XElement col30Element = new XElement(xmlnsNamespace + "col",
                new XAttribute("width", "30%"));
            tableElement.Add(col30Element);

            XElement col70Element = new XElement(xmlnsNamespace + "col",
                new XAttribute("width", "70%"));
            tableElement.Add(col70Element);

            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

            // создание табличных секций, кроме "Телосложения".
            if (vitalParametersSectionModel.VitalParameters != null)
            {
                foreach (var parametr in vitalParametersSectionModel.VitalParameters)
                {
                    tbodyElement.Add(GenerateVitalParametersSectionTRElement(parametr));
                }
            }
            // создание табличной секции "Телосложение".
            if (vitalParametersSectionModel.BodyType is not null)
            {
                var bodyTypeModel = new VitalParameterModel()
                {
                    Caption = "Телосложение",
                    ID = "vv1_4",
                    Value = vitalParametersSectionModel.BodyType
                };
                tbodyElement.Add(GenerateVitalParametersSectionTRElement(bodyTypeModel));
            }

            tableElement.Add(tbodyElement);

            textElement.Add(tableElement);
            return textElement;
        }

        /// <summary>
        /// Создает элемент "tr" для таблицы секции "Витальные параметры".
        /// </summary>
        /// <param name="caption">Заголовок.</param>
        /// <param name="contentElements">Элемент контанта.</param>
        /// <returns>Элемент "tr" для таблицы секции "Витальные параметры".</returns>
        private static XElement GenerateVitalParametersSectionTRElement(VitalParameterModel vitalParameterModel)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");

            XElement tdCaptionElement = new XElement(xmlnsNamespace + "td", vitalParameterModel.Caption);
            trElement.Add(tdCaptionElement);

            XElement tdContentElement = new XElement(xmlnsNamespace + "td");

            XElement contentElement = new XElement(xmlnsNamespace + "content",
                new XAttribute("ID", vitalParameterModel.ID),
                vitalParameterModel.Unit is not null ? $"{vitalParameterModel.Value} {vitalParameterModel.Unit}" : vitalParameterModel.Value);
            tdContentElement.Add(contentElement);

            trElement.Add(tdContentElement);

            return trElement;
        }

        /// <summary>
        /// Создает кодированные элементы "entry" для секции "Витальные параметры".
        /// </summary>
        /// <param name="vitalParametersSectionModel">Модель "Витальные параметры".</param>
        /// <returns></returns>
        private static List<XElement> GenerateVitalParametersSectionCodingElements(VitalParametersSectionModel vitalParametersSectionModel)
        {
            if (vitalParametersSectionModel == null)
            {
                return null;
            }
            
            List<XElement> entryElements = new List<XElement>();

            if (vitalParametersSectionModel.VitalParameters is not null)
            {
                foreach (var parameter in vitalParametersSectionModel.VitalParameters)
                {
                    entryElements.Add(GenerateVitalParametersSectionEntryElement(
                            parameter.Code,
                            parameter.EntryDisplayName,
                            parameter.DateMetering.ToString("yyyyMMddHHmm+0300"), 
                        $"#{parameter.ID}",
                        parameter.EntryType,
                        parameter.EntryValue,
                        parameter.EntryUnit));
                }
            }
            
            if (vitalParametersSectionModel.BodyType is not null)
            {
                ValueElementModel valueElementModel = new ValueElementModel()
                {
                    Type = "CD",
                    Code = "1",
                    CodeSystem = "1.2.643.5.1.13.13.11.1492",
                    CodeSystemVersion = "1.1",
                    CodeSystemName = "Типы телосложения",
                    DisplayName = vitalParametersSectionModel.BodyType.ToLower()
                };
                entryElements.Add(GenerateVitalParametersSectionEntryElement_light("bodyTypeParameters", valueElementModel));
            }

            return entryElements;
        }

        /// <summary>
        /// Создает элемент "entry" для секции "Витильные параметры".
        /// </summary>
        /// <param name="codeElementName">Наименование элемента "code".</param>
        /// <param name="effectiveTimeValue">Дата измерения.</param>
        /// <param name="referenceValue">Ссылка на секцию из таблицы.</param>
        /// <param name="valueType">Значение "type".</param>
        /// <param name="valueValue">Значение "value".</param>
        /// <param name="valueUnit">Значение "unit".</param>
        /// <returns>Элемент "entry" для секции "Витильные параметры".</returns>
        private static XElement GenerateVitalParametersSectionEntryElement(
            string codeElementCode,
            string codeElementDisplayName,
            string effectiveTimeValue,
            string referenceValue,
            string valueType,
            string valueValue,
            string valueUnit = null)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement organizerElement = new XElement(xmlnsNamespace + "organizer",
                new XAttribute("classCode", "CLUSTER"),
                new XAttribute("moodCode", "EVN"));

            XElement statusCodeElement = new XElement(xmlnsNamespace + "statusCode",
                new XAttribute("code", "completed"));
            organizerElement.Add(statusCodeElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime",
                new XAttribute("value", effectiveTimeValue));
            organizerElement.Add(effectiveTimeElement);

            XElement componentElement = new XElement(xmlnsNamespace + "component",
                new XAttribute("typeCode", "COMP"));
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));
            
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeElementCode,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.262",
                    codeSystemVersionValue: "3.3",
                    codeSystemNameValue: "Витальные параметры",
                    displayNameValue: codeElementDisplayName));

            XElement originalTextElement = new XElement(xmlnsNamespace + "originalText");
            XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                new XAttribute("value", referenceValue));
            originalTextElement.Add(referenceElement);
            codeElement.Add(originalTextElement);
            observationElement.Add(codeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", valueType),
                new XAttribute("value", valueValue));
            if (valueUnit != null)
            {
                valueElement.Add(new XAttribute("unit", valueUnit));
            }
            observationElement.Add(valueElement);

            componentElement.Add(observationElement);
            organizerElement.Add(componentElement);

            entryElement.Add(organizerElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "entry" для секции "Витильные параметры" (легкая разметка).
        /// </summary>
        /// <param name="codeElementName">Наименование элемента "code".</param>
        /// <param name="valueElementModel">Модель элемента "value".</param>
        /// <returns>Элемент "entry" для секции "Витильные параметры" (легкая разметка).</returns>
        private static XElement GenerateVitalParametersSectionEntryElement_light(string codeElementName, ValueElementModel valueElementModel)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            var codeValue = GetCodeValue(codeElementName);
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue,
                    displayNameValue: codeValue.displayNameValue));
            observationElement.Add(codeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", valueElementModel.Type),
                new XAttribute("code", valueElementModel.Code),
                new XAttribute("codeSystem", valueElementModel.CodeSystem),
                new XAttribute("codeSystemVersion", valueElementModel.CodeSystemVersion),
                new XAttribute("codeSystemName", valueElementModel.CodeSystemName),
                new XAttribute("displayName", valueElementModel.DisplayName));
            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            return entryElement;
        }

        #endregion
        
        #region Direction state

        /// <summary>
        /// Генерирование секции "Состояние при направлении".
        /// </summary>
        /// <param name="directionStateSectionModel">Модель секции "Сосотояние при направлении".</param>
        /// <returns>Элемент "component" секции "Состояние при направлении".</returns>
        private static XElement GenerateDirectionStateSectionElement(DirectionStateSectionModel directionStateSectionModel)
        {
            if (directionStateSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "STATECUR"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Текущее состояние"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "СОСТОЯНИЕ ПРИ НАПРАВЛЕНИИ");
            sectionElement.Add(titleElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph");

            XElement captionElement = new XElement(xmlnsNamespace + "caption",
                "Состояние здоровья гражданина при направлении на медико-социальную экспертизу");
            paragraphElement.Add(captionElement);
            XElement contentElement = new XElement(xmlnsNamespace + "content", directionStateSectionModel.StateText);
            paragraphElement.Add(contentElement);

            textElement.Add(paragraphElement);
            sectionElement.Add(textElement);

            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement observationCodeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "4109"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName",
                    "Состояние здоровья гражданина при направлении на медико-социальную экспертизу"));
            observationElement.Add(observationCodeElement);

            XElement observationValueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), directionStateSectionModel.StateText);
            observationElement.Add(observationValueElement);

            entryElement.Add(observationElement);
            sectionElement.Add(entryElement);

            componentElement.Add(sectionElement);
            return componentElement;
        }

        #endregion

        #region Diagnostic Studies

        /// <summary>
        /// Создает элементы секции "Диагностические исследования и консультации".
        /// </summary>
        /// <param name="diagnosticStudiesSectionModel">Модель секции "Диагностические исследования и результаты".</param>
        /// <returns>Элемент "component" секции 2Диагностические исследования и консультации".</returns>
        private static XElement GenerateDiagnosticStudiesSectionElement(DiagnosticStudiesSectionModel diagnosticStudiesSectionModel)
        {
            if (diagnosticStudiesSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "PROC"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Исследования и процедуры"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "МЕДИЦИНСКИЕ ОБСЛЕДОВАНИЯ");
            sectionElement.Add(titleElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");

            textElement.Add(GenerateTableDiagnosticStudiesSection(diagnosticStudiesSectionModel));

            sectionElement.Add(textElement);

            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observatinElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeObservationElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "4110"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", "Сведения о медицинских обследованиях, необходимых для получения клинико-функциональных данных в зависимости от заболевания при проведении медико-социальной экспертизы"));
            observatinElement.Add(codeObservationElement);

            if (diagnosticStudiesSectionModel.MedicalExaminations != null)
            {
                foreach (var medicalExamination in diagnosticStudiesSectionModel.MedicalExaminations)
                {
                    observatinElement.Add(GenerateEntryReletionshipElementDiagnosticStudiesSection(
                        medicalExamination.Date.ToString("yyyyMMddHHmm+0300"),
                        medicalExamination.Number,
                        medicalExamination.Name,
                        medicalExamination.Result,
                        medicalExamination.ID,
                        medicalExamination.Code));
                }
            }

            entryElement.Add(observatinElement);
            sectionElement.Add(entryElement);

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает табличную часть для секции "Диагностические исследования и консультации".
        /// </summary>
        /// <param name="diagnosticStudiesSectionModel">Модель секции "Диагностические исследования и результаты".</param>
        /// <returns>Табличную часть секции "Диагностические исследования и консультации".</returns>
        private static XElement GenerateTableDiagnosticStudiesSection(DiagnosticStudiesSectionModel diagnosticStudiesSectionModel)
        {
            if (diagnosticStudiesSectionModel == null)
            {
                return null;
            }
            
            XElement tableElement = new XElement(xmlnsNamespace + "table");

            XElement captionElement = new XElement(xmlnsNamespace + "caption",
                "Сведения о медицинских обследованиях, необходимых для получения клинико-функциональных данных в зависимости от заболевания при проведении медико-социальной экспертизы:");
            tableElement.Add(captionElement);
            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

            tbodyElement.Add(GenerateTRElementTableDiagnosticStudiesSection("Дата", "Код", "Наименование", "Результат", true));

            if (diagnosticStudiesSectionModel.MedicalExaminations != null)
            {
                foreach (var medicalExamination in diagnosticStudiesSectionModel.MedicalExaminations)
                {
                    tbodyElement.Add(GenerateTRElementTableDiagnosticStudiesSection(
                        medicalExamination.Date.ToString("dd.MM.yy"),
                        medicalExamination.Number,
                        medicalExamination.Name,
                        medicalExamination.Result));
                }
            }

            tableElement.Add(tbodyElement);
            return tableElement;
        }

        /// <summary>
        /// Генерирование элементов заполнения таблицы секции "Диагностические исследования и консультации".
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <param name="code">Код.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="result">Результат.</param>
        /// <param name="isHeader">Истина - заголовок таблицы.</param>
        /// <returns>Элементы заполнения таблицы секции "Диагностические исследования и консультации".</returns>
        private static XElement GenerateTRElementTableDiagnosticStudiesSection(
            string date, 
            string code, 
            string name,
            string result,
            bool isHeader = false)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");

            string childElementTag;
            if (isHeader)
            {
                childElementTag = "th";
            }
            else
            {
                childElementTag = "td";
            }

            XElement childElementDate = new XElement(xmlnsNamespace + childElementTag, date);
            trElement.Add(childElementDate);

            XElement childElementCode = new XElement(xmlnsNamespace + childElementTag, code);
            trElement.Add(childElementCode);

            XElement childElementName = new XElement(xmlnsNamespace + childElementTag, name);
            trElement.Add(childElementName);

            XElement childElementResult = new XElement(xmlnsNamespace + childElementTag, result);
            trElement.Add(childElementResult);

            return trElement;
        }

        /// <summary>
        /// Создает элемент кодирования секции "Диагностические исследования и консультации".
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <param name="number">Номер.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="result">Результат.</param>
        /// <param name="id">Идентификатор.</param>
        /// <param name="code">Код.</param>
        /// <returns>Элемент кодирования секции "Диагностические исследования и консультации".</returns>
        private static XElement GenerateEntryReletionshipElementDiagnosticStudiesSection(
            string date,
            string number,
            string name,
            string result,
            string id,
            string code)
        {
            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", code),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.857"),
                new XAttribute("codeSystemVersion", "1.2"),
                new XAttribute("codeSystemName", "Медицинские обследования для медико-социальной экспертизы"),
                new XAttribute("displayName", $"{number} {name}"));
            observationElement.Add(codeElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime",
                new XAttribute("value", date));
            observationElement.Add(effectiveTimeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), result);
            observationElement.Add(valueElement);

            XElement referenceElement;
            if (id is not null)
            {
                referenceElement = new XElement(xmlnsNamespace + "reference",
                    new XAttribute("typeCode", "REFR"));

                XElement externalDocument = new XElement(xmlnsNamespace + "externalDocument",
                    new XAttribute("classCode", "DOCCLIN"),
                    new XAttribute("moodCode", "EVN"));

                XElement idElement = new XElement(xmlnsNamespace + "id",
                    new XAttribute("root", "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.51"),
                    new XAttribute("extension", id));
                externalDocument.Add(idElement);

                referenceElement.Add(externalDocument);
            }
            else
            {
                referenceElement = new XElement(xmlnsNamespace + "reference",
                    new XAttribute("nullFlavor", "NI"));
            }
            
            observationElement.Add(referenceElement);

            entryRelationshipElement.Add(observationElement);
            return entryRelationshipElement;
        }

        #endregion

        #region Diagnosis sections

        /// <summary>
        /// Генерирование элемента "component" секции "Диагнозы".
        /// </summary>
        /// <param name="diagnosisSectionModel">Модель секции "Диагнозы".</param>
        /// <returns>"лемент "component" секции "Диагнозы".</returns>
        private static XElement GenerateDiagnosisSection(DiagnosisSectionModel diagnosisSectionModel)
        {
            if (diagnosisSectionModel == null)
            {
                return null;
            }
            
            XElement componentModel = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "DGN"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Диагнозы"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "ДИАГНОЗЫ");
            sectionElement.Add(titleElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");

            textElement.Add(GenerateTableDiagnosisSection(diagnosisSectionModel));

            sectionElement.Add(textElement);

            XElement entryelement = new XElement(xmlnsNamespace + "entry");
            XElement actElement = new XElement(xmlnsNamespace + "act",
                new XAttribute("classCode", "ACT"),
                new XAttribute("moodCode", "EVN"));

            XElement codeActElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "3"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.795"),
                new XAttribute("codeSystemVersion", "2.1"),
                new XAttribute("codeSystemName", "Степень обоснованности диагноза"),
                new XAttribute("displayName", "Заключительный клинический диагноз"));
            actElement.Add(codeActElement);

            if (diagnosisSectionModel.Diagnosis != null)
            {
                foreach (var diagnos in diagnosisSectionModel.Diagnosis)
                {
                    actElement.Add(GenerateCodingElementDiagnosisSection(
                        diagnos.Code,
                        diagnos.Caption,
                        diagnos.Name,
                        diagnos.Result,
                        diagnos.ID));
                }
            }

            entryelement.Add(actElement);
            sectionElement.Add(entryelement);

            componentModel.Add(sectionElement);
            return componentModel;
        }

        /// <summary>
        /// Создает табличную часть для секции "Диагнозы".
        /// </summary>
        /// <param name="diagnosisSectionModel">Модель секции "Диагнозы".</param>
        /// <returns>Табличную часть секции "Диагнозы".</returns>
        private static XElement GenerateTableDiagnosisSection(DiagnosisSectionModel diagnosisSectionModel)
        {
            if (diagnosisSectionModel == null)
            {
                return null;
            }
            
            XElement tableElement = new XElement(xmlnsNamespace + "table");

            XElement captionElement = new XElement(xmlnsNamespace + "caption",
                "Диагноз при направлении на медико-социальную экспертизу:");
            tableElement.Add(captionElement);
            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

            tbodyElement.Add(GenerateTRElementTableDiagnosisSection("Шифр", "Тип", "Текст", true));

            if (diagnosisSectionModel.Diagnosis != null)
            {
                foreach (var diagnos in diagnosisSectionModel.Diagnosis)
                {
                    tbodyElement.Add(GenerateTRElementTableDiagnosisSection(
                        diagnos.ID,
                        diagnos.Caption,
                        diagnos.Result));
                }
            }

            tableElement.Add(tbodyElement);
            return tableElement;
        }

        /// <summary>
        /// Генерирование элементов заполнения таблицы секции "Диагнозы".
        /// </summary>
        /// <param name="cipher">Шифр.</param>
        /// <param name="type">Тип.</param>
        /// <param name="text">Текст.</param>
        /// <param name="isHeader">Истина - заголовок таблицы.</param>
        /// <returns>Элементы заполнения таблицы секции "Диагнозы".</returns>
        private static XElement GenerateTRElementTableDiagnosisSection(
            string cipher,
            string type,
            string text,
            bool isHeader = false)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");

            string childElementTag;
            if (isHeader)
            {
                childElementTag = "th";
            }
            else
            {
                childElementTag = "td";
            }

            XElement childElementChiper = new XElement(xmlnsNamespace + childElementTag, cipher);
            trElement.Add(childElementChiper);

            XElement childElementType = new XElement(xmlnsNamespace + childElementTag, type);
            trElement.Add(childElementType);

            XElement childElementText = new XElement(xmlnsNamespace + childElementTag, text);
            trElement.Add(childElementText);

            return trElement;
        }

        /// <summary>
        /// Генерирование элементов кодирования элементов секции "Диагнозы".
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="caption">Описание.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="result">Результат диагноза.</param>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Элементы кодирования элементов секции "Диагнозы".</returns>
        private static XElement GenerateCodingElementDiagnosisSection(
            string code,
            string caption,
            string name,
            string result,
            string id)
        {
            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", code),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1077"),
                new XAttribute("codeSystemVersion", "1.3"),
                new XAttribute("codeSystemName", "Виды нозологических единиц диагноза"),
                new XAttribute("displayName", caption));
            observationElement.Add(codeElement);

            XElement textElement = new XElement(xmlnsNamespace + "text", result);
            observationElement.Add(textElement);

            XElement statusCodeElement = new XElement(xmlnsNamespace + "statusCode",
                new XAttribute("code", "completed"));
            observationElement.Add(statusCodeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "CD"),
                new XAttribute("code", id),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1005"),
                new XAttribute("codeSystemVersion", "2.19"),
                new XAttribute("codeSystemName", "Международная статистическая классификация болезней и проблем, связанных со здоровьем (10-й пересмотр)"),
                new XAttribute("displayName", name));
            observationElement.Add(valueElement);

            entryRelationshipElement.Add(observationElement);
            return entryRelationshipElement;
        }

        #endregion

        #region Condition Assessment

        /// <summary>
        /// Генерирование элемента "component" секции "Объектизированная оцента состояния".
        /// </summary>
        /// <param name="conditionAssessmentSection">Модель секции "Объектизированная оцента состояния".</param>
        /// <returns>Элемент "component" секции "Объектизированная оцента состояния".</returns>
        private static XElement GenerateConditionAssessmentSectionElement(ConditionAssessmentSectionModel conditionAssessmentSection)
        {
            if (conditionAssessmentSection == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "SCORES"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Объективизированная оценка состояния больного"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "ОБЪЕКТИВИЗИРОВАННАЯ ОЦЕНКА СОСТОЯНИЯ");
            sectionElement.Add(titleElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");

            textElement.Add(GenerateTableConditionAssessmentSection(conditionAssessmentSection));

            sectionElement.Add(textElement);

            if (conditionAssessmentSection.ClinicalPrognosis != null)
            {
                sectionElement.Add(GenerateCodingElementConditionAssessmentSection(
                    conditionAssessmentSection.ClinicalPrognosis.GrateType,
                    conditionAssessmentSection.ClinicalPrognosis.GrateResult,
                    "#ob_1",
                    "4054",
                    "11"));
            }

            if (conditionAssessmentSection.RehabilitationPotential != null)
            {
                sectionElement.Add(GenerateCodingElementConditionAssessmentSection(
                    conditionAssessmentSection.RehabilitationPotential.GrateType,
                    conditionAssessmentSection.RehabilitationPotential.GrateResult,
                    "#ob_2",
                    "4055",
                    "15"));
            }

            if (conditionAssessmentSection.RehabilitationPrognosis != null)
            {
                sectionElement.Add(GenerateCodingElementConditionAssessmentSection(
                    conditionAssessmentSection.RehabilitationPrognosis.GrateType,
                    conditionAssessmentSection.RehabilitationPrognosis.GrateResult,
                    "#ob_3",
                    "4056",
                    "19"));
            }

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает табличную часть для секции "Объектизированная оцента состояния".
        /// </summary>
        /// <param name="conditionAssessmentSection">Модель секции "Объектизированная оцента состояния".</param>
        /// <returns>Табличную часть секции "Объектизированная оцента состояния".</returns>
        private static XElement GenerateTableConditionAssessmentSection(ConditionAssessmentSectionModel conditionAssessmentSection)
        {
            if (conditionAssessmentSection == null)
            {
                return null;
            }
            
            XElement tableElement = new XElement(xmlnsNamespace + "table");
            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");
            XElement theadElement = new XElement(xmlnsNamespace + "thead");

            theadElement.Add(GenerateTRElementTableConditionAssessmentSection("Тип оценки", "Результат", null, true));

            tableElement.Add(theadElement);

            if (conditionAssessmentSection.ClinicalPrognosis != null)
            {
                tbodyElement.Add(GenerateTRElementTableConditionAssessmentSection(
                    conditionAssessmentSection.ClinicalPrognosis.GrateType,
                    conditionAssessmentSection.ClinicalPrognosis.GrateResult,
                    "ob_1"));
            }

            if (conditionAssessmentSection.RehabilitationPotential != null)
            {
                tbodyElement.Add(GenerateTRElementTableConditionAssessmentSection(
                    conditionAssessmentSection.RehabilitationPotential.GrateType,
                    conditionAssessmentSection.RehabilitationPotential.GrateResult,
                    "ob_2"));
            }

            if (conditionAssessmentSection.RehabilitationPrognosis != null)
            {
                tbodyElement.Add(GenerateTRElementTableConditionAssessmentSection(
                    conditionAssessmentSection.RehabilitationPrognosis.GrateType,
                    conditionAssessmentSection.RehabilitationPrognosis.GrateResult,
                    "ob_3"));
            }

            tableElement.Add(tbodyElement);

            return tableElement;
        }

        /// <summary>
        /// Генерирование элементов заполнения таблицы секции "Объектизированная оцента состояния".
        /// </summary>
        /// <param name="cipher">Шифр.</param>
        /// <param name="type">Тип.</param>
        /// <param name="text">Текст.</param>
        /// <param name="isHeader">Истина - заголовок таблицы.</param>
        /// <returns>Элементы заполнения таблицы секции "Объектизированная оцента состояния".</returns>
        private static XElement GenerateTRElementTableConditionAssessmentSection(
            string type,
            string result,
            string reference,
            bool isHeader = false)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");

            string childElementTag;
            if (isHeader)
            {
                childElementTag = "th";
            }
            else
            {
                childElementTag = "td";
            }

            XElement childElementType = new XElement(xmlnsNamespace + childElementTag, type);
            trElement.Add(childElementType);

            if (isHeader)
            {
                XElement childElementResult = new XElement(xmlnsNamespace + childElementTag, result);
                trElement.Add(childElementResult);
            }
            else
            {
                XElement childElementResult = new XElement(xmlnsNamespace + childElementTag);

                XElement contentElement = new XElement(xmlnsNamespace + "content",
                    new XAttribute("ID", reference), result);
                childElementResult.Add(contentElement);

                trElement.Add(childElementResult);
            }


            return trElement;
        }

        /// <summary>
        /// Генерирование элементов кодирования элементов секции "Объектизированная оцента состояния".
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="caption">Описание.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="result">Результат диагноза.</param>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Элементы кодирования элементов секции "Объектизированная оцента состояния".</returns>
        private static XElement GenerateCodingElementConditionAssessmentSection(
            string caption,
            string result,
            string reference,
            string codeCode,
            string codeValue)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", codeCode),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", caption));

            XElement originalTextElement = new XElement(xmlnsNamespace + "originalText");

            XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                new XAttribute("value", reference));
            originalTextElement.Add(referenceElement);

            codeElement.Add(originalTextElement);

            observationElement.Add(codeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "CD"),
                new XAttribute("code", codeValue),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.148"),
                new XAttribute("codeSystemVersion", "1.3"),
                new XAttribute("codeSystemName", "Оценки состояния для медико-социальной экспертизы"),
                new XAttribute("displayName", $"{caption} {result.ToLower()}"));
            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            return entryElement;
        }

        #endregion

        #region Recomendations

        /// <summary>
        /// Создает элемент "component" секции "Рекомендации".
        /// </summary>
        /// <param name="recommendationsSectionModel">Модель секции "Рекомендации".</param>
        /// <returns>Элемент "component" секции "Рекомендации".</returns>
        private static XElement GenerateRecommendationSectionElement(RecommendationsSectionModel recommendationsSectionModel)
        {
            if (recommendationsSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "REGIME"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Режим и рекомендации"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "РЕКОМЕНДАЦИИ");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateCommonRecommendationsSectionElement(recommendationsSectionModel));
            sectionElement.Add(GenerateOtherRecommendationsSectionElement(recommendationsSectionModel.OtherRecommendatons));

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент "основные рекомендации" для секции "Рекомендации".
        /// </summary>
        /// <param name="recommendationsSectionModel">Модель секции "Рекомендации".</param>
        /// <returns>Элемент "основные рекомендации" для секции "Рекомендации".</returns>
        private static XElement GenerateCommonRecommendationsSectionElement(RecommendationsSectionModel recommendationsSectionModel)
        {
            if (recommendationsSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "RECTREAT"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Рекомендованное лечение"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "Рекомендованное лечение");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateTableCommonRecommendationSectionElement(recommendationsSectionModel));

            sectionElement.Add(GenerateStandartCodingCommonRecomendationSectionElement(
                "4111",
                "Рекомендуемые мероприятия по реконструктивной хирургии",
                recommendationsSectionModel.RecommendedMeasuresReconstructiveSurgery));
            sectionElement.Add(GenerateStandartCodingCommonRecomendationSectionElement(
                "4112",
                "Рекомендуемые мероприятия по протезированию и ортезированию, техническим средствам реабилитации",
                recommendationsSectionModel.RecommendedMeasuresProstheticsAndOrthotics));
            sectionElement.Add(GenerateStandartCodingCommonRecomendationSectionElement(
                "4113",
                "Санаторно-курортное лечение",
                recommendationsSectionModel.SpaTreatment));

            if (recommendationsSectionModel.Medications != null)
            {
                foreach (var medic in recommendationsSectionModel.Medications)
                {
                    sectionElement.Add(GenerateMedicalCodingRecomendationSectionElement(medic));
                }
            }

            sectionElement.Add(GenerateStandartCodingCommonRecomendationSectionElement(
                "12132",
                "Перечень медицинских изделий для медицинского применения",
                recommendationsSectionModel.MedicalDevices,
                false));

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент таблицы для наполнения "основные рекомендации" секции "Рекомендации".
        /// </summary>
        /// <param name="recommendationsSectionModel">Модель секции "Рекомендации".</param>
        /// <returns>Элемент таблицы для наполнения "основные рекомендации" секции "Рекомендации".</returns>
        private static XElement GenerateTableCommonRecommendationSectionElement(RecommendationsSectionModel recommendationsSectionModel)
        {
            if (recommendationsSectionModel == null)
            {
                return null;
            }
            
            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement tableElement = new XElement(xmlnsNamespace + "table");
            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

            tbodyElement.Add(GenerateTRTableCommonRecommendationSectionElement("Рекомендуемые мероприятия по реконструктивной хирургии",
                recommendationsSectionModel.RecommendedMeasuresReconstructiveSurgery));
            tbodyElement.Add(GenerateTRTableCommonRecommendationSectionElement("Рекомендуемые мероприятия по протезированию и ортезированию, техническим средствам реабилитации",
                recommendationsSectionModel.RecommendedMeasuresProstheticsAndOrthotics));
            tbodyElement.Add(GenerateTRTableCommonRecommendationSectionElement("Санаторно-курортное лечение",
                recommendationsSectionModel.SpaTreatment));
            tbodyElement.Add(GenerateTRTableMedicalRecommendationSectionElement(
                "Перечень лекарственных препаратов для медицинского применения (заполняется в отношении граждан, пострадавших в результате несчастных случаев на производстве и профессиональных заболеваний)",
                recommendationsSectionModel.Medications));
            tbodyElement.Add(GenerateTRTableCommonRecommendationSectionElement("Перечень медицинских изделий для медицинского применения",
            recommendationsSectionModel.MedicalDevices));

            tableElement.Add(tbodyElement);
            textElement.Add(tableElement);
            return textElement;
        }

        /// <summary>
        /// Создание элемента таблицы заполнения секции.
        /// </summary>
        /// <param name="caption">Описание.</param>
        /// <param name="content">Контент.</param>
        /// <returns>Элемента таблицы заполнения секции.</returns>
        private static XElement GenerateTRTableCommonRecommendationSectionElement(string caption, string content)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");

            XElement tdElement = new XElement(xmlnsNamespace + "td");
            XElement contentElement = new XElement(xmlnsNamespace + "content", caption);
            tdElement.Add(contentElement);
            trElement.Add(tdElement);

            tdElement = new XElement(xmlnsNamespace + "td");
            contentElement = new XElement(xmlnsNamespace + "content", content);
            tdElement.Add(contentElement);
            trElement.Add(tdElement);

            return trElement;
        }

        /// <summary>
        /// Создание элемента таблицы, отвечающего за препараты.
        /// </summary>
        /// <param name="caption">Описание.</param>
        /// <param name="medications">Список препаратов.</param>
        /// <returns>Элемент таблицы, отвечающий за препараты</returns>
        private static XElement GenerateTRTableMedicalRecommendationSectionElement(string caption, List<MedicationModel> medications)
        {
            if (medications == null)
            {
                return null;
            }
            
            XElement trElement = new XElement(xmlnsNamespace + "tr");
            XElement tdCaptionElement = new XElement(xmlnsNamespace + "td");
            XElement captionContentElement = new XElement(xmlnsNamespace + "content", caption);
            tdCaptionElement.Add(captionContentElement);
            trElement.Add(tdCaptionElement);
            
            foreach (var medic in medications)
            {
                XElement tdMedicalElement = new XElement(xmlnsNamespace + "td");
                XElement medicalContentElement = new XElement(xmlnsNamespace + "content");

                medicalContentElement.Add("Международное название:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.InternationalName);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Лекарственная форма:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.DosageForm);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Лекарственная доза:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.Dose);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Код КТРУ:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.KTRUCode);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Продолжительность приема:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.DurationAdmission);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Кратность курсов лечения:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.MultiplicityCoursesTreatment);
                medicalContentElement.Add(NewLineElement);

                medicalContentElement.Add("Кратность приема:");
                medicalContentElement.Add(NewLineElement);
                medicalContentElement.Add(medic.ReceptionFrequency);

                tdMedicalElement.Add(medicalContentElement);
                trElement.Add(tdMedicalElement);
                tdMedicalElement.Add(NewLineElement);
            }

            return trElement;
        }

        /// <summary>
        /// Создает стандартный блок кодирования основных рекомендации секции "Рекомендации".
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="caption">Наименование рекомендации.</param>
        /// <param name="text">Описание рекомендации.</param>
        /// <returns>Стандартный блок кодирования основных рекомендации секции "Рекомендации".</returns>
        private static XElement GenerateStandartCodingCommonRecomendationSectionElement(
            string code,
            string caption,
            string text,
            bool isColaborationCaptionAndText = true)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", code),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", caption));
            observationElement.Add(codeElement);

            var valueText = text;
            if (isColaborationCaptionAndText)
            {
                valueText = $"{caption} {text.ToLower()}";
            }

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), valueText);
            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "entry" для лекарствнного средства раздела "основные рекомендации" секции "Рекомендации".
        /// </summary>
        /// <param name="medicationModel">Модель лекартсвенного средства.</param>
        /// <returns>Элемент "entry" для лекарствнного средства раздела "основные рекомендации" секции "Рекомендации".</returns>
        private static XElement GenerateMedicalCodingRecomendationSectionElement(MedicationModel medicationModel)
        {
            if (medicationModel == null)
            {
                return null;
            }
            
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement substanceAdministrationElement = new XElement(xmlnsNamespace + "substanceAdministration",
                new XAttribute("classCode", "SBADM"),
                new XAttribute("moodCode", "RQO"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "12131"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", "Перечень лекарственных препаратов для медицинского применения"));
            substanceAdministrationElement.Add(codeElement);

            #region consumable element

            XElement consumableElement = new XElement(xmlnsNamespace + "consumable",
                new XAttribute("typeCode", "CSM"));
            XElement manufacturedProductElement = new XElement(xmlnsNamespace + "manufacturedProduct",
                new XAttribute("classCode", "MANU"));
            XElement manufacturedMaterialElement = new XElement(xmlnsNamespace + "manufacturedMaterial",
                new XAttribute("classCode", "MMAT"),
                new XAttribute("determinerCode", "KIND"));

            XElement manufacturedMaterialCodeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", medicationModel.KTRUCode),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.611"),
                new XAttribute("codeSystemVersion", "4.29"),
                new XAttribute("codeSystemName", "Узлы СМНН. ЕСКЛП"),
                new XAttribute("displayName", medicationModel.KTRUName));
            manufacturedMaterialElement.Add(manufacturedMaterialCodeElement);

            manufacturedProductElement.Add(manufacturedMaterialElement);
            consumableElement.Add(manufacturedProductElement);
            substanceAdministrationElement.Add(consumableElement);

            #endregion

            #region entryRelationship elements

            substanceAdministrationElement.Add(GenerateEntryRelationshipRecommendationsSectionElement(
                "12182",
                "Продолжительность приема",
                medicationModel.DurationAdmission));
            substanceAdministrationElement.Add(GenerateEntryRelationshipRecommendationsSectionElement(
                "12183",
                "Кратность курсов лечения",
                medicationModel.MultiplicityCoursesTreatment));
            substanceAdministrationElement.Add(GenerateEntryRelationshipRecommendationsSectionElement(
                "12184",
                "Количество приема",
                medicationModel.ReceptionFrequency));

            #endregion

            entryElement.Add(substanceAdministrationElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "entryRelationship" для блока "основные рекомендации" секции "Рекомендации".
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="displayName">Отображаемое имя.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Элемент "entryRelationship" для блока "основные рекомендации" секции "Рекомендации".</returns>
        private static XElement GenerateEntryRelationshipRecommendationsSectionElement(
            string code,
            string displayName,
            string value)
        {
            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "RQO"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", code),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", displayName));
            observationElement.Add(codeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), value);
            observationElement.Add(valueElement);

            entryRelationshipElement.Add(observationElement);
            return entryRelationshipElement;
        }

        /// <summary>
        /// Создает элемент "component" для "дополнительные рекомендации" секции "Рекомендации".
        /// </summary>
        /// <param name="otherRecommendations">Текст дополнительных рекомендаций.</param>
        /// <returns>Элемент "component" для "дополнительные рекомендации" секции "Рекомендации".</returns>
        private static XElement GenerateOtherRecommendationsSectionElement(string otherRecommendations)
        {
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "RECOTHER"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Прочие рекомендации"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "Прочие рекомендации");
            sectionElement.Add(titleElement);

            #region text element

            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph");

            XElement captionElement = new XElement(xmlnsNamespace + "caption",
                "Рекомендуемые мероприятия по медицинской реабилитации");
            paragraphElement.Add(captionElement, otherRecommendations);

            textElement.Add(paragraphElement);
            sectionElement.Add(textElement);

            #endregion

            #region entry element

            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement observationCodeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "4114"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", "Рекомендуемые мероприятия по медицинской реабилитации"));
            observationElement.Add(observationCodeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), otherRecommendations);
            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            sectionElement.Add(entryElement);

            #endregion

            componentElement.Add(sectionElement);
            return componentElement;
        }

        #endregion

        #region OutsideSpecialMedicalCare

        /// <summary>
        /// Создается секция "Посторонний специальный медицинский уход".
        /// </summary>
        /// <param name="outsideSpecialMedicalCareSectionModel">Модель секции "Посторонний специальный медицинский уход".</param>
        /// <returns>Секция "Посторонний специальный медицинский уход".</returns>
        private static XElement GenerateOutsideSpecialMedicalCareSection(OutsideSpecialMedicalCareSectionModel outsideSpecialMedicalCareSectionModel)
        {
            if (outsideSpecialMedicalCareSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "OUTSPECMEDCARE"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Посторонний специальный медицинский уход"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "ПОСТОРОННИЙ СПЕЦИАЛЬНЫЙ МЕДИЦИНСКИЙ УХОД");
            sectionElement.Add(titleElement);

            #region Text element

            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph", outsideSpecialMedicalCareSectionModel.Text);
            textElement.Add(paragraphElement);
            sectionElement.Add(textElement);

            #endregion

            #region entry element

            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement observationCodeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "12133"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.166"),
                new XAttribute("codeSystemVersion", "1.69"),
                new XAttribute("codeSystemName", "Кодируемые поля CDA документов"),
                new XAttribute("displayName", "Посторонний специальный медицинский уход"));
            observationElement.Add(observationCodeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "ST"), outsideSpecialMedicalCareSectionModel.Text);
            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            sectionElement.Add(entryElement);

            #endregion

            componentElement.Add(sectionElement);
            return componentElement;
        }

        #endregion

        #region AttachmentDocuments

        /// <summary>
        /// Создает элемент "component" секции "Связанные документы".
        /// </summary>
        /// <param name="attachmentDocumentsSectionModel">Модель секции "Связанные документы".</param>
        /// <returns>Элемент "component" секции "Связанные документы".</returns>
        private static XElement GenerateAttachmentDocumentsSectionElement(AttachmentDocumentsSectionModel attachmentDocumentsSectionModel)
        {
            if (attachmentDocumentsSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "LINKDOCS"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.197"),
                new XAttribute("codeSystemVersion", "1.18"),
                new XAttribute("codeSystemName", "Секции электронных медицинских документов"),
                new XAttribute("displayName", "Связанные документы"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "Связанные документы");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateTableAttachmentDocumentsSectionElement(attachmentDocumentsSectionModel));

            if (attachmentDocumentsSectionModel.AttachmentDocuments != null)
            {
                foreach (var document in attachmentDocumentsSectionModel.AttachmentDocuments)
                {
                    sectionElement.Add(GenerateEntryAttachmentDocumentsSectionElement(document));
                }
            }

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент "text" секции "Связанные документы".
        /// </summary>
        /// <param name="attachmentDocumentsSectionModel">Модель секции "Связанные документы".</param>
        /// <returns>Элемент "text" секции "Связанные документы".</returns>
        private static XElement GenerateTableAttachmentDocumentsSectionElement(AttachmentDocumentsSectionModel attachmentDocumentsSectionModel)
        {
            if (attachmentDocumentsSectionModel == null)
            {
                return null;
            }
            
            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement tableElement = new XElement(xmlnsNamespace + "table");
            XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

            if (attachmentDocumentsSectionModel.AttachmentDocuments != null)
            {
                foreach (var document in attachmentDocumentsSectionModel.AttachmentDocuments)
                {
                    tbodyElement.Add(GenerateTRTableAttachmentDocumentsSectionElement(document.Name, document.Result));
                }
            }

            tableElement.Add(tbodyElement);
            textElement.Add(tableElement);
            return textElement;
        }

        /// <summary>
        /// Создает элемент "tr" таблицы секции "Связанные документы".
        /// </summary>
        /// <param name="caption">Описание (значение элемента "th").</param>
        /// <param name="context">Значение (значение элемента "td").</param>
        /// <returns>Элемент "tr" таблицы секции "Связанные документы".</returns>
        private static XElement GenerateTRTableAttachmentDocumentsSectionElement(string caption, string context)
        {
            XElement trElement = new XElement(xmlnsNamespace + "tr");
            
            XElement thElement = new XElement(xmlnsNamespace + "th", caption);
            trElement.Add(thElement);

            XElement tdElement = new XElement(xmlnsNamespace + "td", context);
            trElement.Add(tdElement);
            
            return trElement;
        }

        /// <summary>
        /// Создает элемент "entry" блока кодирования секции "Связанные документы".
        /// </summary>
        /// <param name="medicalDocumentModel">Модель связанного докумнта.</param>
        /// <returns>Элемент "entry" блока кодирования секции "Связанные документы".</returns>
        private static XElement GenerateEntryAttachmentDocumentsSectionElement(MedicalDocumentModel medicalDocumentModel)
        {
            if (medicalDocumentModel == null)
            {
                return null;
            }
            
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement actElement = new XElement(xmlnsNamespace + "act",
                new XAttribute("classCode", "ACT"),
                new XAttribute("moodCode", "EVN"));
            
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "6"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1522"),
                new XAttribute("codeSystemVersion", "4.45"),
                new XAttribute("codeSystemName", "Виды медицинской документации"),
                new XAttribute("displayName", "Протокол инструментального исследования"));
            actElement.Add(codeElement);

            XElement textElement = new XElement(xmlnsNamespace + "text",
                $"{medicalDocumentModel.Name}: {medicalDocumentModel.Result}");
            actElement.Add(textElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime",
                new XAttribute("value", medicalDocumentModel.Created.ToString("yyyyMMddHHmm+0300")));
            actElement.Add(effectiveTimeElement);

            XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                new XAttribute("typeCode", "REFR"));
            XElement externalDocumentElement = new XElement(xmlnsNamespace + "externalDocument",
                new XAttribute("classCode", "DOCCLIN"),
                new XAttribute("moodCode", "EVN"));

            XElement idElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("root", medicalDocumentModel.ReferenceRoot),
                new XAttribute("extension", medicalDocumentModel.ReferenceExtension));
            externalDocumentElement.Add(idElement);
            
            referenceElement.Add(externalDocumentElement);
            actElement.Add(referenceElement);
            
            entryElement.Add(actElement);
            return entryElement;
        }
        
        #endregion

        /// <summary>
        /// Создает элемент "component" с наполнением секции "Направление".
        /// </summary>
        /// <param name="sentSectionModel">Модель секции "Направление".</param>
        /// <returns>Элемент "component" с наполнением секции "Направление".</returns>
        private static XElement GenerateSentSectionElement(SentSectionModel sentSectionModel)
        {
            if (sentSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: sentSectionModel.Code.Code,
                    codeSystemVersionValue: sentSectionModel.Code.CodeSystemVersion,
                    displayNameValue: sentSectionModel.Code.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.197",
                    codeSystemNameValue: "Секции электронных медицинских документов"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "НАПРАВЛЕНИЕ");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateParagraphsElements(sentSectionModel.SentParagraphs));

            sectionElement.Add(GenerateTargetSentElement(sentSectionModel.TargetSent));

            sectionElement.Add(GenerateEntryRelationshipElement("citizenship", code: sentSectionModel.Сitizenship, isEntryElement: true));

            sectionElement.Add(GeneratePatienLocationElement(sentSectionModel.PatientLocationCode, sentSectionModel.PatientLocation));

            sectionElement.Add(GenerateEntryRelationshipElement("militaryDuty", code: sentSectionModel.MilitaryDuty, isEntryElement: true));

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент "Место работы и должность".
        /// </summary>
        /// <param name="workplaceSectionModel">Модель места работы и должности.</param>
        /// <returns>Элемент "Место работы и должность".</returns>
        private static XElement GenerateWorkLocationSectionElement(WorkplaceSectionModel workplaceSectionModel)
        {
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            
            XElement sectionElement = new XElement(xmlnsNamespace + "section");
            if (workplaceSectionModel is null)
            {
                XAttribute nullFlavorSectionAttribute = new XAttribute("nullFlavor", "NA");
                sectionElement.Add(nullFlavorSectionAttribute);
                componentElement.Add(sectionElement);
                return componentElement;
            }

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "WORK",
                    codeSystemVersionValue: "1.18",
                    displayNameValue: "Место работы и должность, условия труда",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.197",
                    codeSystemNameValue: "Секции электронных медицинских документов"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "ТРУДОВАЯ ДЕЯТЕЛЬНОСТЬ");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateParagraphsElements(workplaceSectionModel.WorkPlaceParagraphs));

            if (workplaceSectionModel.WorkActivity != null)
            {
                sectionElement.Add(GenerateWorkActivityElement(workplaceSectionModel.WorkActivity));
            }

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент секции "Образование".
        /// </summary>
        /// <param name="educationSectionModel">Модель секции "Образование".</param>
        /// <returns>Элемент секции "Образование".</returns>
        private static XElement GenerateEducationSectionElement(EducationSectionModel educationSectionModel)
        {
            if (educationSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "EDU",
                    codeSystemVersionValue: "1.18",
                    displayNameValue: "Образование",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.197",
                    codeSystemNameValue: "Секции электронных медицинских документов"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "ОБРАЗОВАНИЕ");
            sectionElement.Add(titleElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph");

            XElement captionElement = new XElement(xmlnsNamespace + "caption", educationSectionModel.FillingSection.Caption);
            paragraphElement.Add(captionElement, educationSectionModel.FillingSection.Content);

            textElement.Add(paragraphElement);
            sectionElement.Add(textElement);

            #region entry organization

            if (educationSectionModel.Organization is not null)
            {
                XElement entryElement = new XElement(xmlnsNamespace + "entry");
                XElement observationElement = new XElement(xmlnsNamespace + "observation",
                    new XAttribute("classCode", "OBS"),
                    new XAttribute("moodCode", "EVN"));

                XElement observationCodeElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: "4100",
                        codeSystemVersionValue: "1.69",
                        displayNameValue: "Сведения о получении образования",
                        codeSystemValue: "1.2.643.5.1.13.13.99.2.166",
                        codeSystemNameValue: "Кодируемые поля CDA документов"));
                observationElement.Add(observationCodeElement);

                XElement performerElement = new XElement(xmlnsNamespace + "performer");
                XElement assignedEntityElement = new XElement(xmlnsNamespace + "assignedEntity");

                XElement idElement = new XElement(xmlnsNamespace + "id",
                    new XAttribute("nullFlavor", "NI"));
                assignedEntityElement.Add(idElement);

                var representedOrganizationElement = GenerateOrganizationElement(educationSectionModel.Organization, "representedOrganization", classCodeAttributValue: "ORG");
                if (representedOrganizationElement != null)
                {
                    foreach (var element in representedOrganizationElement.Elements(xmlnsNamespace + "id"))
                    {
                        element.Add(new XAttribute("extension", "1145"));
                    }
                    assignedEntityElement.Add(representedOrganizationElement);
                }

                performerElement.Add(assignedEntityElement);
                observationElement.Add(performerElement);
                entryElement.Add(observationElement);
                sectionElement.Add(entryElement);
            }

            #endregion

            #region entry class

            if (educationSectionModel.Class is not null)
            {
                XElement entryClassElement = new XElement(xmlnsNamespace + "entry");
                XElement observationClassElement = new XElement(xmlnsNamespace + "observation",
                    new XAttribute("classCode", "OBS"),
                    new XAttribute("moodCode", "EVN"));

                XElement observationCodeClassElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: "12137",
                        codeSystemVersionValue: "1.69",
                        displayNameValue: "Курс",
                        codeSystemValue: "1.2.643.5.1.13.13.99.2.166",
                        codeSystemNameValue: "Кодируемые поля CDA документов"));
                observationClassElement.Add(observationCodeClassElement);

                XElement valueClassElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    educationSectionModel.Class);
                observationClassElement.Add(valueClassElement);

                entryClassElement.Add(observationClassElement);
                sectionElement.Add(entryClassElement);
            }

            #endregion

            #region entry spetiality

            if (educationSectionModel.Spetiality is not null)
            {
                XElement entrySpetialityElement = new XElement(xmlnsNamespace + "entry");
                XElement observationSpetialityElement = new XElement(xmlnsNamespace + "observation",
                    new XAttribute("classCode", "OBS"),
                    new XAttribute("moodCode", "EVN"));

                XElement observationCodeSpetialityElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: "4078",
                        codeSystemVersionValue: "1.69",
                        displayNameValue: "Профессия",
                        codeSystemValue: "1.2.643.5.1.13.13.99.2.166",
                        codeSystemNameValue: "Кодируемые поля CDA документов"));
                observationSpetialityElement.Add(observationCodeSpetialityElement);

                XElement valueSpetialityElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", "ST"),
                    educationSectionModel.Spetiality);
                observationSpetialityElement.Add(valueSpetialityElement);

                entrySpetialityElement.Add(observationSpetialityElement);
                sectionElement.Add(entrySpetialityElement);

            }
            
            #endregion

            componentElement.Add(sectionElement);
            return componentElement;
        }

        /// <summary>
        /// Создает секцию "Анамнез".
        /// </summary>
        /// <param name="anamnezSectionModel">Модель секции "Анамнез".</param>
        /// <returns>Секция "Анамнез".</returns>
        private static XElement GenerateAnamnezSectionElement(AnamnezSectionModel anamnezSectionModel)
        {
            if (anamnezSectionModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement sectionElement = new XElement(xmlnsNamespace + "section");

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "SOCANAM",
                    codeSystemVersionValue: "1.18",
                    displayNameValue: "Социальный анамнез",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.197",
                    codeSystemNameValue: "Секции электронных медицинских документов"));
            sectionElement.Add(codeElement);

            XElement titleElement = new XElement(xmlnsNamespace + "title", "АНАМНЕЗ");
            sectionElement.Add(titleElement);

            sectionElement.Add(GenerateFillingAnamnezSectionElement(anamnezSectionModel));
            sectionElement.Add(GenerateEntryAnamnezSectionElements(anamnezSectionModel));

            componentElement.Add(sectionElement);
            return componentElement;
        }
        
        /// <summary>
        /// Создает элемент "Сведения о трудовой деятельности".
        /// </summary>
        /// <param name="workActivityModel">Модель сведений о трудовой деятельности.</param>
        /// <returns>Элемент "Сведения о трудовой деятельности"</returns>
        private static XElement GenerateWorkActivityElement(WorkActivityModel workActivityModel)
        {
            if (workActivityModel == null)
            {
                return null;
            }
            
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement organizerElement = new XElement(xmlnsNamespace + "organizer",
                new XAttribute("classCode", "CLUSTER"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "4073",
                    codeSystemVersionValue: "1.69",
                    displayNameValue: "Сведения о трудовой деятельности (при осуществлении трудовой деятельности)",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.166",
                    codeSystemNameValue: "Кодируемые поля CDA документов"));
            organizerElement.Add(codeElement);

            XElement statusCodeElement = new XElement(xmlnsNamespace + "statusCode",
                new XAttribute("code", "completed"));
            organizerElement.Add(statusCodeElement);

            XElement participantElement = new XElement(xmlnsNamespace + "participant",
                new XAttribute("typeCode", "LOC"));

            var participantRoleElement = GenerateOrganizationElement(workActivityModel.Workpalace, "participantRole", classCodeAttributValue: "SDLOC");
            participantRoleElement.Elements(xmlnsNamespace + "name").Remove();

            XElement playingEntityElement = new XElement(xmlnsNamespace + "playingEntity");
            XElement nameElement = new XElement(xmlnsNamespace + "name", workActivityModel.Workpalace.Name);
            playingEntityElement.Add(nameElement);
            participantRoleElement.Add(playingEntityElement);

            participantElement.Add(participantRoleElement);
            organizerElement.Add(participantElement);

            organizerElement.Add(GenerateWorkplaceActiityComponenElement("mainProfessionWorkpalceActivity", workActivityModel.MainProfession));
            organizerElement.Add(GenerateWorkplaceActiityComponenElement("qualificationWorkpalceActivity", workActivityModel.Qualification));
            organizerElement.Add(GenerateWorkplaceActiityComponenElement("workExperienceWorkpalceActivity", workActivityModel.WorkExperience));

            XElement componentElement = null;

            if (workActivityModel.WorkPerformeds != null)
            {
                foreach (var workPerformed in workActivityModel.WorkPerformeds)
                {
                    componentElement = GenerateWorkPerformedElement(workPerformed);
                }
            }

            if (componentElement != null)
            {
                organizerElement.Add(componentElement);
            }

            organizerElement.Add(GenerateWorkplaceActiityComponenElement("conditionsWorkpalceActivity", workActivityModel.Conditions));

            entryElement.Add(organizerElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "Выполняемая работа на момент направления на медико-социальную экспертизу".
        /// </summary>
        /// <param name="workPerformed">Модель выполняемой работы.</param>
        /// <returns>Элемент "Выполняемая работа на момент направления на медико-социальную экспертизу".</returns>
        private static XElement GenerateWorkPerformedElement((string Profession, string Speciality, string Position) workPerformed)
        {
            XElement componentElement = new XElement(xmlnsNamespace + "component",
                new XAttribute("typeCode", "COMP"));
            XElement organizerClusterElement = new XElement(xmlnsNamespace + "organizer",
                new XAttribute("classCode", "CLUSTER"),
                new XAttribute("moodCode", "EVN"));

            XElement codeClusterElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "4077",
                    codeSystemVersionValue: "1.69",
                    displayNameValue: "Выполняемая работа на момент направления на медико-социальную экспертизу",
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.166",
                    codeSystemNameValue: "Кодируемые поля CDA документов"));
            organizerClusterElement.Add(codeClusterElement);

            XElement statusCodeClusterElement = new XElement(xmlnsNamespace + "statusCode",
                new XAttribute("code", "completed"));
            organizerClusterElement.Add(statusCodeClusterElement);

            organizerClusterElement.Add(GenerateWorkplaceActiityComponenElement("professionWorkpalceActivity", workPerformed.Profession));
            organizerClusterElement.Add(GenerateWorkplaceActiityComponenElement("specialityWorkpalceActivity", workPerformed.Speciality));
            organizerClusterElement.Add(GenerateWorkplaceActiityComponenElement("positionWorkpalceActivity", workPerformed.Position));

            componentElement.Add(organizerClusterElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент "text" с наполнением секции.
        /// </summary>
        /// <param name="paragraphs">Список параграфов.</param>
        /// <returns>Элемент "text" с наполнением секции "Напарвление".</returns>
        private static XElement GenerateParagraphsElements(List<ParagraphModel> paragraphs)
        {
            if (paragraphs == null)
            {
                return null;
            }
            
            XElement textElement = new XElement(xmlnsNamespace + "text");
            foreach (var paragraph in paragraphs)
            {
                textElement.Add(GenerateParagraphElement(paragraph.Caption, paragraph.Content));
                textElement.Add(NewLineElement);
            }

            return textElement;
        }

        /// <summary>
        /// Генерирует элемент "paragraph".
        /// </summary>
        /// <param name="caption">Заголовок элемента.</param>
        /// <param name="context">Наполнение элемента.</param>
        /// <returns></returns>
        private static XElement GenerateParagraphElement(string caption, List<string> context)
        {
            if (context == null)
            {
                return null;
            }
            
            XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph");

            XElement captionElement = new XElement(xmlnsNamespace + "caption", caption);
            paragraphElement.Add(captionElement);

            XElement contentElement = null;
            foreach (var content in context)
            {
                contentElement = new XElement(xmlnsNamespace + "content", content);
                paragraphElement.Add(contentElement);
                if (context.Count != 1)
                {
                    paragraphElement.Add(NewLineElement);
                }
            }

            return paragraphElement;
        }

        /// <summary>
        /// Создает элемент "entry" с наполнением секции "Кодирование цели направления и медицинской организации, куда направлен пациент".
        /// </summary>
        /// <param name="targetSentModel">Модель кодирования цели направления.</param>
        /// <returns>Элемент "entry" с наполнением секции "Кодирование цели направления и медицинской организации, куда направлен пациент".</returns>
        private static XElement GenerateTargetSentElement(TargetSentModel targetSentModel)
        {
            if (targetSentModel == null)
            {
                return null;
            }
            
            XElement entryElement = new XElement(xmlnsNamespace + "entry");

            entryElement.Add(GenerateActElement(targetSentModel));

            return entryElement;
        }

        /// <summary>
        /// Создает элемент "act" с наполнением дочерних элементов.
        /// </summary>
        /// <param name="targetSentModel">Модель кодирования цели направления.</param>
        /// <returns>Элемент "act" с наполнением дочерних элементов.</returns>
        private static XElement GenerateActElement(TargetSentModel targetSentModel)
        {
            if (targetSentModel == null)
            {
                return null;
            }
            
            XElement actElement = new XElement(xmlnsNamespace + "act",
                new XAttribute("classCode", "ACT"),
                new XAttribute("moodCode", "RQO"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: "34",
                    codeSystemValue: "1.2.643.5.1.13.13.11.1522",
                    codeSystemVersionValue: "4.45",
                    codeSystemNameValue: "Виды медицинской документации",
                    displayNameValue: "Направление на медико-социальную экспертизу"));
            actElement.Add(codeElement);

            XElement statusCodeElement = new XElement(xmlnsNamespace + "statusCode",
                new XAttribute("code", "active"));
            actElement.Add(statusCodeElement);

            XElement performerElement = new XElement(xmlnsNamespace + "performer");
            XElement assignedEntityElement = new XElement(xmlnsNamespace + "assignedEntity");

            XElement idElement = new XElement(xmlnsNamespace + "id",
                new XAttribute("nullFlavor", "NI"));
            assignedEntityElement.Add(idElement);

            assignedEntityElement.Add(GenerateOrganizationElement(targetSentModel.PerformerOrganization, "representedOrganization"));

            performerElement.Add(assignedEntityElement);
            actElement.Add(performerElement);

            actElement.Add(GenerateEntryRelationshipTargetSentElement(targetSentModel.TargetSentTypes));
            
            actElement.Add(GenerateEntryRelationshipElements(
                targetSentModel.SentOrder,
                targetSentModel.Protocol,
                targetSentModel.IsAtHome,
                targetSentModel.IsPalleativeMedicalHelp,
                targetSentModel.NeedPrimaryProsthetics,
                targetSentModel.SentDate));

            return actElement;
        }

        /// <summary>
        /// Создает элемент "entryRelationship" со списком целей направлений.
        /// </summary>
        /// <param name="targetSentTypes">Список целей направлений.</param>
        /// <returns>Элемент "entryRelationship" со списком целей направлений.</returns>
        private static XElement GenerateEntryRelationshipTargetSentElement(List<TypeModel> targetSentTypes)
        {
            if (targetSentTypes is null || targetSentTypes.Count == 0)
            {
                return null;
            }
            
            var codeAttributes = GetCodeValue("sentTarget");
            
            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship");
            XElement observationElement = new XElement((xmlnsNamespace + "observation"));

            foreach (var targetSentType in targetSentTypes)
            {
                XElement codeElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: targetSentType.Code,
                        codeSystemValue: codeAttributes.codeSystemValue,
                        codeSystemVersionValue: targetSentType.CodeSystemVersion,
                        codeSystemNameValue: codeAttributes.codeSystemNameValue,
                        displayNameValue: targetSentType.DisplayName));
                observationElement.Add(codeElement);
            }
            
            entryRelationshipElement.Add(observationElement);
            return entryRelationshipElement;
        }
        
        /// <summary>
        /// Создает списков элементов "entryRelationship".
        /// </summary>
        /// <param name="sentOrder">Порядок обращения.</param>
        /// <param name="protocol">Протокол врачебной комиссии.</param>
        /// <param name="isAtHome">Экспертиза проводится на дому.</param>
        /// <param name="isPalleativeMedicalHelp">Нуждаемость в оказании паллиативной медицинской помощи.</param>
        /// <param name="needPrimaryProsthetics">Нуждаемость в первичном протезировании.</param>
        /// <param name="sentDate">Дата выдачи направления.</param>
        /// <returns>Список элементов "entryRelationship".</returns>
        private static List<XElement> GenerateEntryRelationshipElements(
            TypeModel sentOrder = null,
            ProtocolModel protocol = null,
            bool? isAtHome = null,
            bool? isPalleativeMedicalHelp = null,
            bool? needPrimaryProsthetics = null,
            DateTime? sentDate = null)
        {
            List<XElement> entryRelationshipElements = new List<XElement>();
            
            if (sentOrder != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentOrder",
                    sentOrder));
            }
            if (protocol != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentProtocol",
                    protocol.Protocol,
                    protocol.ProtocolNumber,
                    protocol.ProtocolDate));
            }
            if (isAtHome != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentLocation",
                    value: isAtHome.ToString()));
            }
            if (isPalleativeMedicalHelp != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentPolitiveHelp",
                    value: isPalleativeMedicalHelp.ToString()));
            }
            if (needPrimaryProsthetics != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentPrimaryProsthetics",
                    value: needPrimaryProsthetics.ToString()));
            }
            if (sentDate != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentDate",
                    value: sentDate?.ToString("yyyyMMddHHmm+0300")));
            }

            return entryRelationshipElements;
        }

        /// <summary>
        /// Создает элемент "entry*".
        /// </summary>
        /// <param name="entryRelationshipElementName">Наименование типа элемента.</param>
        /// <param name="code">Значения элемента "code".</param>
        /// <param name="value">Значения элемента "value".</param>
        /// <param name="effectiveTime">Значения элемента "effectiveTime".</param>
        /// <param name="isEntryElement">Истина - простой "entry" элемент.</param>
        /// <returns>Элемент "entry*".</returns>
        private static XElement GenerateEntryRelationshipElement(
            string entryRelationshipElementName,
            TypeModel code = null,
            string value = null,
            DateTime? effectiveTime = null,
            bool isEntryElement = false)
        {
            var codeAttributes = GetCodeValue(entryRelationshipElementName);
            if (code != null)
            {
                codeAttributes.codeValue = code.Code;
                codeAttributes.codeSystemVersionValue = code.CodeSystemVersion;
                codeAttributes.displayNameValue = code.DisplayName;
            }

            XElement entryElement;

            if(isEntryElement)
            {
                entryElement = new XElement(xmlnsNamespace + "entry");
            }
            else
            {
                entryElement = new XElement(xmlnsNamespace + "entryRelationship",
                    new XAttribute("typeCode", "SUBJ"),
                    new XAttribute("inversionInd", "true"));
            }

            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeAttributes.codeValue,
                    codeSystemValue: codeAttributes.codeSystemValue,
                    codeSystemVersionValue: codeAttributes.codeSystemVersionValue,
                    codeSystemNameValue: codeAttributes.codeSystemNameValue,
                    displayNameValue: codeAttributes.displayNameValue));
            observationElement.Add(codeElement);

            if (effectiveTime != null)
            {
                XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime",
                    new XAttribute("value", effectiveTime?.ToString("yyyMMddHHmm+0300")));
                observationElement.Add(effectiveTimeElement);
            }
            if (value != null)
            {
                XElement valueElement = new XElement(xmlnsNamespace + "value");
                if (entryRelationshipElementName == "sentProtocol")
                {
                    XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "ST");
                    valueElement.Add(typeAttribute, value);
                }
                else
                if (entryRelationshipElementName == "sentDate")
                {
                    XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "TS");
                    valueElement.Add(typeAttribute);
                    XAttribute valueAttribute = new XAttribute("value", value);
                    valueElement.Add(valueAttribute);
                }
                else
                {
                    XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "BL");
                    valueElement.Add(typeAttribute);
                    XAttribute valueAttribute = new XAttribute("value", value.ToLower());
                    valueElement.Add(valueAttribute);
                }
                observationElement.Add(valueElement);
            }

            entryElement.Add(observationElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "Местонахождение гражданина".
        /// </summary>
        /// <param name="patientLocationCode">Код местонахождения гражданина.</param>
        /// <param name="patientLocation">Модель местанахождения гражданина.</param>
        /// <returns>Элемент "Местонахождение гражданина"</returns>
        private static XElement GeneratePatienLocationElement(TypeModel patientLocationCode, OrganizationModel patientLocation)
        {
            //if (patientLocation == null)
            //{
            //    return null;
            //}

            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement actElement = new XElement(xmlnsNamespace + "act",
                new XAttribute("classCode", "ACT"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: patientLocationCode.Code,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.856",
                    codeSystemVersionValue: patientLocationCode.CodeSystemVersion,
                    codeSystemNameValue: "Местонахождение граждан для медико-социальной экспертизы",
                    displayNameValue: patientLocationCode.DisplayName));
            actElement.Add(codeElement);

            XElement participantElement;
            
            //if (patientLocation is not null)
            //{
                participantElement = new XElement(xmlnsNamespace + "participant",
                    new XAttribute("typeCode", "LOC"));

                participantElement.Add(GenerateOrganizationElement(patientLocation, "participantRole"));
            //}
            //else
            //{
            //    participantElement = new XElement(xmlnsNamespace + "participant",
            //        new XAttribute("typeCode", "LOC"),
            //        new XAttribute("nullFlavor", "NI"));
            //}
            
            actElement.Add(participantElement);

            entryElement.Add(actElement);
            return entryElement;
        }

        /// <summary>
        /// Создает элемент "component".
        /// </summary>
        /// <param name="nameElement">Название элемента.</param>
        /// <param name="contextElement">Контакт элемента.</param>
        /// <returns>Элемент "component"</returns>
        private static XElement GenerateWorkplaceActiityComponenElement(string nameElement, string contextElement)
        {
            XElement componentElement = new XElement(xmlnsNamespace + "component",
                new XAttribute("typeCode", "COMP"));
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            var codeValue = GetCodeValue(nameElement);

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    displayNameValue: codeValue.displayNameValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue));
            observationElement.Add(codeElement);

            XElement textElement = new XElement(xmlnsNamespace + "text", contextElement);
            observationElement.Add(textElement);

            componentElement.Add(observationElement);
            return componentElement;
        }

        /// <summary>
        /// Генерирует элементы для наполнения секции "Анамнез".
        /// </summary>
        /// <param name="anamnezSectionModel">Модель секции "Анамнез".</param>
        /// <returns>Элементы для наполнения секции "Анамнез".</returns>
        private static XElement GenerateFillingAnamnezSectionElement(AnamnezSectionModel anamnezSectionModel)
        {
            if (anamnezSectionModel == null)
            {
                return null;
            }
            
            XElement textElement = new XElement(xmlnsNamespace + "text");

            if (anamnezSectionModel.Disability != null)
            {
                XElement disabilityParagraphElement = new XElement(xmlnsNamespace + "paragraph");

                XElement disabilityCaptionElement = new XElement(xmlnsNamespace + "caption", "Инвалидность");
                disabilityParagraphElement.Add(disabilityCaptionElement);

                XElement disabilityContentElement1 = new XElement(xmlnsNamespace + "content",
                    new XAttribute("ID", "socanam3"), anamnezSectionModel.Disability.GroupText);
                disabilityParagraphElement.Add(disabilityContentElement1);

                disabilityParagraphElement.Add(NewLineElement);

                disabilityParagraphElement.Add("Находился на инвалидности на момент направления: ");

                XElement disabilityContentElement2 = new XElement(xmlnsNamespace + "content",
                    new XAttribute("ID", "socanam34"), anamnezSectionModel.Disability.TimeDisability);
                disabilityParagraphElement.Add(disabilityContentElement2);
                disabilityParagraphElement.Add(NewLineElement);

                textElement.Add(disabilityParagraphElement);

                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.DegreeDisability is not null 
                && anamnezSectionModel.DegreeDisability.DegreeDisabilities is not null 
                && anamnezSectionModel.DegreeDisability.DegreeDisabilities.Count != 0)
            {
                XElement degreeDisabilityParagraphElement = new XElement(xmlnsNamespace + "paragraph");

                XElement degreeDisabilityCaptionElement = new XElement(xmlnsNamespace + "caption", "Степень утраты профессиональной трудоспособности");
                degreeDisabilityParagraphElement.Add(degreeDisabilityCaptionElement);

                foreach (var degreeDisability in anamnezSectionModel.DegreeDisability.DegreeDisabilities)
                {
                    XElement degreeDisabilityContentElement = new XElement(xmlnsNamespace + "content",
                        new XAttribute("ID", degreeDisability.ID), degreeDisability.FullText);
                    degreeDisabilityParagraphElement.Add(degreeDisabilityContentElement);
                    degreeDisabilityParagraphElement.Add(NewLineElement);
                }

                textElement.Add(degreeDisabilityParagraphElement);

                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.SeenOrganizations != null)
            {
                textElement.Add(GenerateParagraphElement("Наблюдается в организациях, оказывающих лечебно-профилактическую помощь", 
                    new List<string>() { anamnezSectionModel.SeenOrganizations } ));
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.MedicalAnamnez != null)
            {
                textElement.Add(GenerateParagraphElement("Анамнез заболевания",
                    new List<string>() { anamnezSectionModel.MedicalAnamnez }));
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.LifeAnamnez != null)
            {
                textElement.Add(GenerateParagraphElement("Анамнез жизни",
                    new List<string>() { anamnezSectionModel.LifeAnamnez }));
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.ActualDevelopment != null)
            {
                var paragraphElement = GenerateParagraphElement(
                    "Физическое развитие (в отношении детей в возрасте до 3 лет)",
                    new List<string>() { anamnezSectionModel.ActualDevelopment });
                if (paragraphElement != null)
                {
                    var contentParagraphElement = paragraphElement.Elements(xmlnsNamespace + "content");
                    if (contentParagraphElement != null)
                    {
                        foreach (var element in contentParagraphElement)
                        {
                            element.Add(new XAttribute("ID", "socanam4"));
                        }
                    }
                }

                textElement.Add(paragraphElement);
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.TemporaryDisabilitys != null)
            {
                XElement tableElement = new XElement(xmlnsNamespace + "table",
                    new XAttribute("width", "100%"));

                XElement captionTableElement = new XElement(xmlnsNamespace + "caption", "Временная нетрудоспособность:");
                tableElement.Add(captionTableElement);

                #region Column elements

                XElement column1Element = new XElement(xmlnsNamespace + "col",
                    new XAttribute("width", "10%"));
                tableElement.Add(column1Element);
                XElement column2Element = new XElement(xmlnsNamespace + "col",
                    new XAttribute("width", "20%"));
                tableElement.Add(column2Element);
                XElement column3Element = new XElement(xmlnsNamespace + "col",
                    new XAttribute("width", "70%"));
                tableElement.Add(column3Element);

                #endregion

                #region Table body elements

                XElement tbodyElement = new XElement(xmlnsNamespace + "tbody");

                #region Table body header elements

                XElement trHeaderElement = new XElement(xmlnsNamespace + "tr");

                XElement th1Element = new XElement(xmlnsNamespace + "th", "Дата начала");
                trHeaderElement.Add(th1Element);
                XElement th2Element = new XElement(xmlnsNamespace + "th", "Дата окончания");
                trHeaderElement.Add(th2Element);
                XElement th3Element = new XElement(xmlnsNamespace + "th", "Число дней");
                trHeaderElement.Add(th3Element);
                XElement th4Element = new XElement(xmlnsNamespace + "th", "Шифр МКБ");
                trHeaderElement.Add(th4Element);

                tbodyElement.Add(trHeaderElement);

                #endregion

                if (anamnezSectionModel.TemporaryDisabilitys != null)
                {
                    foreach (var temporaryDisability in anamnezSectionModel.TemporaryDisabilitys)
                    {
                        XElement trContentElement = new XElement(xmlnsNamespace + "tr");

                        if (temporaryDisability.DateStart is not null)
                        {
                            XElement td1Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DateStart?.ToString("dd.MM.yyyy"));
                            trContentElement.Add(td1Element);
                        }

                        if (temporaryDisability.DateFinish is not null)
                        {
                            XElement td2Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DateFinish?.ToString("dd.MM.yyyy"));
                            trContentElement.Add(td2Element);
                        }
                        
                        XElement td3Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DayCount);
                        trContentElement.Add(td3Element);
                        XElement td4Element = new XElement(xmlnsNamespace + "td", temporaryDisability.CipherMKB);
                        trContentElement.Add(td4Element);

                        tbodyElement.Add(trContentElement);
                    }
                }

                tableElement.Add(tbodyElement);

                #endregion

                textElement.Add(tableElement);
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.CertificateDisabilityNumber != null)
            {
                textElement.Add(GenerateParagraphElement("Листок нетрудоспособности в форме электронного документа",
                    new List<string>() { anamnezSectionModel.CertificateDisabilityNumber }));
                textElement.Add(NewLineElement);
            }

            if (anamnezSectionModel.EffectityAction != null)
            {
                textElement.Add(GenerateParagraphElement("Результаты и эффективность проведенных мероприятий медицинской реабилитации",
                    anamnezSectionModel.EffectityAction));
            }

            return textElement;
        }

        /// <summary>
        /// Генерирует список елементов "entry" для секции "Анамнез".
        /// </summary>
        /// <param name="anamnezSectionModel"></param>
        /// <returns></returns>
        private static List<XElement> GenerateEntryAnamnezSectionElements(AnamnezSectionModel anamnezSectionModel)
        {
            if (anamnezSectionModel == null)
            {
                return null;
            }
            
            List<XElement> entryElements = new List<XElement>();

            if (anamnezSectionModel.StartYear != null)
            {
                entryElements.Add(GenerateEntryAnamnezSectionElement_Light("startYearAnamnez", "ST", anamnezSectionModel.StartYear.ToString()));
            }
            if (anamnezSectionModel.MedicalAnamnez != null)
            {
                entryElements.Add(GenerateEntryAnamnezSectionElement_Light("medicalAnamnezAnamnez", "ST", anamnezSectionModel.MedicalAnamnez));
            }
            if (anamnezSectionModel.LifeAnamnez != null)
            {
                entryElements.Add(GenerateEntryAnamnezSectionElement_Light("lifeAnamnezAnamnez", "ST", anamnezSectionModel.LifeAnamnez));
            }
            if (anamnezSectionModel.ActualDevelopment != null)
            {
                var entryElement = GenerateEntryAnamnezSectionElement_Light("actualDevelopmentAnamnez", "ST", anamnezSectionModel.ActualDevelopment);

                foreach (var entryElementChild in entryElement.Elements())
                {
                    if (entryElementChild.Name == xmlnsNamespace + "observation")
                    {
                        foreach (var observationElementChild in entryElementChild.Elements())
                        {
                            if (observationElementChild.Name == xmlnsNamespace + "value") observationElementChild.Remove();
                            if (observationElementChild.Name == xmlnsNamespace + "code")
                            {
                                XElement originalTextElement = new XElement(xmlnsNamespace + "originalText");

                                XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                                    new XAttribute("value", "#socanam4"));
                                originalTextElement.Add(referenceElement);

                                observationElementChild.Add(originalTextElement);
                            }
                        }
                        XElement textElement = new XElement(xmlnsNamespace + "text", anamnezSectionModel.ActualDevelopment);
                        entryElementChild.Add(textElement);
                    }
                }

                entryElements.Add(entryElement);
            }
            if (anamnezSectionModel.TemporaryDisabilitys != null)
            {
                XElement temporaryDisabilitysEntryElement = new XElement(xmlnsNamespace + "entry");
                XElement temporaryDisabilitysOrganizerElement = new XElement(xmlnsNamespace + "organizer",
                    new XAttribute("classCode", "CLUSTER"),
                    new XAttribute("moodCode", "EVN"));
                XElement temporaryDisabilitysStatusCodeElement = new XElement(xmlnsNamespace + "statusCode",
                    new XAttribute("code", "completed"));
                temporaryDisabilitysOrganizerElement.Add(temporaryDisabilitysStatusCodeElement);

                if (anamnezSectionModel.TemporaryDisabilitys != null)
                {
                    foreach (var temporaryDisability in anamnezSectionModel.TemporaryDisabilitys)
                    {
                        temporaryDisabilitysOrganizerElement.Add(GenerateTemporaryDisabilitysElement(temporaryDisability));
                    }
                }

                XElement certificateDisabilityNumberComponentElement = new XElement(xmlnsNamespace + "component",
                    new XAttribute("typeCode", "COMP"));
                XElement certificateDisabilityNumberObservationElement = new XElement(xmlnsNamespace + "observation",
                    new XAttribute("classCode", "OBS"),
                    new XAttribute("moodCode", "EVN"));

                var codeValue = GetCodeValue("certificateDisabilityNumberAnamnez");
                XElement certificateDisabilityNumberCodeElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: codeValue.codeValue,
                        codeSystemValue: codeValue.codeSystemValue,
                        codeSystemVersionValue: codeValue.codeSystemVersionValue,
                        codeSystemNameValue: codeValue.codeSystemNameValue,
                        displayNameValue: codeValue.displayNameValue));
                certificateDisabilityNumberObservationElement.Add(certificateDisabilityNumberCodeElement);

                XElement certificateDisabilityNumberValueElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", "INT"),
                    new XAttribute("value", GetNumbersFromString(anamnezSectionModel.CertificateDisabilityNumber)));
                certificateDisabilityNumberObservationElement.Add(certificateDisabilityNumberValueElement);

                certificateDisabilityNumberComponentElement.Add(certificateDisabilityNumberObservationElement);
                temporaryDisabilitysOrganizerElement.Add(certificateDisabilityNumberComponentElement);

                temporaryDisabilitysEntryElement.Add(temporaryDisabilitysOrganizerElement);
                entryElements.Add(temporaryDisabilitysEntryElement);
            }
            if (anamnezSectionModel.DegreeDisability is not null
                && anamnezSectionModel.DegreeDisability.DegreeDisabilities is not null
                && anamnezSectionModel.DegreeDisability.DegreeDisabilities.Count != 0)
            {
                foreach (var degreeDisability in anamnezSectionModel.DegreeDisability.DegreeDisabilities)
                {
                    entryElements.Add(GenerateDegreeDisabilityElement(
                        degreeDisability.DateTo,
                        degreeDisability.Term,
                        degreeDisability.Percent,
                        degreeDisability.ID));
                }
            }
            if (anamnezSectionModel.Disability != null)
            {
                entryElements.Add(GenerateDisabilityElement(anamnezSectionModel.Disability));
            }
            if (anamnezSectionModel.IPRANumber != null)
            {
                var codeValue = GetCodeValue("IPRANumberAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueType:"ST", valueValue: anamnezSectionModel.IPRANumber));
            }
            if (anamnezSectionModel.ProtocolNumber != null)
            {
                var codeValue = GetCodeValue("protocolNumberAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueType: "ST", valueValue: anamnezSectionModel.ProtocolNumber));
            }
            if (anamnezSectionModel.ProtocolDate != null)
            {
                var codeValue = GetCodeValue("protocolDateAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueType: "TS", valueValue: anamnezSectionModel.ProtocolDate?.ToString("yyyyMMdd")));
            }
            if (anamnezSectionModel.Results != null)
            {
                var codeValue = GetCodeValue("resultsAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueType: "ST", valueValue: anamnezSectionModel.Results));
            }
            if (anamnezSectionModel.ResultRestorationFunctions != null)
            {
                var codeValue = GetCodeValue("resultRestorationFunctionsAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                var valueModel = new ValueElementModel()
                {
                    Type = "CD",
                    Code = "2",
                    CodeSystem = "1.2.643.5.1.13.13.11.1475",
                    CodeSystemVersion = "2.1",
                    CodeSystemName = "Результаты индивидуальной программы реабилитации инвалидов",
                    DisplayName = anamnezSectionModel.ResultRestorationFunctions
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueModel:valueModel));
            }
            if (anamnezSectionModel.ResultCompensationFunction != null)
            {
                var codeValue = GetCodeValue("resultCompensationFunctionAnamnez");
                var codeModel = new CodeElementModel()
                {
                    Code = codeValue.codeValue,
                    CodeSystemVersion = codeValue.codeSystemVersionValue,
                    CodeSystem = codeValue.codeSystemValue,
                    CodeSystemName = codeValue.codeSystemNameValue,
                    DisplayName = codeValue.displayNameValue
                };
                var valueModel = new ValueElementModel()
                {
                    Type = "CD",
                    Code = "2",
                    CodeSystem = "1.2.643.5.1.13.13.11.1475",
                    CodeSystemVersion = "2.1",
                    CodeSystemName = "Результаты индивидуальной программы реабилитации инвалидов",
                    DisplayName = anamnezSectionModel.ResultCompensationFunction
                };
                entryElements.Add(GenerateCodingElementAnamnezSection(codeModel, valueModel: valueModel));
            }


            return entryElements;
        }

        /// <summary>
        /// Генерирование элемента "entry" для секции "Анамнез". (лайт версия).
        /// </summary>
        /// <param name="entryName">Наименование элемента.</param>
        /// <param name="type">Типа контанта.</param>
        /// <param name="content">Контент.</param>
        /// <returns>Элемент "entry".</returns>
        private static XElement GenerateEntryAnamnezSectionElement_Light(string entryName, string type, string content)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            var codeValue = GetCodeValue(entryName);
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue,
                    displayNameValue: codeValue.displayNameValue));
            observationElement.Add(codeElement);

            XElement valuElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", type),
                content);
            observationElement.Add(valuElement);

            entryElement.Add(observationElement);
            return entryElement;
        }

        /// <summary>
        /// Генерирование элемента "entry" для секции "Анамнез". (Временная нетрудоспособность).
        /// </summary>
        /// <param name="temporaryDisabilityModel">Модель "Временная нетрудоспособность".</param>
        /// <returns>Элемента "entry" для секции "Анамнез". (Временная нетрудоспособность).</returns>
        private static XElement GenerateTemporaryDisabilitysElement(TemporaryDisabilityModel temporaryDisabilityModel)
        {
            if (temporaryDisabilityModel == null)
            {
                return null;
            }
            
            XElement componentElement = new XElement(xmlnsNamespace + "component",
                new XAttribute("typeCode", "COMP"));
            XElement actElement = new XElement(xmlnsNamespace + "act",
                new XAttribute("classCode", "ACT"),
                new XAttribute("moodCode", "EVN"));

            var codeValue = GetCodeValue("temporaryDisabilitysAnamnez");
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue,
                    displayNameValue: codeValue.displayNameValue));
            actElement.Add(codeElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime");
            XElement lowElement = new XElement(xmlnsNamespace + "low");
            if (temporaryDisabilityModel.DateStart is not null)
            {
                lowElement.Add(new XAttribute("value", temporaryDisabilityModel.DateStart?.ToString("yyyyMMdd")));
            }
            else
            {
                lowElement.Add(new XAttribute("nullFlavor", "NAV"));
            }

            XElement highElement = new XElement(xmlnsNamespace + "high");
            if (temporaryDisabilityModel.DateFinish is not null)
            {
                highElement.Add(new XAttribute("value", temporaryDisabilityModel.DateFinish?.ToString("yyyyMMdd")));
            }
            else
            {
                highElement.Add(new XAttribute("nullFlavor", "NAV"));
            }
            
            effectiveTimeElement.Add(lowElement);
            effectiveTimeElement.Add(highElement);
            actElement.Add(effectiveTimeElement);

            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            XElement observationEntryRelationshipElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));
            XElement entryRelationshipCodeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: temporaryDisabilityModel.CipherMKB,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1005",
                    codeSystemVersionValue: "2.19",
                    codeSystemNameValue: "Международная статистическая классификация болезней и проблем, связанных со здоровьем (10-й пересмотр)",
                    displayNameValue: temporaryDisabilityModel.Diagnosis));
            observationEntryRelationshipElement.Add(entryRelationshipCodeElement);
            entryRelationshipElement.Add(observationEntryRelationshipElement);
            actElement.Add(entryRelationshipElement);

            componentElement.Add(actElement);
            return componentElement;
        }

        /// <summary>
        /// Генерирование степени утраты профессиональной трудоспособности.
        /// </summary>
        /// <param name="degreeDisabilityDateTo">Дата, до которой установлена степень утраты профессиональной трудоспособности.</param>
        /// <param name="degreeDisabilityTime">Срок, на который установлена степень утраты профессиональной трудоспособности.</param>
        /// <param name="degreeDisabilityPercent">Процент утраты профессиональной трудоспособности.</param>
        /// <param name="socanomeNumber">Номер секции (ссылки).</param>
        /// <returns>Элемент "entry" степени утраты профессиональной трудоспособности.</returns>
        private static XElement GenerateDegreeDisabilityElement(
            DateTime? degreeDisabilityDateTo, 
            string degreeDisabilityTime, 
            int? degreeDisabilityPercent,
            string socanomeNumber)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            var codeValue = GetCodeValue("degreeDisabilityAnamnez");
            XElement codeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValue.codeValue,
                    codeSystemValue: codeValue.codeSystemValue,
                    codeSystemVersionValue: codeValue.codeSystemVersionValue,
                    codeSystemNameValue: codeValue.codeSystemNameValue,
                    displayNameValue: codeValue.displayNameValue));
            observationElement.Add(codeElement);

            XElement textElement = new XElement(xmlnsNamespace + "text");
            XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                new XAttribute("value", $"#{socanomeNumber}"));
            textElement.Add(referenceElement);
            observationElement.Add(textElement);

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime");
            XElement highElement = new XElement(xmlnsNamespace + "high");
            if (degreeDisabilityDateTo is not null)
            {
                highElement.Add(new XAttribute("value", degreeDisabilityDateTo?.ToString("yyyyMMdd")));
            }
            else
            {
                highElement.Add(new XAttribute("nullFlavor", "NAV"));
            }
            effectiveTimeElement.Add(highElement);
            observationElement.Add(effectiveTimeElement);

            XElement valueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "INT"),
                new XAttribute("value", degreeDisabilityPercent));
            observationElement.Add(valueElement);

            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            XElement observationEntryRelationshipElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            var codeValueTimeEntryRelationship = GetCodeValue("degreeDisabilityTimeAnamnez");
            XElement entryRelationshipCodeElement = new XElement(xmlnsNamespace + "code",
                GetTypeElementAttributes(
                    codeValue: codeValueTimeEntryRelationship.codeValue,
                    codeSystemValue: codeValueTimeEntryRelationship.codeSystemValue,
                    codeSystemVersionValue: codeValueTimeEntryRelationship.codeSystemVersionValue,
                    codeSystemNameValue: codeValueTimeEntryRelationship.codeSystemNameValue,
                    displayNameValue: codeValueTimeEntryRelationship.displayNameValue));
            observationEntryRelationshipElement.Add(entryRelationshipCodeElement);

            XElement entryRelationshipValueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute(xsiNamespace + "type", "CD"),
                new XAttribute("code", "2"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.99.2.325"),
                new XAttribute("codeSystemVersion", "1.2"),
                new XAttribute("codeSystemName",
                    "Срок, на который установлена степень утраты профессиональной трудоспособности"),
                new XAttribute("displayName", degreeDisabilityTime));
            observationEntryRelationshipElement.Add(entryRelationshipValueElement);

            entryRelationshipElement.Add(observationEntryRelationshipElement);
            observationElement.Add(entryRelationshipElement);
            entryElement.Add(observationElement);
            return entryElement;
        }

        /// <summary>
        /// Генерирование элемента "entry" от "Инвалидность".
        /// </summary>
        /// <param name="disabilityModel">Модель "Инвалидность".</param>
        /// <returns>Элемент "entry" от "Инвалидность".</returns>
        private static XElement GenerateDisabilityElement(DisabilityModel disabilityModel)
        {
            if (disabilityModel == null)
            {
                return null;
            }
            
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            #region code element

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", "3"),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1053"),
                new XAttribute("codeSystemVersion", "3.3"),
                new XAttribute("codeSystemName", "Группы инвалидности"),
                new XAttribute("displayName", $"{disabilityModel.Group} группа"));

            XElement originalTextElement = new XElement(xmlnsNamespace + "originalText");
            XElement referenceElement = new XElement(xmlnsNamespace + "reference",
                new XAttribute("value", "#socanam3"));
            originalTextElement.Add(referenceElement);
            codeElement.Add(originalTextElement);

            XElement qualifierElement = new XElement(xmlnsNamespace + "qualifier");
            XElement qualifierValueElement = new XElement(xmlnsNamespace + "value",
                new XAttribute("code", 2),
                new XAttribute("codeSystem", "1.2.643.5.1.13.13.11.1041"),
                new XAttribute("codeSystemVersion", "1.1"),
                new XAttribute("codeSystemName", "Тип установления инвалидности (впервые, повторно)"),
                new XAttribute("displayName", disabilityModel.GroupOrder));
            qualifierElement.Add(qualifierValueElement);
            codeElement.Add(qualifierElement);

            observationElement.Add(codeElement);

            #endregion

            #region effectiveTime element

            XElement effectiveTimeElement = new XElement(xmlnsNamespace + "effectiveTime");

            XElement lowElement = new XElement(xmlnsNamespace + "low",
                new XAttribute("value", disabilityModel.DateDisabilityStart?.ToString("yyyyMMdd")));
            effectiveTimeElement.Add(lowElement);

            if (disabilityModel.DateDisabilityFinish != null)
            {
                XElement highElement = new XElement(xmlnsNamespace + "high",
                    new XAttribute("value", disabilityModel.DateDisabilityFinish?.ToString("yyyyMMdd")));
                effectiveTimeElement.Add(highElement);
            }

            observationElement.Add(effectiveTimeElement);

            #endregion

            #region entryRelationship elements

            ValueElementModel groopTimeValue = new ValueElementModel()
            {
                Type = "CD",
                Code = "6",
                CodeSystem = "1.2.643.5.1.13.13.99.2.358",
                CodeSystemVersion = "1.3",
                CodeSystemName = "Срок, на который установлена инвалидность",
                DisplayName = disabilityModel.GroupTime
            };
            observationElement.Add(GenerateEntryRelationshipDisabilityElement("groopTimeAnamnez", valueModel: groopTimeValue));

            ValueElementModel timeDisabilityValue = new ValueElementModel()
            {
                Type = "CD",
                Code = "4",
                CodeSystem = "1.2.643.5.1.13.13.11.1490",
                CodeSystemVersion = "1.1",
                CodeSystemName =
                    "Период, в течение которого гражданин находился на инвалидности на дату направления на медико - социальную экспертизу",
                DisplayName = disabilityModel.TimeDisability
            };
            observationElement.Add(GenerateEntryRelationshipDisabilityElement("timeDisabilityAnamnez", valueModel: timeDisabilityValue));

            if (disabilityModel.CauseOfDisability != null)
            {
                CodeElementModel codeValue = new CodeElementModel()
                {
                    Code = "4",
                    CodeSystem = "1.2.643.5.1.13.13.11.1474",
                    CodeSystemVersion = "2.2",
                    CodeSystemName = "Причины инвалидности",
                    DisplayName = disabilityModel.CauseOfDisability
                };
                observationElement.Add(GenerateEntryRelationshipDisabilityElement("groopTimeAnamnez", codeModel: codeValue));
            }

            #endregion

            entryElement.Add(observationElement);
            return entryElement;
        }

        /// <summary>
        /// Генерирует элемент "entryRelationship" для "Инвалидности".
        /// </summary>
        /// <param name="codeElementName">Наименование элемента.</param>
        /// <param name="codeModel">Модель элемента "code".</param>
        /// <param name="valueModel">Модель элемента "value".</param>
        /// <returns>Элемент "entryRelationship" для "Инвалидности".</returns>
        private static XElement GenerateEntryRelationshipDisabilityElement(string codeElementName, CodeElementModel codeModel = null, ValueElementModel valueModel = null)
        {
            XElement entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                new XAttribute("typeCode", "COMP"));
            if (codeModel != null)
            {
                entryRelationshipElement = new XElement(xmlnsNamespace + "entryRelationship",
                    new XAttribute("typeCode", "CAUS"),
                    new XAttribute("inversionInd", "true"));
            }

            XElement observationElement = null;

            if (codeModel == null)
            {
                observationElement = new XElement(xmlnsNamespace + "observation",
                    new XAttribute("classCode", "OBS"),
                    new XAttribute("moodCode", "EVN"));
                var codeValue = GetCodeValue(codeElementName);
                XElement codeElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: codeValue.codeValue,
                        codeSystemValue: codeValue.codeSystemValue,
                        codeSystemVersionValue: codeValue.codeSystemVersionValue,
                        codeSystemNameValue: codeValue.codeSystemNameValue,
                        displayNameValue: codeValue.displayNameValue));
                observationElement.Add(codeElement);
            }
            else
            {
                XElement actElement = new XElement(xmlnsNamespace + "act",
                    new XAttribute("classCode", "ACT"),
                    new XAttribute("moodCode", "EVN"));
                XElement codeElement = new XElement(xmlnsNamespace + "code",
                    GetTypeElementAttributes(
                        codeValue: codeModel.Code,
                        codeSystemValue: codeModel.CodeSystem,
                        codeSystemVersionValue: codeModel.CodeSystemVersion,
                        codeSystemNameValue: codeModel.CodeSystemName,
                        displayNameValue: codeModel.DisplayName));
                actElement.Add(codeElement);
                entryRelationshipElement.Add(actElement);
            }

            if (valueModel != null)
            {
                XElement valueElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", valueModel.Type),
                    new XAttribute("code", valueModel.Code),
                    new XAttribute("codeSystem", valueModel.CodeSystem),
                    new XAttribute("codeSystemVersion", valueModel.CodeSystemVersion),
                    new XAttribute("codeSystemName", valueModel.CodeSystemName),
                    new XAttribute("displayName", valueModel.DisplayName));
                observationElement.Add(valueElement);
            }

            entryRelationshipElement.Add(observationElement);
            return entryRelationshipElement;
        }

        /// <summary>
        /// Генерирует элементы "Кодирование..." для секции "Анамнез".
        /// </summary>
        /// <param name="codeModel">Модель элемента "code".</param>
        /// <param name="valueModel">Модель элемента "value".</param>
        /// <param name="valueType">Тип элемента "value".</param>
        /// <param name="valueValue">Значение элемента "value". (не указывается вместе с valueModel).</param>
        /// <returns></returns>
        private static XElement GenerateCodingElementAnamnezSection(
            CodeElementModel codeModel,
            ValueElementModel valueModel = null,
            string valueType = null,
            string valueValue = null)
        {
            XElement entryElement = new XElement(xmlnsNamespace + "entry");
            XElement observationElement = new XElement(xmlnsNamespace + "observation",
                new XAttribute("classCode", "OBS"),
                new XAttribute("moodCode", "EVN"));

            XElement codeElement = new XElement(xmlnsNamespace + "code",
                new XAttribute("code", codeModel.Code),
                new XAttribute("codeSystem", codeModel.CodeSystem),
                new XAttribute("codeSystemVersion", codeModel.CodeSystemVersion),
                new XAttribute("codeSystemName", codeModel.CodeSystemName),
                new XAttribute("displayName", codeModel.DisplayName));
            observationElement.Add(codeElement);

            XElement valueElement = null;

            if (valueType == "ST")
            {
                valueElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", valueType), valueValue);
            } else if (valueType == "TS")
            {
                valueElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", valueType),
                    new XAttribute("value", valueValue));
            }
            else
            {
                valueElement = new XElement(xmlnsNamespace + "value",
                    new XAttribute(xsiNamespace + "type", valueModel.Type),
                    new XAttribute("code", valueModel.Code),
                    new XAttribute("codeSystem", valueModel.CodeSystem),
                    new XAttribute("codeSystemVersion", valueModel.CodeSystemVersion),
                    new XAttribute("codeSystemName", valueModel.CodeSystemName),
                    new XAttribute("displayName", valueModel.DisplayName));
            }

            observationElement.Add(valueElement);

            entryElement.Add(observationElement);
            return entryElement;
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

        /// <summary>
        /// Получить значения "code" элемента..
        /// </summary>
        /// <param name="elementName">Наименование элемента.</param>
        /// <returns>Значения "code" элемента.</returns>
        private static (string codeValue, string codeSystemValue, string codeSystemVersionValue, string codeSystemNameValue, string displayNameValue) GetCodeValue(string elementName)
        {
            var values = new Dictionary<string, (string codeValue, string codeSystemValue, string codeSystemVersionValue, string codeSystemNameValue, string displayNameValue)>()
            {
                { "sentTarget", ("10", "1.2.643.5.1.13.13.99.2.147", "1.5", "Цели направления на медико-социальную экспертизу", "Разработка индивидуальной программы реабилитации или абилитации инвалида (ребенка-инвалида)") },
                { "sentOrder", ("2", "1.2.643.5.1.13.13.11.1007", "2.1", "Вид случая госпитализации или обращения (первичный, повторный)", "Повторный") },
                { "sentProtocol", ("4059", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Протокол врачебной комиссии") },
                { "sentLocation", ("4060", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Медико-социальную экспертизу необходимо проводить на дому (Отметка)") },
                { "sentPolitiveHelp", ("4061", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Нуждаемость в оказании паллиативной медицинской помощи") },
                { "sentPrimaryProsthetics", ("7012", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Нуждаемость в первичном протезировании") },
                { "sentDate", ("4062", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Дата выдачи направления на МСЭ") },
                { "citizenship", ("1", "1.2.643.5.1.13.13.99.2.315", "2.1", "Категории гражданства", "Гражданин Российской Федерации") },
                { "militaryDuty", ("4", "1.2.643.5.1.13.13.99.2.314", "1.2", "Отношение к воинской обязанности", "Гражданин, не состоящий на воинском учёте") },

                #region Workpalce Activity

                { "mainProfessionWorkpalceActivity", ("4074", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Основная профессия (специальность, должность)") },
                { "qualificationWorkpalceActivity", ("4075", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Квалификация (класс, разряд, категория, звание)") },
                { "workExperienceWorkpalceActivity", ("4076", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Стаж работы") },
                { "professionWorkpalceActivity", ("4078", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Профессия") },
                { "specialityWorkpalceActivity", ("4079", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Специальность") },
                { "positionWorkpalceActivity", ("4080", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Должность") },
                { "workAtTimeWorkpalceActivity", ("4077", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Выполняемая работа на момент направления на медико-социальную экспертизу") },
                { "conditionsWorkpalceActivity", ("4081", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Условия и характер выполняемого труда") },

                #endregion

                #region Anamnez

                { "startYearAnamnez", ("4101", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Год, с которого наблюдается в медицинской организации") },
                { "medicalAnamnezAnamnez", ("4102", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Анамнез заболевания") },
                { "lifeAnamnezAnamnez", ("4103", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Анамнез жизни") },
                { "actualDevelopmentAnamnez", ("4082", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Физическое развитие (в отношении детей в возрасте до 3 лет)") },
                { "temporaryDisabilitysAnamnez", ("4057", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Временная нетрудоспособность") },
                { "certificateDisabilityNumberAnamnez", ("4063", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Номер электронного листка нетрудоспособности") },
                { "degreeDisabilityAnamnez", ("4058", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Степень утраты профессиональной трудоспособности (%)") },
                { "degreeDisabilityTimeAnamnez", ("4083", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Срок, на который установлена степень утраты профессиональной трудоспособности") },
                { "groopTimeAnamnez", ("4115", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Срок, на который установлена инвалидность") },
                { "timeDisabilityAnamnez", ("4169", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Период, в течение которого гражданин находился на инвалидности на момент направления на медико-социальную экспертизу") },
                { "IPRANumberAnamnez", ("4104", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Номер ИПРА") },
                { "protocolNumberAnamnez", ("4105", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Номер протокола проведения медико-социальной экспертизы") },
                { "protocolDateAnamnez", ("4106", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Дата протокола проведения медико-социальной экспертизы") },
                { "resultsAnamnez", ("4107", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Результаты и эффективность проведенных мероприятий медицинской реабилитации, рекомендованных индивидуальной программой реабилитации или абилитации инвалида (ребенка-инвалида) (ИПРА) (текстовое описание)") },
                { "resultRestorationFunctionsAnamnez", ("4064", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Восстановление нарушенных функций") },
                { "resultCompensationFunctionAnamnez", ("4065", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Достижение компенсации утраченных/отсутствующих функций") },

                #endregion

                #region Vital parameters

                { "mainVitalParameters", ("VITALPARAM", "1.2.643.5.1.13.13.99.2.197", "1.18", "Секции электронных медицинских документов", "Витальные параметры") },
                { "bodyMassVitalParameters", ("50", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Масса тела") },
                { "birthWeightVitalParameters", ("50", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Масса тела") },
                { "growthVitalParameters", ("51", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Длина тела") },
                { "IMTVitalParameters", ("10", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Индекс массы тела") },
                { "physiologicalShipmentsVolumeVitalParameters", ("56", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Суточный объём физиологических отправлений") },
                { "waistVitalParameters", ("54", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Окружность талии") },
                { "hipsVitalParameters", ("55", "1.2.643.5.1.13.13.99.2.262", "3.3", "Витальные параметры", "Окружность бёдер") },
                { "bodyTypeParameters", ("4108", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Телосложение") }

                #endregion
            };

            return values[elementName];
        }

        /// <summary>
        /// Получить все цифры (строкой) из строки.
        /// </summary>
        /// <param name="mainString">Строка для преобразования.</param>
        /// <returns>Преобразованная строка с цифрами.</returns>
        private static string GetNumbersFromString(string mainString)
        {
            return new string(mainString.Where(t => Char.IsDigit(t)).ToArray()); ;
        }

        #endregion
    }
}
