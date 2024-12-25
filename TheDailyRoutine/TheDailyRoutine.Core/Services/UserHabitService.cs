using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Infrastructure.Data.Models;
using TheDailyRoutine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;

namespace TheDailyRoutine.Core.Services
{
    public class UserHabitService : IUserHabitService
    {
        private readonly ApplicationDbContext _context;

        public UserHabitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string error)> AddHabitToUserAsync(string userId, int habitId, int frequency)
        {
            try
            {
                if (await _context.UsersHabits.AnyAsync(uh =>
                    uh.UserId == userId && uh.HabitId == habitId))
                {
                    return (false, "User already has this habit.");
                }

                var habit = await _context.Habits.FindAsync(habitId);
                if (habit == null)
                {
                    return (false, "Habit not found.");
                }

                var userHabit = new UserHabit
                {
                    UserId = userId,
                    HabitId = habitId,
                    Frequency = frequency,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.UsersHabits.AddAsync(userHabit);
                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to add habit: {ex.Message}");
            }
        }

        public async Task<(bool success, string error)> RemoveHabitFromUserAsync(string userId, int habitId)
        {
            try
            {
                var userHabit = await _context.UsersHabits
                    .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HabitId == habitId);

                if (userHabit == null)
                {
                    return (false, "Habit not found for this user.");
                }

                _context.UsersHabits.Remove(userHabit);
                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to remove habit: {ex.Message}");
            }
        }

        public async Task<(bool success, string error)> UpdateHabitFrequencyAsync(
            string userId, int habitId, int newFrequency)
        {
            try
            {
                var userHabit = await _context.UsersHabits
                    .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HabitId == habitId);

                if (userHabit == null)
                {
                    return (false, "Habit not found for this user.");
                }

                userHabit.Frequency = newFrequency;
                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to update frequency: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UserHabitDetailsModel>> GetUserHabitsAsync(string userId)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .Select(uh => new UserHabitDetailsModel
                {
                    HabitId = uh.HabitId,
                    Title = uh.Habit.Title,
                    Description = uh.Habit.Description,
                    Frequency = uh.Frequency,
                    CreatedAt = uh.CreatedAt,
                   /* CurrentStreak = CalculateCurrentStreak(uh.Completions),
                    BestStreak = CalculateBestStreak(uh.Completions),
                    CompletionRate = CalculateCompletionRate(uh.Completions),
                    RecentCompletions = uh.Completions
                        .OrderByDescending(c => c.CompletedAt)
                        .Take(10)
                        .Select(c => new CompletionDetailsModel
                        {
                            Id = c.Id,
                            CompletedAt = c.CompletedAt,
                            Completed = c.Completed,
                            Notes = c.Notes
                        })*/
                })
                .OrderBy(h => h.Title)
                .ToListAsync();

            return userHabits;
        }

        public async Task<UserHabitDetailsModel?> GetUserHabitAsync(string userId, int habitId)
        {
            return await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId && uh.HabitId == habitId)
                .Select(uh => new UserHabitDetailsModel
                {
                    HabitId = uh.HabitId,
                    Title = uh.Habit.Title,
                    Description = uh.Habit.Description,
                    Frequency = uh.Frequency,
                    CreatedAt = uh.CreatedAt,
                    CurrentStreak = CalculateCurrentStreak(uh.Completions),
                    BestStreak = CalculateBestStreak(uh.Completions),
                    CompletionRate = CalculateCompletionRate(uh.Completions),
                    RecentCompletions = uh.Completions
                        .OrderByDescending(c => c.CompletedAt)
                        .Take(10)
                        .Select(c => new CompletionDetailsModel
                        {
                            Id = c.Id,
                            CompletedAt = c.CompletedAt,
                            Completed = c.Completed,
                            Notes = c.Notes
                        })
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserHabitDetailsModel>> GetHabitsNeedingAttentionAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            var lastWeek = today.AddDays(-7);

            var habitsNeedingAttention = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .Where(uh => !uh.Completions.Any(c =>
                    c.CompletedAt.Date == today && c.Completed) ||
                    CalculateCompletionRate(uh.Completions.Where(c =>
                        c.CompletedAt >= lastWeek)) < 70)
                .Select(uh => new UserHabitDetailsModel
                {
                    HabitId = uh.HabitId,
                    Title = uh.Habit.Title,
                    Description = uh.Habit.Description,
                    Frequency = uh.Frequency,
                    CreatedAt = uh.CreatedAt,
                    CurrentStreak = CalculateCurrentStreak(uh.Completions),
                    CompletionRate = CalculateCompletionRate(uh.Completions)
                })
                .ToListAsync();

            return habitsNeedingAttention;
        }

        public async Task<IEnumerable<UserHabitDetailsModel>> GetMostConsistentHabitsAsync(
            string userId, int count = 5)
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);

            return await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .Select(uh => new UserHabitDetailsModel
                {
                    HabitId = uh.HabitId,
                    Title = uh.Habit.Title,
                    Description = uh.Habit.Description,
                    Frequency = uh.Frequency,
                    CurrentStreak = CalculateCurrentStreak(uh.Completions),
                    CompletionRate = CalculateCompletionRate(
                        uh.Completions.Where(c => c.CompletedAt >= lastMonth))
                })
                .OrderByDescending(h => h.CompletionRate)
                .ThenByDescending(h => h.CurrentStreak)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IDictionary<int, int>> GetCurrentStreaksAsync(string userId)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            return userHabits.ToDictionary(
                uh => uh.HabitId,
                uh => CalculateCurrentStreak(uh.Completions)
            );
        }

        public async Task<bool> HasCompletedDailyGoalsAsync(string userId, DateTime date)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            foreach (var habit in userHabits)
            {
                var completedToday = habit.Completions
                    .Any(c => c.CompletedAt.Date == date.Date && c.Completed);

                if (!completedToday && habit.Frequency == 1) // Daily habits
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<double> GetCompletionRateAsync(
            string userId, int habitId, DateTime startDate, DateTime endDate)
        {
            var userHabit = await _context.UsersHabits
                .Include(uh => uh.Completions)
                .FirstOrDefaultAsync(uh =>
                    uh.UserId == userId &&
                    uh.HabitId == habitId);

            if (userHabit == null)
            {
                return 0;
            }

            var completionsInRange = userHabit.Completions
                .Where(c => c.CompletedAt.Date >= startDate.Date &&
                           c.CompletedAt.Date <= endDate.Date);

            return CalculateCompletionRate(completionsInRange);
        }

        #region Helper Methods

        private int CalculateCurrentStreak(IEnumerable<Completion> completions)
        {
            if (!completions.Any())
                return 0;

            var orderedCompletions = completions
                .OrderByDescending(c => c.CompletedAt)
                .Where(c => c.Completed)
                .ToList();

            if (!orderedCompletions.Any())
                return 0;

            var currentStreak = 1;
            var lastDate = orderedCompletions.First().CompletedAt.Date;

            // If the last completion is not today or yesterday, streak is broken
            if (lastDate < DateTime.UtcNow.Date.AddDays(-1))
                return 0;

            for (int i = 1; i < orderedCompletions.Count; i++)
            {
                var currentDate = orderedCompletions[i].CompletedAt.Date;

                if (lastDate.AddDays(-1) == currentDate)
                {
                    currentStreak++;
                    lastDate = currentDate;
                }
                else
                {
                    break;
                }
            }

            return currentStreak;
        }

        private int CalculateBestStreak(IEnumerable<Completion> completions)
        {
            if (!completions.Any())
                return 0;

            var orderedCompletions = completions
                .OrderBy(c => c.CompletedAt)
                .Where(c => c.Completed)
                .ToList();

            if (!orderedCompletions.Any())
                return 0;

            var currentStreak = 1;
            var bestStreak = 1;
            var lastDate = orderedCompletions.First().CompletedAt.Date;

            foreach (var completion in orderedCompletions.Skip(1))
            {
                var currentDate = completion.CompletedAt.Date;

                if (lastDate.AddDays(1) == currentDate)
                {
                    currentStreak++;
                    if (currentStreak > bestStreak)
                    {
                        bestStreak = currentStreak;
                    }
                }
                else
                {
                    currentStreak = 1;
                }

                lastDate = currentDate;
            }

            return bestStreak;
        }

        private double CalculateCompletionRate(IEnumerable<Completion> completions)
        {
            if (!completions.Any())
                return 0;

            var totalCompletions = completions.Count();
            var successfulCompletions = completions.Count(c => c.Completed);

            return Math.Round((double)successfulCompletions / totalCompletions * 100, 2);
        }

        private double CalculateCompletionRate(IEnumerable<Completion> completions, DateTime since)
        {
            return CalculateCompletionRate(completions.Where(c => c.CompletedAt >= since));
        }

        public Task<IEnumerable<string>> GetActiveUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<(string userId, int habitId), int>> GetCurrentStreaksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, List<UserHabitDetailsModel>>> GetHabitsNeedingAttentionAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
