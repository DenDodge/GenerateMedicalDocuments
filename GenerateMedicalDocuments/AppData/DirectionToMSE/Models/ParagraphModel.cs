namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель наполнения секции.
    /// </summary>
    public class ParagraphModel
    {
        /// <summary>
        /// Описание параметра.
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Значение параметра.
        /// </summary>
        public string Content { get; set; }
    }
}
