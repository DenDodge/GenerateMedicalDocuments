using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "СВЯЗАННЫЕ ДОКУМЕНТЫ".
    /// </summary>
    public class AttachmentDocumentsSectionModel
    {
        /// <summary>
        /// Список связанных документов.
        /// </summary>
        public List<MedicalDocumentModel> AttachmentDocuments { get; set; } = null;
    }
}
