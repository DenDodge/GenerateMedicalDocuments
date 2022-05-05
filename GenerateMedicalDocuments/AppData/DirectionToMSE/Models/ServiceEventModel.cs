using System;
using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// [1..1] Проведённая врачебная комиссия.
    /// </summary>
    public class ServiceEventModel
    {
        /// <summary>
        /// Тип документированных событий.
        /// </summary>
        public TypeModel Code { get; set; }
        /// <summary>
        /// [1..1] Дата начала.
        /// </summary>
        public DateTime StartServiceDate { get; set; }
        /// <summary>
        /// [0..1] Дата окончания.
        /// </summary>
        public DateTime FinishServiceDate { get; set; }
        /// <summary>
        /// [0..1] Форма оказания медицинской помощи.
        /// </summary>
        public TypeModel ServiceForm { get; set; } = null;
        /// <summary>
        /// [0..1] Вид оказания медицинской помощи.
        /// </summary>
        public TypeModel ServiceType { get; set; } = null;
        /// <summary>
        /// [0..1] Условия оказания медицинской помощи.
        /// </summary>
        public TypeModel ServiceCond { get; set; } = null;
        /// <summary>
        /// [1..1] Непосредственный исполнитель документированного события.
        /// </summary>
        public PerformerModel Performer { get; set; }
        /// <summary>
        /// [0..*] Участиники документированного события.
        /// </summary>
        public List<PerformerModel> OtherPerformers { get; set; }

    }
}
