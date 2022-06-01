namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель [1..1] СЕКЦИЯ: СОСТОЯНИЕ ПРИ НАПРАВЛЕНИИ.
    /// </summary>
    public class DirectionStateSectionModel
    {
        /// <summary>
        /// Состояние здоровья гражданина при направлении на медико-социальную экспертизу.
        /// </summary>
        public string StateText { get; set; }
    }
}
