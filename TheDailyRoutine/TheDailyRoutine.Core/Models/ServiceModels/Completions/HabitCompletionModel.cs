using System.ComponentModel.DataAnnotations;
using static TheDailyRoutine.Infrastructure.Data.Constants.DataConstants;

namespace TheDailyRoutine.Core.Models.ServiceModels.Completions
{
    public class HabitCompletionModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        [StringLength(CompletionNoteMaxLength,
            MinimumLength = CompletionNoteMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Notes { get; set; } = string.Empty;
    }
}