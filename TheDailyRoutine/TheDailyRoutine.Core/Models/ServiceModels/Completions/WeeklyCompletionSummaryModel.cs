using TheDailyRoutine.Core.Models.ServiceModels.Statistics;

namespace TheDailyRoutine.Core.Models.ServiceModels.Completions
{
    public class WeeklyCompletionSummaryModel
    {
        public DateTime WeekStartDate { get; set; }

        public DateTime WeekEndDate { get; set; }

        public double AverageCompletionRate { get; set; }

        public ICollection<DailyCompletionSummaryModel> DailySummaries { get; set; } =
            new List<DailyCompletionSummaryModel>();

        public ICollection<HabitWeeklyProgressModel> HabitProgress { get; set; } =
            new List<HabitWeeklyProgressModel>();
    }
}