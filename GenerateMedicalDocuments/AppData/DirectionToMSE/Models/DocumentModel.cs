using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента "IdentityDoc".
    /// [1..1] Документ, удостоверяющий личность пациента, серия, номер, кем выдан.
    /// </summary>
    public class DocumentModel
    {
        /// <summary>
        /// [1..1] Тип документа.
        /// </summary>
        public TypeModel IdentityCardType { get; set; }
        /// <summary>
        /// [1..1] Серия документа.
        /// </summary>
        public int Series { get; set; }
        /// <summary>
        /// [1..1] Номер документа.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// [1..1] Кем выдан документ.
        /// </summary>
        public string IssueOrgName { get; set; }
        /// <summary>
        /// Для документа удоставеряющего личность - [1..1] Кем выдан документ, код подразделения.
        /// </summary>
        public string IssueOrgCode { get; set; } = null;
        /// <summary>
        /// [1..1] Дата выдачи документа.
        /// </summary>
        public DateTime IssueDate { get; set; }
    }
}
