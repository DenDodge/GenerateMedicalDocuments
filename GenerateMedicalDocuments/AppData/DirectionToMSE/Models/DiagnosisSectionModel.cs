using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции [1..1] СЕКЦИЯ: ДИАГНОЗЫ.
    /// </summary>
    public class DiagnosisSectionModel
    {
        /// <summary>
        /// Диагнозы.
        /// </summary>
        public List<DiagnosticModel> Diagnosis { get; set; }
    }
}
