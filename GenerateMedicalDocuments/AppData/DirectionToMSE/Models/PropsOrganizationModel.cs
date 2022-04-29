namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель реквизитов организации.
    /// </summary>
    public class PropsOrganizationModel
    {
        /// <summary>
        /// [0..1] Код ОГРН.
        /// </summary>
        public string OGRN { get; set; } = null;
        /// <summary>
        /// [0..1] Код ОГРНИП.
        /// </summary>
        public string OGRNIP { get; set; } = null;
        /// <summary>
        /// [0..1] Код ОКПО.
        /// </summary>
        public string OKPO { get; set; } = null;
        /// <summary>
        /// [0..1] Код ОКАТО.
        /// </summary>
        public string OKATO { get; set; } = null;
    }
}
