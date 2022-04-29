using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель документа "Направление на Медико-социальную экспертизу".
    /// </summary>
    public class DirectionToMSEDocumentModel
    {
        /// <summary>
        /// [1..1] Уникальный идентификатор документа.
        /// </summary>
        public IDType ID { get; set; }
        /// <summary>
        /// [1..1] Уникальный идентификатор набора версий документа.
        /// </summary>
        public IDType SetID { get; set; }
        /// <summary>
        /// [1..1] Номер версии данного документа.
        /// </summary>
        public int VersionNumber { get; set; }
        /// <summary>
        /// [1..1] Дата создания документа.
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// [1..1] ДАННЫЕ О ПАЦИЕНТЕ.
        /// </summary>
        public RecordTargetModel RecordTarget { get; set; }
    }
}
