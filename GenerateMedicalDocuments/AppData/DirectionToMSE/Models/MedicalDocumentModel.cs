using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель связанного медицинского документа.
    /// </summary>
    public class MedicalDocumentModel
    {
        /// <summary>
        /// Наименование документа.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дата создания документа.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Серия документа.
        /// </summary>
        public string Series { get; set; } = null;
        /// <summary>
        /// Номер документа.
        /// </summary>
        public string Number { get; set; } = null;
        /// <summary>
        /// Ссылка параметра "root".
        /// </summary>
        public string ReferenceRoot { get; set; }
        /// <summary>
        /// Ссылка параметра "extension".
        /// </summary>
        public string ReferenceExtension { get; set; }
        /// <summary>
        /// Результат докумета.
        /// </summary>
        public string Result { get; set; }
    }
}
