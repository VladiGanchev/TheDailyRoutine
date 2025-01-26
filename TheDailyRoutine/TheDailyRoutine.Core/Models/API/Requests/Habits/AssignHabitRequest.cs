using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.API.Requests
{
    public class AssignHabitRequest
    {
        [Required]
        public int HabitId { get; set; }

        [Required]
        [Range(1, 30)] // Assuming this is a reasonable range for frequency
        public int Frequency { get; set; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
