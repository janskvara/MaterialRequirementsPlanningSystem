using DataAcess.Entities.Enums;
using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Tuple<DateTime, DateTime>> ShiftsPerDay { get; set; } = new();
        public List<DayOfWeek> WorkDaysOfWeek { get; set; } = new();
        public TypeOfProduction TypeOfProduction { get; set; }
    }
}
