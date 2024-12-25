using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;
using TheDailyRoutine.Core.Models.ServiceModels.Statistics;

namespace TheDailyRoutine.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        public Task<IEnumerable<AchievementProgressModel>> GetAchievementProgressAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HabitStreakModel>> GetAllStreaksAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyCompletionSummaryModel>> GetDailyTrendsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DayOfWeekStatsModel>> GetDayOfWeekStatsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HabitCorrelationModel>> GetHabitCorrelationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<HabitStatisticsModel> GetHabitStatisticsAsync(string userId, int habitId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<MonthlyProgressModel> GetMonthlyProgressAsync(string userId, DateTime month)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HabitSuccessModel>> GetMostSuccessfulHabitsAsync(string userId, int count = 5)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetOverallCompletionRateAsync(string userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TimeOfDayStatsModel>> GetTimeOfDayStatsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
