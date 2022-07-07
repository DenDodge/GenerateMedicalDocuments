using System.Xml.Linq;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers;
using GenerateMedicalDocuments.AppData.DirectionToMSE.Models;

namespace GenerateMedicalDocuments
{
    /// <summary>
    /// Класс для создания и сохранения документа "Направление на медико-социальную экспертизу".
    /// </summary>
    public class DirectionToMSE
    {
        /// <summary>
        /// Получить XML документ "Направление на медико-социальную экспертизу" по модели документа.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <returns>XML документ "Направление на медико-социальную экспертизу" по модели документа.</returns>
        public XDocument GetDirectionToMSEDocumentXML(DirectionToMSEDocumentModel documentModel)
        {
            if (documentModel == null)
            {
                return null;
            }

            XDocument document = CreatingXMLDocumentHelper.GetXMLDocument(documentModel);
            return document;
        }

        /// <summary>
        /// Генерирует HTML документ и созраняет файл по указанному пути.
        /// </summary>
        /// <param name="documentModel">Модель документа.</param>
        /// <param name="savePatch">Путь для сохранения HTML файла.</param>
        public void CreationHTMLDocument(DirectionToMSEDocumentModel documentModel, string savePatch)
        {
            if (documentModel == null)
            {
                return;
            }

            CreatingHTMLDocumentHelper creatingHtmlDocumentHelper = new CreatingHTMLDocumentHelper(savePatch);
            creatingHtmlDocumentHelper.CreateHTMLDocument(documentModel);
        }
        
        /// <summary>
        /// Сохранить XML файл.
        /// </summary>
        /// <param name="xmlDocument">XML файл.</param>
        /// <param name="saveFilePatch">Путь сохранения файла.</param>
        public void SaveXmlDocument(XDocument xmlDocument, string saveFilePatch)
        {
            xmlDocument.Save(saveFilePatch);
        }
    }
}
