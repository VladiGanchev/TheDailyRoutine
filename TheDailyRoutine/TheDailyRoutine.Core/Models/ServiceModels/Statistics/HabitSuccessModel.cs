using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitSuccessModel
    {
        public int HabitId { get; set; }
        public string Title { get; set; } = string.Empty;
        public double SuccessRate { get; set; }
        // Added properties
        public int ConsecutiveDaysCompleted { get; set; }
        public string Category { get; set; } = string.Empty;
        public TimeSpan AverageCompletionTime { get; set; }
        public List<string> RelatedSuccessfulHabits { get; set; } = new List<string>();
    }
}
