namespace DataAcess.Models
{
    public class ProductRequirement
    {
        public string PartNumber { get; set; } = string.Empty;
        public double Quantity { get; set; }
    }

    public class ProductRequirementWithSequence: ProductRequirement
    {
        public int Sequence { get; set; }
    }
}
