using System;
using System.IO;
using System.Threading.Tasks;
using EMKService;

namespace ServiceTestApp
{
    internal class Program
    {
        private static Guid iemkTocken = new Guid("740b3687-dfb7-4045-8b00-c549e5998cbf");
        private static Guid lpuTocken = new Guid("f1bb4d39-86fc-404f-99d2-6a05ecc15faa");
        
        static async Task Main(string[] args)
        {
            var res= await EmkService();
        }

        /// <summary>
        /// Метод сервиса EMK.
        /// </summary>
        /// <returns></returns>
        private static async Task<string> EmkService()
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
                    iemkTocken.ToString(),
                    lpuTocken.ToString(),
                    Guid.NewGuid().ToString(),
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
    }
}
