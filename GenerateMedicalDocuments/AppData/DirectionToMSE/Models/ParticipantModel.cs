namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель сведений об источнике оплаты.
    /// </summary>
    public class ParticipantModel
    {
        /// <summary>
        /// [1..1] Кодирование источника оплаты.
        /// </summary>
        public TypeModel Code { get; set; }
        /// <summary>
        /// Документ основание.
        /// </summary>
        public BasisDocumentModel DocInfo { get; set; }
        /// <summary>
        /// [0..1] Сведения об организации (страховой компании или юридического лица).
        /// </summary>
        public OrganizationModel ScopingOrganization { get; set; } = null;
    }
}
