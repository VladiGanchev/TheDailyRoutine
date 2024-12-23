using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.Enums;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class TimeOfDayStatsModel
    {
        public TimeSlot TimeSlot { get; set; }
        public int TotalAttempts { get; set; }
        public int SuccessfulAttempts { get; set; }
        public double SuccessRate { get; set; }
        // Added properties
        public TimeSpan AverageDuration { get; set; }
        public bool IsPeakPerformanceTime { get; set; }

        public string TimeOfDayDescription => TimeSlot switch
        {
            TimeSlot.EarlyMorning => "5:00 AM - 8:59 AM",
            TimeSlot.Morning => "9:00 AM - 11:59 AM",
            TimeSlot.Afternoon => "12:00 PM - 4:59 PM",
            TimeSlot.Evening => "5:00 PM - 8:59 PM",
            TimeSlot.Night => "9:00 PM - 4:59 AM",
            _ => "Unknown"
        };
    }
}
