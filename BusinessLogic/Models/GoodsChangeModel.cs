namespace BusinessLogic.Models
{
    public class GoodsChangeModel
    {
        public GoodsChangeModel(DateTimeOffset timeOfChange, double change, double newCapacity)
        {
            TimeOfChange= timeOfChange;
            Change= change;
            NewCapacity= newCapacity;
        }

        public DateTimeOffset TimeOfChange { get; }
        public double Change { get; }
        public double NewCapacity { get; }
    }
}
