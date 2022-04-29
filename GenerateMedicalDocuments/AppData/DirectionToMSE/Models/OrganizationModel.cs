using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель организации.
    /// </summary>
    public class OrganizationModel
    {
        /// <summary>
        /// [1..1] Уникальный идентификатор организации.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// [0..1] Номер лицензии на осуществление медицинской деятельности. Обязательно указывать при внесении данных о индивидуальном предпринимателе.
        /// </summary>
        public LicenseModel License { get; set; }
        /// <summary>
        /// [1..1] Реквизиты организации.
        /// </summary>
        public PropsOrganizationModel Props { get; set; }
        /// <summary>
        /// [1..1] Наименование организации.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// [1..1] Контакты организации (телефон).
        /// </summary>
        public TelecomModel ContactPhoneNumber { get; set; }
        /// <summary>
        /// [0..*] Контакты организации (факс, веб-сайт).
        /// </summary>
        public List<TelecomModel> Contacts { get; set; } = null;
        /// <summary>
        /// [1..1] Адрес организации.
        /// </summary>
        public AddressModel Address { get; set; }
    }
}
