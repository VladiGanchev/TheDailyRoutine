using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;

namespace TheDailyRoutine.Core.Models.ServiceModels.Habits
{
    public class UserHabitServiceModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int HabitId { get; set; }

        [Required]
        public int Frequency { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public IEnumerable<CompletionServiceModel> Completions { get; set; } = new List<CompletionServiceModel>();

    }
}
