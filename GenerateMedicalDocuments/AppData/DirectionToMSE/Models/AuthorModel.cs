namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    public class AuthorModel : PeopleModel
    {
        /// <summary>
        /// [1..1] Уникальный идентификатор автора в МИС.
        /// </summary>
        public IDType ID { get; set; }
        /// <summary>
        /// [1..1] Код должности автора.
        /// </summary>
        public TypeModel Position { get; set; }
        /// <summary>
        /// [0..1] Адрес автора документа.
        /// </summary>
        public AddressModel Address { get; set; } = null;
        /// <summary>
        /// [1..1] Фамилия, Имя, Отчество автора.
        /// </summary>
        public NameModel Name { get; set; }
        /// <summary>
        /// [0..1] Место работы автора.
        /// </summary>
        public OrganizationModel RepresentedOrganization { get; set; } = null;
    }
}
