using GenerateMedicalDocuments;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;
using System;

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
                }
            };

            return documentModel;
        }
    }
}
