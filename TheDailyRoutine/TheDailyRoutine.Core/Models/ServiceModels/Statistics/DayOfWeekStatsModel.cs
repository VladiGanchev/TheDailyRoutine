using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class DayOfWeekStatsModel
    {
        public DayOfWeek Day { get; set; }
        public int TotalAttempts { get; set; }
        public int SuccessfulAttempts { get; set; }
        public double SuccessRate { get; set; }
        // Added 
        public double AveragePerDay => TotalAttempts > 0 ? (double)SuccessfulAttempts / TotalAttempts : 0;
        public bool IsMostSuccessfulDay { get; set; }

    }
}
