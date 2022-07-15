namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Helpers.MainHelpers
{
    /// <summary>
    /// Помощник валидных значений.
    /// </summary>
    public static class ValidationContents
    {
        /// <summary>
        /// Значение "медикосоциальную экспертизу необходимо проводить на дому".
        /// </summary>
        public static string MedicalExaminationNeedAtHome = "медикосоциальную экспертизу необходимо проводить на дому";

        /// <summary>
        /// Значение "нуждается в оказании паллиативной медицинской".
        /// </summary>
        public static string NeedPalliativeMedicalHelp = "нуждается в оказании паллиативной медицинской";

        /// <summary>
        /// Значение "нуждается в первичном протезировании".
        /// </summary>
        public static string NeedPrimaryProsthetics = "нуждается в первичном протезировании";

        #region Citizenship
        
        /// <summary>
        /// Значение "Гражданин Российской Федерации".
        /// </summary>
        public static string CitizenOfRussianFederation = "Гражданин Российской Федерации";

        /// <summary>
        /// Значение "Гражданин Российской Федерации и иностранного государства (двойное гражданство)".
        /// </summary>
        public static string DualCitizenship =
            "Гражданин Российской Федерации и иностранного государства (двойное гражданство)";

        /// <summary>
        /// Значение "Иностранный гражданин".
        /// </summary>
        public static string ForeignCitizen = "Иностранный гражданин";

        /// <summary>
        /// Значение "Лицо без гражданства".
        /// </summary>
        public static string StatelessPerson = "Лицо без гражданства";

        #endregion

        #region PurposeOfReferral

        /// <summary>
        /// Значение "Установление группы инвалидности".
        /// </summary>
        public static string EstablishingDisabilityGroup = "Установление группы инвалидности";

        /// <summary>
        /// Значение "Установление категории \"ребенок-инвалид\"".
        /// </summary>
        public static string EstablishingCategoryDisabledChild = "Установление категории \"ребенок-инвалид\"";

        /// <summary>
        /// Значение "Установление причины инвалидности".
        /// </summary>
        public static string EstablishingCauseDisability = "Установление причины инвалидности";

        /// <summary>
        /// Значение "Установление времени наступления инвалидности".
        /// </summary>
        public static string EstablishingTimeOfOnsetOfDisability = "Установление времени наступления инвалидности";

        /// <summary>
        /// Значение "Установление срока инвалидности".
        /// </summary>
        public static string EstablishmentOfTermOfDisability = "Установление срока инвалидности";

        /// <summary>
        /// Значение "Определение степени утраты профессиональной трудоспособности в процентах".
        /// </summary>
        public static string DeterminationOfDegreeOfLossOfProfessionalAbilityToWorkInPercent =
            "Определение степени утраты профессиональной трудоспособности в процентах";

        /// <summary>
        /// Значение "Определение стойкой утраты трудоспособности сотрудника органа внутренних дел Российской Федерации; сотрудника органов принудительного исполнения Российской Федерации".
        /// </summary>
        public static string DeterminationOfPermanentDisabilityOfEmployeeOfInternalAffairsBodyOfRussianFederation =
            "Определение стойкой утраты трудоспособности сотрудника органа внутренних дел Российской Федерации; сотрудника органов принудительного исполнения Российской Федерации";

        /// <summary>
        /// Значение "Определение нуждаемости по состоянию здоровья в постоянном постороннем уходе (помощи, надзоре) отца, матери, жены, родного брата, родной сестры, дедушки, бабушки или усыновителя гражданина, призываемого на военную службу (военнослужащего, проходящего военную службу по контракту)".
        /// </summary>
        public static string DeterminingNeedForHealthReasonsInConstantOutsideCare =
            "Определение нуждаемости по состоянию здоровья в постоянном постороннем уходе (помощи, надзоре) отца, матери, жены, родного брата, родной сестры, дедушки, бабушки или усыновителя гражданина, призываемого на военную службу (военнослужащего, проходящего военную службу по контракту)";

        /// <summary>
        /// Значение "Разработка индивидуальной программы реабилитации или абилитации инвалида (ребенка-инвалида)".
        /// </summary>
        public static string DevelopmentOfAnIndividualProgramForTheRehabilitationOrHabilitationOfDisabledPerson =
            "Разработка индивидуальной программы реабилитации или абилитации инвалида (ребенка-инвалида)";

        /// <summary>
        /// Значение "Разработка программы реабилитации пострадавшего в результате несчастного случая на производстве и профессионального заболевания".
        /// </summary>
        public static string DevelopmentOfProgramForRehabilitationOfVictimAsEesultOfAnAccident =
            "Разработка программы реабилитации пострадавшего в результате несчастного случая на производстве и профессионального заболевания";

        #endregion

        #region AttitudeTowardsMilitaryService

        /// <summary>
        /// Значение "Гражданин, состоящий на воинском учёте".
        /// </summary>
        public static string CitizenInMilitary = "Гражданин, состоящий на воинском учёте";

        /// <summary>
        /// Значение "Гражданин, поступающий на воинский учёт".
        /// </summary>
        public static string CitizenEnteringMilitaryRegistration = "Гражданин, поступающий на воинский учёт";

        /// <summary>
        /// Значение "Гражданин, не состоящий на воинском учёте, но обязанный состоять на воинском учёте".
        /// </summary>
        public static string CitizenNotEegisteredMilitaryButPbligedRegisteredMilitary =
            "Гражданин, не состоящий на воинском учёте, но обязанный состоять на воинском учёте";

        /// <summary>
        /// Значение "Гражданин, не состоящий на воинском учёте".
        /// </summary>
        public static string CitizenNotEegisteredMilitary = "Гражданин, не состоящий на воинском учёте";

        #endregion
    }
}