using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель протокола врочебной комисии.
    /// </summary>
    public class ProtocolModel
    {
        /// <summary>
        /// [1..1] Кодирование даты и номера протокола врачебной комиссии.
        /// </summary>
        public TypeModel Protocol { get; set; }
        /// <summary>
        /// [1..1] Дата протокола врачебной комиссии.
        /// </summary>
        public DateTime ProtocolDate { get; set; }
        /// <summary>
        /// [1..1] Номер протокола врачебной комиссии.
        /// </summary>
        public string ProtocolNumber { get; set; }
    }
}
