using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    public class SentSectionModel
    {
        /// <summary>
        /// [1..1] код секции.
        /// </summary>
        public TypeModel Code { get; set; }
        /// <summary>
        /// Наполнение секции "Направление".
        /// </summary>
        public List<ParagraphModel> SentParagraphs { get; set; }
        /// <summary>
        /// [1..1] Кодирование цели направления и медицинской организации, куда направлен пациент.
        /// </summary>
        public TargetSentModel TargetSent { get; set; }
        /// <summary>
        /// [1..1] кодирование гражданства.
        /// </summary>
        public TypeModel Сitizenship { get; set; }
        /// <summary>
        /// [1..1] Тип местонахождения гражданина.
        /// </summary>
        public TypeModel PatientLocationCode { get; set; }
        /// <summary>
        /// [1..1] Гражданин находится.
        /// </summary>
        public OrganizationModel PatientLocation { get; set; }
        /// <summary>
        /// [1..1] кодирование отношения к воинской обязанности.
        /// </summary>
        public TypeModel MilitaryDuty { get; set; }
    }
}
