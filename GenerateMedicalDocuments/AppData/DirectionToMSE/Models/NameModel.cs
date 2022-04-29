namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель имени человека.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// [1..1] Фамилия.
        /// </summary>
        public string Family { get; set; }
        /// <summary>
        /// [1..1] Имя.
        /// </summary>
        public string Given { get; set; }
        /// <summary>
        /// [1..1] Отчество.
        /// </summary>
        public string Patronymic { get; set; }
    }
}
