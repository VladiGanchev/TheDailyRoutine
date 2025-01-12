namespace TheDailyRoutine.Core.Models.ServiceModels.Completions
{
    public class DailyCompletionSummaryModel
    {
        public DateTime Date { get; set; }

        public int TotalHabits { get; set; }

        public int CompletedHabits { get; set; }

        public double CompletionRate { get; set; }

        public ICollection<HabitCompletionModel> HabitCompletions { get; set; } =
            new List<HabitCompletionModel>();
    }
}