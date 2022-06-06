using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "Рекомендации".
    /// </summary>
    public class RecommendationsSectionModel
    {
        /// <summary>
        /// Рекомендуемые мероприятия по реконструктивной хирургии.
        /// </summary>
        public string RecommendedMeasuresReconstructiveSurgery { get; set; }
        /// <summary>
        /// Рекомендуемые мероприятия по протезированию и ортезированию, техническим средствам реабилитации.
        /// </summary>
        public string RecommendedMeasuresProstheticsAndOrthotics { get; set; }
        /// <summary>
        /// Санаторно-курортное лечение.
        /// </summary>
        public string SpaTreatment { get; set; }
        /// <summary>
        /// Перечень лекарственных препаратов.
        /// </summary>
        public List<MedicationModel> Medications { get; set; }
        /// <summary>
        /// Перечень медицинских изделий для медицинского применения.
        /// </summary>
        public string MedicalDevices { get; set; }
        /// <summary>
        /// Прочие рекомендации.
        /// </summary>
        public string OtherRecommendatons { get; set; }
    }
}
