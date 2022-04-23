using GenerateMedicalDocuments;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;

namespace GenerateMedicalDocumentsTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectionToMSEDocument document = new DirectionToMSEDocument()
            {
                CreationDate = new DateTime(2021, 06, 20, 16, 10, 0, DateTimeKind.Utc),
                Patient = new Patient
                {
                    Id = 585233,
                    SNILS = 44578444510,
                    IdentityDocument = new IdentityDocument
                    {
                        IdentityCardType = new DocumentType
                        {
                            Type = "CD",
                            Code = "1",
                            CodeSystem = "1.2.643.5.1.13.13.99.2.48",
                            CodeSystemVersion = "5.1",
                            CodeSystemName = "Документы, удостоверяющие личность",
                            DisplayName = "Паспорт гражданина Российской Федерации"
                        },
                        Series = 1234,
                        Number = 123456,
                        IssueOrgName = "ОВД 'Твардовское' ОУФМС России по городу Москве",
                        IssueOrgCode = "770-095",
                        IssueDate = new DateTime(2005, 02, 18)
                    },
                    InsurancePolicy = new InsurancePolicy
                    {
                        InsurancePolicyType = new DocumentType
                        {
                            Type = "CD",
                            Code = "1",
                            CodeSystem = "1.2.643.5.1.13.13.11.1035",
                            CodeSystemVersion = "1.3",
                            CodeSystemName = "Виды полиса обязательного медицинского страхования",
                            DisplayName = "Полис ОМС старого образца"
                        },
                        Series = "ЧБ",
                        Number = 1334602
                    }
                }
            };
            DirectionToMSE directionToMSE = new DirectionToMSE(document);
            directionToMSE.CreateDirectionTOMSEDocumentXML("testDocument.xml");
        }
    }
}
