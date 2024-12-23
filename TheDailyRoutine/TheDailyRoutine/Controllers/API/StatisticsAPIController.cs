using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Controllers.API
{
    public class StatisticsAPIController : BaseAPIController
    {
        private readonly IStatisticsService _statisticsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StatisticsAPIController> _logger;

        public StatisticsAPIController(
            IStatisticsService statisticsService,
            UserManager<ApplicationUser> userManager,
            ILogger<StatisticsAPIController> logger)
        {
            _statisticsService = statisticsService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets overall completion rate for specified date range
        /// </summary>
        [HttpGet("completion-rate")]
        public async Task<IActionResult> GetCompletionRate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var rate = await _statisticsService.GetOverallCompletionRateAsync(userId, startDate, endDate);
                return Success(new { completionRate = rate });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving completion rate");
                return Error("Failed to retrieve completion rate");
            }
        }

        /// <summary>
        /// Gets habit-specific statistics
        /// </summary>
        [HttpGet("habit/{habitId}")]
        public async Task<IActionResult> GetHabitStatistics(
            int habitId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var stats = await _statisticsService.GetHabitStatisticsAsync(userId, habitId, startDate, endDate);

                if (stats == null)
                {
                    return NotFoundError("Habit not found");
                }

                return Success(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving habit statistics");
                return Error("Failed to retrieve habit statistics");
            }
        }

        /// <summary>
        /// Gets time-of-day statistics
        /// </summary>
        [HttpGet("time-analysis")]
        public async Task<IActionResult> GetTimeAnalysis([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var timeStats = await _statisticsService.GetTimeOfDayStatsAsync(userId, startDate, endDate);
                var dayStats = await _statisticsService.GetDayOfWeekStatsAsync(userId, startDate, endDate);

                return Success(new { timeOfDay = timeStats, dayOfWeek = dayStats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving time analysis");
                return Error("Failed to retrieve time analysis");
            }
        }

        /// <summary>
        /// Gets all current streaks
        /// </summary>
        [HttpGet("streaks")]
        public async Task<IActionResult> GetStreaks()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var streaks = await _statisticsService.GetAllStreaksAsync(userId);
                return Success(streaks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving streaks");
                return Error("Failed to retrieve streaks");
            }
        }

        /// <summary>
        /// Gets habit correlations
        /// </summary>
        [HttpGet("correlations")]
        public async Task<IActionResult> GetCorrelations()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var correlations = await _statisticsService.GetHabitCorrelationsAsync(userId);
                return Success(correlations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving correlations");
                return Error("Failed to retrieve correlations");
            }
        }

        /// <summary>
        /// Gets achievement progress
        /// </summary>
        [HttpGet("achievements")]
        public async Task<IActionResult> GetAchievements()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var achievements = await _statisticsService.GetAchievementProgressAsync(userId);
                return Success(achievements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving achievements");
                return Error("Failed to retrieve achievements");
            }
        }

        /// <summary>
        /// Gets monthly progress summary
        /// </summary>
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyProgress([FromQuery] DateTime month)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var progress = await _statisticsService.GetMonthlyProgressAsync(userId, month);
                return Success(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monthly progress");
                return Error("Failed to retrieve monthly progress");
            }
        }
    }
}