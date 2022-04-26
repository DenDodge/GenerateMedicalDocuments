using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель документа "Направление на Медико-социальную экспертизу".
    /// </summary>
    public class DirectionToMSEDocumentModel
    {
        public IDType ID { get; set; }
        public IDType SetID { get; set; }
        public int VersionNumber { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
