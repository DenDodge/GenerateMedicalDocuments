using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// [0..1] СЕКЦИЯ: ДИАГНОСТИЧЕСКИЕ ИССЛЕДОВАНИЯ И КОНСУЛЬТАЦИИ.
    /// </summary>
    public class DiagnosticStudiesSectionModel
    {
        /// <summary>
        /// Список исследований.
        /// </summary>
        public List<MedicalExaminationModel> MedicalExaminations { get; set; }
    }
}
