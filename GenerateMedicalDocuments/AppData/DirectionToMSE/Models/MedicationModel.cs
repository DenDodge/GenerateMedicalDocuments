namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель "Медикамент".
    /// </summary>
    public class MedicationModel
    {
        /// <summary>
        /// Международное название.
        /// </summary>
        public string InternationalName { get; set; }
        /// <summary>
        /// Лекарственная форма.
        /// </summary>
        public string DosageForm { get; set; }
        /// <summary>
        /// Лекарственная доза.
        /// </summary>
        public string Dose { get; set; }
        /// <summary>
        /// Код КТРУ.
        /// </summary>
        public string KTRUCode { get; set; }
        /// <summary>
        /// Наименование КТРУ.
        /// </summary>
        public string KTRUName { get; set; }
        /// <summary>
        /// Продолжительность приема.
        /// </summary>
        public string DurationAdmission { get; set; }
        /// <summary>
        /// Кратность курсов лечения.
        /// </summary>
        public string MultiplicityCoursesTreatment { get; set; }
        /// <summary>
        /// Кратность приема.
        /// </summary>
        public string ReceptionFrequency { get; set; }
    }
}
