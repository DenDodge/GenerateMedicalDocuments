namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель типа докуммента.
    /// </summary>
    public class DocumentType
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string CodeSystem { get; set; }
        public string CodeSystemVersion { get; set; }
        public string CodeSystemName { get; set; }
        public string DisplayName { get; set; }
    }
}
