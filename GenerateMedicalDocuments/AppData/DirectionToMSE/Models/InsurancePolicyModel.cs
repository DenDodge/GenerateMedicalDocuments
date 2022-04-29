namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель данных элемента "InsurancePolicy".
    /// [1..1] Полис ОМС.
    /// </summary>
    public class InsurancePolicyModel
    {
        /// <summary>
        /// [1..1] Тип полиса ОМС.
        /// </summary>
        public TypeModel InsurancePolicyType { get; set; }
        /// <summary>
        /// [0..1] Серия полиса ОМС.
        /// Только для старых версий полиса.
        /// </summary>
        public string Series { get; set; } = null;
        /// <summary>
        /// [1..1] Номер полиса ОМС.
        /// </summary>
        public string Number { get; set; }
    }
}
