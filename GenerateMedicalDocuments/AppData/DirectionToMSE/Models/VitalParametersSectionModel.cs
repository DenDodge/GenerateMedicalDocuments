using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "Витальные параметры".
    /// </summary>
    public class VitalParametersSectionModel
    {
        /// <summary>
        /// Список витальных параметров.
        /// </summary>
        public List<VitalParameterModel> VitalParameters { get; set; }
        /// <summary>
        /// Телосложение.
        /// </summary>
        public string BodyType { get; set; }
    }
}
