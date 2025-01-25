using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheDailyRoutine.Infrastructure.Data.Constants.DataConstants;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(HabitDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool Predefined { get; set; }


        // поле IsPublic за публичност на навика
        public bool IsPublic { get; set; }  

        public IEnumerable<UserHabit> UsersHabits { get; set; } = new List<UserHabit>();
    }
}
