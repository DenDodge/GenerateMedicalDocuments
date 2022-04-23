using System.Xml.Linq;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;

namespace GenerateMedicalDocuments
{
    public class DirectionToMSE
    {
        #region Private Fields

        private DirectionToMSEDocument documentModel;

        #endregion

        #region Constructors

        public DirectionToMSE(DirectionToMSEDocument document)
        {
            this.documentModel = document;
        }

        #endregion

        /// <summary>
        /// Создать XML документ "Направление на медико-социальную экспертизу".
        /// </summary>
        public void CreateDirectionTOMSEDocumentXML(string documentSavePatch)
        {
            XDocument xmlDocument = new XDocument();
            GenerateXMLHelpers.GetXMLDocument(xmlDocument, documentModel);
            GenerateXMLHelpers.SaveDocument(xmlDocument, documentSavePatch);
        }
    }
}
