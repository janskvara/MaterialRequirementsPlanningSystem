using DataAcess.Entities.Enums;
using DataAcess.Entities;
namespace BusinessLogic.Models
{
    public class SummaryDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Shift> ShiftsPerDay { get; set; } = new();
        public List<string> WorkDaysOfWeek { get; set; } = new();
        public string TypeOfProduction { get; set; } = string.Empty;
        public double MaximumCapacityPerDay { get; set; }
        public double MaximumCapacityPerWeek { get; set; }
        public Dictionary<string,ProductInformationModel> Products { get; set; } = new();
    }
}
