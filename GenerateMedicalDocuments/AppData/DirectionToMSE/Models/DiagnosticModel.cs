namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель диагноза.
    /// </summary>
    public class DiagnosticModel
    {
        /// <summary>
        /// Шифр.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание.
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Врачебное описание.
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Код для кодирования элемента.
        /// </summary>
        public string Code { get; set; }
    }
}
