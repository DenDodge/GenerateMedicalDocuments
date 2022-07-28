namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель [1..1] СЕКЦИЯ: ОБЪЕКТИВИЗИРОВАННАЯ ОЦЕНКА СОСТОЯНИЯ.
    /// </summary>
    public class ConditionAssessmentSectionModel
    {
        /// <summary>
        /// Клинический прогноз.
        /// </summary>
        public ConditionGrateModel ClinicalPrognosis { get; set; } = null;
        /// <summary>
        /// Реабилитационный потенциал.
        /// </summary>
        public ConditionGrateModel RehabilitationPotential { get; set; } = null;
        /// <summary>
        /// Реабилитационный прогноз.
        /// </summary>
        public ConditionGrateModel RehabilitationPrognosis { get; set; } = null;
    }
}
