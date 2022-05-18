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
            structuredBodyElement.Add(GenerateWorkLocationSection(documentBodyModel.WorkplaceSection));

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
        private static XElement GenerateWorkLocationSection(WorkplaceSectionModel workplaceSectionModel)
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
        /// Создает элемент "text" с наполнением секции "Напарвление".
        /// </summary>
        /// <param name="paragraphs">Список параграфов.</param>
        /// <returns>Элемент "text" с наполнением секции "Напарвление".</returns>
        private static XElement GenerateParagraphsElements(List<ParagraphModel> paragraphs)
        {
            XElement textElement = new XElement(xmlnsNamespace + "text");

            foreach (var paragraph in paragraphs)
            {
                XElement paragraphElement = new XElement(xmlnsNamespace + "paragraph");

                XElement captionElement = new XElement(xmlnsNamespace + "caption", paragraph.Caption);
                XElement contentElement = new XElement(xmlnsNamespace + "content", paragraph.Content);
                paragraphElement.Add(captionElement);
                paragraphElement.Add(contentElement);

                textElement.Add(paragraphElement);

                XElement newLineElement = new XElement(xmlnsNamespace + "br");
                textElement.Add(newLineElement);
            }

            return textElement;
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
                { "workAtTimeWorkpalceActivity", ("4077", "1.2.643.5.1.13.13.99.2.166", "1.69", "Кодируемые поля CDA документов", "Выполняемая работа на момент направления на медико-социальную экспертизу") }

                #endregion
            };

            return values[elementName];
        }

        #endregion
    }
}
