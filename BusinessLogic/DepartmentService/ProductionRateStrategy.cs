using BusinessLogic.Models;

namespace BusinessLogic.DepartmentService 
{ 
    public interface IStrategy
    {
        internal double Calculate(List<StationModel> stationEntities);
    }

    public class ProductionRateStrategy : IStrategy
    {
        public double Calculate(List<StationModel> stationEntities)
        {
            return 0;
        }
    }

    public class ProductionRateOfFlowLineMassProductionStrategy : IStrategy
    {
        public double Calculate(List<StationModel> stationEntities)
        {
            var maxOperationTimeInSecond = stationEntities.Max(r => r.OperationTimeInSecond);
            var maxTransferTimeInSecond = stationEntities.Max(r => r.TransferTimeInSecond);
            return 1/((maxOperationTimeInSecond + maxTransferTimeInSecond)*(double)stationEntities.Count);
        }
    }
}
