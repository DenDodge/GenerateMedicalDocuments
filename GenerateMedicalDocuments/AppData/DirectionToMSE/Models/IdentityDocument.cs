using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель документа удоставеряющего личность.
    /// </summary>
    public class IdentityDocument
    {
        /// <summary>
        /// Тип документа.
        /// </summary>
        public DocumentType IdentityCardType { get; set; }
        /// <summary>
        /// Серия документа.
        /// </summary>
        public int Series { get; set; }
        /// <summary>
        /// Номер документа.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Кем выдан документ.
        /// </summary>
        public string IssueOrgName { get; set; }
        /// <summary>
        /// Кем выдан документ, код подразделения.
        /// </summary>
        public string IssueOrgCode { get; set; }
        /// <summary>
        /// Дата выдачи документа.
        /// </summary>
        public DateTime IssueDate { get; set; }
    }
}
