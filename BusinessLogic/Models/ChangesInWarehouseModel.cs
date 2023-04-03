namespace BusinessLogic.Models
{
    public class ChangesInWarehouseModel
    {
        public Dictionary<string, List<GoodsChange>> ChangesOfProducts { get; set; } = new();

        public void AddGoodsChange(string partNumber, DateTimeOffset timeOfChange, double change,  double newCapacity)
        {
            var goodsChange = new GoodsChange(timeOfChange, change, newCapacity);
            if (ChangesOfProducts.TryGetValue(partNumber, out var changesOfProducts))
            {
                changesOfProducts.Add(goodsChange);
            }
            else
            {
                var newGoodsChanges = new List<GoodsChange>{goodsChange};
                ChangesOfProducts.Add(partNumber, newGoodsChanges);
            }
        }
    }

    public class GoodsChange
    {
        public GoodsChange(DateTimeOffset timeOfChange, double change, double newCapacity)
        {
            TimeOfChange= timeOfChange;
            Change= change;
            //PreviousCapacity= previousCapacity;
            NewCapacity= newCapacity;
        }

        public DateTimeOffset TimeOfChange { get; }
        public double Change { get; }
        //public double PreviousCapacity { get; }
        public double NewCapacity { get; }
    }
}
