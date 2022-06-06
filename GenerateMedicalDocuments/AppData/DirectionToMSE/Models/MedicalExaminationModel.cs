using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель Исследования.
    /// </summary>
    public class MedicalExaminationModel
    {
        /// <summary>
        /// Дата документа.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Номер документа. (Код).
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Идентификатор исследования.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Код для кодирования медицинского исследования.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Наименование исследования.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Результат исследования.
        /// </summary>
        public string Result { get; set; }
    }
}
