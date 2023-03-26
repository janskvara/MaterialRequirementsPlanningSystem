namespace DataAcess.Entities.Enums
{
    public enum TypeOfProduction
    {
        None = 0,
        JobShopWithProductionQuantityOne = 1,
        SequentialBatchProduction = 2,
        SimultaneousBatchProduction = 3,
        QuantityMassProduction = 4,
        FlowLineMassProduction = 5
    }
}
