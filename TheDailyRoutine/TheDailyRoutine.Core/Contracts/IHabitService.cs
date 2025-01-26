using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;

namespace TheDailyRoutine.Core.Contracts
{
    public interface IHabitService
    {
        /// <summary>
        /// Gets all predefined habits with their associated user habits
        /// </summary>
        Task<IEnumerable<HabitServiceModel>> GetAllPredefinedHabitsAsync();

        /// <summary>
        /// Gets a specific habit by ID with its associated user habits
        /// </summary>
        Task<HabitServiceModel?> GetHabitByIdAsync(int id);

        /// <summary>
        /// Gets habits for a specific user
        /// </summary>
        Task<IEnumerable<HabitServiceModel>> GetUserHabitsAsync(string userId);

        /// <summary>
        /// Adds a new predefined habit
        /// </summary>
        /// <returns>The ID of the newly created habit</returns>
        Task<(bool success, int habitId, string error)> AddPredefinedHabitAsync(AddHabitServiceModel model);

        /// <summary>
        /// Updates an existing predefined habit
        /// </summary>
        Task<(bool success, string error)> UpdatePredefinedHabitAsync(EditHabitServiceModel model);

        /// <summary>
        /// Checks if a habit exists by its title
        /// </summary>
        Task<bool> HabitExistsAsync(string title);

        /// <summary>
        /// Deletes a predefined habit if it's not being used by any users
        /// </summary>
        Task<(bool success, string error)> DeletePredefinedHabitAsync(int id);

        /// <summary>
        /// Gets the total number of users for a specific habit
        /// </summary>
        Task<int> GetHabitUserCountAsync(int habitId);


        /// <summary>
        /// Gets completion statistics for a habit
        /// </summary>
        Task<(int totalCompletions, double completionRate)> GetHabitStatisticsAsync(int habitId);

        //Добавено за публик
        /// <summary>
        /// Gets all public predefined habits with their associated user habits
        /// </summary>
    }
}
