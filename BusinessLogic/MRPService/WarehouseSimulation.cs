using BusinessLogic.Models;

namespace BusinessLogic.MRPService
{
    public class WarehouseSimulation
    {
        public Dictionary<string, List<GoodsChangeModel>> ChangesOfProducts { get; set; } = new();

        public void AddGoodsChange(string partNumber, DateTimeOffset timeOfChange, double change, double newCapacity)
        {
            var goodsChange = new GoodsChangeModel(timeOfChange, change, newCapacity);
            if (ChangesOfProducts.TryGetValue(partNumber, out var changesOfProducts))
            {
                changesOfProducts.Add(goodsChange);
            }
            else
            {
                var newGoodsChanges = new List<GoodsChangeModel> { goodsChange };
                ChangesOfProducts.Add(partNumber, newGoodsChanges);
            }
        }

    }
}
