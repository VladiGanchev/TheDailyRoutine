using TheDailyRoutine.Core.Models.Enums;


namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class UserStatisticsSummaryModel
    {
        public int TotalActiveHabits { get; set; }
        public int TotalCompletions { get; set; }
        public double OverallSuccessRate { get; set; }
        public int LongestStreak { get; set; }
        public List<string> TopPerformingHabits { get; set; } = new List<string>();
        public List<string> AreasForImprovement { get; set; } = new List<string>();
        public TimeSlot MostProductiveTimeSlot { get; set; }
        public DayOfWeek MostProductiveDay { get; set; }
        public int TotalAchievements { get; set; }
        public string NextMilestone { get; set; } = string.Empty;
    }

}
