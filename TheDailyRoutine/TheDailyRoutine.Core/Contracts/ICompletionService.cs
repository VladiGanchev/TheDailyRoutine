using System.Collections.Generic;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;
using TheDailyRoutine.Core.Models.ServiceModels.Statistics;

namespace TheDailyRoutine.Core.Contracts
{
    public interface ICompletionService
    {
        /// <summary>
        /// Marks a habit as complete for a specific date
        /// </summary>
        Task<(bool success, string error)> MarkHabitAsCompleteAsync(
            string userId,
            int habitId,
            DateTime date,
            string notes);

        /// <summary>
        /// Marks a habit as incomplete for a specific date
        /// </summary>
        Task<(bool success, string error)> MarkHabitAsIncompleteAsync(
            string userId,
            int habitId,
            DateTime date);

        /// <summary>
        /// Gets all completions for a user's habit within a date range
        /// </summary>
        Task<IEnumerable<CompletionDetailsModel>> GetHabitCompletionsAsync(
            string userId,
            int habitId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets a daily summary of all habit completions for a user
        /// </summary>
        Task<DailyCompletionSummaryModel> GetDailyCompletionSummaryAsync(
            string userId,
            DateTime date);

        /// <summary>
        /// Gets completion details for a specific completion entry
        /// </summary>
        Task<CompletionDetailsModel?> GetCompletionDetailsAsync(int completionId);

        /// <summary>
        /// Updates the notes for a completion entry
        /// </summary>
        Task<(bool success, string error)> UpdateCompletionNotesAsync(
            int completionId,
            string newNotes);

        /// <summary>
        /// Gets weekly completion summary for all habits
        /// </summary>
        Task<WeeklyCompletionSummaryModel> GetWeeklyCompletionSummaryAsync(
            string userId,
            DateTime weekStartDate);

        /// <summary>
        /// Gets streaks summary for all habits
        /// </summary>
        Task<IEnumerable<HabitStreakModel>> GetHabitStreaksAsync(string userId);
    }
}