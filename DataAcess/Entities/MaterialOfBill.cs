using DataAcess.Entities.Enums;

namespace DataAcess.Entities
{
    public class MaterialOfBill
    {
        public int Order { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ModelType ModelType { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
