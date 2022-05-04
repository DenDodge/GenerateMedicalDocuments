using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель документа-основания.
    /// </summary>
    public class BasisDocumentModel
    {
        /// <summary>
        /// [1..1] Тип документа-основания.
        /// </summary>
        public TypeModel IdentityDocType { get; set; }
        /// <summary>
        /// Тип полиса ОМС.
        /// </summary>
        public TypeModel InsurancePolicyType { get; set; }
        /// <summary>
        /// [1..1] Серия документа.
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        /// [1..1] Номер документа.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// [1..1] ИНН организации или физического лица.
        /// </summary>
        public long INN { get; set; }
        /// <summary>
        /// [1..1] Дата начала действия документа.
        /// </summary>
        public DateTime StartDateDocument { get; set; }
        /// <summary>
        /// [1..1] Дата конца действия документа.
        /// </summary>
        public DateTime FinishDateDocument { get; set; }
    }
}
