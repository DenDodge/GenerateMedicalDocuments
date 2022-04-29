namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель данных уполномоченного представителя.
    /// </summary>
    public class GuardianModel : PeopleModel
    {
        /// <summary>
        /// [0..1] Документ, удостоверяющий полномочия представителя, серия, номер, кем выдан.
        /// </summary>
        public DocumentModel AuthorityDocument { get; set; }
        /// <summary>
        /// [1..1] ФИО представителя.
        /// </summary>
        public NameModel Name { get; set; }
    }
}
