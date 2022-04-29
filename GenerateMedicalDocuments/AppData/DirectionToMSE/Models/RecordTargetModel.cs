namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента "recordTarget".
    /// [1..1] ДАННЫЕ О ПАЦИЕНТЕ.
    /// </summary>
    public class RecordTargetModel
    {
        /// <summary>
        /// Модель элемента "patientRole".
        /// [1..1] ПАЦИЕНТ (роль).
        /// </summary>
        public PatientModel PatientRole { get; set; }
    }
}
