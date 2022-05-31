using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "Витальные параметры".
    /// </summary>
    public class VitalParametersSectionModel
    {
        /// <summary>
        /// Масса тела.
        /// </summary>
        public int BodyMass { get; set; }
        /// <summary>
        /// Дата измерения массы тела.
        /// </summary>
        public DateTime DateMeteringBodyMass { get; set; }
        /// <summary>
        /// Масса тела при рождении (в отношении детей в возрасте до 3 лет).
        /// </summary>
        public int BirthWeight { get; set; }
        /// <summary>
        /// Дата измерения массы тела при рождении.
        /// </summary>
        public DateTime DateMeteringBirthWeight { get; set; }
        /// <summary>
        /// Рост.
        /// </summary>
        public double Growth { get; set; }
        /// <summary>
        /// Дата измерения роста.
        /// </summary>
        public DateTime DateMeteringGrowth { get; set; }
        /// <summary>
        /// Индекс массы тела.
        /// </summary>
        public double IMT { get; set; }
        /// <summary>
        /// Дата измерения индекса массы тела.
        /// </summary>
        public DateTime DateMeteringIMT { get; set; }
        /// <summary>
        /// Телосложение.
        /// </summary>
        public string BodyType { get; set; }
        /// <summary>
        /// Суточный объём физиологических отправлений.
        /// </summary>
        public int PhysiologicalShipmentsVolume { get; set; }
        /// <summary>
        /// Дата измерения суточного объема фищиологических отправлений.
        /// </summary>
        public DateTime DateMeteringPhysiologicalShipmentsVolume { get; set; }
        /// <summary>
        /// Объём талии.
        /// </summary>
        public int Waist { get; set; }
        /// <summary>
        /// Дата измерения объёма талии.
        /// </summary>
        public DateTime DateMeteringWaist { get; set; }
        /// <summary>
        /// Объём бёдер.
        /// </summary>
        public int Hips { get; set; }
        /// <summary>
        /// Дата измерения объема бёдер.
        /// </summary>
        public DateTime DateMeteringHips { get; set; }
    }
}
