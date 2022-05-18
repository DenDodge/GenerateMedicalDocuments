using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель сведений о турдовой деятельности.
    /// </summary>
    public class WorkActivityModel
    {
        /// <summary>
        /// [1..1] Место работы.
        /// </summary>
        public OrganizationModel Workpalace { get; set; }
        /// <summary>
        /// [1..1] Основная профессия (специальность, должность).
        /// </summary>
        public string MainProfession { get; set; }
        /// <summary>
        /// [1..1] Квалификация (класс, разряд, категория, звание).
        /// </summary>
        public string Qualification { get; set; }
        /// <summary>
        /// [1..1] Стаж работы.
        /// </summary>
        public string WorkExperience { get; set; }
        /// <summary>
        /// [1..*] Выполняемая работа на момент направления на медико-социальную экспертизу.
        /// </summary>
        public List<(string Profession, string Speciality, string Position)> WorkPerformeds { get; set; }
        /// <summary>
        /// [1..1] Условия и характер выполняемого труда.
        /// </summary>
        public string Conditions { get; set; }
    }
}
