using DataAcess.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class DepartmentEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Shift> ShiftsPerDay { get; set; } = new();
        public List<DayOfWeek> WorkDaysOfWeek { get; set; } = new();
        public TypeOfProduction TypeOfProduction { get; set; }
    }

    public class Shift
    {
        public Shift(DateTime startTime, DateTime finishTime)
        {
            StartTime = startTime.TimeOfDay;
            EndTime = finishTime.TimeOfDay;
            ShiftDurationInSeconds = (EndTime - StartTime).TotalSeconds;
            ShiftDurationInMinutes = (EndTime - StartTime).TotalMinutes;
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double ShiftDurationInSeconds { get; set; }
        public double ShiftDurationInMinutes { get; set; }
    }
}
