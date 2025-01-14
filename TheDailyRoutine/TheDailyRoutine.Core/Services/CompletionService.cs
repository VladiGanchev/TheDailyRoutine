using Microsoft.EntityFrameworkCore;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;
using TheDailyRoutine.Core.Models.ServiceModels.Statistics;
using TheDailyRoutine.Infrastructure.Data;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Core.Services
{
    public class CompletionService : ICompletionService
    {
        private readonly ApplicationDbContext _context;

        public CompletionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string error)> MarkHabitAsCompleteAsync(
            string userId, int habitId, DateTime date, string notes)
        {
            try
            {
                var userHabit = await _context.UsersHabits
                    .Include(uh => uh.Completions)
                    .FirstOrDefaultAsync(uh =>
                        uh.UserId == userId &&
                        uh.HabitId == habitId);

                if (userHabit == null)
                {
                    return (false, "Habit not found for this user.");
                }

                var existingCompletion = userHabit.Completions
                    .FirstOrDefault(c => c.CompletedAt.Date == date.Date);

                if (existingCompletion != null)
                {
                    existingCompletion.Completed = true;
                    existingCompletion.Notes = notes;
                }
                else
                {
                    var completion = new Completion
                    {
                        UserHabit = userHabit,
                        CompletedAt = date,
                        Completed = true,
                        Notes = notes
                    };
                    await _context.Completions.AddAsync(completion);
                }

                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to mark habit as complete: {ex.Message}");
            }
        }

        public async Task<(bool success, string error)> MarkHabitAsIncompleteAsync(
            string userId, int habitId, DateTime date)
        {
            try
            {
                var userHabit = await _context.UsersHabits
                    .Include(uh => uh.Completions)
                    .FirstOrDefaultAsync(uh =>
                        uh.UserId == userId &&
                        uh.HabitId == habitId);

                if (userHabit == null)
                {
                    return (false, "Habit not found for this user.");
                }

                var completion = userHabit.Completions
                    .FirstOrDefault(c => c.CompletedAt.Date == date.Date);

                if (completion != null)
                {
                    completion.Completed = false;
                    await _context.SaveChangesAsync();
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to mark habit as incomplete: {ex.Message}");
            }
        }

        public async Task<IEnumerable<CompletionDetailsModel>> GetHabitCompletionsAsync(
            string userId, int habitId, DateTime startDate, DateTime endDate)
        {
            return await _context.Completions
                .Include(c => c.UserHabit)
                .Where(c => c.UserHabit.UserId == userId &&
                           c.CompletedAt.Date >= startDate.Date &&
                           c.CompletedAt.Date <= endDate.Date)
                .OrderByDescending(c => c.CompletedAt)
                .Select(c => new CompletionDetailsModel
                {
                    Id = c.Id,
                    CompletedAt = c.CompletedAt,
                    Completed = c.Completed,
                    Notes = c.Notes
                })
                .ToListAsync();
        }

        public async Task<DailyCompletionSummaryModel> GetDailyCompletionSummaryAsync(
            string userId, DateTime date)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            var completions = new List<HabitCompletionModel>();
            foreach (var userHabit in userHabits)
            {
                var completion = userHabit.Completions
                    .FirstOrDefault(c => c.CompletedAt.Date == date.Date);

                completions.Add(new HabitCompletionModel
                {
                    HabitId = userHabit.HabitId,
                    Title = userHabit.Habit.Title,
                    IsCompleted = completion?.Completed ?? false,
                    CompletedAt = completion?.CompletedAt,
                    Notes = completion?.Notes ?? string.Empty
                });
            }

            var totalHabits = completions.Count;
            var completedHabits = completions.Count(c => c.IsCompleted);
            var completionRate = totalHabits > 0
                ? (double)completedHabits / totalHabits * 100
                : 0;

            return new DailyCompletionSummaryModel
            {
                Date = date,
                TotalHabits = totalHabits,
                CompletedHabits = completedHabits,
                CompletionRate = Math.Round(completionRate, 2),
                HabitCompletions = completions
            };
        }

        public async Task<CompletionDetailsModel?> GetCompletionDetailsAsync(int completionId)
        {
            return await _context.Completions
                .Where(c => c.Id == completionId)
                .Select(c => new CompletionDetailsModel
                {
                    Id = c.Id,
                    CompletedAt = c.CompletedAt,
                    Completed = c.Completed,
                    Notes = c.Notes
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(bool success, string error)> UpdateCompletionNotesAsync(
            int completionId, string newNotes)
        {
            try
            {
                var completion = await _context.Completions
                    .FindAsync(completionId);

                if (completion == null)
                {
                    return (false, "Completion record not found.");
                }

                completion.Notes = newNotes;
                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to update completion notes: {ex.Message}");
            }
        }

        public async Task<WeeklyCompletionSummaryModel> GetWeeklyCompletionSummaryAsync(
            string userId, DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.AddDays(6);
            var dailySummaries = new List<DailyCompletionSummaryModel>();

            // Get daily summaries for each day of the week
            for (var date = weekStartDate; date <= weekEndDate; date = date.AddDays(1))
            {
                var dailySummary = await GetDailyCompletionSummaryAsync(userId, date);
                dailySummaries.Add(dailySummary);
            }

            // Get habits progress for the week
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            var habitProgress = userHabits.Select(uh =>
            {
                var completionsThisWeek = uh.Completions.Count(c =>
                    c.CompletedAt.Date >= weekStartDate &&
                    c.CompletedAt.Date <= weekEndDate &&
                    c.Completed);

                var requiredFrequency = uh.Frequency;
                var completionRate = requiredFrequency > 0
                    ? (double)completionsThisWeek / requiredFrequency * 100
                    : 0;

                return new HabitWeeklyProgressModel
                {
                    HabitId = uh.HabitId,
                    Title = uh.Habit.Title,
                    RequiredFrequency = requiredFrequency,
                    ActualCompletions = completionsThisWeek,
                    CompletionRate = Math.Round(completionRate, 2)
                };
            }).ToList();

            return new WeeklyCompletionSummaryModel
            {
                WeekStartDate = weekStartDate,
                WeekEndDate = weekEndDate,
                AverageCompletionRate = Math.Round(dailySummaries.Average(ds => ds.CompletionRate), 2),
                DailySummaries = dailySummaries,
                HabitProgress = habitProgress
            };
        }

        public async Task<IEnumerable<HabitStreakModel>> GetHabitStreaksAsync(string userId)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            var streaks = new List<HabitStreakModel>();

            foreach (var userHabit in userHabits)
            {
                var (currentStreak, bestStreak, lastCompletedDate, isActive) =
                    CalculateStreaks(userHabit.Completions);

                streaks.Add(new HabitStreakModel
                {
                    HabitId = userHabit.HabitId,
                    Title = userHabit.Habit.Title,
                    CurrentStreak = currentStreak,
                    BestStreak = bestStreak,
                    LastCompletedDate = lastCompletedDate,
                    IsStreakActive = isActive
                });
            }

            return streaks;
        }

        #region Helper Methods

        private (int currentStreak, int bestStreak, DateTime lastCompletedDate, bool isActive)
            CalculateStreaks(ICollection<Completion> completions)
        {
            if (!completions.Any(c => c.Completed))
            {
                return (0, 0, DateTime.MinValue, false);
            }

            var orderedCompletions = completions
                .Where(c => c.Completed)
                .OrderByDescending(c => c.CompletedAt)
                .ToList();

            var lastCompletedDate = orderedCompletions.First().CompletedAt.Date;
            var isActive = lastCompletedDate >= DateTime.UtcNow.Date.AddDays(-1);

            // Calculate current streak
            int currentStreak = 1;
            for (int i = 1; i < orderedCompletions.Count; i++)
            {
                if (orderedCompletions[i].CompletedAt.Date ==
                    orderedCompletions[i - 1].CompletedAt.Date.AddDays(-1))
                {
                    currentStreak++;
                }
                else
                {
                    break;
                }
            }

            // Calculate best streak
            int bestStreak = 1;
            int tempStreak = 1;

            var ascCompletions = completions
                .Where(c => c.Completed)
                .OrderBy(c => c.CompletedAt)
                .ToList();

            for (int i = 1; i < ascCompletions.Count; i++)
            {
                if (ascCompletions[i].CompletedAt.Date ==
                    ascCompletions[i - 1].CompletedAt.Date.AddDays(1))
                {
                    tempStreak++;
                    bestStreak = Math.Max(bestStreak, tempStreak);
                }
                else
                {
                    tempStreak = 1;
                }
            }

            return (currentStreak, bestStreak, lastCompletedDate, isActive);
        }

        private bool IsStreakBroken(DateTime lastCompletionDate)
        {
            // A streak is broken if the last completion was more than one day ago
            return lastCompletionDate.Date < DateTime.UtcNow.Date.AddDays(-1);
        }

        private DateTime GetWeekStartDate(DateTime date)
        {
            // Assuming weeks start on Monday
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        #endregion
    }
}