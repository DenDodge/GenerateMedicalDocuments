using System;
using System.IO;
using System.Threading.Tasks;
using EMKService;
using ServiceTestApp.PixService;

namespace ServiceTestApp
{
    internal class Program
    {
        private static string iemkTocken = "f498062a-e84f-44db-b0ba-deb7236df292";
        private static string mpiTocken = "a8dc08ba-964f-4d58-b63b-603a95dc0ffa";
        
        private static string lpuId= "f1bb4d39-86fc-404f-99d2-6a05ecc15faa";
        private static string lpuOID = "1.2.643.2.69.1.2.319";
        
        static async Task Main(string[] args)
        {
            PixServiceClient pixClient =
                new PixServiceClient(PixServiceClient.EndpointConfiguration.BasicHttpBinding_IPixService);
            var pixServiceResult = await PixServiceAddPatient(pixClient);
            
            
            //var emkServiceResult= await EmkServiceAddMedRecord();
        }

        /// <summary>
        /// Тестирование метода AddPatient PIX сервиса.
        /// </summary>
        /// <param name="service">PXI сервис.</param>
        /// <returns></returns>
        private static async Task<string> PixServiceAddPatient(PixServiceClient service)
        {
            var patient = GetTestPatient();
            
            try
            {
                var patientid = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C";
                await service.AddPatientAsync(patientid, lpuId, patient);
                return null;
            }
            catch (Exception e)
            {
                //TODO: 12.06.22 "Неправильный идентификатор системы".
                // создал тикет.
                Console.WriteLine(e);
                return e.Message;
            }
        }
        
        /// <summary>
        /// Метод сервиса EMK.
        /// </summary>
        /// <returns></returns>
        private static async Task<string> EmkServiceAddMedRecord()
        {
            string pathToGeneratedXMLDocument =
                "D:\\WORK_YAMED\\testDocument.xml";
            byte[] byteDocument = null;
            using (FileStream fs = File.OpenRead(pathToGeneratedXMLDocument))
            {
                byteDocument = new byte[fs.Length];
                await fs.ReadAsync(byteDocument, 0, byteDocument.Length);
            }

            EmkServiceClient emkClient = new EmkServiceClient(EmkServiceClient.EndpointConfiguration.BasicHttpBinding_IEmkService);
            MedDocument medDocument = new MedDocument();
            MedDocumentDtoDocumentAttachment documentAttachments = new MedDocumentDtoDocumentAttachment();
            documentAttachments.Data = byteDocument;
            medDocument.Attachments = new MedDocumentDtoDocumentAttachment[1];
            medDocument.Attachments[0] = documentAttachments;

            try
            {
                await emkClient.AddMedRecordAsync(
                    iemkTocken,
                    lpuId,
                    "88f6f255-250a-4f85-b81c-7111509f264e",
                    null,
                    medDocument,
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
                        IdProvider = "22003",
                        IssuedDate = new DateTime(1994, 02, 04),
                        ProviderName = "Единый полис",
                        RegionCode = "128",
                        StartDate = new DateTime(1995, 02, 05)
                    }
                },
                FamilyName = "Артемьев",
                GivenName = "Виктор",
                IdBloodType = 8,
                IdGlobal = null,
                IdLivingAreaType = 2,
                IdPatientMIS = "Identificator21.01.2021_05:49:10538",
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
