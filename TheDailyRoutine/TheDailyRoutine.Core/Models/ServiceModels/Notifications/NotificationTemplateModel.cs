namespace TheDailyRoutine.Core.Models.ServiceModels.Notifications
{
    public class NotificationTemplateModel
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public IDictionary<string, string> Placeholders { get; set; } =
            new Dictionary<string, string>();
    }
}
