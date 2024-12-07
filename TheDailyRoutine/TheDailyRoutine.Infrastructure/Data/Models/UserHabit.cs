using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class UserHabit
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int HabitId { get; set; }

        public Habit Habit { get; set; } = null!;

        [Required]
        public int Frequency { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<Completion> Completions { get; set; } = new List<Completion>();
    }
}
