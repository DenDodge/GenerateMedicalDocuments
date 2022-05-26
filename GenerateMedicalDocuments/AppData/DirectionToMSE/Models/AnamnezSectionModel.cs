using System;
using System.Collections.Generic;

namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель секции "Анамнез".
    /// </summary>
    public class AnamnezSectionModel
    {
        /// <summary>
        /// Инвалидность.
        /// </summary>
        public DisabilityModel Disability { get; set; } = null;
        /// <summary>
        /// Степень утраты профессиональной трудоспособности.
        /// </summary>
        public DegreeDisabilityModel DegreeDisability { get; set; } = null;

        /// <summary>
        /// Наблюдается в организациях, оказывающих лечебно-профилактическую помощь.
        /// </summary>
        public string SeenOrganizations { get; set; } = null;

        /// <summary>
        /// Анамнез заболевания.
        /// </summary>
        public string MedicalAnamnez { get; set; } = null;

        /// <summary>
        /// Анамнез жизни.
        /// </summary>
        public string LifeAnamnez { get; set; } = null;

        /// <summary>
        /// Физическое развитие (в отношении детей в возрасте до 3 лет).
        /// </summary>
        public string ActualDevelopment { get; set; } = null;
        /// <summary>
        /// Временная нетрудоспособность.
        /// </summary>
        public List<TemporaryDisabilityModel> TemporaryDisabilitys { get; set; } = null;
        /// <summary>
        /// Листок нетрудоспособности в форме электронного документа.
        /// </summary>
        public string CertificateDisabilityNumber { get; set; } = null;
        /// <summary>
        /// Результаты и эффективность проведенных мероприятий медицинской реабилитации.
        /// </summary>
        public List<string> EffectityAction { get; set; } = null;

        /// <summary>
        /// Год, с которого наблюдается в медицинской организации.
        /// </summary>
        public int? StartYear { get; set; } = null;
    }

    /// <summary>
    /// Модель "Инвалидность".
    /// </summary>
    public class DisabilityModel
    {
        /// <summary>
        /// Группа инвалидности.
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Находится на инвалидности на момент направления.
        /// </summary>
        public string TimeDisability { get; set; }
    }

    /// <summary>
    /// Модель "Временная нетрудоспособность".
    /// </summary>
    public class TemporaryDisabilityModel
    {
        /// <summary>
        /// Дата начала.
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        /// Дата окончания.
        /// </summary>
        public DateTime DateFinish { get; set; }
        /// <summary>
        /// Число дней.
        /// </summary>
        public string DayCount { get; set; }
        /// <summary>
        /// Шифр МКБ.
        /// </summary>
        public string CipherMKB { get; set; }
        /// <summary>
        /// Диагноз.
        /// </summary>
        public string Diagnosis { get; set; }
    }

    /// <summary>
    /// Модель "Степень утраты профессиональной трудоспособности".
    /// </summary>
    public class DegreeDisabilityModel
    {
        /// <summary>
        /// Секция 1\3.
        /// </summary>
        public string Section31 { get; set; } = null;

        /// <summary>
        /// Секция 2\3.
        /// </summary>
        public string Section32 { get; set; } = null;

        /// <summary>
        /// Секция 3\3.
        /// </summary>
        public string Section33 { get; set; } = null;
    }
}
