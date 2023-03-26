using DataAcess.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class DepartmentEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ShiftDurationInHours { get; set; }
        public int WorkDaysPerWeek { get; set; }
        public TypeOfProduction TypeOfProduction { get; set; }
    }
}
