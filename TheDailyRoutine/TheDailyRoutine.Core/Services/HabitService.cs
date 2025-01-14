using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels;
using TheDailyRoutine.Infrastructure.Data;
using TheDailyRoutine.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;

namespace TheDailyRoutine.Core.Services
{
    public class HabitService : IHabitService
    {
        private readonly ApplicationDbContext _context;

        public HabitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HabitServiceModel>> GetAllPredefinedHabitsAsync()
        {
            return await _context.Habits
                .Where(h => h.Predefined)
                .Select(h => new HabitServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Description = h.Description,
                    Predefined = h.Predefined,
                    UsersHabits = h.UsersHabits.Select(uh => new UserHabitServiceModel
                    {
                        UserId = uh.UserId,
                        HabitId = uh.HabitId,
                        Frequency = uh.Frequency,
                        CreatedAt = uh.CreatedAt,
                        Completions = uh.Completions.Select(c => new CompletionServiceModel
                        {
                            Id = c.Id,
                            UserHabitUserId = c.UserHabit.UserId,
                            UserHabitHabitId = c.UserHabit.HabitId,
                            CompletedAt = c.CompletedAt,
                            Completed = c.Completed,
                            Notes = c.Notes
                        })
                    })
                })
                .OrderBy(h => h.Title)
                .ToListAsync();
        }

        public async Task<HabitServiceModel?> GetHabitByIdAsync(int id)
        {
            return await _context.Habits
                .Where(h => h.Id == id)
                .Select(h => new HabitServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Description = h.Description,
                    Predefined = h.Predefined,
                    UsersHabits = h.UsersHabits.Select(uh => new UserHabitServiceModel
                    {
                        UserId = uh.UserId,
                        HabitId = uh.HabitId,
                        Frequency = uh.Frequency,
                        CreatedAt = uh.CreatedAt
                    })
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<HabitServiceModel>> GetUserHabitsAsync(string userId)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Habit)
                .Include(uh => uh.Completions)
                .Where(uh => uh.UserId == userId)
                .Select(uh => new HabitServiceModel
                {
                    Id = uh.Habit.Id,
                    Title = uh.Habit.Title,
                    Description = uh.Habit.Description,
                    Predefined = uh.Habit.Predefined,
                    UsersHabits = new[]
                    {
                        new UserHabitServiceModel
                        {
                            UserId = uh.UserId,
                            HabitId = uh.HabitId,
                            Frequency = uh.Frequency,
                            CreatedAt = uh.CreatedAt,
                            Completions = uh.Completions.Select(c => new CompletionServiceModel
                            {
                                Id = c.Id,
                                UserHabitUserId = c.UserHabit.UserId,
                                UserHabitHabitId = c.UserHabit.HabitId,
                                CompletedAt = c.CompletedAt,
                                Completed = c.Completed,
                                Notes = c.Notes
                            })
                        }
                    }
                })
                .ToListAsync();

            return userHabits;
        }

        public async Task<(bool success, int habitId, string error)> AddPredefinedHabitAsync(AddHabitServiceModel model)
        {
            if (await HabitExistsAsync(model.Title))
            {
                return (false, 0, "A habit with this title already exists.");
            }

            try
            {
                var habit = new Habit
                {
                    Title = model.Title,
                    Description = model.Description,
                    Predefined = true
                };

                await _context.Habits.AddAsync(habit);
                await _context.SaveChangesAsync();

                return (true, habit.Id, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, 0, $"Failed to add habit: {ex.Message}");
            }
        }

        public async Task<(bool success, string error)> UpdatePredefinedHabitAsync(EditHabitServiceModel model)
        {
            try
            {
                var habit = await _context.Habits
                    .FirstOrDefaultAsync(h => h.Id == model.Id && h.Predefined);

                if (habit == null)
                {
                    return (false, $"Predefined habit with ID {model.Id} not found.");
                }

                // Check if the new title conflicts with another habit
                if (await _context.Habits.AnyAsync(h =>
                    h.Id != model.Id &&
                    h.Title.ToLower() == model.Title.ToLower()))
                {
                    return (false, "A habit with this title already exists.");
                }

                habit.Title = model.Title;
                habit.Description = model.Description;

                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to update habit: {ex.Message}");
            }
        }

        public async Task<bool> HabitExistsAsync(string title)
        {
            return await _context.Habits
                .AnyAsync(h => h.Title.ToLower() == title.ToLower());
        }

        public async Task<(bool success, string error)> DeletePredefinedHabitAsync(int id)
        {
            try
            {
                var habit = await _context.Habits
                    .Include(h => h.UsersHabits)
                    .FirstOrDefaultAsync(h => h.Id == id && h.Predefined);

                if (habit == null)
                {
                    return (false, $"Predefined habit with ID {id} not found.");
                }

                if (habit.UsersHabits.Any())
                {
                    return (false, "Cannot delete a habit that is being used by users.");
                }

                _context.Habits.Remove(habit);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to delete habit: {ex.Message}");
            }
        }

        public async Task<int> GetHabitUserCountAsync(int habitId)
        {
            return await _context.UsersHabits
                .CountAsync(uh => uh.HabitId == habitId);
        }

        public async Task<(int totalCompletions, double completionRate)> GetHabitStatisticsAsync(int habitId)
        {
            var userHabits = await _context.UsersHabits
                .Include(uh => uh.Completions)
                .Where(uh => uh.HabitId == habitId)
                .ToListAsync();

            if (!userHabits.Any())
            {
                return (0, 0);
            }

            var totalCompletions = userHabits
                .SelectMany(uh => uh.Completions)
                .Count(c => c.Completed);

            var totalAttempts = userHabits
                .SelectMany(uh => uh.Completions)
                .Count();

            var completionRate = totalAttempts > 0
                ? (double)totalCompletions / totalAttempts * 100
                : 0;

            return (totalCompletions, Math.Round(completionRate, 2));
        }
    }
}