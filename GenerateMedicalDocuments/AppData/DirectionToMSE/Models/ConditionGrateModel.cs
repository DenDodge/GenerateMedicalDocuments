namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель оценки состояния.
    /// </summary>
    public class ConditionGrateModel
    {
        /// <summary>
        /// Тип оценки.
        /// </summary>
        public string GrateType { get; set; }
        /// <summary>
        /// Результат оценки.
        /// </summary>
        public string GrateResult { get; set; }

    }
}
