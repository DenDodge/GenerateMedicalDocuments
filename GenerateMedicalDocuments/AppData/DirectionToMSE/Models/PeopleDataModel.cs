using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель данных о человеке (ФИО, дата рождения...).
    /// </summary>
    public class PeopleDataModel
    {
        /// <summary>
        /// [1..1] ФИО.
        /// </summary>
        public NameModel Name { get; set; }
        /// <summary>
        /// [1..1] Пол.
        /// </summary>
        public TypeModel Gender { get; set; }
        /// <summary>
        /// [1..1] Дата рождения.
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
