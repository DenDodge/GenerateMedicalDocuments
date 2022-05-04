using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель данных о лице, придавшем юридическую силу документу.
    /// </summary>
    public class LegalAuthenticatorModel
    {
        /// <summary>
        /// [1..1] Дата подписи документа лицом, придавшем юридическую силу документу.
        /// </summary>
        public DateTime SignatureDate { get; set; }
        /// <summary>
        /// [1..1] Лицо, придавшен юридическую силу документу (роль).
        /// </summary>
        public AuthorModel AssignedEntity { get; set; }
    }
}
