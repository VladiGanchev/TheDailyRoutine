using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Controllers.API
{
    public class CompletionsAPIController : BaseAPIController
    {
        private readonly ICompletionService _completionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompletionsAPIController> _logger;

        public CompletionsAPIController(
            ICompletionService completionService,
            UserManager<ApplicationUser> userManager,
            ILogger<CompletionsAPIController> logger)
        {
            _completionService = completionService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets the daily completion summary for a specific date
        /// </summary>
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailySummary([FromQuery] DateTime? date)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var targetDate = date ?? DateTime.Today;
                var summary = await _completionService.GetDailyCompletionSummaryAsync(userId, targetDate);
                return Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving daily summary");
                return Error("Failed to retrieve daily summary");
            }
        }

        /// <summary>
        /// Gets the weekly completion summary
        /// </summary>
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklySummary([FromQuery] DateTime? startDate)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var weekStart = startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var summary = await _completionService.GetWeeklyCompletionSummaryAsync(userId, weekStart);
                return Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weekly summary");
                return Error("Failed to retrieve weekly summary");
            }
        }

        /// <summary>
        /// Marks a habit as complete for a specific date
        /// </summary>
        [HttpPost("complete/{habitId}")]
        public async Task<IActionResult> MarkComplete(
            int habitId,
            [FromBody] MarkCompleteRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var (success, error) = await _completionService.MarkHabitAsCompleteAsync(
                    userId,
                    habitId,
                    request.Date ?? DateTime.Now,
                    request.Notes ?? string.Empty);

                if (!success)
                {
                    return Error(error);
                }

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking habit as complete");
                return Error("Failed to mark habit as complete");
            }
        }

        /// <summary>
        /// Marks a habit as incomplete for a specific date
        /// </summary>
        [HttpPost("incomplete/{habitId}")]
        public async Task<IActionResult> MarkIncomplete(
            int habitId,
            [FromBody] MarkIncompleteRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var (success, error) = await _completionService.MarkHabitAsIncompleteAsync(
                    userId,
                    habitId,
                    request.Date ?? DateTime.Now);

                if (!success)
                {
                    return Error(error);
                }

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking habit as incomplete");
                return Error("Failed to mark habit as incomplete");
            }
        }

        /// <summary>
        /// Gets completion history for a specific date range
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? habitId = null)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var completions = await _completionService.GetHabitCompletionsAsync(
                    userId,
                    habitId ?? 0,
                    startDate,
                    endDate);

                return Success(completions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving completion history");
                return Error("Failed to retrieve completion history");
            }
        }

        /// <summary>
        /// Updates the notes for a completion
        /// </summary>
        [HttpPut("notes/{completionId}")]
        public async Task<IActionResult> UpdateNotes(
            int completionId,
            [FromBody] UpdateNotesRequest request)
        {
            try
            {
                var (success, error) = await _completionService.UpdateCompletionNotesAsync(
                    completionId,
                    request.Notes);

                if (!success)
                {
                    return Error(error);
                }

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating completion notes");
                return Error("Failed to update completion notes");
            }
        }
    }

    public class MarkCompleteRequest
    {
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
    }

    public class MarkIncompleteRequest
    {
        public DateTime? Date { get; set; }
    }

    public class UpdateNotesRequest
    {
        public string Notes { get; set; } = string.Empty;
    }
}