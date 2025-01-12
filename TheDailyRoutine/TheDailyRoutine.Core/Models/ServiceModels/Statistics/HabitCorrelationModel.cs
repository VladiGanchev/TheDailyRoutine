using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitCorrelationModel
    {
        public int HabitId1 { get; set; }
        public string HabitTitle1 { get; set; } = string.Empty;
        public int HabitId2 { get; set; }
        public string HabitTitle2 { get; set; } = string.Empty;
        public double CorrelationCoefficient { get; set; }
        // Added properties
        public string CorrelationStrength => GetCorrelationStrength(CorrelationCoefficient);
        public bool IsPositiveCorrelation => CorrelationCoefficient > 0;

        private string GetCorrelationStrength(double coefficient)
        {
            var abs = Math.Abs(coefficient);
            if (abs >= 0.7) return "Strong";
            if (abs >= 0.4) return "Moderate";
            return "Weak";
        }
    }
}
