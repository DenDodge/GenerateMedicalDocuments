namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель лицензии.
    /// </summary>
    public class LicenseModel
    {

        /// <summary>
        /// [1..1] Номер лицензии.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// [1..1] Наименование организации, выдавшей лицензию, и дата выдачи лицензии.
        /// </summary>
        public string AssigningAuthorityName { get; set; }
    }
}
