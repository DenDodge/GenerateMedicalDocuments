using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EMKService;
using ServiceTestApp.PixService;

namespace ServiceTestApp
{
    internal class Program
    {
        private static string sign = @"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
GAYIKoUDA4EDAQESDDAwNzcxMDQ3NDM3NTEsMCoGA1UEAwwj0JzQuNC90LrQvtC80YHQstGP0LfR
jCDQoNC+0YHRgdC40LiCEE5tR4sm8n1lf3aOAlzj05MwgZgGA1UdHwSBkDCBjTAtoCugKYYnaHR0
cDovL3JlZXN0ci1wa2kucnUvY2RwL2d1Y19nb3N0MTIuY3JsMC2gK6AphidodHRwOi8vY29tcGFu
eS5ydC5ydS9jZHAvZ3VjX2dvc3QxMi5jcmwwLaAroCmGJ2h0dHA6Ly9yb3N0ZWxlY29tLnJ1L2Nk
cC9ndWNfZ29zdDEyLmNybDBDBggrBgEFBQcBAQQ3MDUwMwYIKwYBBQUHMAKGJ2h0dHA6Ly9yZWVz
dHItcGtpLnJ1L2NkcC9ndWNfZ29zdDEyLmNydDCB9QYFKoUDZHAEgeswgegMNNCf0JDQmtCcIMKr
0JrRgNC40L/RgtC+0J/RgNC+IEhTTcK7INCy0LXRgNGB0LjQuCAyLjAMQ9Cf0JDQmiDCq9CT0L7Q
u9C+0LLQvdC+0Lkg0YPQtNC+0YHRgtC+0LLQtdGA0Y/RjtGJ0LjQuSDRhtC10L3RgtGAwrsMNdCX
0LDQutC70Y7Rh9C10L3QuNC1IOKEliAxNDkvMy8yLzIvMjMg0L7RgiAwMi4wMy4yMDE4DDTQl9Cw
0LrQu9GO0YfQtdC90LjQtSDihJYgMTQ5LzcvNi8xMDUg0L7RgiAyNy4wNi4yMDE4MAoGCCqFAwcB
AQMCA0EANtAlZ3WbgmfxYXry/xBNdPdaGlQ6qB240vudx5J8VjmWpTDc4ybuEfrvsW2R8K0UJEQ0
UK85qEGpECzJdG87nTCCDhwwgg3JoAMCAQICEEAengCyrQ6wQ1tDcLZnVwswCgYIKoUDBwEBAwIw
ggGJMSIwIAYJKoZIhvcNAQkBFhNjYV90ZW5zb3JAdGVuc29yLnJ1MRgwFgYFKoUDZAESDTEwMjc2
MDA3ODc5OTQxGjAYBggqhQMDgQMBARIMMDA3NjA1MDE2MDMwMQswCQYDVQQGEwJSVTExMC8GA1UE
CAwoNzYg0K/RgNC+0YHQu9Cw0LLRgdC60LDRjyDQvtCx0LvQsNGB0YLRjDEfMB0GA1UEBwwW0LMu
INCv0YDQvtGB0LvQsNCy0LvRjDE2MDQGA1UECQwt0JzQvtGB0LrQvtCy0YHQutC40Lkg0L/RgNC+
0YHQv9C10LrRgiwg0LQuIDEyMTAwLgYDVQQLDCfQo9C00L7RgdGC0L7QstC10YDRj9GO0YnQuNC5
INGG0LXQvdGC0YAxMDAuBgNVBAoMJ9Ce0J7QniAi0JrQntCc0J/QkNCd0JjQryAi0KLQldCd0JfQ
ntCgIjEwMC4GA1UEAwwn0J7QntCeICLQmtCe0JzQn9CQ0J3QmNCvICLQotCV0J3Ql9Ce0KAiMB4X
DTIxMDkyOTA5MjU0MVoXDTIyMDkyOTA5MzU0MVowggIBMUAwPgYDVQQJDDfQo9Cb0JjQptCQINCb
0J7QnNCQ0JrQmNCd0JAsINCU0J7QnCAxNy3QkCwg0J7QpNCY0KEgNTA2MSYwJAYDVQQIDB3QmtGD
0YDRgdC60LDRjyDQvtCx0LvQsNGB0YLRjDEeMBwGA1UEBwwV0JPQntCg0J7QlCDQmtCj0KDQodCa
MQswCQYDVQQGEwJSVTEwMC4GA1UEKgwn0JDQvdC00YDQtdC5INCh0YLQsNC90LjRgdC70LDQstC+
0LLQuNGHMRswGQYDVQQEDBLQktCw0YHQuNC70LXQvdC60L4xLDAqBgNVBAMMI9Ce0J7QniAi0JrQ
ntCc0J/QkNCd0JjQryAi0K/QnNCV0JQiMTAwLgYDVQQMDCfQk9CV0J3QldCg0JDQm9Cs0J3Qq9CZ
INCU0JjQoNCV0JrQotCe0KAxLDAqBgNVBAoMI9Ce0J7QniAi0JrQntCc0J/QkNCd0JjQryAi0K/Q
nNCV0JQiMSYwJAYJKoZIhvcNAQkBFhdhaXMteWFtZWQuYnVoQHlhbmRleC5ydTEaMBgGCCqFAwOB
AwEBEgw0NjMyMjU1MTA2MjIxFTATBgUqhQNkBBIKNDYzMjIyNDc2NDEWMBQGBSqFA2QDEgswNTI0
NjkxMzI1NDEYMBYGBSqFA2QBEg0xMTc0NjMyMDAwMzU1MGYwHwYIKoUDBwEBAQEwEwYHKoUDAgIk
AAYIKoUDBwEBAgIDQwAEQGIarDC7vXgU1l8yE93p49gaLSnvVyZNumJVbAv9W+33wAfUs2q3dU4m
eZWHWiLo22IQPw9qQRtQrj6pNDA6IYejggmIMIIJhDAOBgNVHQ8BAf8EBAMCA/gwWAYDVR0lBFEw
TwYHKoUDAgIiGQYHKoUDAgIiGgYHKoUDAgIiBgYJKoUDAzoDAQEDBgkqhQMDOgMBAQUGCCqFAwMI
ZAETBggrBgEFBQcDAgYIKwYBBQUHAwQwHQYDVR0gBBYwFDAIBgYqhQNkcQEwCAYGKoUDZHECMCEG
BSqFA2RvBBgMFtCa0YDQuNC/0YLQvtCf0YDQviBDU1AwDAYFKoUDZHIEAwIBATCCAloGByqFAwIC
MQIEggJNMIICSTCCAjcWEmh0dHBzOi8vc2Jpcy5ydS9jcAyCAhvQmNC90YTQvtGA0LzQsNGG0LjQ
vtC90L3Ri9C1INGB0LjRgdGC0LXQvNGLLCDQv9GA0LDQstC+0L7QsdC70LDQtNCw0YLQtdC70LXQ
vCDQuNC70Lgg0L7QsdC70LDQtNCw0YLQtdC70LXQvCDQv9GA0LDQsiDQvdCwINC30LDQutC+0L3Q
vdGL0YUg0L7RgdC90L7QstCw0L3QuNGP0YUg0LrQvtGC0L7RgNGL0YUg0Y/QstC70Y/QtdGC0YHR
jyDQntCe0J4gItCa0L7QvNC/0LDQvdC40Y8gItCi0LXQvdC30L7RgCIsINCwINGC0LDQutC20LUg
0LIg0LjQvdGE0L7RgNC80LDRhtC40L7QvdC90YvRhSDRgdC40YHRgtC10LzQsNGFLCDRg9GH0LDR
gdGC0LjQtSDQsiDQutC+0YLQvtGA0YvRhSDQv9GA0L7QuNGB0YXQvtC00LjRgiDQv9GA0Lgg0LjR
gdC/0L7Qu9GM0LfQvtCy0LDQvdC40Lgg0YHQtdGA0YLQuNGE0LjQutCw0YLQvtCyINC/0YDQvtCy
0LXRgNC60Lgg0LrQu9GO0YfQtdC5INGN0LvQtdC60YLRgNC+0L3QvdC+0Lkg0L/QvtC00L/QuNGB
0LgsINCy0YvQv9GD0YnQtdC90L3Ri9GFINCe0J7QniAi0JrQvtC80L/QsNC90LjRjyAi0KLQtdC9
0LfQvtGAIgMCBeAEDBvKr/ghTzRoz6NmpzBNBgNVHREERjBEpEIwQDE+MDwGCSqGSIb3DQEJAhYv
SU5OPTQ2MzIyMjQ3NjQvS1BQPTQ2MzIwMTAwMS9PR1JOPTExNzQ2MzIwMDAzNTUwggHHBggrBgEF
BQcBAQSCAbkwggG1MEYGCCsGAQUFBzABhjpodHRwOi8vdGF4NC50ZW5zb3IucnUvb2NzcC10ZW5z
b3JjYS0yMDIxX2dvc3QyMDEyL29jc3Auc3JmMF4GCCsGAQUFBzAChlJodHRwOi8vdGF4NC50ZW5z
b3IucnUvdGVuc29yY2EtMjAyMV9nb3N0MjAxMi9jZXJ0ZW5yb2xsL3RlbnNvcmNhLTIwMjFfZ29z
dDIwMTIuY3J0MDoGCCsGAQUFBzAChi5odHRwOi8vdGVuc29yLnJ1L2NhL3RlbnNvcmNhLTIwMjFf
Z29zdDIwMTIuY3J0MEMGCCsGAQUFBzAChjdodHRwOi8vY3JsLnRlbnNvci5ydS90YXg0L2NhL3Rl
bnNvcmNhLTIwMjFfZ29zdDIwMTIuY3J0MEQGCCsGAQUFBzAChjhodHRwOi8vY3JsMi50ZW5zb3Iu
cnUvdGF4NC9jYS90ZW5zb3JjYS0yMDIxX2dvc3QyMDEyLmNydDBEBggrBgEFBQcwAoY4aHR0cDov
L2NybDMudGVuc29yLnJ1L3RheDQvY2EvdGVuc29yY2EtMjAyMV9nb3N0MjAxMi5jcnQwKwYDVR0Q
BCQwIoAPMjAyMTA5MjkwOTI1NDFagQ8yMDIyMDkyOTA5MjU0MVowggEzBgUqhQNkcASCASgwggEk
DCsi0JrRgNC40L/RgtC+0J/RgNC+IENTUCIgKNCy0LXRgNGB0LjRjyA0LjApDFMi0KPQtNC+0YHR
gtC+0LLQtdGA0Y/RjtGJ0LjQuSDRhtC10L3RgtGAICLQmtGA0LjQv9GC0L7Qn9GA0L4g0KPQpiIg
0LLQtdGA0YHQuNC4IDIuMAxP0KHQtdGA0YLQuNGE0LjQutCw0YIg0YHQvtC+0YLQstC10YLRgdGC
0LLQuNGPIOKEliDQodCkLzEyNC0zOTY2INC+0YIgMTUuMDEuMjAyMQxP0KHQtdGA0YLQuNGE0LjQ
utCw0YIg0YHQvtC+0YLQstC10YLRgdGC0LLQuNGPIOKEliDQodCkLzEyOC0zNTkyINC+0YIgMTcu
MTAuMjAxODCCAWgGA1UdHwSCAV8wggFbMFigVqBUhlJodHRwOi8vdGF4NC50ZW5zb3IucnUvdGVu
c29yY2EtMjAyMV9nb3N0MjAxMi9jZXJ0ZW5yb2xsL3RlbnNvcmNhLTIwMjFfZ29zdDIwMTIuY3Js
MDSgMqAwhi5odHRwOi8vdGVuc29yLnJ1L2NhL3RlbnNvcmNhLTIwMjFfZ29zdDIwMTIuY3JsMEGg
P6A9hjtodHRwOi8vY3JsLnRlbnNvci5ydS90YXg0L2NhL2NybC90ZW5zb3JjYS0yMDIxX2dvc3Qy
MDEyLmNybDBCoECgPoY8aHR0cDovL2NybDIudGVuc29yLnJ1L3RheDQvY2EvY3JsL3RlbnNvcmNh
LTIwMjFfZ29zdDIwMTIuY3JsMEKgQKA+hjxodHRwOi8vY3JsMy50ZW5zb3IucnUvdGF4NC9jYS9j
cmwvdGVuc29yY2EtMjAyMV9nb3N0MjAxMi5jcmwwggFfBgNVHSMEggFWMIIBUoAUV94jGe+BgSwM
1x785820tkAh8TKhggEspIIBKDCCASQxHjAcBgkqhkiG9w0BCQEWD2RpdEBtaW5zdnlhei5ydTEL
MAkGA1UEBhMCUlUxGDAWBgNVBAgMDzc3INCc0L7RgdC60LLQsDEZMBcGA1UEBwwQ0LMuINCc0L7R
gdC60LLQsDEuMCwGA1UECQwl0YPQu9C40YbQsCDQotCy0LXRgNGB0LrQsNGPLCDQtNC+0LwgNzEs
MCoGA1UECgwj0JzQuNC90LrQvtC80YHQstGP0LfRjCDQoNC+0YHRgdC40LgxGDAWBgUqhQNkARIN
MTA0NzcwMjAyNjcwMTEaMBgGCCqFAwOBAwEBEgwwMDc3MTA0NzQzNzUxLDAqBgNVBAMMI9Cc0LjQ
vdC60L7QvNGB0LLRj9C30Ywg0KDQvtGB0YHQuNC4ggph582qAAAAAAUaMB0GA1UdDgQWBBRWoQSc
TYcDu4mIRrdhlWjYqEQjMzAKBggqhQMHAQEDAgNBAE/brYp2wL0xHvTwyWP2Did+mJaR8RqA7z3i
BMet2rr9bLze/BJrsuFJR3cp9ZhHmnRi104qflxynpvXtoFeLIkxggSCMIIEfgIBATCCAZ8wggGJ
MSIwIAYJKoZIhvcNAQkBFhNjYV90ZW5zb3JAdGVuc29yLnJ1MRgwFgYFKoUDZAESDTEwMjc2MDA3
ODc5OTQxGjAYBggqhQMDgQMBARIMMDA3NjA1MDE2MDMwMQswCQYDVQQGEwJSVTExMC8GA1UECAwo
NzYg0K/RgNC+0YHQu9Cw0LLRgdC60LDRjyDQvtCx0LvQsNGB0YLRjDEfMB0GA1UEBwwW0LMuINCv
0YDQvtGB0LvQsNCy0LvRjDE2MDQGA1UECQwt0JzQvtGB0LrQvtCy0YHQutC40Lkg0L/RgNC+0YHQ
v9C10LrRgiwg0LQuIDEyMTAwLgYDVQQLDCfQo9C00L7RgdGC0L7QstC10YDRj9GO0YnQuNC5INGG
0LXQvdGC0YAxMDAuBgNVBAoMJ9Ce0J7QniAi0JrQntCc0J/QkNCd0JjQryAi0KLQldCd0JfQntCg
IjEwMC4GA1UEAwwn0J7QntCeICLQmtCe0JzQn9CQ0J3QmNCvICLQotCV0J3Ql9Ce0KAiAhBAHp4A
sq0OsENbQ3C2Z1cLMAwGCCqFAwcBAQICBQCgggJjMBgGCSqGSIb3DQEJAzELBgkqhkiG9w0BBwEw
HAYJKoZIhvcNAQkFMQ8XDTIyMDYxNzE4MDM1MVowLwYJKoZIhvcNAQkEMSIEIBcc3DaBRphI9QHP
Y80Qdn4qU7Ka1iKdKoHKbCM2dHwhMIIB9gYLKoZIhvcNAQkQAi8xggHlMIIB4TCCAd0wggHZMAoG
CCqFAwcBAQICBCBKGVT6aHIFo4NMLQ0WxDjJx0U9rkWK41iUkk7yHSNVlzCCAacwggGRpIIBjTCC
AYkxIjAgBgkqhkiG9w0BCQEWE2NhX3RlbnNvckB0ZW5zb3IucnUxGDAWBgUqhQNkARINMTAyNzYw
MDc4Nzk5NDEaMBgGCCqFAwOBAwEBEgwwMDc2MDUwMTYwMzAxCzAJBgNVBAYTAlJVMTEwLwYDVQQI
DCg3NiDQr9GA0L7RgdC70LDQstGB0LrQsNGPINC+0LHQu9Cw0YHRgtGMMR8wHQYDVQQHDBbQsy4g
0K/RgNC+0YHQu9Cw0LLQu9GMMTYwNAYDVQQJDC3QnNC+0YHQutC+0LLRgdC60LjQuSDQv9GA0L7R
gdC/0LXQutGCLCDQtC4gMTIxMDAuBgNVBAsMJ9Cj0LTQvtGB0YLQvtCy0LXRgNGP0Y7RidC40Lkg
0YbQtdC90YLRgDEwMC4GA1UECgwn0J7QntCeICLQmtCe0JzQn9CQ0J3QmNCvICLQotCV0J3Ql9Ce
0KAiMTAwLgYDVQQDDCfQntCe0J4gItCa0J7QnNCf0JDQndCY0K8gItCi0JXQndCX0J7QoCICEEAe
ngCyrQ6wQ1tDcLZnVwswHwYIKoUDBwEBAQEwEwYHKoUDAgIkAAYIKoUDBwEBAgIEQLSCuTG1kk7W
VZUJN3/UFQBU/mMk2ODS0DChWqUCepQzk3e3XpAmY4VfWgaIC5Ze4588Mn3Gs3joj80Qzd3Zh20=";

        private static string iemkTocken = "f498062a-e84f-44db-b0ba-deb7236df292";
        private static string mpiTocken = "a8dc08ba-964f-4d58-b63b-603a95dc0ffa";
        
        private static string lpuId= "f1bb4d39-86fc-404f-99d2-6a05ecc15faa";
        private static string lpuOID = "1.2.643.2.69.1.2.319";
        
        static async Task Main(string[] args)
        {
            /*PixServiceClient pixClient =
                new PixServiceClient(PixServiceClient.EndpointConfiguration.BasicHttpBinding_IPixService);
            var pixServiceResult = await PixServiceAddPatient(pixClient);*/
            //await PixServiceGetPatient(pixClient);

            var emkServiceResult= await EmkServiceAddMedRecord();
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
                await service.AddPatientAsync(iemkTocken, lpuId, patient);
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
                "..\\testDocument.xml";
            byte[] byteDocument = null;
            using (FileStream fs = File.OpenRead(pathToGeneratedXMLDocument))
            {
                byteDocument = new byte[fs.Length];
                await fs.ReadAsync(byteDocument, 0, byteDocument.Length);
            }

            EmkServiceClient emkClient = new EmkServiceClient(EmkServiceClient.EndpointConfiguration.BasicHttpBinding_IEmkService);
            MedDocument medDocument = new MedDocument()
            {
                Attachments = new List<MedDocumentDtoDocumentAttachment>()
                    {
                        new MedDocumentDtoDocumentAttachment()
                        {
                            Data = byteDocument,
                            OrganizationSign = Convert.FromBase64String(sign),
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
                                        IdLpu = "f1bb4d39-86fc-404f-99d2-6a05ecc15faa",
                                        IdSpeciality = 30,
                                        IdPosition = 122,
                                    },
                                    Sign = Convert.FromBase64String(sign),
                                }
                            },
                            MimeType = "text/xml",
                            Url = null
                        }
                    },
                CreationDate = DateTime.Now,
                Header = "Тестовая отправка направления на МСЭ",
                IdDocumentMis = "1",
                IdMedDocumentType = 145,
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
                    IdLpu = "f1bb4d39-86fc-404f-99d2-6a05ecc15faa",
                    IdSpeciality = 30,
                    IdPosition = 122
                }
            };

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
