namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель полиса ОМС.
    /// </summary>
    public class InsurancePolicy
    {
        /// <summary>
        /// Тип.
        /// </summary>
        public DocumentType InsurancePolicyType { get; set; }
        /// <summary>
        /// Серия.
        /// - может не сущестовать.
        /// </summary>
        public string? Series { get; set; }
        /// <summary>
        /// Номер.
        /// </summary>
        public int Number { get; set; }

    }
}
