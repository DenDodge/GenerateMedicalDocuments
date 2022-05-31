using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            if (addressModel == null)
            {
                return null;
            }
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
        /// Создает элемент организации.
        /// </summary>
        /// <param name="organizationModel">Модель организации, направляющей на МСЭ.</param>
        /// <param name="elementName">Наименование элемента.</param>
        /// <param name="classCodeAttributValue">Значение атрибута "classCode".</param>
        /// <param name="rootValue">Значение параметра root в ID.</param>
        /// <returns>Элемент организации.</returns>
        private static XElement GenerateOrganizationElement(OrganizationModel organizationModel, string elementName, string classCodeAttributValue = null, string rootValue = null)
        {
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

            if (organizationModel.Name != null)
            {
                XElement nameElement = new XElement(xmlnsNamespace + "name", organizationModel.Name);
                organizationElement.Add(nameElement);
            }

            if (organizationModel.ContactPhoneNumber != null)
            {
                organizationElement.Add(GenerateTelecomElement(organizationModel.ContactPhoneNumber, true));
            }

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

        /// <summary>
        /// Создает элемент "author".
        /// </summary>
        /// <param name="authorDateModel">Модель автора документа.</param>
        /// <returns>Элемент "author".</returns>
        private static XElement GenerateAuthorElement(AuthorDataModel authorDateModel)
        {
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

            if (authorModel.ContactPhoneNumber != null)
            {
                assignedElement.Add(GenerateTelecomElement(authorModel.ContactPhoneNumber));
            }

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

            XElement insurancePolicyTypeElement = new XElement(identityNamespace + "InsurancePolicyType",
                GetTypeElementAttributes(
                    typeValue: "CD",
                    codeValue: basisDocumentModel.InsurancePolicyType.Code,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1035",
                    codeSystemVersionValue: basisDocumentModel.InsurancePolicyType.CodeSystemVersion,
                    codeSystemNameValue: "Виды полиса обязательного медицинского страхования",
                    displayNameValue: basisDocumentModel.InsurancePolicyType.DisplayName));
            docInfoElement.Add(insurancePolicyTypeElement);

            XElement seriesElement = new XElement(identityNamespace + "Series",
                new XAttribute(xsiNamespace + "type", "ST"),
                basisDocumentModel.Series);
            docInfoElement.Add(seriesElement);

            XElement numberElement = new XElement(identityNamespace + "Number",
                new XAttribute(xsiNamespace + "type", "ST"),
                basisDocumentModel.Number);
            docInfoElement.Add(numberElement);

            XElement INNElement = new XElement(identityNamespace + "INN",
                new XAttribute(xsiNamespace + "type", "ST"),
                basisDocumentModel.INN);
            docInfoElement.Add(INNElement);

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
        private static XElement GenerateEffectiveTimeElement(DateTime startDate, DateTime finishDate, XNamespace namespaceValue, bool isUseTime = false)
        {
            string startDateString;
            string finishDateString;
            if (isUseTime)
            {
                startDateString = startDate.ToString("yyyyMMddHHmm+0300");
                finishDateString = finishDate.ToString("yyyyMMddHHmm+0300");
            }
            else
            {
                startDateString = startDate.ToString("yyyyMMdd");
                finishDateString = finishDate.ToString("yyyyMMdd");
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

            XElement highElement = new XElement(namespaceValue + "high");
            if (!isUseTime)
            {
                XAttribute typeAttribute = new XAttribute(xsiNamespace + "type", "TS");
                highElement.Add(typeAttribute);
            }
            XAttribute highValueAttribute = new XAttribute("value", finishDateString);
            highElement.Add(highValueAttribute);
            effectiveTimeElement.Add(highElement);

            return effectiveTimeElement;
        }

        /// <summary>
        /// Создает элемент "documentationOf".
        /// </summary>
        /// <param name="serviceEventModel">Модель сведений о документируемом событии.</param>
        /// <returns></returns>
        private static XElement GenerateDocumentationOfElement(ServiceEventModel serviceEventModel)
        {
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

            serviceEventElement.Add(GenerateEffectiveTimeElement(serviceEventModel.StartServiceDate, serviceEventModel.FinishServiceDate, xmlnsNamespace, true));

            XElement serviceFormElement = new XElement(medServiceNamespace + "serviceForm",
                GetTypeElementAttributes(
                    codeValue: serviceEventModel.ServiceForm.Code,
                    codeSystemVersionValue: serviceEventModel.ServiceForm.CodeSystemVersion,
                    displayNameValue: serviceEventModel.ServiceForm.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1551",
                    codeSystemNameValue: "Формы оказания медицинской помощи"));
            serviceEventElement.Add(serviceFormElement);

            XElement serviceTypeElement = new XElement(medServiceNamespace + "serviceType",
                GetTypeElementAttributes(
                    codeValue: serviceEventModel.ServiceType.Code,
                    codeSystemVersionValue: serviceEventModel.ServiceType.CodeSystemVersion,
                    displayNameValue: serviceEventModel.ServiceType.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.11.1034",
                    codeSystemNameValue: "Виды медицинской помощи"));
            serviceEventElement.Add(serviceTypeElement);

            XElement serviceCondElement = new XElement(medServiceNamespace + "serviceCond",
                GetTypeElementAttributes(
                    codeValue: serviceEventModel.ServiceCond.Code,
                    codeSystemVersionValue: serviceEventModel.ServiceCond.CodeSystemVersion,
                    displayNameValue: serviceEventModel.ServiceCond.DisplayName,
                    codeSystemValue: "1.2.643.5.1.13.13.99.2.322",
                    codeSystemNameValue: "Условия оказания медицинской помощи"));
            serviceEventElement.Add(serviceCondElement);

            serviceEventElement.Add(GeneratePerformerElement(serviceEventModel.Performer, "PPRF"));
            
            foreach(var performer in serviceEventModel.OtherPerformers)
            {
                serviceEventElement.Add(GeneratePerformerElement(performer, "SPRF"));
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
            XElement componentElement = new XElement(xmlnsNamespace + "component");
            XElement structuredBodyElement = new XElement(xmlnsNamespace + "structuredBody");

            structuredBodyElement.Add(GenerateSentSectionElement(documentBodyModel.SentSection));
            structuredBodyElement.Add(GenerateWorkLocationSectionElement(documentBodyModel.WorkplaceSection));
            structuredBodyElement.Add(GenerateEducationSectionElement(documentBodyModel.EducationSection));
            structuredBodyElement.Add(GenerateAnamnezSectionElement(documentBodyModel.AnamnezSection));

            componentElement.Add(structuredBodyElement);
            return componentElement;
        }

        /// <summary>
        /// Создает элемент "component" с наполнением секции "Направление".
        /// </summary>
        /// <param name="sentSectionModel">Модель секции "Направление".</param>
        /// <returns>Элемент "component" с наполнением секции "Направление".</returns>
        private static XElement GenerateSentSectionElement(SentSectionModel sentSectionModel)
        {
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
            foreach (var element in representedOrganizationElement.Elements(xmlnsNamespace + "id"))
            {
                element.Add(new XAttribute("extension", "1145"));
            }
            assignedEntityElement.Add(representedOrganizationElement);

            performerElement.Add(assignedEntityElement);
            observationElement.Add(performerElement);
            entryElement.Add(observationElement);
            sectionElement.Add(entryElement);

            #endregion

            #region entry class

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

            #endregion

            #region entry spetiality

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

            foreach (var workPerformed in workActivityModel.WorkPerformeds)
            {
                componentElement = GenerateWorkPerformedElement(workPerformed);
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

            actElement.Add(GenerateEntryRelationshipElements(
                targetSentModel.TargetSentType,
                targetSentModel.SentOrder,
                targetSentModel.Protocol,
                targetSentModel.IsAtHome,
                targetSentModel.IsPalleativeMedicalHelp,
                targetSentModel.NeedPrimaryProsthetics,
                targetSentModel.SentDate));

            return actElement;
        }

        /// <summary>
        /// Создает списков элементов "entryRelationship".
        /// </summary>
        /// <param name="targetSentType">Цель направления.</param>
        /// <param name="sentOrder">Порядок обращения.</param>
        /// <param name="protocol">Протокол врачебной комиссии.</param>
        /// <param name="isAtHome">Экспертиза проводится на дому.</param>
        /// <param name="isPalleativeMedicalHelp">Нуждаемость в оказании паллиативной медицинской помощи.</param>
        /// <param name="needPrimaryProsthetics">Нуждаемость в первичном протезировании.</param>
        /// <param name="sentDate">Дата выдачи направления.</param>
        /// <returns>Список элементов "entryRelationship".</returns>
        private static List<XElement> GenerateEntryRelationshipElements(
            TypeModel targetSentType = null,
            TypeModel sentOrder = null,
            ProtocolModel protocol = null,
            bool? isAtHome = null,
            bool? isPalleativeMedicalHelp = null,
            bool? needPrimaryProsthetics = null,
            DateTime? sentDate = null)
        {
            List<XElement> entryRelationshipElements = new List<XElement>();

            if (targetSentType != null)
            {
                entryRelationshipElements.Add(GenerateEntryRelationshipElement(
                    "sentTarget",
                    targetSentType));
            }
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

            XElement participantElement = new XElement(xmlnsNamespace + "participant",
                new XAttribute("typeCode", "LOC"));

            participantElement.Add(GenerateOrganizationElement(patientLocation, "participantRole"));

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

            if (anamnezSectionModel.DegreeDisability != null)
            {
                XElement degreeDisabilityParagraphElement = new XElement(xmlnsNamespace + "paragraph");

                XElement degreeDisabilityCaptionElement = new XElement(xmlnsNamespace + "caption", "Степень утраты профессиональной трудоспособности");
                degreeDisabilityParagraphElement.Add(degreeDisabilityCaptionElement);

                if (anamnezSectionModel.DegreeDisability.Section31Text != null)
                {
                    XElement degreeDisabilityContentElement = new XElement(xmlnsNamespace + "content",
                        new XAttribute("ID", "socanam31"), anamnezSectionModel.DegreeDisability.Section31Text);
                    degreeDisabilityParagraphElement.Add(degreeDisabilityContentElement);
                    degreeDisabilityParagraphElement.Add(NewLineElement);
                }

                if (anamnezSectionModel.DegreeDisability.Section32Text != null)
                {
                    XElement degreeDisabilityContentElement = new XElement(xmlnsNamespace + "content",
                        new XAttribute("ID", "socanam32"), anamnezSectionModel.DegreeDisability.Section32Text);
                    degreeDisabilityParagraphElement.Add(degreeDisabilityContentElement);
                    degreeDisabilityParagraphElement.Add(NewLineElement);
                }

                if (anamnezSectionModel.DegreeDisability.Section33Text != null)
                {
                    XElement degreeDisabilityContentElement = new XElement(xmlnsNamespace + "content",
                        new XAttribute("ID", "socanam33"), anamnezSectionModel.DegreeDisability.Section33Text);
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
                var contentParagraphElement = paragraphElement.Elements(xmlnsNamespace + "content");
                foreach (var element in contentParagraphElement)
                {
                    element.Add(new XAttribute("ID", "socanam4"));
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

                foreach (var temporaryDisability in anamnezSectionModel.TemporaryDisabilitys)
                {
                    XElement trContentElement = new XElement(xmlnsNamespace + "tr");

                    XElement td1Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DateStart.ToString("dd.MM.yyyy"));
                    trContentElement.Add(td1Element);
                    XElement td2Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DateFinish.ToString("dd.MM.yyyy"));
                    trContentElement.Add(td2Element);
                    XElement td3Element = new XElement(xmlnsNamespace + "td", temporaryDisability.DayCount);
                    trContentElement.Add(td3Element);
                    XElement td4Element = new XElement(xmlnsNamespace + "td", temporaryDisability.CipherMKB);
                    trContentElement.Add(td4Element);

                    tbodyElement.Add(trContentElement);
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

                foreach (var temporaryDisability in anamnezSectionModel.TemporaryDisabilitys)
                {
                    temporaryDisabilitysOrganizerElement.Add(GenerateTemporaryDisabilitysElement(temporaryDisability));
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
            if (anamnezSectionModel.DegreeDisability != null)
            {
                if (anamnezSectionModel.DegreeDisability.Section31Text != null &&
                    anamnezSectionModel.DegreeDisability.Section31DateTo != null &&
                    anamnezSectionModel.DegreeDisability.Section31Time != null &&
                    anamnezSectionModel.DegreeDisability.Section31Percent != null)
                {
                    entryElements.Add(GenerateDegreeDisabilityElement(
                        anamnezSectionModel.DegreeDisability.Section31DateTo, 
                        anamnezSectionModel.DegreeDisability.Section31Time, 
                        anamnezSectionModel.DegreeDisability.Section31Percent,
                        "socanam31"));
                }
                if (anamnezSectionModel.DegreeDisability.Section32Text != null &&
                    anamnezSectionModel.DegreeDisability.Section32DateTo != null &&
                    anamnezSectionModel.DegreeDisability.Section32Time != null &&
                    anamnezSectionModel.DegreeDisability.Section32Percent != null)
                {
                    entryElements.Add(GenerateDegreeDisabilityElement(
                        anamnezSectionModel.DegreeDisability.Section32DateTo,
                        anamnezSectionModel.DegreeDisability.Section32Time,
                        anamnezSectionModel.DegreeDisability.Section32Percent,
                        "socanam32"));
                }
                if (anamnezSectionModel.DegreeDisability.Section33Text != null &&
                    anamnezSectionModel.DegreeDisability.Section33DateTo != null &&
                    anamnezSectionModel.DegreeDisability.Section33Time != null &&
                    anamnezSectionModel.DegreeDisability.Section33Percent != null)
                {
                    entryElements.Add(GenerateDegreeDisabilityElement(
                        anamnezSectionModel.DegreeDisability.Section33DateTo,
                        anamnezSectionModel.DegreeDisability.Section33Time,
                        anamnezSectionModel.DegreeDisability.Section33Percent,
                        "socanam33"));
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
            XElement lowElement = new XElement(xmlnsNamespace + "low",
                new XAttribute("value", temporaryDisabilityModel.DateStart.ToString("yyyyMMdd")));
            XElement highElement = new XElement(xmlnsNamespace + "high",
                new XAttribute("value", temporaryDisabilityModel.DateFinish.ToString("yyyyMMdd")));
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
            XElement highElement = new XElement(xmlnsNamespace + "high",
                new XAttribute("value", degreeDisabilityDateTo?.ToString("yyyyMMdd")));
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
        /// Получить значения "code" элемента "entryRelationship".
        /// </summary>
        /// <param name="entryRelationshipElementName">Наименование элемента "entryRelationship".</param>
        /// <returns>Значения "code" элемента "entryRelationship".</returns>
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
                { "resultCompensationFunctionAnamnez", ("4065", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Достижение компенсации утраченных/отсутствующих функций") }

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
