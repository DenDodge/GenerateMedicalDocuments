using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель документа "Направление на Медико-социальную экспертизу".
    /// </summary>
    public class DirectionToMSEDocumentModel
    {
        /// <summary>
        /// [1..1] Уникальный идентификатор документа.
        /// </summary>
        public IDType ID { get; set; }
        /// <summary>
        /// [1..1] Уникальный идентификатор набора версий документа.
        /// </summary>
        public IDType SetID { get; set; }
        /// <summary>
        /// [1..1] Номер версии данного документа.
        /// </summary>
        public int VersionNumber { get; set; }
        /// <summary>
        /// [1..1] Дата создания документа.
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// [1..1] ДАННЫЕ О ПАЦИЕНТЕ.
        /// </summary>
        public RecordTargetModel RecordTarget { get; set; }
        /// <summary>
        /// [1..1] Данные об авторе документа.
        /// </summary>
        public AuthorDataModel Author { get; set; }
        /// <summary>
        /// [1..1] Организация-владелец документа (организация).
        /// </summary>
        public OrganizationModel RepresentedCustodianOrganization { get; set; }
        /// <summary>
        /// [1..1] ДАННЫЕ О ЛИЦЕ, ПРИДАВШЕМ ЮРИДИЧЕСКУЮ СИЛУ ДОКУМЕНТУ.
        /// </summary>
        public LegalAuthenticatorModel LegalAuthenticator { get; set; }
        /// <summary>
        /// [1..1] СВЕДЕНИЯ ОБ ИСТОЧНИКЕ ОПЛАТЫ.
        /// </summary>
        public ParticipantModel Participant { get; set; }
        /// <summary>
        /// [1..1] Проведённая врачебная комиссия.
        /// </summary>
        public ServiceEventModel ServiceEvent { get; set; }
        /// <summary>
        /// [1..1] ТЕЛО ДОКУМЕНТА.
        /// </summary>
        public DocumentBodyModel DocumentBody { get; set; }
    }
}
