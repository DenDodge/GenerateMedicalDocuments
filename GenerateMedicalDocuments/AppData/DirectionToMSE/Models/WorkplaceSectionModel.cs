using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "Место работы и должность".
    /// </summary>
    public class WorkplaceSectionModel
    {
        /// <summary>
        /// Наполнение секции "Место работы и должность".
        /// </summary>
        public List<ParagraphModel> WorkPlaceParagraphs { get; set; }
        /// <summary>
        /// [0..1] Кодирование Сведения о трудовой деятельности.
        /// </summary>
        public WorkActivityModel WorkActivity { get; set; } = null;
    }
}
