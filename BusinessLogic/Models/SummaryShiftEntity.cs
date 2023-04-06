using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class SummaryShiftEntity
    {
        public string Id { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public List<string> NamesOfManufacturedModels { get; set; } = new();
        public List<ProductRequirementModel> BillOfMaterials { get; set; } = new();
    }
}
