namespace BusinessLogic.Models
{
    public class MaterialOfBillModel
    {
        public int Order { get; set; }// sequence
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ModelType { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
