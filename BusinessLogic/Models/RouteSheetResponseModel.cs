using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class RouteSheetResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double ProductionCapacityPerWeek { get; set; }
        public DepartmentEntity Department { get; set; } = null!;
        public List<StationModel> StationList { get; set; } = new List<StationModel>();
    }
}
