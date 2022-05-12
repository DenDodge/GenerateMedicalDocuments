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
                         Number = 1334602,
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
                            DisplayName = "Заведующий отделением"
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
                        Paragraphs = new List<ParagraphModel>()
                        {
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин направляется на медико-социальную экспертизу",
                                    Content = "повторно"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Цель направления",
                                    Content = "для разработки индивидуальной программы реабилитации инвалида"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Протокол врачебной комиссии медицинской организации, содержащий решение о направлении гражданина на медико-социальную экспертизу",
                                    Content = "№ 123 от 20 мая 2018 г."
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин по состоянию здоровья не может явиться в бюро (главное бюро, Федеральное бюро) медико-социальной экспертизы",
                                    Content = "медико-социальную экспертизу необходимо проводить на дому"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Нуждаемость в оказании паллиативной медицинской помощи",
                                    Content = "гражданин нуждается в паллиативной медицинской помощи"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Нахождение на лечении в стационаре в связи с операцией по ампутации  (реампутации)  конечности (конечностей), нуждающийся в первичном протезировании",
                                    Content = "гражданин не нуждается в первичном протезировании"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Дата выдачи гражданину направления на медико-социальную экспертизу медицинской организацией",
                                    Content = "20 июня 2018 г."
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданство",
                                    Content = "гражданин Российской Федерации"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Гражданин находится",
                                    Content = "текст"
                                }
                            },
                            {
                                new ParagraphModel()
                                {
                                    Caption = "Отношения к воинской обязанности",
                                    Content = "гражданин, не состоящий на воинском учёте"
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
                    }
                }
            };

            return documentModel;
        }
    }
}
