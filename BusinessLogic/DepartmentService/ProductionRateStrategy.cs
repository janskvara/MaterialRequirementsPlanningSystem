using BusinessLogic.Models;

namespace BusinessLogic.DepartmentService 
{ 
    public interface IStrategy
    {
        internal double CalculateCycleTime(List<StationModel> stationEntities);
    }

    public class ProductionRateStrategy : IStrategy
    {
        public double CalculateCycleTime(List<StationModel> stationEntities)
        {
            return 0;
        }
    }

    public class ProductionRateOfFlowLineMassProductionStrategy : IStrategy
    {
        public double CalculateCycleTime(List<StationModel> stationEntities)
        {
            var maxOperationTimeInSecond = stationEntities.Max(r => r.OperationTimeInSecond);
            var maxTransferTimeInSecond = stationEntities.Max(r => r.TransferTimeInSecond);
            return 1/((double)maxOperationTimeInSecond + maxTransferTimeInSecond);
        }
    }
}
