namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель тела документа.
    /// </summary>
    public class DocumentBodyModel
    {
        /// <summary>
        /// [1..1] СЕКЦИЯ: НАПРАВЛЕН.
        /// </summary>
        public SentSectionModel SentSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: Место работы, должность.
        /// </summary>
        public WorkplaceSectionModel WorkplaceSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: Образование
        /// </summary>
        public EducationSectionModel EducationSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: АНАМНЕЗ.
        /// </summary>
        public AnamnezSectionModel AnamnezSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: ВИТАЛЬНЫЕ ПАРАМЕТРЫ.
        /// </summary>
        public VitalParametersSectionModel VitalParametersSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: СОСТОЯНИЕ ПРИ НАПРАВЛЕНИИ.
        /// </summary>
        public DirectionStateSectionModel DirectionStateSection { get; set; }
        /// <summary>
        /// [0..1] СЕКЦИЯ: ДИАГНОСТИЧЕСКИЕ ИССЛЕДОВАНИЯ И КОНСУЛЬТАЦИИ.
        /// </summary>
        public DiagnosticStudiesSectionModel DiagnosticStudiesSection { get; set; } = null;
        /// <summary>
        /// [1..1] СЕКЦИЯ: ДИАГНОЗЫ.
        /// </summary>
        public DiagnosisSectionModel DiagnosisSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: ОБЪЕКТИВИЗИРОВАННАЯ ОЦЕНКА СОСТОЯНИЯ.
        /// </summary>
        public ConditionAssessmentSection ConditionAssessmentSection { get; set; }
        /// <summary>
        /// [1..1] СЕКЦИЯ: РЕКОМЕНДАЦИИ.
        /// </summary>
        public RecommendationsSectionModel RecommendationsSection { get; set; }
        /// <summary>
        /// [0..1] СЕКЦИЯ: Посторонний специальный медицинский уход.
        /// </summary>
        public OutsideSpecialMedicalCareSection OutsideSpecialMedicalCareSection { get; set; } = null;
        /// <summary>
        /// [0..1] СЕКЦИЯ: СВЯЗАННЫЕ ДОКУМЕНТЫ.
        /// </summary>
        public AttachmentDocumentsSection AttachmentDocumentsSection { get; set; } = null;
    }
}
