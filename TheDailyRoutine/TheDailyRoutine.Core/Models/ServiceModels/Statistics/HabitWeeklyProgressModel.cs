namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitWeeklyProgressModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int RequiredFrequency { get; set; }

        public int ActualCompletions { get; set; }

        public double CompletionRate { get; set; }

        public bool IsOnTrack => ActualCompletions >= RequiredFrequency;
    }
}