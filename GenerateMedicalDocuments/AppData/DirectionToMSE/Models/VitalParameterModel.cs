using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель витального параметра.
    /// </summary>
    public class VitalParameterModel
    {
        /// <summary>
        /// Идентификатор параметра.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Код парамера.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Наименование параметра.
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Наименование параметра в блоке кодирования.
        /// </summary>
        public string EntryDisplayName { get; set; }
        /// <summary>
        /// Значение параметра.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Значение параметра в блоке кодирования.
        /// </summary>
        public string EntryValue { get; set; }
        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// Еденица измерения в блоке кодирования.
        /// </summary>
        public string EntryUnit { get; set; }
        /// <summary>
        /// Тип в блоке кодирования.
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Дата измерения.
        /// </summary>
        public DateTime DateMetering { get; set; }
    }
}