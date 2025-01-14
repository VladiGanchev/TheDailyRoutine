using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Infrastructure.Data.Constants;

namespace TheDailyRoutine.Core.Models.ServiceModels.Completions
{
    public class CompletionServiceModel
    {
        public int Id { get; set; }

        public int UserHabitHabitId { get; set; }

        public string UserHabitUserId { get; set; } = string.Empty;

        [Required]
        public DateTime CompletedAt { get; set; }

        public bool Completed { get; set; }

        [Required(ErrorMessage = DataConstants.RequireErrorMessage)]
        [StringLength(DataConstants.CompletionNoteMaxLength,
            MinimumLength = DataConstants.CompletionNoteMinLength,
            ErrorMessage = DataConstants.StringLengthErrorMessage)]
        public string Notes { get; set; } = string.Empty;

    }
}
