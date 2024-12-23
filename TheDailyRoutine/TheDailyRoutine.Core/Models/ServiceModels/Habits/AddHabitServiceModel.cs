using System.ComponentModel.DataAnnotations;
using TheDailyRoutine.Infrastructure.Data.Constants;

namespace TheDailyRoutine.Core.Models.ServiceModels.Habits
{
    public class AddHabitServiceModel
    {
        [Required(ErrorMessage = DataConstants.RequireErrorMessage)] //Infrastructure.Data.Constants.DataConstants.RequireErrorMessage)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = DataConstants.RequireErrorMessage)]
        [StringLength(DataConstants.HabitDescriptionMaxLength,
            MinimumLength = DataConstants.HabitDescriptionMinLength,
            ErrorMessage = DataConstants.StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

    }
}
