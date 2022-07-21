using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элементов:
    /// - "addr" [0..1] Адрес постоянной регистрации пациента.
    /// - "fias:Address" [1..1] Кодирование адреса по ФИАС.
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// Для адреса пациента - [1..1] Тип адреса.
        /// </summary>
        public TypeModel Type { get; set; } = null;
        /// <summary>
        /// [1..1] Адрес текстом.
        /// </summary>
        public string StreetAddressLine { get; set; }
        /// <summary>
        /// [1..1] Кодирование субъекта РФ (Код региона в ФНС по справочнику "Субъекты Российской Федерации" (OID:1.2.643.5.1.13.13.99.2.206)).
        /// </summary>
        public TypeModel StateCode { get; set; }
        /// <summary>
        /// [1..1] Почтовый индекс.
        /// </summary>
        public int? PostalCode { get; set; }
        /// <summary>
        /// [1..1] Кодирование адреса по ФИАС.
        /// [1..1] Глобальный уникальный идентификатор адресного объекта
        /// </summary>
        public Guid AOGUID { get; set; }
        /// <summary>
        /// [1..1] Кодирование адреса по ФИАС.
        /// [1..1] Глобальный уникальный идентификатор дома.
        /// </summary>
        public Guid? HOUSEGUID { get; set; }

        #region Other fields from print form

        /// <summary>
        /// Государство.
        /// </summary>
        public string Nation { get; set; } = null;

        /// <summary>
        /// Субъект Российской Федерации.
        /// </summary>
        public string SubjectOfRussianFediration { get; set; } = null;

        /// <summary>
        /// Район.
        /// </summary>
        public string District { get; set; } = null;

        /// <summary>
        /// Наименование населенного пункта.
        /// </summary>
        public string LocalityName { get; set; } = null;

        /// <summary>
        /// Улица.
        /// </summary>
        public string Street { get; set; } = null;

        /// <summary>
        /// Дом.
        /// </summary>
        public string House { get; set; } = null;

        /// <summary>
        /// Квартира.
        /// </summary>
        public string Apartment { get; set; } = null;

        #endregion
    }
}
