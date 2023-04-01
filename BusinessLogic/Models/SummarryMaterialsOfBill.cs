using DataAcess.Entities.Enums;

namespace BusinessLogic.Models
{
    public class SummarryMaterialOfBill
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ModelType ModelType { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
