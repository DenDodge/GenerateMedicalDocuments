using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EMKService;
using ServiceTestApp.PixService;

namespace ServiceTestApp
{
    internal class Program
    {
        private static string iemkTocken = "f498062a-e84f-44db-b0ba-deb7236df292";
        private static string mpiTocken = "a8dc08ba-964f-4d58-b63b-603a95dc0ffa";
        
        private static string lpuId= "395f273c-a727-456b-ba72-c8fc98963917";
        private static string lpuOID = "1.2.643.2.69.1.2.319";
        
        static async Task Main(string[] args)
        {
            PixServiceClient pixClient =
                new PixServiceClient(PixServiceClient.EndpointConfiguration.BasicHttpBinding_IPixService);
            var pixServiceResult = await PixServiceAddPatient(pixClient); 
            var patient = await PixServiceGetPatient(pixClient);

            var emkServiceResult= await EmkServiceAddMedRecord(patient[0].IdPatientMIS);
        }

        /// <summary>
        /// Тестирование метода AddPatient PIX сервиса.
        /// </summary>
        /// <param name="service">PIX сервис.</param>
        /// <returns></returns>
        private static async Task<string> PixServiceAddPatient(PixServiceClient service)
        {
            var patient = GetTestPatient();
            
            try
            {
                await service.AddPatientAsync(iemkTocken, lpuId, patient);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message;
            }
        }

        /// <summary>
        /// Метод сервиса PIX для получения пациента.
        /// </summary>
        /// <returns></returns>
        private static async Task<PatientDto[]> PixServiceGetPatient(PixServiceClient service)
        {
            PatientDto pac = new PatientDto { IdPatientMIS = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C" };
            PatientDto[] patient;
            try
            {
                patient = await service.GetPatientAsync(iemkTocken, lpuId, pac, SourceType.Reg);
                return patient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        /// <summary>
        /// Метод сервиса EMK.
        /// </summary>
        /// <returns></returns>
        private static async Task<string> EmkServiceAddMedRecord(string idPatient)
        {
            string pathToGeneratedXMLDocument =
                "testDocument.xml";
            string pathToSigGeneratedXMLDocument = "testDocument.xml.sig";
            byte[] byteDocument = null;
            byte[] byteSigDocument = null;
            using (FileStream fs = File.OpenRead(pathToGeneratedXMLDocument))
            {
                byteDocument = new byte[fs.Length];
                await fs.ReadAsync(byteDocument, 0, byteDocument.Length);
            }
            using (FileStream fs = File.OpenRead(pathToSigGeneratedXMLDocument))
            {
                byteSigDocument = new byte[fs.Length];
                await fs.ReadAsync(byteSigDocument, 0, byteSigDocument.Length);
            }

            EmkServiceClient emkClient = new EmkServiceClient(EmkServiceClient.EndpointConfiguration.BasicHttpBinding_IEmkService);
            MedDocument medDocument = new MedDocument()
            {
                
            };

            ReferralMSE referal = new ReferralMSE()
            {
                Attachments = new List<MedDocumentDtoDocumentAttachment>()
                {
                    new MedDocumentDtoDocumentAttachment()
                    {
                        Data = byteDocument,
                        OrganizationSign = byteSigDocument,
                        PersonalSigns = new List<MedDocumentDtoPersonalSign>()
                        {
                            new MedDocumentDtoPersonalSign()
                            {
                                Doctor = new MedicalStaff()
                                {
                                    Person = new PersonWithIdentity()
                                    {
                                        HumanName = new HumanName()
                                        {
                                            FamilyName = "Привалов",
                                            GivenName = "Александр",
                                            MiddleName = "Иванович"
                                        },
                                        IdPersonMis = "1"
                                    },
                                    IdLpu = lpuId,
                                    IdSpeciality = 30,
                                    IdPosition = 122,
                                },
                                Sign = byteSigDocument,
                            }
                        },
                        MimeType = "text/xml",
                        Url = null
                    }
                },
                CreationDate = DateTime.Now,
                Header = "Направление на медико-социальную экспертизу (редакция 5)",
                IdDocumentMis = "1",
                IdMedDocumentType = byte.Parse("145"),
                RelatedMedDoc = new List<string>
                {
                    "0"
                },
                Observations = new List<Observation>()
                {
                    new Observation()
                    {
                        Code = 214,
                        DateTime = DateTime.Now,
                        Interpretation = "E",
                        ValueQuantity = new BooleanValue()
                        {
                            Value = true,
                        },
                        ReferenceRanges = new List<ReferenceRange>()
                        {
                            new ReferenceRange
                            {
                                RangeType = 2,
                                IdUnit = 506,
                                Value = "666"
                            }
                        }
                    }
                },
                Author = new MedicalStaff()
                {
                    Person = new PersonWithIdentity()
                    {
                        HumanName = new HumanName()
                        {
                            FamilyName = "Привалов",
                            GivenName = "Александр",
                            MiddleName = "Иванович"
                        },
                        IdPersonMis = "2341"
                    },
                    IdLpu = lpuId,
                    IdSpeciality = 30,
                    IdPosition = 122
                }
            };
            
            try
            {
                await emkClient.AddMedRecordAsync(
                    iemkTocken,
                    lpuId,
                    idPatient,
                    null,
                    referal,
                    null);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message;
            }
            
        }
        
        /// <summary>
        /// Получить тестовые данные пацинта.
        /// </summary>
        /// <returns></returns>
        private static PatientDto GetTestPatient()
        {
            return new PatientDto()
            {
                Addresses = new AddressDto[]
                {
                    new AddressDto()
                    {
                        Appartment = null,
                        Building = "454",
                        City = "0100000000000",
                        GeoData = "43.072812,-79.040128",
                        IdAddressType = 2,
                        PostalCode = 454100,
                        Street = "01000001000000100",
                        StringAddress = "г. Санкт-Петербург, пр. Хошимина, д. 11 к.1"
                    },
                    new AddressDto()
                    {
                        Appartment = "21",
                        Building = "88",
                        City = "0100000000000",
                        GeoData = "78, 64",
                        IdAddressType = 4,
                        PostalCode = 672157,
                        Street = "01000001000000100",
                        StringAddress = "194044, Россия, Санкт-Петербург, Пироговская наб. 5/2"
                    }
                },
                BirthDate = new DateTime(1986, 06, 07),
                BirthPlace = null,
                ContactPerson = null,
                Contacts = new ContactDto[]
                {
                    new ContactDto()
                    {
                        ContactValue = "89238364654",
                        IdContactType = 1
                    },
                    new ContactDto()
                    {
                        ContactValue = "vova@gmail.com",
                        IdContactType = 3
                    }
                },
                DeathTime = null,
                Documents = new DocumentDto[]
                {
                    new DocumentDto()
                    {
                        DocN = "606226",
                        DocS = "5258",
                        DocumentName = null,
                        ExpiredDate = new DateTime(2020, 02, 19),
                        IdDocumentType = 14,
                        IdProvider = null,
                        IssuedDate = new DateTime(2007, 09, 03),
                        ProviderName = "УФМС",
                        RegionCode = "128",
                        StartDate = null
                    },
                    new DocumentDto()
                    {
                        DocN = "84879331472",
                        DocS = null,
                        DocumentName = null,
                        ExpiredDate = new DateTime(2010, 12, 01),
                        IdDocumentType = 223,
                        IdProvider = null,
                        IssuedDate = new DateTime(2006, 09, 03),
                        ProviderName = "ПФР",
                        RegionCode = "128",
                        StartDate = null
                    },
                    new DocumentDto()
                    {
                        DocN = "7853310842002178",
                        DocS = null,
                        DocumentName = null,
                        ExpiredDate = new DateTime(2000, 06, 02),
                        IdDocumentType = 228,
                        IdProvider = "46003",
                        IssuedDate = new DateTime(1994, 02, 04),
                        ProviderName = "Единый полис",
                        RegionCode = "46",
                        StartDate = new DateTime(1995, 02, 05)
                    }
                },
                FamilyName = "Артемьев",
                GivenName = "Виктор",
                IdBloodType = 8,
                IdGlobal = null,
                IdLivingAreaType = 2,
                IdPatientMIS = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C",
                IsVip = false,
                Job = null,
                MiddleName = "Антонович",
                Privilege = null,
                Sex = 1,
                SocialGroup = 4,
                SocialStatus = "2"
            };
        }
    }
}
