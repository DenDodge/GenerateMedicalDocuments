namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента "patientRole".
    /// [1..1] ПАЦИЕНТ (роль).
    /// </summary>
    public class PatientModel : PeopleModel
    {
        /// <summary>
        /// [1..1] Уникальный идентификатор пациента в МИС.
        /// </summary>
        public IDType ID { get; set; }
        /// <summary>
        /// [1..1] Полис ОМС.
        /// </summary>
        public InsurancePolicyModel InsurancePolicy { get; set; }
        /// <summary>
        /// [0..1] Адрес постоянной регистрации пациента.
        /// </summary>
        public AddressModel? PermanentAddress { get; set; } = null;
        /// <summary>
        /// [1..1] Фамилия, имя, отчество, дата рождения, пол.
        /// </summary>
        public PeopleDataModel PatientData { get; set; }
        /// <summary>
        /// [0..1] Законный (уполномоченный) представитель.
        /// </summary>
        public GuardianModel Guardian { get; set; } = null;
        /// <summary>
        /// [1..1] Организация (ЛПУ или его филиал), направившая на медико-социальную экспертизу.
        /// </summary>
        public OrganizationModel ProviderOrganization { get; set; }
    }
}
