namespace BusinessLogic.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ShiftDurationInHours { get; set; }
        public int WorkDaysPerWeek { get; set; }
        public string TypeOfProduction { get; set; } = string.Empty;
    }
}
