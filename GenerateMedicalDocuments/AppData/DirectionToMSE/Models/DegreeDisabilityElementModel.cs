using System;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента степени утраты трудоспособности.
    /// </summary>
    public class DegreeDisabilityElementModel
    {
        /// <summary>
        /// Идентификатор элемента.
        /// </summary>
        public string ID { get; set; } = null;
        /// <summary>
        /// Полный текст.
        /// </summary>
        public string FullText { get; set; } = null;
        /// <summary>
        /// Дата до которой установлена степень утраты профессиональной трудоспособности
        /// </summary>
        public DateTime? DateTo { get; set; } = null;
        /// <summary>
        /// Срок.
        /// </summary>
        public TermModel Term { get; set; } = null;
        /// <summary>
        /// Процент.
        /// </summary>
        public int? Percent { get; set; } = null;
    }

    /// <summary>
    /// Модель параметра "Срок, на который установлена степень утраты профессиональной трудоспособности".
    /// </summary>
    public class TermModel
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Срок, на который установлена степень утраты профессиональной трудоспособности
        /// </summary>
        public string DisplayName { get; set; }
    }
}