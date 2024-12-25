using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class MonthlyProgressModel
    {
        public DateTime Month { get; set; }
        public int TotalHabits { get; set; }
        public int TotalCompletions { get; set; }
        public double CompletionRate { get; set; }
        // Added properties
        public int DaysInMonth => DateTime.DaysInMonth(Month.Year, Month.Month);
        public double AverageCompletionsPerDay => TotalCompletions / (double)DaysInMonth;
        public List<int> DailyCompletions { get; set; } = new List<int>();
        public bool IsImprovement { get; set; } // Compared to previous month
        public double ImprovementPercentage { get; set; }
    }
}
