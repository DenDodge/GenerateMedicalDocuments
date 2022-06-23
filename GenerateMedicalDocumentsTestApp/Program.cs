using GenerateMedicalDocuments;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GenerateMedicalDocumentsTestApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var documentModel = await GetDocumentModel();
            //var documentModel = await GetDocumentModelOnJson();

            DirectionToMSE directionToMSE = new DirectionToMSE();
            // var xmlDocument = directionToMSE.GetDirectionToMSEDocumentXML(documentModel);
            // directionToMSE.SaveDocument(xmlDocument, "xmlDocument.xml");

            directionToMSE.CreationHTMLDocument(documentModel, "htmlDocument.html");
        }

        /// <summary>
        /// Получение тестовых данных документа.
        /// </summary>
        /// <returns>Тестовые данные документа.</returns>
        private static async Task<DirectionToMSEDocumentModel> GetDocumentModel()
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
                            { new TelecomModel() { Value = "bogekat@mail.ru" } }
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
                                { new TelecomModel() { Value = "+79161234576", Use = "MC" } },
                                { new TelecomModel() { Value = "bogelen@mail.ru" } }
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
                                { new TelecomModel() { Value = "74957503971", Use = "WP" } },
                                { new TelecomModel() { Value = "https://gp180.mos.ru/" } }
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
                },
                Author = new AuthorDataModel()
                {
                    SignatureDate = new DateTime(2021, 06, 20, 12, 20, 00),
                    Author = new AuthorModel()
                    {
                        ID = new IDType()
                        {
                            Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.70",
                            Extension = "2341"
                        },
                        SNILS = "32165477709",
                        Position = new TypeModel()
                        {
                            Code = "122",
                            CodeSystemVersion = "7.5",
                            DisplayName = "врач-хирург"
                        },
                        ActualAddress = new AddressModel()
                        {
                            StreetAddressLine = "г Москва, Ленинградский пр-кт, д 78 к 3, кв 12",
                            StateCode = new TypeModel()
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 125315,
                            AOGUID = new Guid("9c3e9392-0324-4d21-9cf5-70076f1b5e15"),
                            HOUSEGUID = new Guid("417432da-c526-4158-aa01-fb083797ac5e")
                        },
                        ContactPhoneNumber = new TelecomModel()
                        {
                            Value = "+74954241311"
                        },
                        Contacts = new List<TelecomModel>()
                        {
                            { new TelecomModel() { Use = "WP", Value = "+79261234588"} },
                            { new TelecomModel() { Value = "a.privalov@oblhosp.volgograd.ru" } },
                            { new TelecomModel() { Value = "74954241311" } }
                        },
                        Name = new NameModel()
                        {
                            Family = "Привалов",
                            Given = "Александр",
                            Patronymic = "Иванович"
                        },
                        RepresentedOrganization = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.2.77.7973",
                            Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                            ContactPhoneNumber = new TelecomModel()
                            {
                                Use = "WP",
                                Value = "+74957503971"
                            },
                            Contacts = new List<TelecomModel>()
                            {
                                { new TelecomModel() { Use = "WP", Value = "74957503971" } },
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
                },
                RepresentedCustodianOrganization = new OrganizationModel()
                {
                    ID = "1.2.643.5.1.13.13.12.2.77.7973",
                    Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                    ContactPhoneNumber = new TelecomModel()
                    {
                        Use = "WP",
                        Value = "+74957503971"
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
                },
                LegalAuthenticator = new LegalAuthenticatorModel()
                {
                    SignatureDate = new DateTime(2021, 06, 20, 16, 10, 00),
                    AssignedEntity = new AuthorModel()
                    {
                        ID = new IDType()
                        {
                            Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.70",
                            Extension = "2341"
                        },
                        SNILS = "88599674111",
                        Position = new TypeModel()
                        {
                            Code = "7",
                            CodeSystemVersion = "7.5",
                            DisplayName = "Заведующий отделением"
                        },
                        ActualAddress = new AddressModel()
                        {
                            StreetAddressLine = "г Москва, Мичуринский пр-кт, д 16, кв 9",
                            StateCode = new TypeModel()
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 119192,
                            AOGUID = new Guid("0f072841-643f-4081-baf0-b16760fede91"),
                            HOUSEGUID = new Guid("4f22318c-43a4-4040-872d-6d0b88bbe000")
                        },
                        ContactPhoneNumber = new TelecomModel()
                        {
                            Value = "+74954244567"
                        },
                        Contacts = new List<TelecomModel>()
                        {
                            { new TelecomModel() { Use = "MC", Value = "+79031234588" } },
                            { new TelecomModel() { Value = "steaf@gmail.com" } },
                            { new TelecomModel() { Value = "74954244567"} }
                        },
                        Name = new NameModel()
                        {
                            Family = "Степанов",
                            Given = "Андрей",
                            Patronymic = "Фёдорович"
                        },
                        RepresentedOrganization = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.2.77.7973",
                            Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                            ContactPhoneNumber = new TelecomModel()
                            {
                                Use = "WP",
                                Value = "+74957503971"
                            },
                            Contacts = new List<TelecomModel>()
                            {
                                { new TelecomModel() { Use = "WP", Value = "74957503971" } },
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
                },
                Participant = new ParticipantModel()
                {
                    Code = new TypeModel()
                    {
                        Code = "1",
                        CodeSystemVersion = "5.1",
                        DisplayName = "ОМС"
                    },
                    DocInfo = new BasisDocumentModel()
                    {
                         IdentityDocType = new TypeModel()
                         {
                             Code = "1",
                             CodeSystemVersion = "1.1",
                             DisplayName = "Полис ОМС"
                         },
                         InsurancePolicyType = new TypeModel()
                         {
                             Code = "1",
                             CodeSystemVersion = "1.3",
                             DisplayName = "Полис ОМС старого образца"
                         },
                         Series = "ЧБ",
                         Number = "1334602",
                         INN = 213546789,
                         StartDateDocument = new DateTime(2019, 05, 01),
                         FinishDateDocument = new DateTime(2029, 05, 02)
                    },
                    ScopingOrganization = new OrganizationModel()
                    {
                        ID = "77013",
                        Name = "ООО \"СК \"ИНГОССТРАХ-М\"",
                        ContactPhoneNumber = new TelecomModel()
                        {
                            Use = "WP",
                            Value = "+74957295571"
                        },
                        Address = new AddressModel()
                        {
                            StreetAddressLine = "г Москва, ул Рочдельская, д 15 стр 35",
                            StateCode = new TypeModel()
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 123376,
                            AOGUID = new Guid("6bfbfcbb-87a7-4674-a8e5-bf5f1bbfbf92"),
                            HOUSEGUID = new Guid("0b33a0e1-3427-409a-a3b7-e57716819f38")
                        }
                    }
                },
                ServiceEvent = new ServiceEventModel()
                {
                    Code = new TypeModel()
                    {
                        Code = "5",
                        CodeSystemVersion = "2.3",
                        DisplayName = "Врачебная комиссия"
                    },
                    StartServiceDate = new DateTime(2021, 06, 20, 10, 10, 00),
                    FinishServiceDate = new DateTime(2021, 06, 20, 16, 10, 00),
                    ServiceForm = new TypeModel()
                    {
                        Code = "1",
                        CodeSystemVersion = "1.1",
                        DisplayName = "плановая"
                    },
                    ServiceType = new TypeModel()
                    {
                        Code = "2",
                        CodeSystemVersion = "4.2",
                        DisplayName = "Первичная врачебная медико-санитарная помощь"
                    },
                    ServiceCond = new TypeModel()
                    {
                        Code = "2",
                        CodeSystemVersion = "1.2",
                        DisplayName = "Амбулаторно"
                    },
                    Performer = new PerformerModel()
                    {
                        ID = new IDType()
                        {
                            Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.70",
                            Extension = "2341"
                        },
                        SNILS = "88599674111",
                        Position = new TypeModel()
                        {
                            Code = "7",
                            CodeSystemVersion = "7.5",
                            DisplayName = "заведующий (начальник) структурного подразделения (отдела, отделения, лаборатории, кабинета, отряда и другое) медицинской организации - врач-специалист"
                        },
                        Address = new AddressModel()
                        {
                            StreetAddressLine = "г Москва, Мичуринский пр-кт, д 16, кв 9",
                            StateCode = new TypeModel()
                            {
                                Code = "77",
                                CodeSystemVersion = "6.3",
                                DisplayName = "г. Москва"
                            },
                            PostalCode = 119192,
                            AOGUID = new Guid("0f072841-643f-4081-baf0-b16760fede91"),
                            HOUSEGUID = new Guid("4f22318c-43a4-4040-872d-6d0b88bbe000")
                        },
                        ContactPhoneNumber = new TelecomModel()
                        {
                            Value = "+74954244567"
                        },
                        Contacts = new List<TelecomModel>()
                        {
                            { new TelecomModel() { Use = "MC", Value = "+79031234588" } },
                            { new TelecomModel() { Value = "steaf@gmail.com" } },
                            { new TelecomModel() { Value = "74954244567" } }
                        },
                        Name = new NameModel()
                        {
                            Family = "Степанов",
                            Given = "Андрей",
                            Patronymic = "Фёдорович"
                        },
                        RepresentedOrganization = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.2.77.7973",
                            Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                            ContactPhoneNumber = new TelecomModel()
                            {
                                Use = "WP",
                                Value = "+74957503971"
                            },
                            Contacts = new List<TelecomModel>()
                            {
                                { new TelecomModel() { Use = "WP", Value = "74957503971"} },
                                { new TelecomModel() { Value = "https://gp180.mos.ru/" } }
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
                    },
                    OtherPerformers = new List<PerformerModel>()
                    {
                        {
                            new PerformerModel()
                            {
                                ID = new IDType()
                                {
                                    Root = "1.2.643.5.1.13.13.12.2.77.7973.100.1.1.70",
                                    Extension = "2341"
                                },
                                SNILS = "32165477709",
                                Position = new TypeModel()
                                {
                                    Code = "122",
                                    CodeSystemVersion = "7.5",
                                    DisplayName = "врач-хирург"
                                },
                                ActualAddress = new AddressModel()
                                {
                                    StreetAddressLine = "г Москва, Ленинградский пр-кт, д 78 к 3, кв 12",
                                    StateCode = new TypeModel()
                                    {
                                        Code = "77",
                                        CodeSystemVersion = "6.3",
                                        DisplayName = "г. Москва"
                                    },
                                    PostalCode = 125315,
                                    AOGUID = new Guid("9c3e9392-0324-4d21-9cf5-70076f1b5e15"),
                                    HOUSEGUID = new Guid("417432da-c526-4158-aa01-fb083797ac5e")
                                },
                                ContactPhoneNumber = new TelecomModel()
                                {
                                    Value = "+74954241311"
                                },
                                Contacts = new List<TelecomModel>()
                                {
                                    { new TelecomModel() { Use = "MC", Value = "+79261234588" } },
                                    { new TelecomModel() { Value = "a.privalov@oblhosp.volgograd.ru" } },
                                    { new TelecomModel() { Value = "74954241311" } }
                                },
                                Name = new NameModel()
                                {
                                    Family = "Привалов",
                                    Given = "Александр",
                                    Patronymic = "Иванович"
                                },
                                RepresentedOrganization = new OrganizationModel()
                                {
                                    ID = "1.2.643.5.1.13.13.12.2.77.7973",
                                    Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1",
                                    ContactPhoneNumber = new TelecomModel()
                                    {
                                        Use = "WP",
                                        Value = "+74957503971"
                                    },
                                    Contacts = new List<TelecomModel>()
                                    {
                                        { new TelecomModel() { Use = "WP", Value = "74957503971" } },
                                        { new TelecomModel() { Value = "https://gp180.mos.ru/" } }
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
                    }
                },
                DocumentBody = new DocumentBodyModel()
                {
                    SentSection = new SentSectionModel()
                    {
                        Code = new TypeModel()
                        {
                            Code = "SCOPORG",
                            CodeSystemVersion = "1.18",
                            DisplayName = "Цель направления и медицинская организация, куда направлен"
                        },
                        SentParagraphs = new List<ParagraphModel>()
                        {
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин направляется на медико-социальную экспертизу",
                                    Content = new List<string>() { "повторно" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Цель направления",
                                    Content = new List<string>() { "для разработки индивидуальной программы реабилитации инвалида" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Протокол врачебной комиссии медицинской организации, содержащий решение о направлении гражданина на медико-социальную экспертизу",
                                    Content = new List<string>() { "№ 123 от 20 мая 2018 г." }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин по состоянию здоровья не может явиться в бюро (главное бюро, Федеральное бюро) медико-социальной экспертизы",
                                    Content = new List<string>() { "медико-социальную экспертизу необходимо проводить на дому" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Нуждаемость в оказании паллиативной медицинской помощи",
                                    Content = new List<string>() { "гражданин нуждается в паллиативной медицинской помощи" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Нахождение на лечении в стационаре в связи с операцией по ампутации  (реампутации)  конечности (конечностей), нуждающийся в первичном протезировании",
                                    Content = new List<string>() { "гражданин не нуждается в первичном протезировании" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Дата выдачи гражданину направления на медико-социальную экспертизу медицинской организацией",
                                    Content = new List<string>() { "20 июня 2018 г." }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданство",
                                    Content = new List<string>() { "гражданин Российской Федерации" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин находится",
                                    Content = new List<string>() { "текст" }
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Отношения к воинской обязанности",
                                    Content = new List<string>() { "гражданин, не состоящий на воинском учёте" }
                                }
                            }
                        },
                        TargetSent = new TargetSentModel()
                        {
                            SentType = new TypeModel()
                            {
                                Code = "34",
                                CodeSystemVersion = "4.45",
                                DisplayName = "Направление на медико-социальную экспертизу"
                            },
                            PerformerOrganization = new OrganizationModel()
                            {
                                ID = "1.2.643.5.1.13.13.12.2.77.1270",
                                Name = "ФКУ \"ГБ МСЭ ФМБА России\"",
                                ContactPhoneNumber = new TelecomModel()
                                {
                                    Use = "WP",
                                    Value = "+74957545117"
                                },
                                Address = new AddressModel()
                                {
                                    StreetAddressLine = "г Москва, ул Гамалеи, д 13",
                                    StateCode = new TypeModel()
                                    {
                                        Code = "77",
                                        CodeSystemVersion = "6.3",
                                        DisplayName = "г. Москва"
                                    },
                                    PostalCode = 123098,
                                    AOGUID = new Guid("57cda4e0-989a-4727-964d-4ef890272bf5"),
                                    HOUSEGUID = new Guid("c9d59fb9-91f2-4229-b8a9-55fe602c3c26")
                                }
                            },
                            TargetSentType = new TypeModel()
                            {
                                Code = "10",
                                CodeSystemVersion = "1.5",
                                DisplayName = "Разработка индивидуальной программы реабилитации или абилитации инвалида (ребенка-инвалида)"
                            },
                            SentOrder = new TypeModel()
                            {
                                Code = "2",
                                CodeSystemVersion = "2.1",
                                DisplayName = "Повторный"
                            },
                            Protocol = new ProtocolModel()
                            {
                                Protocol = new TypeModel()
                                {
                                    Code = "4059",
                                    CodeSystemVersion = "1.69",
                                    DisplayName = "Протокол врачебной комиссии"
                                },
                                ProtocolDate = new DateTime(2021, 05, 20, 16, 10, 00),
                                ProtocolNumber = "123"
                            },
                            IsAtHome = true,
                            IsPalleativeMedicalHelp = true,
                            NeedPrimaryProsthetics = false,
                            SentDate = new DateTime(2021, 06, 20, 16, 10, 00)
                        },
                        Сitizenship = new TypeModel()
                        {
                            Code = "1",
                            CodeSystemVersion = "2.1",
                            DisplayName = "Гражданин Российской Федерации"
                        },
                        PatientLocationCode = new TypeModel()
                        {
                            Code = "4",
                            CodeSystemVersion = "1.1",
                            DisplayName = "Иная организация"
                        },
                        PatientLocation = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.2.77.7973",
                            Props = new PropsOrganizationModel()
                            {
                                OGRN = "1037734008575"
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
                        },
                        MilitaryDuty = new TypeModel()
                        {
                            Code = "4",
                            CodeSystemVersion = "1.2",
                            DisplayName = "Гражданин, не состоящий на воинском учёте"
                        }
                    },
                    WorkplaceSection = new WorkplaceSectionModel()
                    {
                        WorkPlaceParagraphs = new List<ParagraphModel>
                        {
                            new ParagraphModel { Caption = "Основная профессия", Content = new List<string>() { "текст" } },
                            new ParagraphModel { Caption = "Квалификация", Content = new List<string>() { "текст" } },
                            new ParagraphModel { Caption = "Стаж", Content = new List<string>() { "текст"} },
                            new ParagraphModel { Caption = "Выполняемая работа", Content = new List<string>() { "текст" } },
                            new ParagraphModel { Caption = "Условия труда", Content = new List<string>() { "текст" } },
                            new ParagraphModel { Caption = "Место работы", Content = new List<string>() { "текст" } },
                            new ParagraphModel { Caption = "Адрес места работы.", Content = new List<string>() { "текст" } },
                        },
                        WorkActivity = new WorkActivityModel()
                        {
                            Workpalace = new OrganizationModel()
                            {
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
                                },
                                Name = "ГБУЗ \"ГП №180 ДЗМ\" Филиал №1"
                            },
                            MainProfession = "Основная профессия (специальность, должность)",
                            Qualification = "Квалификация (класс, разряд, категория, звание)",
                            WorkExperience = "Стаж работы",
                            WorkPerformeds = new List<(string Profession, string Speciality, string Position)>()
                            {
                                ("Профессия", "Специальность", "Должность")
                            },
                            Conditions = "Условия и характер выполняемого труда"
                        }
                    },
                    EducationSection = new EducationSectionModel()
                    {
                        FillingSection = new ParagraphModel()
                        {
                            Caption = "Сведения о получении образования",
                            Content = new List<string>() { "Организация, адрес, курс, профессия." }
                        },
                        Organization = new OrganizationModel()
                        {
                            ID = "1.2.643.5.1.13.13.12.4.70.184",
                            Name = "Томское областное медицинское училище",
                            Address = new AddressModel()
                            {
                                StreetAddressLine = "г Томск, ул Смирнова, д 44",
                                StateCode = new TypeModel()
                                {
                                    Code = "70",
                                    CodeSystemVersion = "6.3",
                                    DisplayName = "Томская область"
                                },
                                PostalCode = 634027,
                                AOGUID = new Guid("1564e297-fc5d-453f-8b7e-a90d444e01e7"),
                                HOUSEGUID = new Guid("4e8ed1a2-13a5-4579-a17d-a692625b5aea")
                            }
                        },
                        Class = "2",
                        Spetiality = "Стоматология"
                    },
                    AnamnezSection = new AnamnezSectionModel()
                    {
                        Disability = new DisabilityModel()
                        {
                            Group = 3,
                            GroupOrder = "Повторно",
                            GroupTime = "бессрочно",
                            GroupText = "3 группа (установлена: повторно, бессрочно)",
                            TimeDisability = "четыре и более лет",
                            DateDisabilityStart = new DateTime(1999, 03, 28),
                            CauseOfDisability = "Общее заболевание"
                        },
                        DegreeDisability = new DegreeDisabilityModel()
                        {
                            Section31Text = "60% (на 1 год до 01.01.2019)",
                            Section31DateTo = new DateTime(2019, 01, 01),
                            Section31Time = "Один год",
                            Section31Percent = 60,
                            Section32Text = "80% (на 1 год до 01.01.2018)",
                            Section32DateTo = new DateTime(2018, 01, 01),
                            Section32Time = "Один год",
                            Section32Percent = 80,
                            Section33Text = "90% (на 1 год до 01.01.2017)",
                            Section33DateTo = new DateTime(2017, 01, 01),
                            Section33Time = "Один год",
                            Section33Percent = 90
                        },
                        SeenOrganizations = "с 2000  года.",
                        MedicalAnamnez = "Пациентка поступила для дообследования по поводу опухоли с поражением проксимального отдела правой голени и правого коленного сустава. При обследовании по данным морфологии выявлена саркома мягких тканей правой голени низкой степени злокачественности. Учитывая местную распространенность опухолевого процесса пациентке показано хирургическое лечение в объеме удаления опухоли с резекцией проксимального отдела правой большеберцовой кости и эндопротезированием.",
                        LifeAnamnez = "Эпидемиологический анамнез: контактов с инфекционными больными за время обращения не было. В эндемичных районах тех или иных инфекций, загрязнённых радиацией и химикатами территориях, за время обращения не находилась.",
                        ActualDevelopment = "физическое развитие (в отношении детей в возрасте до 3 лет)",
                        TemporaryDisabilitys = new List<TemporaryDisabilityModel>()
                        {
                            new TemporaryDisabilityModel()
                            {
                                DateStart = new DateTime(2018, 10, 2),
                                DateFinish = new DateTime(2018, 10, 10),
                                DayCount = "9 дней",
                                CipherMKB = "T23.2",
                                Diagnosis = "Термический ожог запястья и кисти второй степени"
                            },
                            new TemporaryDisabilityModel()
                            {
                                DateStart = new DateTime(2018, 05, 1),
                                DateFinish = new DateTime(2018, 05, 20),
                                DayCount = "20 дней",
                                CipherMKB = "I25.1",
                                Diagnosis = "Атеросклеротическая болезнь сердца"
                            }
                        },
                        CertificateDisabilityNumber = "№ ЭЛН 123456789",
                        EffectityAction = new List<string>()
                        {
                            "индивидуальная программа реабилитации инвалида № 123 к протоколу проведения медико-социальной экспертизы № 222 от 20 июля 2018 г.:",
                            "Восстановление нарушенных функций: частичное",
                            "Достижение компенсации утраченных либо отсутствующих функций: положительные результаты отсутствуют"
                        },
                        StartYear = 2000,
                        IPRANumber = "123",
                        ProtocolNumber = "123",
                        ProtocolDate = new DateTime(2018, 10, 01),
                        Results = "результаты",
                        ResultRestorationFunctions = "Частичное",
                        ResultCompensationFunction = "Частичное"
                    },
                    VitalParametersSection = new VitalParametersSectionModel()
                    {
                        VitalParameters = new List<VitalParameterModel>()
                        {
                            new VitalParameterModel()
                            {
                                Caption = "Масса тела",
                                EntryDisplayName = "Масса тела",
                                ID = "vv1_1",
                                Code = "50",
                                DateMetering = new DateTime(2021, 05, 25, 10, 10, 00),
                                Value = "80",
                                EntryValue = "80000",
                                Unit = "кг",
                                EntryUnit = "гр.",
                                EntryType = "PQ"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "Масса тела при рождении (в отношении детей в возрасте до 3 лет)",
                                EntryDisplayName = "Масса тела",
                                ID = "vv1_11",
                                Code = "50",
                                DateMetering = new DateTime(1985, 03, 31),
                                Value = "4",
                                EntryValue = "4000",
                                Unit = "кг",
                                EntryUnit = "гр.",
                                EntryType = "PQ"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "Рост",
                                EntryDisplayName = "Длина тела",
                                ID = "vv1_2",
                                Code = "51",
                                DateMetering = new DateTime(2021, 05, 25, 10, 10, 00),
                                Value = "1,56",
                                EntryValue = "156",
                                Unit = "м",
                                EntryUnit = "см",
                                EntryType = "PQ"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "ИМТ",
                                EntryDisplayName = "Индекс массы тела",
                                ID = "vv1_3",
                                Code = "10",
                                DateMetering = new DateTime(2018, 05, 25, 10, 10, 00),
                                Value = "32,87",
                                EntryValue = "32.87",
                                EntryType = "REAL"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "Суточный объём физиологических отправлений",
                                EntryDisplayName = "Суточный объём физиологических отправлений",
                                ID = "vv1_5",
                                Code = "56",
                                DateMetering = new DateTime(2018, 05, 25, 10,10, 00),
                                Value = "2000",
                                EntryValue = "2000",
                                Unit = "мл",
                                EntryUnit = "мл",
                                EntryType = "PQ"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "Объём талии",
                                EntryDisplayName = "Окружность талии",
                                ID = "vv1_6",
                                Code = "54",
                                DateMetering = new DateTime(2018, 05, 25, 10, 10, 00),
                                Value = "65",
                                EntryValue = "65",
                                Unit = "см",
                                EntryUnit = "см",
                                EntryType = "PQ"
                            },
                            new VitalParameterModel()
                            {
                                Caption = "Объём бёдер",
                                EntryDisplayName = "Окружность бёдер",
                                ID = "vv1_7",
                                Code = "55",
                                DateMetering = new DateTime(2018, 05, 25, 10, 10, 00),
                                Value = "97",
                                EntryValue = "97",
                                Unit = "см",
                                EntryUnit = "см",
                                EntryType = "PQ"
                            }
                        },
                        BodyType = "нормостеническое"
                    },
                    DirectionStateSection = new DirectionStateSectionModel()
                    {
                        StateText = "Жалоб нет. Физическое развитие нормальное.	Психофизиологическая выносливость в норме.	Эмоциональная устойчивость в норме."
                    },
                    DiagnosticStudiesSection = new DiagnosticStudiesSectionModel()
                    {
                        MedicalExaminations = new List<MedicalExaminationModel>()
                        {
                            new MedicalExaminationModel()
                            {
                                Date = new DateTime(2021, 05, 01, 16, 10, 00),
                                Number = "A04.01.001",
                                Name = "Ультразвуковое исследование мягких тканей (одна анатомическая зона)",
                                Result = "результат",
                                ID = "324576",
                                Code = "174"
                            },
                            new MedicalExaminationModel()
                            {
                                Date = new DateTime(2021, 05, 01, 17, 10, 00),
                                Number = "A06.09.007",
                                Name = "Рентгенография легких",
                                Result = "результат",
                                ID = "895543",
                                Code = "64"
                            },
                            new MedicalExaminationModel()
                            {
                                Date = new DateTime(2021, 05, 01, 17, 10 ,00),
                                Number = "B01.015.001",
                                Name = "Прием (осмотр, консультация) врача-кардиолога первичный",
                                Result = "результат",
                                ID = "8754871",
                                Code = "512"
                            }
                        }
                    },
                    DiagnosisSection = new DiagnosisSectionModel()
                    {
                        Diagnosis = new List<DiagnosticModel>()
                        {
                            new DiagnosticModel()
                            {
                                ID = "C49.2",
                                Code = "1",
                                Name = "Злокачественное новообразование соединительной и мягких тканей нижней конечности, включая тазобедренную область",
                                Caption = "Основное заболевание",
                                Result = "Фибромиксоидная саркома мягких тканей верхней трети правой голени с подрастанием к большеберцовой кости и мягким тканям правого коленного сустава T2bN0M0G1 Стадия: I."
                            },
                            new DiagnosticModel()
                            {
                                ID = "I25",
                                Code = "3",
                                Name = "Хроническая ишемическая болезнь сердца",
                                Caption = "Сопутствующая патология",
                                Result = "ИБС, стенокардия напряжения, 3ФК. Постоянная форма мерцательной аритмии, ХСН 2А. 3ФК. Гипертоническая болезнь 2 стадии, АГ 2 степени, риск 4. Ожирение 1 степени."
                            },
                            new DiagnosticModel()
                            {
                                ID = "I25.1",
                                Code = "7",
                                Name = "Атеросклеротическая болезнь сердца",
                                Caption = "Осложнение сопутствующего заболевания",
                                Result = "Атеросклеротический кардиосклероз."
                            }
                        }
                    },
                    ConditionAssessmentSection = new ConditionAssessmentSection()
                    {
                        ClinicalPrognosis = new ConditionGrateModel()
                        {
                            GrateType = "Клинический прогноз",
                            GrateResult = "Относительно благоприятный"
                        },
                        RehabilitationPotential = new ConditionGrateModel()
                        {
                            GrateType = "Реабилитационный потенциал",
                            GrateResult = "Удовлетворительный"
                        },
                        RehabilitationPrognosis = new ConditionGrateModel()
                        {
                            GrateType = "Реабилитационный прогноз",
                            GrateResult = "Сомнительный (неопределенный)"
                        }
                    },
                    RecommendationsSection = new RecommendationsSectionModel()
                    {
                        RecommendedMeasuresReconstructiveSurgery = "отсутствуют",
                        RecommendedMeasuresProstheticsAndOrthotics = "отсутствуют",
                        SpaTreatment = "не требуется",
                        Medications = new List<MedicationModel>()
                        {
                            new MedicationModel()
                            {
                                InternationalName = "Ацетилсалециловая кислота",
                                DosageForm = "Таблетки",
                                Dose = "100 мг+75 мг",
                                KTRUCode = "21.20.10.131-000003-1-00050-0000000000000",
                                KTRUName = "Стандартизованное_МНН:АЦЕТИЛСАЛИЦИЛОВАЯ КИСЛОТА+КЛОПИДОГРЕЛ Стандартизованная_лекарственная_форма:ТАБЛЕТКИ Стандартизованная_лекарственная_доза:100 мг+75 мг Код_КТРУ:21.20.10.131-000003-1-00050-0000000000000",
                                DurationAdmission = "1 неделя",
                                MultiplicityCoursesTreatment = "раз в 3 месяца",
                                ReceptionFrequency = "100-300 мг/день"
                            }
                        },
                        MedicalDevices = "текст",
                        OtherRecommendatons = "отсутствуют"
                    },
                    OutsideSpecialMedicalCareSection = new OutsideSpecialMedicalCareSectionModel()
                    {
                        Text = "Текст"
                    },
                    AttachmentDocumentsSection = new AttachmentDocumentsSectionModel()
                    {
                        AttachmentDocuments = new List<MedicalDocumentModel>()
                        {
                            new MedicalDocumentModel()
                            {
                                Name = "Ультразвуковое исследование мягких тканей (одна анатомическая зона)",
                                Result = "результат",
                                Created = new DateTime(2021, 06, 10, 16, 10, 00),
                                ReferenceRoot = "1.2.643.5.1.13.13.12.2.77.8312.100.1.1.51",
                                ReferenceExtension = "2568973"
                            },
                            new MedicalDocumentModel()
                            {
                                Name = "Рентгенография легких",
                                Result = "результат",
                                Created = new DateTime(2021, 06, 10, 16, 10, 00),
                                ReferenceRoot = "1.2.643.5.1.13.13.12.2.77.8312.100.1.1.51",
                                ReferenceExtension = "2568973"
                            }
                        }
                    }
                }
            };
            
            JsonSerializerOptions jsonSerializeOption = new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            string pathToSaveFile = "myJson.json";
            
            using (FileStream fs = new FileStream(pathToSaveFile, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync<DirectionToMSEDocumentModel>(fs, documentModel, jsonSerializeOption);
            }
            
            return documentModel;
        }

        private static async Task<DirectionToMSEDocumentModel> GetDocumentModelOnJson()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader("super.json"))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            using (FileStream fs = new FileStream("super.json", FileMode.Open))
            {
                var model = await JsonSerializer.DeserializeAsync<DirectionToMSEDocumentModel>(fs);
                return model;
            }
        }
    }
}
