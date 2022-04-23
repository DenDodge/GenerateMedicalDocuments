namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель пациента.
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// СНИЛС.
        /// </summary>
        public long SNILS { get; set; }

        /// <summary>
        /// Документ удоставеряющий личность.
        /// </summary>
        public IdentityDocument IdentityDocument { get; set; }
        /// <summary>
        /// Полис ОМС.
        /// </summary>
        public InsurancePolicy InsurancePolicy { get; set; }
    }
}
