using GenerateMedicalDocuments;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;

namespace GenerateMedicalDocumentsTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var documentModel = GetDocumentModel();

            DirectionToMSE directionToMSE = new DirectionToMSE();
            var xmlDocument = directionToMSE.GetDirectionTOMSEDocumentXML(documentModel);
            directionToMSE.SaveDocument(xmlDocument, "testDocument.xml");
        }

        private static DirectionToMSEDocumentModel GetDocumentModel()
        {
            DirectionToMSEDocumentModel documentModel = new DirectionToMSEDocumentModel();

            documentModel = new DirectionToMSEDocumentModel()
            {
                CreateDate = new DateTime(2021, 06, 20, 16, 10, 00),
                VersionNumber = 2,
                ID = new IDType()
                {
                    Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.51",
                    Extension = "7854321"
                },
                SetID = new IDType()
                {
                    Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.50",
                    Extension = "7854321"
                },
                RecordTarget = new RecordTargetModel()
                {
                    PatientRole = new PatientModel()
                    {
                        ID = new IDType()
                        {
                            Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.10",
                            Extension = "585233"
                        },
                        SNILS = "44578444510",
                        IdentityDocument = new DocumentModel()
                        {
                            IdentityCardType = new TypeModel()
                            {
                                Code = "1",
                                CodeSystemVersion = "5.1",
                                DisplayName = "Паспорт гражданина Российской Федерации"
                            },
                            Series = 1234,
                            Number = 123456,
                            IssueOrgName = "ОВД 'Твардовское' ОУФМС России по городу Москве",
                            IssueOrgCode = "770-095",
                            IssueDate = new DateTime(2005, 02, 18)
                        },
                        InsurancePolicy = new InsurancePolicyModel()
                        {
                            InsurancePolicyType = new TypeModel()
                            {
                                Code = "1",
                                CodeSystemVersion = "1.3",
                                DisplayName = "Полис ОМС старого образца"
                            },
                            Series = "ЧБ",
                            Number = "1334602"
                        },
                        PermanentAddress = new AddressModel()
                        {
                            Type = new TypeModel
                            {
                                Code = "1",
                                CodeSystemVersion = "1.3",
                                DisplayName = "Адрес по месту жительства (постоянной регистрации)"
                            },
                            StreetAddressLine = "г Москва, ул Исаковского, д 28 к 2, кв 589",
                            StateCode = new TypeModel
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 123181,
                            AOGUID = new Guid("d1c8d6db-f1b9-49db-895e-c3a93997da77"),
                            HOUSEGUID = new Guid("6e9eda8d-22d9-481a-bb4f-a5c221c194a4")
                        },
                        ActualAddress = new AddressModel()
                        {
                            Type = new TypeModel()
                            {
                                Code = "3",
                                CodeSystemVersion = "1.3",
                                DisplayName = "Адрес фактического проживания (пребывания)"
                            },
                            StreetAddressLine = "г Москва, ул Исаковского, д 28 к 2, кв 589",
                            StateCode = new TypeModel()
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 123181,
                            AOGUID = new Guid("d1c8d6db-f1b9-49db-895e-c3a93997da77"),
                            HOUSEGUID = new Guid("6e9eda8d-22d9-481a-bb4f-a5c221c194a4")
                        },
                        ContactPhoneNumber = new TelecomModel()
                        {
                            Value = "+74954243210"
                        },
                        Contacts = new List<TelecomModel>()
                        {
                            { new TelecomModel() { Value = "+79161234567", Use = "MC" } },
                            { new TelecomModel() { Value = "bogekat@mail.ru"} }
                        },
                        PatientData = new PeopleDataModel()
                        {
                            Name = new NameModel()
                            {
                                Family = "Богатырева",
                                Given = "Екатерина",
                                Patronymic = "Ивановна"
                            },
                            Gender = new TypeModel()
                            {
                                Code = "2",
                                CodeSystemVersion = "2.1",
                                DisplayName = "Женский"
                            },
                            BirthDate = new DateTime(1985, 03, 31)
                        },
                        Guardian = new GuardianModel()
                        {
                            SNILS = "44578444510",
                            IdentityDocument = new DocumentModel()
                            {
                                IdentityCardType = new TypeModel()
                                {
                                    Code = "1",
                                    CodeSystemVersion = "5.1",
                                    DisplayName = "Паспорт гражданина Российской Федерации"
                                },
                                Series = 4509,
                                Number = 356432,
                                IssueOrgName = "ОВД 'Твардовское' ОУФМС России по городу Москве",
                                IssueOrgCode = "770-095",
                                IssueDate = new DateTime(1992, 02, 18)
                            },
                            AuthorityDocument = new DocumentModel()
                            {
                                IdentityCardType = new TypeModel()
                                {
                                    Code = "2",
                                    CodeSystemVersion = "1.2",
                                    DisplayName = "Решение о назначении лица опекуном"
                                },
                                Series = 1122,
                                Number = 334455,
                                IssueOrgName = "Орган опеки и попечительства",
                                IssueDate = new DateTime(2000, 02, 18)
                            },
                            ActualAddress = new AddressModel()
                            {
                                StreetAddressLine = "г Москва, ул Исаковского, д 28 к 2, кв 589",
                                StateCode = new TypeModel()
                                {
                                    Code = "77",
                                    CodeSystemVersion = "6.3",
                                    DisplayName = "г. Москва"
                                },
                                PostalCode = 123181,
                                AOGUID = new Guid("d1c8d6db-f1b9-49db-895e-c3a93997da77"),
                                HOUSEGUID = new Guid("6e9eda8d-22d9-481a-bb4f-a5c221c194a4")
                            },
                            ContactPhoneNumber = new TelecomModel()
                            {
                                Value = "+74954243201"
                            },
                            Contacts = new List<TelecomModel>()
                            {
                                { new TelecomModel() { Value = "+79161234576", Use = "MC"} },
                                { new TelecomModel() { Value = "bogelen@mail.ru"} }
                            },
                            Name = new NameModel()
                            {
                                Family = "Богатырева",
                                Given = "Елена",
                                Patronymic = "Николаевна"
                            }
                        },
                        ProviderOrganization = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.2.77.7973",
                            License = new LicenseModel()
                            {
                                Number = "ЛО-77-01-018109",
                                AssigningAuthorityName = "Департамент здравоохранения города Москвы. Дата регистрации: 23.05.2019"
                            },
                            Props = new PropsOrganizationModel()
                            {
                                OGRN = "1037734008575",
                                OKATO = "45283577000"
                            },
                            Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                            ContactPhoneNumber = new TelecomModel()
                            {
                                Value = "+74957503971",
                                Use = "WP"
                            },
                            Contacts = new List<TelecomModel>()
                            {
                                { new TelecomModel() { Value = "74957503971", Use = "WP"} },
                                { new TelecomModel() { Value = "https://gp180.mos.ru/"} }
                            },
                            Address = new AddressModel()
                            {
                                StreetAddressLine = "г Москва, ул Кулакова, д 23",
                                StateCode = new TypeModel()
                                {
                                    Code = "77",
                                    CodeSystemVersion = "6.3",
                                    DisplayName = "г. Москва"
                                },
                                PostalCode = 123592,
                                AOGUID = new Guid("13952531-8e6d-4540-b249-814478b00c6b"),
                                HOUSEGUID = new Guid("f9816342-0e35-47b6-87c7-379340011ff3")
                            }
                        }
                    }
                }
            };

            return documentModel;
        }
    }
}
