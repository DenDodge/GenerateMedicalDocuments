namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель [1..1] СЕКЦИЯ: Образование.
    /// </summary>
    public class EducationSectionModel
    {
        /// <summary>
        /// [1..1] Наполнение секции.
        /// </summary>
        public ParagraphModel FillingSection { get; set; }
        /// <summary>
        /// [0..1] Наименование и адрес образовательной организации, в которой гражданин получает образование.
        /// </summary>
        public OrganizationModel Organization { get; set; } = null;
        /// <summary>
        /// [0..1] Курс обучения.
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// [0..1] Профессия (специальность), для получения которой проводится обучение.
        /// </summary>
        public string Spetiality { get; set; }
    }
}
