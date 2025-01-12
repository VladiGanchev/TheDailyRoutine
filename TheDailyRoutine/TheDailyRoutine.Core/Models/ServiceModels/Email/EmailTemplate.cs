namespace TheDailyRoutine.Core.Models.ServiceModels.Email
{
    public class EmailTemplate
    {
        public string Subject { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;
        public string PlainTextContent { get; set; } = string.Empty;
        public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
    }
}
