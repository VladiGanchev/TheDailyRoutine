using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.API.Responses.Statistics
{
    public class StatsSummaryResponse
    {
        public double OverallCompletionRate { get; set; }
        public int TotalHabits { get; set; }
        public int ActiveHabits { get; set; }
        public int LongestStreak { get; set; }
        public List<string> TopPerformingHabits { get; set; } = new List<string>();
        public List<string> AreasForImprovement { get; set; } = new List<string>();
    }
}
