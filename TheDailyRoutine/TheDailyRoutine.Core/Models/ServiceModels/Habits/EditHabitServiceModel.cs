using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Infrastructure.Data.Constants;

namespace TheDailyRoutine.Core.Models.ServiceModels.Habits
{
    public class EditHabitServiceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = DataConstants.RequireErrorMessage)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = DataConstants.RequireErrorMessage)]
        [StringLength(DataConstants.HabitDescriptionMaxLength,
            MinimumLength = DataConstants.HabitDescriptionMinLength,
            ErrorMessage = DataConstants.StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

    }
}
