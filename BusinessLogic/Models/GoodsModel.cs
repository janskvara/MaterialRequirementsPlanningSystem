namespace BusinessLogic.Models
{
    public class GoodsModel
    {
        public GoodsModel(string partNumber, string description, string unit, double maxCapacity,
            double actualCapacity, double minimumThresholdCapacity)
        {
            PartNumber= partNumber;
            Description= description;
            Unit= unit;
            MaxCapacityOfProduct= maxCapacity;
            ActualCapacity= actualCapacity;
            MinimumThresholdCapacity= minimumThresholdCapacity;
            ActualCapacityInPercentage = Math.Round((ActualCapacity / MaxCapacityOfProduct) * 100, 2);
        }

        public string PartNumber { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public string Unit { get; } = string.Empty;
        public double MaxCapacityOfProduct { get; }
        public double ActualCapacity { get; }
        public double MinimumThresholdCapacity { get; }
        public double ActualCapacityInPercentage { get; }
    }
}
