using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// [1..1] Данные об авторе документа.
    /// </summary>
    public class AuthorDataModel
    {
        /// <summary>
        /// [1..1] Дата подписи документа автором.
        /// </summary>
        public DateTime SignatureDate { get; set; }
        /// <summary>
        /// [1..1] АВТОР (роль).
        /// </summary>
        public AuthorModel Author { get; set; }
    }
}
