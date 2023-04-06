using System;
using System.Collections.Generic;
using System.Linq;
namespace BusinessLogic.Models
{
    public class ShiftModel
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double ShiftDurationInSeconds { get; }
        public double ShiftDurationInMinutes { get; }

        public ShiftModel(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            ShiftDurationInSeconds = (EndTime - StartTime).TotalSeconds;
            ShiftDurationInMinutes = (EndTime - StartTime).TotalMinutes;
        }
    }
}
