namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitStreakModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CurrentStreak { get; set; }

        public int BestStreak { get; set; }

        public DateTime LastCompletedDate { get; set; }

        public bool IsStreakActive { get; set; }
    }
}