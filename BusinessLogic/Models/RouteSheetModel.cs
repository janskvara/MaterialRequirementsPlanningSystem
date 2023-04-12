using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class RouteSheetModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Department { get; set; }
        public List<StationModel> StationList { get; set; } = new List<StationModel>();
    }
}
