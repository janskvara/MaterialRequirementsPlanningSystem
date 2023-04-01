namespace BusinessLogic.Models
{
    public class ChangesInWarehouseModel
    {
        public Dictionary<string, List<GoodsChange>> ChangesOfProducts { get; set; } = new();
    }

    public class GoodsChange
    {
        public DateTimeOffset TimeOfChange { get; set; }
        public double PreviousCapacity { get; set; }
        public double NextCapacity { get; set; }
        public double ActualCapacity { get; set; }
    }
}
