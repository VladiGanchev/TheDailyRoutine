using TheDailyRoutine.Core.Models.ServiceModels.Completions;
using TheDailyRoutine.Core.Models.ServiceModels.Statistics;

namespace TheDailyRoutine.Core.Contracts
{
    public interface IStatisticsService
    {
        /// <summary>
        /// Gets completion rate for all habits in a date range
        /// </summary>
        Task<double> GetOverallCompletionRateAsync(
            string userId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets daily completion trends
        /// </summary>
        Task<IEnumerable<DailyCompletionSummaryModel>> GetDailyTrendsAsync(
            string userId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets statistics for a specific habit
        /// </summary>
        Task<HabitStatisticsModel> GetHabitStatisticsAsync(
            string userId,
            int habitId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets the most successful habits for a user
        /// </summary>
        Task<IEnumerable<HabitSuccessModel>> GetMostSuccessfulHabitsAsync(
            string userId,
            int count = 5);

        /// <summary>
        /// Gets completion statistics by time of day
        /// </summary>
        Task<IEnumerable<TimeOfDayStatsModel>> GetTimeOfDayStatsAsync(
            string userId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets completion statistics by day of week
        /// </summary>
        Task<IEnumerable<DayOfWeekStatsModel>> GetDayOfWeekStatsAsync(
            string userId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets streak information for all habits
        /// </summary>
        Task<IEnumerable<HabitStreakModel>> GetAllStreaksAsync(string userId);

        /// <summary>
        /// Gets habit correlation statistics
        /// </summary>
        Task<IEnumerable<HabitCorrelationModel>> GetHabitCorrelationsAsync(string userId);

        /// <summary>
        /// Gets user's achievement progress
        /// </summary>
        Task<IEnumerable<AchievementProgressModel>> GetAchievementProgressAsync(string userId);

        /// <summary>
        /// Gets monthly progress summary
        /// </summary>
        Task<MonthlyProgressModel> GetMonthlyProgressAsync(
            string userId,
            DateTime month);
    }
}