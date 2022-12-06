using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        /// <summary>
        /// Номер ИПРА.
        /// </summary>
        public string IPRANumber { get; set; } = null;
        /// <summary>
        /// Номер протокола проведения медико-социальной экспертизы.
        /// </summary>
        public string ProtocolNumber { get; set; } = null;
        /// <summary>
        /// Дата протокола проведения медико-социальной экспертизы.
        /// </summary>
        public DateTime? ProtocolDate { get; set; } = null;
        /// <summary>
        /// Результаты и эффективность проведенных мероприятий медицинской реабилитации, рекомендованных индивидуальной программой реабилитации или абилитации инвалида (ребенка-инвалида) (ИПРА) (текстовое описание).
        /// </summary>
        public string Results { get; set; } = null;
        /// <summary>
        /// Результаты и эффективность проведенных мероприятий медицинской реабилитации, Восстановление нарушенных функций.
        /// </summary>
        public ResultFunctionModel ResultRestorationFunctions { get; set; } = null;
        /// <summary>
        /// Результаты и эффективность проведенных мероприятий медицинской реабилитации, Достижение компенсации утраченных либо отсутствующих функций.
        /// </summary>
        public ResultFunctionModel ResultCompensationFunction { get; set; } = null;
    }

    /// <summary>
    /// Модель "Инвалидность".
    /// </summary>
    public class DisabilityModel
    {
        /// <summary>
        /// Группа инвалидности.
        /// </summary>
        public GroupModel Group { get; set; } = null;
        /// <summary>
        /// Группа инвалидности (полный текст).
        /// </summary>
        public string GroupText { get; set; } = null;
        /// <summary>
        /// Порядок установления инвалидности.
        /// </summary>
        public GroupOrderModel GroupOrder { get; set; } = null;
        /// <summary>
        /// Срок, на который установлена степень утраты профессиональной трудоспособности.
        /// </summary>
        public GroupTimeModel GroupTime { get; set; } = null;
        /// <summary>
        /// Находится на инвалидности на момент направления.
        /// </summary>
        public TimeDisabilityModel TimeDisability { get; set; }
        /// <summary>
        /// Дата установления инвалидности.
        /// </summary>
        public DateTime? DateDisabilityStart { get; set; } = null;
        /// <summary>
        /// Дата, до которой установлена инвалидность.
        /// </summary>
        public DateTime? DateDisabilityFinish { get; set; } = null;
        /// <summary>
        /// Причина инвалидности.
        /// </summary>
        public CauseOfDisabilityModel CauseOfDisability { get; set; } = null;
    }

    /// <summary>
    /// Модель "Временная нетрудоспособность".
    /// </summary>
    public class TemporaryDisabilityModel
    {
        /// <summary>
        /// Дата начала.
        /// </summary>
        public DateTime? DateStart { get; set; } = null;

        /// <summary>
        /// Дата окончания.
        /// </summary>
        public DateTime? DateFinish { get; set; } = null;
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
        public List<DegreeDisabilityElementModel> DegreeDisabilities { get; set; } = null;
    }

    /// <summary>
    /// Модель результата и эффективности проводимых мероприятий.
    /// </summary>
    public class ResultFunctionModel
    {
        /// <summary>
        /// Код.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Результат.
        /// </summary>
        public string Result { get; set; }
    }

    /// <summary>
    /// Модель параметра "Находится на инвалидности в момент направления".
    /// </summary>
    public class TimeDisabilityModel
    {
        /// <summary>
        /// Код.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Группа инвалидности.
        /// </summary>
        public string Disability { get; set; }
    }

    /// <summary>
    /// Модель параметра "Группы инвалидности"
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// Группа инвалидности
        /// </summary>
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Модель параметра "Тип установления инвалидности (впервые, повторно)"
    /// </summary>
    public class GroupOrderModel
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// Установление инвалидности (впервые, повторно)
        /// </summary>
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Модель параметра "Срок, на который установлена инвалидность"
    /// </summary>
    public class GroupTimeModel
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// Срок, на который установлена инвалидность
        /// </summary>
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Модель параметра "Причины инвалидности"
    /// </summary>
    public class CauseOfDisabilityModel
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// Причина инвалидности
        /// </summary>
        public string DisplayName { get; set; }
    }
}
