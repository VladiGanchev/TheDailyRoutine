using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheDailyRoutine.Infrastructure.Data.Constants.DataConstants;


namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class Completion
    {
        public int Id { get; set; }
        public int UserHabitId { get; set; }
        public UserHabit UserHabit { get; set; } = null!;
        public DateTime CompletedAt { get; set; }
        public bool Completed { get; set; }

        [Required]
        [MaxLength(CompletionNoteMaxLength)]
        public string Notes { get; set; } = string.Empty;

    }
}
