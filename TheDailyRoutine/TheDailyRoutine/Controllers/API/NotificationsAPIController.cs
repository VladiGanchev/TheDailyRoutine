using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels;
using TheDailyRoutine.Infrastructure.Data.Models;
using TheDailyRoutine.Infrastructure.Data.Enums;
using TheDailyRoutine.Core.Models.ServiceModels.Notifications;

namespace TheDailyRoutine.Controllers.API
{
    public class NotificationsAPIController : BaseAPIController
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NotificationsAPIController> _logger;

        public NotificationsAPIController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager,
            ILogger<NotificationsAPIController> logger)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets unread notifications for the current user
        /// </summary>
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
                return Success(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread notifications");
                return Error("Failed to retrieve notifications");
            }
        }

        /// <summary>
        /// Gets notification history for specified date range
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetNotificationHistory(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] NotificationType? type = null)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var notifications = await _notificationService.GetNotificationHistoryAsync(
                    userId, startDate, endDate, type);
                return Success(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification history");
                return Error("Failed to retrieve notification history");
            }
        }

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        [HttpPost("mark-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var success = await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);

                if (!success)
                {
                    return Error("Failed to mark notification as read");
                }

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read");
                return Error("Failed to mark notification as read");
            }
        }

        /// <summary>
        /// Gets user notification preferences
        /// </summary>
        [HttpGet("preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var preferences = await _notificationService.GetUserNotificationPreferencesAsync(userId);
                return Success(preferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification preferences");
                return Error("Failed to retrieve preferences");
            }
        }

        /// <summary>
        /// Updates user notification preferences
        /// </summary>
        [HttpPut("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] UserNotificationPreferencesModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError();
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var success = await _notificationService.UpdateUserNotificationPreferencesAsync(userId, model);

                if (!success)
                {
                    return Error("Failed to update preferences");
                }

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification preferences");
                return Error("Failed to update preferences");
            }
        }
    }
}