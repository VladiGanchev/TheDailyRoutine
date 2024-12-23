using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Controllers.API
{
    public class HabitsAPIController : BaseAPIController
    {
        private readonly IHabitService _habitService;
        private readonly IUserHabitService _userHabitService;
        private readonly ICompletionService _completionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HabitsAPIController> _logger;

        public HabitsAPIController(
            IHabitService habitService,
            IUserHabitService userHabitService,
            ICompletionService completionService,
            UserManager<ApplicationUser> userManager,
            ILogger<HabitsAPIController> logger)
        {
            _habitService = habitService;
            _userHabitService = userHabitService;
            _completionService = completionService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("predefined")]
        public async Task<IActionResult> GetPredefinedHabits()
        {
            try
            {
                var habits = await _habitService.GetAllPredefinedHabitsAsync();
                return Success(habits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving predefined habits");
                return Error("Failed to retrieve predefined habits");
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyHabits()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var habits = await _userHabitService.GetUserHabitsAsync(userId);
                return Success(habits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user habits");
                return Error("Failed to retrieve your habits");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddHabit([FromBody] AddHabitServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError();
            }

            try
            {
                var (success, habitId, error) = await _habitService.AddPredefinedHabitAsync(model);

                if (!success)
                {
                    return Error(error);
                }

                return Success(new { habitId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding habit");
                return Error("Failed to add habit");
            }
        }

        [HttpPost("complete/{habitId}")]
        public async Task<IActionResult> CompleteHabit(int habitId, [FromBody] string notes = "")
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var (success, error) = await _completionService.MarkHabitAsCompleteAsync(
                    userId,
                    habitId,
                    DateTime.Now,
                    notes);

                if (!success)
                {
                    return Error(error);
                }

                return Success(message: "Habit marked as complete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing habit");
                return Error("Failed to complete habit");
            }
        }

        [HttpDelete("{habitId}")]
        public async Task<IActionResult> RemoveHabit(int habitId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var (success, error) = await _userHabitService.RemoveHabitFromUserAsync(userId, habitId);

                if (!success)
                {
                    return Error(error);
                }

                return Success(message: "Habit removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing habit");
                return Error("Failed to remove habit");
            }
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayHabits()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var dailySummary = await _completionService.GetDailyCompletionSummaryAsync(
                    userId,
                    DateTime.Today);

                return Success(dailySummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving today's habits");
                return Error("Failed to retrieve today's habits");
            }
        }
    }
}