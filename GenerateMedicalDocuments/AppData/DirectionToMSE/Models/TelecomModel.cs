namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента "telecom".
    /// </summary>
    public class TelecomModel
    {
        /// <summary>
        /// Значение элемента "telecom".
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Значение атрибута "telecom".
        /// </summary>
        public string Use { get; set; } = null;

        /// <summary>
        /// Значение элемента "telecom value=":" ".
        /// </summary>
        public string ValueAttr { get; set; } = null;
    }
}
