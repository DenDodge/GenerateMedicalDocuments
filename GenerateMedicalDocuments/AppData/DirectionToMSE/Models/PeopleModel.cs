using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Обобщенная модель человека.
    /// </summary>
    public class PeopleModel
    {
        /// <summary>
        /// СНИЛС.
        /// [1..1] - для пациента.
        /// [0..1] - для законного представителя.
        /// </summary>
        public string SNILS { get; set; } = null;
        /// <summary>
        /// Документ, удостоверяющий личность , серия, номер, кем выдан.
        /// [1..1] - для пациента.
        /// [0..1] - для законного представителя.
        /// </summary>
        public DocumentModel IdentityDocument { get; set; } = null;
        /// <summary>
        /// [0..1] Адрес фактического места жительства пациента.
        /// </summary>
        public AddressModel? ActualAddress { get; set; } = null;
        /// <summary>
        /// Контакты (телефон).
        /// [1..1] - для пациента.
        /// [0..1] - для законного представителя.
        /// </summary>
        public TelecomModel ContactPhoneNumber { get; set; }
        /// <summary>
        /// [0..*] Контакты  (мобильный телефон, электронная почта, факс, url).
        /// </summary>
        public List<TelecomModel> Contacts { get; set; } = null;
    }
}
