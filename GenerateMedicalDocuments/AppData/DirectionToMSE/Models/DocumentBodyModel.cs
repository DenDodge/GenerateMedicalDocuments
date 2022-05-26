namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель тела документа.
    /// </summary>
    public class DocumentBodyModel
    {
        /// <summary>
        /// [1..1] СЕКЦИЯ: НАПРАВЛЕН.
        /// </summary>
        public SentSectionModel SentSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: Место работы, должность.
        /// </summary>
        public WorkplaceSectionModel WorkplaceSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: Образование
        /// </summary>
        public EducationSectionModel EducationSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: АНАМНЕЗ.
        /// </summary>
        public AnamnezSectionModel AnamnezSection { get; set; }
    }
}
