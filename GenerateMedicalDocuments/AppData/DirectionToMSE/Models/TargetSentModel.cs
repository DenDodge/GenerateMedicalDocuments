using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель кодирования цели направления и медицинской организации, куда направлен пациент.
    /// </summary>
    public class TargetSentModel
    {
        /// <summary>
        /// [1..1] Тип направления.
        /// </summary>
        public TypeModel SentType { get; set; }
        /// <summary>
        /// [1..1] Кодирование организации, куда направлен пациент.
        /// </summary>
        public OrganizationModel PerformerOrganization { get; set; }
        /// <summary>
        /// [1..1] Цель направления.
        /// </summary>
        public TypeModel TargetSentType { get; set; }
        /// <summary>
        /// [1..1] Порядок обращения.
        /// </summary>
        public TypeModel SentOrder { get; set; }
        /// <summary>
        /// [1..1] Дата и номер протокола врачебной комиссии.
        /// </summary>
        public ProtocolModel Protocol { get; set; }
        /// <summary>
        /// [1..1] Экспертиза проводится на дому.
        /// </summary>
        public bool IsAtHome { get; set; }
        /// <summary>
        /// [1..1] Нуждаемость в оказании паллиативной медицинской помощи.
        /// </summary>
        public bool IsPalleativeMedicalHelp { get; set; }
        /// <summary>
        /// [1..1] Нуждаемость в первичном протезировании.
        /// </summary>
        public bool NeedPrimaryProsthetics { get; set; }
        /// <summary>
        /// [1..1] Дата выдачи направления.
        /// </summary>
        public DateTime SentDate { get; set; }
    }
}
