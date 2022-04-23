using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель дукумента "Направление на медико-социальную экспертизу".
    /// </summary>
    public class DirectionToMSEDocument
    {
        /// <summary>
        /// Дата создания документа.
        /// </summary>
        public DateTime CreationDate { get; set; }
        public Patient Patient { get; set; }
    }
}
