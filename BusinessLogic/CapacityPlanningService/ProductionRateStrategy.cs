using DataAcess.Entities;

namespace BusinessLogic.CapacityPlanningService
{
    public interface IStrategy
    {
        internal double Calculate(List<StationEntity> stationEntities);
    }

    public class ProductionRateStrategy : IStrategy
    {
        public double Calculate(List<StationEntity> stationEntities)
        {
            return 0;
        }
    }

    public class ProductionRateOfFlowLineMassProductionStrategy : IStrategy
    {
        public double Calculate(List<StationEntity> stationEntities)
        {
            var maxOperationTimeInSecond = stationEntities.Max(r => r.OperationTimeInSecond);
            var maxTransferTimeInSecond = stationEntities.Max(r => r.TransferTimeInSecond);
            return 1/((maxOperationTimeInSecond + maxTransferTimeInSecond)*(double)stationEntities.Count);
        }
    }
}
