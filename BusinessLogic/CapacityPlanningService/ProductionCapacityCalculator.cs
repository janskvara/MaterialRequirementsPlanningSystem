using DataAcess.Entities;
using DataAcess.Entities.Enums;

namespace BusinessLogic.CapacityPlanningService
{
    internal static class ProductionCapacityCalculator
    {
        internal static double CalculateProductionCapacityPerWeek(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            return CalculateProductionCapacityPerShift(departure, stationEntities) * departure.WorkDaysPerWeek;
        }

        internal static double CalculateProductionCapacityPerShift(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            return Math.Floor(CalculateProductionCapacityPerHour(departure, stationEntities) * departure.ShiftDurationInHours);
        }

        internal static double CalculateProductionCapacityPerHour(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            switch (departure.TypeOfProduction)
            {
                case TypeOfProduction.None:
                case TypeOfProduction.SequentialBatchProduction:
                case TypeOfProduction.SimultaneousBatchProduction:
                case TypeOfProduction.QuantityMassProduction:
                case TypeOfProduction.JobShopWithProductionQuantityOne:
                    return new ProductionRateStrategy().Calculate(stationEntities);
                case TypeOfProduction.FlowLineMassProduction:
                    var productionRatePerSecond = new ProductionRateOfFlowLineMassProductionStrategy().Calculate(stationEntities);
                    return productionRatePerSecond * 60 * 60;
            }
            return 0;  
        }
    }
}
