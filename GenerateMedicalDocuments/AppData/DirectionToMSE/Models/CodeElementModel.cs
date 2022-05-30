namespace GenerateMedicalDocuments.AppData.DirectionToMSE.Models
{
    /// <summary>
    /// Модель элемента "code".
    /// </summary>
    public class CodeElementModel
    {
        public string Code { get; set; } = null;
        public string CodeSystem { get; set; } = null;
        public string CodeSystemVersion { get; set; } = null;
        public string CodeSystemName { get; set; } = null;
        public string DisplayName { get; set; } = null;
    }
}
