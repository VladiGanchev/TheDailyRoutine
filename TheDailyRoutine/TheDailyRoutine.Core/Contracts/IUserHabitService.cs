using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;

namespace TheDailyRoutine.Core.Contracts
{
    public interface IUserHabitService
    {
        /// <summary>
        /// Adds a habit to a user's list of habits
        /// </summary>
        Task<(bool success, string error)> AddHabitToUserAsync(string userId, int habitId, int frequency);

        /// <summary>
        /// Removes a habit from a user's list
        /// </summary>
        Task<(bool success, string error)> RemoveHabitFromUserAsync(string userId, int habitId);

        /// <summary>
        /// Updates the frequency of a user's habit
        /// </summary>
        Task<(bool success, string error)> UpdateHabitFrequencyAsync(string userId, int habitId, int newFrequency);

        /// <summary>
        /// Gets all habits for a specific user
        /// </summary>
        Task<IEnumerable<UserHabitDetailsModel>> GetUserHabitsAsync(string userId);

        /// <summary>
        /// Gets a specific habit for a user with its completion details
        /// </summary>
        Task<UserHabitDetailsModel?> GetUserHabitAsync(string userId, int habitId);

        /// <summary>
        /// Gets habits that need attention (missing completions)
        /// </summary>
        Task<IEnumerable<UserHabitDetailsModel>> GetHabitsNeedingAttentionAsync(string userId);

        /// <summary>
        /// Gets the user's most consistent habits
        /// </summary>
        Task<IEnumerable<UserHabitDetailsModel>> GetMostConsistentHabitsAsync(string userId, int count = 5);

        /// <summary>
        /// Gets user's current streaks for all habits
        /// </summary>
        Task<IDictionary<int, int>> GetCurrentStreaksAsync(string userId);

        /// <summary>
        /// Checks if a user has completed their habit goals for the day
        /// </summary>
        Task<bool> HasCompletedDailyGoalsAsync(string userId, DateTime date);

        /// <summary>
        /// Gets the completion rate for a specific period
        /// </summary>
        Task<double> GetCompletionRateAsync(string userId, int habitId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<string>> GetActiveUsersAsync();
        Task<IDictionary<(string userId, int habitId), int>> GetCurrentStreaksAsync();
        Task<Dictionary<string, List<UserHabitDetailsModel>>> GetHabitsNeedingAttentionAsync(DateTime date);

    }
}
