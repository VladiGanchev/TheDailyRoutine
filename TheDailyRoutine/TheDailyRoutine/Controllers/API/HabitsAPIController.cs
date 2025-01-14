using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.API.Requests;
using TheDailyRoutine.Core.Models.ServiceModels;
using TheDailyRoutine.Core.Models.ServiceModels.Habits;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Controllers.API
{
    [Route("api/habits")]
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
        [HttpGet("debug")]
        public async Task<IActionResult> DebugAuth()
        {
            var userId = _userManager.GetUserId(User);
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var habits = await _userHabitService.GetUserHabitsAsync(userId);

            return Ok(new
            {
                userId,
                isAuthenticated,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
               , habits
            });
        }


        [HttpGet("my")]
        public async Task<IActionResult> GetMyHabits()
        {
            // todo: service validation method? 

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var habits = await _userHabitService.GetUserHabitsAsync(userId);
                return Ok(new { success = true, data = habits }); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user habits");
                return BadRequest(new { success = false, error = "Failed to retrieve your habits" });
            }
        }

        // [Authorize(Roles = "Admin")]  
        [HttpPost("predefined")]      
        public async Task<IActionResult> AddPredefinedHabit([FromBody] AddHabitServiceModel model)
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
                    _logger.LogWarning("Failed to add predefined habit: {Error}", error);
                    return Error(error);
                }

                _logger.LogInformation("Admin {UserId} added new predefined habit: {Title}",
                    _userManager.GetUserId(User),
                    model.Title);

                return Success(new { habitId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding predefined habit");
                return Error("An error occurred while adding the predefined habit");
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

        [HttpPost("assign")]
        public async Task<IActionResult> AssignHabit([FromBody] AssignHabitRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError();
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var (success, error) = await _userHabitService.AddHabitToUserAsync(
                    userId,
                    model.HabitId,
                    model.Frequency);

                if (!success)
                {
                    return Error(error);
                }

                return Success("Habit assigned successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning habit to user");
                return Error("Failed to assign habit");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("predefined/{id}")]
        public async Task<IActionResult> UpdatePredefinedHabit(int id, [FromBody] EditHabitServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError();
            }

            if (id != model.Id)
            {
                return ValidationError("ID in URL does not match ID in request body");
            }

            try
            {
                var (success, error) = await _habitService.UpdatePredefinedHabitAsync(model);

                if (!success)
                {
                    _logger.LogWarning("Failed to update predefined habit: {Error}", error);
                    return Error(error);
                }

                // Get the updated habit to return in response
                var updatedHabit = await _habitService.GetHabitByIdAsync(id);
                if (updatedHabit == null)
                {
                    return NotFoundError("Updated habit not found");
                }

                _logger.LogInformation("Admin {UserId} updated predefined habit {HabitId}: {Title}",
                    _userManager.GetUserId(User),
                    id,
                    model.Title);

                return Success(updatedHabit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating predefined habit");
                return Error("Failed to update habit");
            }
        }

    }
}